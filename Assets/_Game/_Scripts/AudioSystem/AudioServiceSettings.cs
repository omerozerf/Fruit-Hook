using UnityEngine;

namespace _Game._Scripts.AudioSystem
{
    [CreateAssetMenu(
        fileName = "AudioServiceSettingsSO",
        menuName = "Game/Audio/Audio Service Settings",
        order = 0)]
    public class AudioServiceSettingsSO : ScriptableObject
    {
        [Header("Clips (3 SFX)")] [SerializeField]
        private AudioClip _bubbleSword;

        [SerializeField] private AudioClip _damageHit;
        [SerializeField] private AudioClip _swordHit;

        [Header("Music (Optional)")] [SerializeField]
        private AudioClip _music;

        [SerializeField] private bool _playMusicOnStart = true;

        [Header("Default Volumes")] [SerializeField] [Range(0f, 1f)]
        private float _defaultSfxVolume = 1f;

        [SerializeField] [Range(0f, 1f)] private float _defaultMusicVolume = 0.5f;

        [Header("Random Pitch (Optional)")] [SerializeField]
        private bool _randomizePitch = true;

        [SerializeField] [Range(0.8f, 1.2f)] private float _pitchMin = 0.95f;
        [SerializeField] [Range(0.8f, 1.2f)] private float _pitchMax = 1.05f;

        public AudioClip BubbleSword => _bubbleSword;
        public AudioClip DamageHit => _damageHit;
        public AudioClip SwordHit => _swordHit;

        public AudioClip Music => _music;
        public bool PlayMusicOnStart => _playMusicOnStart;

        public float DefaultSfxVolume => _defaultSfxVolume;
        public float DefaultMusicVolume => _defaultMusicVolume;

        public bool RandomizePitch => _randomizePitch;
        public float PitchMin => _pitchMin;
        public float PitchMax => _pitchMax;
    }
}