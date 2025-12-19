using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "EnemyCreateSettings",
        menuName = "Game/Enemy/Enemy Create Settings",
        order = 0)]
    public class EnemyCreateSettingsSO : ScriptableObject
    {
        [Header("Spawn Randomness")]
        [SerializeField, Min(1)] private int _candidateSampleCount = 300;
        [SerializeField, Min(1)] private int _topCandidatesToChooseFrom = 10;

        [Header("Constraints")]
        [SerializeField, Min(0f)] private float _minEnemyDistance = 3f;

        [Header("Optional Determinism")]
        [SerializeField] private bool _useFixedSeed;
        [SerializeField] private int _fixedSeed = 12345;

        public int CandidateSampleCount => _candidateSampleCount;
        public int TopCandidatesToChooseFrom => _topCandidatesToChooseFrom;
        public float MinEnemyDistance => _minEnemyDistance;

        public bool UseFixedSeed => _useFixedSeed;
        public int FixedSeed => _fixedSeed;

        
        private void OnValidate()
        {
            _candidateSampleCount = Mathf.Max(1, _candidateSampleCount);
            _topCandidatesToChooseFrom = Mathf.Max(1, _topCandidatesToChooseFrom);
            _minEnemyDistance = Mathf.Max(0f, _minEnemyDistance);
        }
    }
}