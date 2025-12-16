using _Game._Scripts.Patterns;
using _Game._Scripts.Patterns.SingletonPattern;
using UnityEngine;

namespace _Game._Scripts.AudioSystem
{
    public sealed class AudioService : Singleton<AudioService>
    {
        public enum SfxId
        {
            BubbleSword = 0,
            DamageHit = 1,
            SwordHit = 2
        }

        [Header("Clips (3 SFX)")]
        [SerializeField] private AudioClip _bubbleSword;
        [SerializeField] private AudioClip _damageHit;
        [SerializeField] private AudioClip _swordHit;

        [Header("Music (Optional)")]
        [SerializeField] private AudioClip _music;
        [SerializeField] private bool _playMusicOnStart = true;

        [Header("Volumes")]
        [SerializeField, Range(0f, 1f)] private float _sfxVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float _musicVolume = 0.5f;

        [Header("Random Pitch (Optional)")]
        [SerializeField] private bool _randomizePitch = true;
        [SerializeField, Range(0.8f, 1.2f)] private float _pitchMin = 0.95f;
        [SerializeField, Range(0.8f, 1.2f)] private float _pitchMax = 1.05f;

        private const string PrefSfx = "audio_sfx_01";
        private const string PrefMusic = "audio_music_01";

        private AudioSource _sfxSource;
        private AudioSource _musicSource;

        private bool _unlocked;

        private void Awake()
        {
            CreateSources();
            LoadVolumes();
        }

        private void Start()
        {
            // WebGL / mobile: genelde ilk kullanıcı etkileşimine kadar ses başlamaz.
            // Bu yüzden müziği direkt başlatmak yerine unlock sonrası başlatıyoruz.
            if (!_playMusicOnStart) return;
            TryStartMusic();
        }

        private void Update()
        {
            if (_unlocked) return;

            if (Input.GetMouseButtonDown(0))
            {
                UnlockAudio();
                TryStartMusic();
            }
        }

        public void PlaySfx(SfxId id)
        {
            UnlockAudio();

            var clip = GetClip(id);
            if (!clip) return;

            _sfxSource.volume = _sfxVolume;

            if (_randomizePitch)
                _sfxSource.pitch = Random.Range(Mathf.Min(_pitchMin, _pitchMax), Mathf.Max(_pitchMin, _pitchMax));
            else
                _sfxSource.pitch = 1f;

            _sfxSource.PlayOneShot(clip);
        }

        public void SetSfxVolume01(float v)
        {
            _sfxVolume = Mathf.Clamp01(v);
            PlayerPrefs.SetFloat(PrefSfx, _sfxVolume);
            PlayerPrefs.Save();
        }

        public void SetMusicVolume01(float v)
        {
            _musicVolume = Mathf.Clamp01(v);
            if (_musicSource) _musicSource.volume = _musicVolume;

            PlayerPrefs.SetFloat(PrefMusic, _musicVolume);
            PlayerPrefs.Save();
        }

        public void StopMusic()
        {
            if (_musicSource) _musicSource.Stop();
        }

        public void PlayMusic()
        {
            UnlockAudio();
            StartMusicInternal();
        }

        private void TryStartMusic()
        {
            if (!_music) return;
            if (!_unlocked) return;
            StartMusicInternal();
        }

        private void StartMusicInternal()
        {
            if (!_music) return;
            if (_musicSource.isPlaying && _musicSource.clip == _music) return;

            _musicSource.clip = _music;
            _musicSource.loop = true;
            _musicSource.volume = _musicVolume;
            _musicSource.pitch = 1f;
            _musicSource.Play();
        }

        private void UnlockAudio()
        {
            if (_unlocked) return;
            _unlocked = true;
        }

        private AudioClip GetClip(SfxId id)
        {
            switch (id)
            {
                case SfxId.BubbleSword: return _bubbleSword;
                case SfxId.DamageHit: return _damageHit;
                case SfxId.SwordHit: return _swordHit;
                default: return null;
            }
        }

        private void CreateSources()
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.playOnAwake = false;
            _sfxSource.loop = false;
            _sfxSource.spatialBlend = 0f;

            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.playOnAwake = false;
            _musicSource.loop = true;
            _musicSource.spatialBlend = 0f;
        }

        private void LoadVolumes()
        {
            if (PlayerPrefs.HasKey(PrefSfx)) _sfxVolume = PlayerPrefs.GetFloat(PrefSfx);
            if (PlayerPrefs.HasKey(PrefMusic)) _musicVolume = PlayerPrefs.GetFloat(PrefMusic);

            if (_musicSource) _musicSource.volume = _musicVolume;
        }
    }
}