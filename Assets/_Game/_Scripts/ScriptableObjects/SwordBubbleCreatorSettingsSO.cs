using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "SwordBubbleCreatorSettings",
        menuName = "Game/Sword Bubble/Creator Settings",
        order = 0)]
    public class SwordBubbleCreatorSettingsSO : ScriptableObject
    {
        [Header("Pool")]
        [SerializeField] private int _prewarmCount = 8;

        [Header("Spawn Timing (seconds)")]
        [SerializeField] private float _baseSpawnInterval = 3f;
        [SerializeField] private float _intervalVariation = 1.5f; // final = base + Random(-variation, +variation)
        [SerializeField] private bool _spawnOnStart = true;

        [Header("Spawn Rules")]
        [SerializeField] private float _minDistanceFromPlayer = 1.5f;
        [SerializeField] private int _positionTryCount = 12;

        [Header("Enemy Died")]
        [SerializeField] private int _dropCount;
        [SerializeField] private float _dropRadius;

        
        public int PrewarmCount => _prewarmCount;
        public float BaseSpawnInterval => _baseSpawnInterval;
        public float IntervalVariation => _intervalVariation;
        public bool SpawnOnStart => _spawnOnStart;
        
        public float MinDistanceFromPlayer => _minDistanceFromPlayer;
        public int PositionTryCount => _positionTryCount;
        public int DropCount => _dropCount;
        public float DropRadius => _dropRadius;
    }
}