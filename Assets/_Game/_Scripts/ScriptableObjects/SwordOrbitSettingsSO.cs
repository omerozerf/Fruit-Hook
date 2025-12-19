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
        [SerializeField] private int _startCount;

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

        [Header("Scratch Erase")]
        [SerializeField] private bool _enableSwordScratchErase = true;
        [SerializeField, Range(0.01f, 1f)] private float _scratchPressure = 1f;
        [SerializeField, Min(0.001f)] private float _scratchWidth = 0.5f;
        [SerializeField, Min(0.01f)] private float _scratchLength = 2.5f;

        [Header("Orbit Center Erase")]
        [SerializeField] private bool _enableOrbitCenterErase;
        [SerializeField, Min(0.001f)] private float _orbitCenterEraseRadius = 0.5f;
        [SerializeField, Range(0.01f, 1f)] private float _orbitCenterErasePressure = 1f;


        public int StartCount => _startCount;
        public SwordOrbitController.OrbitSettings Orbit => _orbit;
        public SwordOrbitController.DespawnSettings Despawn => _despawn;
        public int PrewarmCount => _prewarmCount;
        public SwordOrbitController.TestSettings Test => _test;
        public float ShakeDuration => _shakeDuration;
        public float ShakeStrength => _shakeStrength;

        public bool EnableSwordScratchErase => _enableSwordScratchErase;
        public float ScratchPressure => _scratchPressure;
        public float ScratchWidth => _scratchWidth;
        public float ScratchLength => _scratchLength;

        public bool EnableOrbitCenterErase => _enableOrbitCenterErase;
        public float OrbitCenterEraseRadius => _orbitCenterEraseRadius;
        public float OrbitCenterErasePressure => _orbitCenterErasePressure;
    }
}