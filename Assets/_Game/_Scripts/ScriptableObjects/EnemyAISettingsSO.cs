using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyAISettingsSO", menuName = "Game/AI/Enemy AI Settings", order = 0)]
    public class EnemyAISettingsSO : ScriptableObject
    {
        [Header("Scan")] [SerializeField] private float _scanInterval;

        [SerializeField] private float _enemyScanRadius;
        [SerializeField] private float _swordScanRadius;
        [SerializeField] private int _maxScanHits;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private LayerMask _swordLayer;

        [Header("Combat")] [SerializeField] private float _attackRange;

        [SerializeField] private float _threatEnterRange;
        [SerializeField] private float _threatExitRange;
        [SerializeField] [Range(0f, 1f)] private float _minFleeStrength;

        [Header("Steering Weights")] [SerializeField]
        private float _seekWeight;

        [SerializeField] private float _avoidWeight;

        [Header("Wander")] [SerializeField] private float _wanderChangeInterval;

        [SerializeField] private float _wanderStrength;

        [Header("Debug / Gizmos")] [SerializeField]
        private bool _drawGizmos = true;

        public float ScanInterval => _scanInterval;
        public float EnemyScanRadius => _enemyScanRadius;
        public float SwordScanRadius => _swordScanRadius;
        public int MaxScanHits => _maxScanHits;
        public LayerMask EnemyLayer => _enemyLayer;
        public LayerMask SwordLayer => _swordLayer;

        public float AttackRange => _attackRange;
        public float ThreatEnterRange => _threatEnterRange;
        public float ThreatExitRange => _threatExitRange;
        public float MinFleeStrength => _minFleeStrength;

        public float SeekWeight => _seekWeight;
        public float AvoidWeight => _avoidWeight;

        public float WanderChangeInterval => _wanderChangeInterval;
        public float WanderStrength => _wanderStrength;

        public bool DrawGizmos => _drawGizmos;
    }
}