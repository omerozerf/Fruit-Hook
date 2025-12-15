using _Game._Scripts.PlayerSystem;
using _Game._Scripts.SwordOrbitSystem;
using UnityEngine;

namespace _Game._Scripts.EnemyMoveSystem
{
    /// <summary>
    /// Enemy AI: Tarama -> Karar -> Input üret -> PlayerMovement'e bas.
    /// Rakip taraması LayerMask ile yapılır.
    /// Sword bubble taraması LayerMask ile yapılır.
    /// </summary>
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

        [Header("References")]
        [SerializeField] private ExternalMoveInputSource _externalMoveInputSource;
        [SerializeField] private SwordOrbitController _swordOrbitController;

        [Header("Scan")]
        [SerializeField] private float _scanInterval = 0.12f;
        [SerializeField] private float _enemyScanRadius = 7f;
        [SerializeField] private float _swordScanRadius = 7f;
        [SerializeField] private int _maxScanHits = 32;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private LayerMask _swordLayer;

        [Header("Combat")]
        [SerializeField] private float _attackRange = 1.6f;
        [SerializeField] private float _threatRange = 2.4f;

        [Header("Steering Weights")]
        [SerializeField] private float _seekWeight = 1f;
        [SerializeField] private float _avoidWeight = 1.4f;

        [Header("Wander")]
        [SerializeField] private float _wanderChangeInterval = 1.2f;
        [SerializeField] private float _wanderStrength = 1f;

        [Header("Debug / Gizmos")]
        [SerializeField] private bool _drawGizmos = true;

        private float m_ScanTimer;
        private float m_WanderTimer;

        private Collider2D[] m_EnemyHits;
        private Collider2D[] m_SwordHits;

        private Transform m_ClosestEnemy;
        private Transform m_ClosestSword;

        private Vector2 m_AvoidVector;
        private Vector2 m_WanderVector;
        private Vector2 m_FinalMoveInput;

        private AIState m_State;

        
        private void Awake()
        {
            m_EnemyHits = new Collider2D[Mathf.Max(4, _maxScanHits)];
            m_SwordHits = new Collider2D[Mathf.Max(4, _maxScanHits)];
        }

        private void Update()
        {
            TickScanTimer();
            TickWanderTimer();

            // 1) Perception (tarama)
            if (ShouldScanNow())
            {
                ScanEnvironment();
                m_ScanTimer = 0f;
            }

            // 2) Decision (karar)
            m_State = DecideState();

            // 3) Action (input üretimi)
            m_FinalMoveInput = ComputeMoveInput(m_State);

            // 4) En sonda input bas (kural)
            if (_externalMoveInputSource)
                _externalMoveInputSource.PushInput(m_FinalMoveInput);
        }

        #region Timers

        private void TickScanTimer()
        {
            m_ScanTimer += Time.deltaTime;
        }

        private bool ShouldScanNow()
        {
            return m_ScanTimer >= _scanInterval;
        }

        private void TickWanderTimer()
        {
            m_WanderTimer += Time.deltaTime;
            if (m_WanderTimer >= _wanderChangeInterval)
            {
                m_WanderTimer = 0f;
                m_WanderVector = Random.insideUnitCircle.normalized * _wanderStrength;
            }
        }

        #endregion

        #region Perception

        private void ScanEnvironment()
        {
            Vector2 pos = transform.position;

            m_ClosestEnemy = ScanClosestByLayer(pos, _enemyScanRadius, _enemyLayer, m_EnemyHits, out var _);
            m_ClosestSword = ScanClosestByLayer(pos, _swordScanRadius, _swordLayer, m_SwordHits, out var _);

            m_AvoidVector = ComputeAvoidVector(pos, m_ClosestEnemy);
        }

        private Transform ScanClosestByLayer(
            Vector2 center,
            float radius,
            LayerMask layerMask,
            Collider2D[] buffer,
            out float closestDistSqr)
        {
            closestDistSqr = float.PositiveInfinity;

            int count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, layerMask);
            Transform closest = null;

            for (int i = 0; i < count; i++)
            {
                Collider2D c = buffer[i];
                if (!c) continue;

                Transform t = c.transform;
                float dSqr = ((Vector2)t.position - center).sqrMagnitude;

                if (dSqr < closestDistSqr)
                {
                    closestDistSqr = dSqr;
                    closest = t;
                }
            }

            return closest;
        }

        private Vector2 ComputeAvoidVector(Vector2 myPos, Transform enemy)
        {
            if (!enemy) return Vector2.zero;

            float dist = Vector2.Distance(myPos, enemy.position);
            if (dist > _threatRange) return Vector2.zero;

            int mySwords = GetMySwordCount();
            int enemySwords = GetEnemySwordCount(enemy);

            // Dezavantajlıysam ve çok yakınsa kaçınma vektörü üret
            if (enemySwords > mySwords)
            {
                Vector2 away = (myPos - (Vector2)enemy.position);
                if (away.sqrMagnitude < 0.0001f) away = Random.insideUnitCircle;
                return away.normalized * Mathf.InverseLerp(_threatRange, 0.2f, dist);
            }

            return Vector2.zero;
        }

        #endregion

        #region Decision

        private AIState DecideState()
        {
            Vector2 myPos = transform.position;

            bool hasEnemy = m_ClosestEnemy != null;
            bool hasSword = m_ClosestSword != null;

            int mySwords = GetMySwordCount();
            int enemySwords = hasEnemy ? GetEnemySwordCount(m_ClosestEnemy) : 0;

            bool enemyInAttackRange = hasEnemy && Vector2.Distance(myPos, m_ClosestEnemy.position) <= _attackRange;
            bool enemyIsThreatClose = hasEnemy && Vector2.Distance(myPos, m_ClosestEnemy.position) <= _threatRange;

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

        #endregion

        #region Action

        private Vector2 ComputeMoveInput(AIState state)
        {
            Vector2 myPos = transform.position;

            switch (state)
            {
                case AIState.Flee:
                    return ComposeSteering(Vector2.zero, m_AvoidVector);

                case AIState.AttackEnemy:
                    // Saldırı anında durup attack tetikleyebilirsin (sen dolduracaksın)
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
            Vector2 v = (seekDir * _seekWeight) + (avoidDir * _avoidWeight);
            if (v.sqrMagnitude < 0.0001f) return Vector2.zero;
            return Vector2.ClampMagnitude(v, 1f);
        }

        private Vector2 GetDirTo(Vector2 from, Transform target)
        {
            if (!target) return Vector2.zero;
            Vector2 d = (Vector2)target.position - from;
            if (d.sqrMagnitude < 0.0001f) return Vector2.zero;
            return d.normalized;
        }

        #endregion

        #region Fill-By-You (Sen dolduracaksın)

        /// <summary>
        /// Enemy'nin mevcut kılıç sayısı. (Kendi weapon/sword holder sisteminden çek)
        /// </summary>
        private int GetMySwordCount()
        {
            return _swordOrbitController.GetSwordCount();
        }

        /// <summary>
        /// Rakibin kılıç sayısı. Rakip objesinde kendi sisteminden çek.
        /// </summary>
        private int GetEnemySwordCount(Transform enemy)
        {
            var playerCollisionController = enemy.GetComponent<PlayerCollisionController>();
            var orbitController = playerCollisionController.GetSwordOrbitController();
            return orbitController.GetSwordCount();
        }

        /// <summary>
        /// Saldırı tetikleme. Anim/sinyal/damage sistemi burada bağlanacak.
        /// </summary>
        private void RequestAttack(Transform enemy)
        {
            if (!enemy) return;

            // TODO: Burayı sen doldur.
            // Örn: _attackSignal.Fire(enemy);
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            if (!_drawGizmos) return;

            Vector3 p = transform.position;

            // Scan radii
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(p, _enemyScanRadius);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(p, _swordScanRadius);

            // Attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(p, _attackRange);

            // Threat range
            Gizmos.color = new Color(1f, 0.5f, 0f, 1f);
            Gizmos.DrawWireSphere(p, _threatRange);

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

            // Perpendicular (dikey) line at movement direction
            if (m_FinalMoveInput.sqrMagnitude > 0.0001f)
            {
                Vector3 start = p + (Vector3)m_FinalMoveInput.normalized * 0.8f;
                Vector3 perp = Vector3.Cross(m_FinalMoveInput.normalized, Vector3.forward).normalized;

                float perpLength = 0.4f;
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(start - perp * perpLength, start + perp * perpLength);
            }
        }

        #endregion
    }
}