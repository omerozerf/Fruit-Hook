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

        private float _scanTimer;
        private float _wanderTimer;

        private Collider2D[] _enemyHits;
        private Collider2D[] _swordHits;

        private Transform _closestEnemy;
        private Transform _closestSword;

        private Vector2 _avoidVector;
        private Vector2 _wanderVector;
        private Vector2 _finalMoveInput;

        private AIState _state;

        private void Awake()
        {
            _enemyHits = new Collider2D[Mathf.Max(4, _maxScanHits)];
            _swordHits = new Collider2D[Mathf.Max(4, _maxScanHits)];
        }

        private void Update()
        {
            TickScanTimer();
            TickWanderTimer();

            // 1) Perception (tarama)
            if (ShouldScanNow())
            {
                ScanEnvironment();
                _scanTimer = 0f;
            }

            // 2) Decision (karar)
            _state = DecideState();

            // 3) Action (input üretimi)
            _finalMoveInput = ComputeMoveInput(_state);

            // 4) En sonda input bas (kural)
            if (_externalMoveInputSource)
                _externalMoveInputSource.PushInput(_finalMoveInput);
        }

        #region Timers

        private void TickScanTimer()
        {
            _scanTimer += Time.deltaTime;
        }

        private bool ShouldScanNow()
        {
            return _scanTimer >= _scanInterval;
        }

        private void TickWanderTimer()
        {
            _wanderTimer += Time.deltaTime;
            if (_wanderTimer >= _wanderChangeInterval)
            {
                _wanderTimer = 0f;
                _wanderVector = Random.insideUnitCircle.normalized * _wanderStrength;
            }
        }

        #endregion

        #region Perception

        private void ScanEnvironment()
        {
            Vector2 pos = transform.position;

            _closestEnemy = ScanClosestByLayer(pos, _enemyScanRadius, _enemyLayer, _enemyHits, out var _);
            _closestSword = ScanClosestByLayer(pos, _swordScanRadius, _swordLayer, _swordHits, out var _);

            _avoidVector = ComputeAvoidVector(pos, _closestEnemy);
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

            bool hasEnemy = _closestEnemy != null;
            bool hasSword = _closestSword != null;

            int mySwords = GetMySwordCount();
            int enemySwords = hasEnemy ? GetEnemySwordCount(_closestEnemy) : 0;

            bool enemyInAttackRange = hasEnemy && Vector2.Distance(myPos, _closestEnemy.position) <= _attackRange;
            bool enemyIsThreatClose = hasEnemy && Vector2.Distance(myPos, _closestEnemy.position) <= _threatRange;

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
                    return ComposeSteering(Vector2.zero, _avoidVector);

                case AIState.AttackEnemy:
                    // Saldırı anında durup attack tetikleyebilirsin (sen dolduracaksın)
                    RequestAttack(_closestEnemy);
                    return Vector2.zero;

                case AIState.SeekSword:
                    return ComposeSteering(GetDirTo(myPos, _closestSword), _avoidVector);

                case AIState.ChaseEnemy:
                    return ComposeSteering(GetDirTo(myPos, _closestEnemy), _avoidVector);

                case AIState.Wander:
                    return ComposeSteering(_wanderVector, _avoidVector);

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
            // TODO: Burayı sen doldur.
            // Örn: return _weaponHolder.SwordCount;
            return 0;
        }

        /// <summary>
        /// Rakibin kılıç sayısı. Rakip objesinde kendi sisteminden çek.
        /// </summary>
        private int GetEnemySwordCount(Transform enemy)
        {
            // TODO: Burayı sen doldur.
            // Örn: enemy.GetComponent<WeaponHolderHandler>().SwordCount;
            return 0;
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
            if (_closestEnemy)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(p, _closestEnemy.position);
                Gizmos.DrawSphere(_closestEnemy.position, 0.12f);
            }

            if (_closestSword)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(p, _closestSword.position);
                Gizmos.DrawSphere(_closestSword.position, 0.10f);
            }

            // Vectors
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(p, p + (Vector3)_avoidVector);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(p, p + (Vector3)_finalMoveInput);

            // Perpendicular (dikey) line at movement direction
            if (_finalMoveInput.sqrMagnitude > 0.0001f)
            {
                Vector3 start = p + (Vector3)_finalMoveInput.normalized * 0.8f;
                Vector3 perp = Vector3.Cross(_finalMoveInput.normalized, Vector3.forward).normalized;

                float perpLength = 0.4f;
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(start - perp * perpLength, start + perp * perpLength);
            }
        }

        #endregion
    }
}