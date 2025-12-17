using _Game._Scripts.PlayerSystem;
using _Game._Scripts.ScriptableObjects;
using _Game._Scripts.SwordOrbitSystem;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Game._Scripts.EnemyMoveSystem
{
    public class EnemyAIController : MonoBehaviour
    {
        public enum AIState
        {
            Idle,
            Wander,
            SeekSword,
            ChaseEnemy,
            AttackEnemy,
            Flee
        }

        [Header("References")] [SerializeField]
        private ExternalMoveInputSource _externalMoveInputSource;

        [SerializeField] private SwordOrbitController _swordOrbitController;

        [Header("Settings")] [SerializeField] private EnemyAISettingsSO _settings;

        private Vector2 m_AvoidVector;

        private Transform m_ClosestEnemy;
        private Transform m_ClosestSword;

        private Collider2D[] m_EnemyHits;
        private Vector2 m_FinalMoveInput;

        private float m_ScanTimer;

        private Collider2D[] m_SelfColliders;

        private AIState m_State;
        private Collider2D[] m_SwordHits;
        private float m_WanderTimer;
        private Vector2 m_WanderVector;


        private void Awake()
        {
            if (_settings == null)
            {
                Debug.LogError($"{nameof(EnemyAIController)} on '{name}' has no settings assigned.");
                enabled = false;
                return;
            }

            m_EnemyHits = new Collider2D[Mathf.Max(4, _settings.MaxScanHits)];
            m_SwordHits = new Collider2D[Mathf.Max(4, _settings.MaxScanHits)];
            m_SelfColliders = GetComponentsInChildren<Collider2D>(true);
        }


        private void Update()
        {
            TickScanTimer();
            TickWanderTimer();

            if (ShouldScanNow())
            {
                ScanEnvironment();
                m_ScanTimer = 0f;
            }

            m_State = DecideState();

            m_FinalMoveInput = ComputeMoveInput(m_State);

            if (_externalMoveInputSource)
                _externalMoveInputSource.PushInput(m_FinalMoveInput);
        }

        private void OnDrawGizmos()
        {
            if (_settings == null || !_settings.DrawGizmos) return;

            var p = transform.position;

            // Scan radii
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(p, _settings.EnemyScanRadius);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(p, _settings.SwordScanRadius);

            // Attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(p, _settings.AttackRange);

            // Threat range (enter/exit)
            Gizmos.color = new Color(1f, 0.5f, 0f, 1f);
            Gizmos.DrawWireSphere(p, _settings.ThreatEnterRange);

            Gizmos.color = new Color(1f, 0.5f, 0f, 0.45f);
            Gizmos.DrawWireSphere(p, _settings.ThreatExitRange);

            // Targets
            if (m_ClosestEnemy)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(p, m_ClosestEnemy.position);
                Gizmos.DrawSphere(m_ClosestEnemy.position, 0.12f);
            }

            if (m_ClosestSword)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(p, m_ClosestSword.position);
                Gizmos.DrawSphere(m_ClosestSword.position, 0.10f);
            }

            // Vectors
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(p, p + (Vector3)m_AvoidVector);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(p, p + (Vector3)m_FinalMoveInput);

            if (m_FinalMoveInput.sqrMagnitude > 0.0001f)
            {
                var dir = (Vector3)m_FinalMoveInput.normalized;
                var perp = Vector3.Cross(dir, Vector3.forward).normalized;

                var forwardLength = Mathf.Max(_settings.EnemyScanRadius, _settings.SwordScanRadius);
                var halfWidth = 0.35f;

                var start = p;
                var end = p + dir * forwardLength;

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(start + perp * halfWidth, end + perp * halfWidth);
                Gizmos.DrawLine(start - perp * halfWidth, end - perp * halfWidth);
            }
#if UNITY_EDITOR
            // State label above enemy
            var labelPos = p + Vector3.up * 1.2f;
            Handles.color = Color.white;
            Handles.Label(labelPos, m_State.ToString());
#endif
        }


        private void TickScanTimer()
        {
            m_ScanTimer += Time.deltaTime;
        }

        private bool ShouldScanNow()
        {
            return m_ScanTimer >= _settings.ScanInterval;
        }

        private void TickWanderTimer()
        {
            m_WanderTimer += Time.deltaTime;
            if (m_WanderTimer >= _settings.WanderChangeInterval)
            {
                m_WanderTimer = 0f;
                m_WanderVector = Random.insideUnitCircle.normalized * _settings.WanderStrength;
            }
        }


        private void ScanEnvironment()
        {
            Vector2 pos = transform.position;

            m_ClosestEnemy = ScanClosestByLayer(pos, _settings.EnemyScanRadius, _settings.EnemyLayer, m_EnemyHits,
                out var _);
            m_ClosestSword = ScanClosestByLayer(pos, _settings.SwordScanRadius, _settings.SwordLayer, m_SwordHits,
                out var _);

            m_AvoidVector = ComputeAvoidVector(pos, m_ClosestEnemy, m_State == AIState.Flee);
        }

        private Transform ScanClosestByLayer(
            Vector2 center,
            float radius,
            LayerMask layerMask,
            Collider2D[] buffer,
            out float closestDistSqr)
        {
            closestDistSqr = float.PositiveInfinity;

            var count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, layerMask);
            Transform closest = null;

            for (var i = 0; i < count; i++)
            {
                var c = buffer[i];
                if (!c) continue;

                if (IsSelfCollider(c))
                    continue;

                var t = c.transform;
                var dSqr = ((Vector2)t.position - center).sqrMagnitude;

                if (dSqr < closestDistSqr)
                {
                    closestDistSqr = dSqr;
                    closest = t;
                }
            }

            return closest;
        }

        private bool IsSelfCollider(Collider2D c)
        {
            if (!c) return false;

            if (c.transform == transform || c.transform.IsChildOf(transform))
                return true;

            if (m_SelfColliders == null) return false;

            for (var i = 0; i < m_SelfColliders.Length; i++)
                if (m_SelfColliders[i] == c)
                    return true;

            return false;
        }

        private Vector2 ComputeAvoidVector(Vector2 myPos, Transform enemy, bool isCurrentlyFleeing)
        {
            if (!enemy) return Vector2.zero;

            var dist = Vector2.Distance(myPos, enemy.position);

            if (isCurrentlyFleeing)
            {
                if (dist > _settings.ThreatExitRange) return Vector2.zero;
            }
            else
            {
                if (dist > _settings.ThreatEnterRange) return Vector2.zero;
            }

            var mySwords = GetMySwordCount();
            var enemySwords = GetEnemySwordCount(enemy);

            if (enemySwords <= mySwords)
                return Vector2.zero;

            var away = myPos - (Vector2)enemy.position;
            if (away.sqrMagnitude < 0.0001f) away = Random.insideUnitCircle;
            away = away.normalized;

            var strength = Mathf.InverseLerp(_settings.ThreatExitRange, _settings.ThreatEnterRange, dist);
            strength = Mathf.Clamp01(strength);

            if (isCurrentlyFleeing)
                strength = Mathf.Max(strength, _settings.MinFleeStrength);

            return away * strength;
        }


        private AIState DecideState()
        {
            Vector2 myPos = transform.position;

            bool hasEnemy = m_ClosestEnemy;
            bool hasSword = m_ClosestSword;

            var mySwords = GetMySwordCount();
            var enemySwords = hasEnemy ? GetEnemySwordCount(m_ClosestEnemy) : 0;

            var enemyDist = hasEnemy ? Vector2.Distance(myPos, m_ClosestEnemy.position) : float.PositiveInfinity;
            var enemyIsThreatClose = hasEnemy && (m_State == AIState.Flee
                ? enemyDist <= _settings.ThreatExitRange
                : enemyDist <= _settings.ThreatEnterRange);
            var enemyInAttackRange = hasEnemy && enemyDist <= _settings.AttackRange;

            // 1) Kaçınma (bariz tehdit)
            if (enemyIsThreatClose && enemySwords > mySwords)
                return AIState.Flee;

            // 2) Saldırı (kılıcı varsa ve rakipten fazlaysa + menzilde)
            if (enemyInAttackRange && mySwords > enemySwords && mySwords > 0)
                return AIState.AttackEnemy;

            // 3) Kılıç topla (kılıç yoksa veya güçlenmek istiyorsan)
            if (hasSword && (mySwords == 0 || mySwords <= enemySwords))
                return AIState.SeekSword;

            // 4) Rakibe yaklaş (avantajlıyım ama menzilde değilim)
            if (hasEnemy && mySwords > enemySwords && mySwords > 0)
                return AIState.ChaseEnemy;

            // 5) Eğer kılıç var ama düşman yoksa yine kılıca gidebilir
            if (hasSword && !hasEnemy)
                return AIState.SeekSword;

            // 6) Yoksa wander
            return AIState.Wander;
        }


        private Vector2 ComputeMoveInput(AIState state)
        {
            Vector2 myPos = transform.position;

            switch (state)
            {
                case AIState.Flee:
                    return ComposeSteering(Vector2.zero, m_AvoidVector);

                case AIState.AttackEnemy:
                    RequestAttack(m_ClosestEnemy);
                    return Vector2.zero;

                case AIState.SeekSword:
                    return ComposeSteering(GetDirTo(myPos, m_ClosestSword), m_AvoidVector);

                case AIState.ChaseEnemy:
                    return ComposeSteering(GetDirTo(myPos, m_ClosestEnemy), m_AvoidVector);

                case AIState.Wander:
                    return ComposeSteering(m_WanderVector, m_AvoidVector);

                default:
                    return Vector2.zero;
            }
        }

        private Vector2 ComposeSteering(Vector2 seekDir, Vector2 avoidDir)
        {
            var v = seekDir * _settings.SeekWeight + avoidDir * _settings.AvoidWeight;
            if (v.sqrMagnitude < 0.0001f) return Vector2.zero;
            return Vector2.ClampMagnitude(v, 1f);
        }

        private Vector2 GetDirTo(Vector2 from, Transform target)
        {
            if (!target) return Vector2.zero;
            var d = (Vector2)target.position - from;
            if (d.sqrMagnitude < 0.0001f) return Vector2.zero;
            return d.normalized;
        }

        private int GetMySwordCount()
        {
            return _swordOrbitController.GetSwordCount();
        }

        private int GetEnemySwordCount(Transform enemy)
        {
            var playerCollisionController = enemy.GetComponent<PlayerCollisionController>();
            var orbitController = playerCollisionController.GetSwordOrbitController();
            return orbitController.GetSwordCount();
        }

        private void RequestAttack(Transform enemy)
        {
            if (!enemy) return;

            // TODO: For attacking
        }
    }
}