using _Game._Scripts.SwordOrbitSystem;
using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "CommonOrbitSettingsSO",
        menuName = "Game/Swords/Orbit Settings",
        order = 0)]
    public class SwordOrbitSettingsSO : ScriptableObject
    {
        [Header("Start")]
        [SerializeField] private int _playerStartCount;
        [SerializeField] private int _aiStartCount;
        
        [Header("Orbit")]
        [SerializeField] private SwordOrbitController.OrbitSettings _orbit = SwordOrbitController.OrbitSettings.Default;

        [Header("Despawn")]
        [SerializeField] private SwordOrbitController.DespawnSettings _despawn = SwordOrbitController.DespawnSettings.Default;

        [Header("Pooling")]
        [SerializeField] private int _prewarmCount = 8;

        [Header("Test")]
        [SerializeField] private SwordOrbitController.TestSettings _test = SwordOrbitController.TestSettings.Default;

        [Header("Camera Shake")]
        [SerializeField] private float _shakeDuration = 0.3f;
        [SerializeField] private float _shakeStrength = 0.7f;


        public int PlayerStartCount => _playerStartCount;
        public int AiStartCount => _aiStartCount;
        public SwordOrbitController.OrbitSettings Orbit => _orbit;
        public SwordOrbitController.DespawnSettings Despawn => _despawn;
        public int PrewarmCount => _prewarmCount;
        public SwordOrbitController.TestSettings Test => _test;
        public float ShakeDuration => _shakeDuration;
        public float ShakeStrength => _shakeStrength;
    }
}