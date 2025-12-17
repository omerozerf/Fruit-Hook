using _Game._Scripts.Patterns.SingletonPattern;
using UnityEngine;

namespace _Game._Scripts.AudioSystem
{
    public class AudioService : Singleton<AudioService>
    {
        [Header("Settings")]
        [SerializeField] private AudioServiceSettingsSO _settings;

        [Header("Runtime Volumes")]
        [SerializeField, Range(0f, 1f)] private float _sfxVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float _musicVolume = 0.5f;

        private const string PREF_SFX = "audio_sfx_01";
        private const string PREF_MUSIC = "audio_music_01";
        
        private AudioSource m_MusicSource;
        private AudioSource m_SfxSource;
        private bool m_Unlocked;


        protected override void Awake()
        {
            base.Awake();
            if (!_settings)
            {
                Debug.LogError($"{nameof(AudioService)} on '{name}' has no {nameof(AudioServiceSettingsSO)} assigned.");
                enabled = false;
                return;
            }

            _sfxVolume = Mathf.Clamp01(_settings.DefaultSfxVolume);
            _musicVolume = Mathf.Clamp01(_settings.DefaultMusicVolume);

            CreateSources();
            LoadVolumes();
        }

        private void Start()
        {
            // WebGL / mobile: genelde ilk kullanıcı etkileşimine kadar ses başlamaz.
            // Bu yüzden müziği direkt başlatmak yerine unlock sonrası başlatıyoruz.
            if (!_settings.PlayMusicOnStart) return;
            TryStartMusic();
        }

        private void Update()
        {
            if (m_Unlocked) return;

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

            m_SfxSource.volume = _sfxVolume;

            if (_settings.RandomizePitch)
                m_SfxSource.pitch = Random.Range(
                    Mathf.Min(_settings.PitchMin, _settings.PitchMax),
                    Mathf.Max(_settings.PitchMin, _settings.PitchMax));
            else
                m_SfxSource.pitch = 1f;

            m_SfxSource.PlayOneShot(clip);
        }

        public void SetSfxVolume01(float v)
        {
            _sfxVolume = Mathf.Clamp01(v);
            PlayerPrefs.SetFloat(PREF_SFX, _sfxVolume);
            PlayerPrefs.Save();
        }

        public void SetMusicVolume01(float v)
        {
            _musicVolume = Mathf.Clamp01(v);
            if (m_MusicSource) m_MusicSource.volume = _musicVolume;

            PlayerPrefs.SetFloat(PREF_MUSIC, _musicVolume);
            PlayerPrefs.Save();
        }

        public void StopMusic()
        {
            if (m_MusicSource) m_MusicSource.Stop();
        }

        public void PlayMusic()
        {
            UnlockAudio();
            StartMusicInternal();
        }

        private void TryStartMusic()
        {
            if (!_settings.Music) return;
            if (!m_Unlocked) return;
            StartMusicInternal();
        }

        private void StartMusicInternal()
        {
            if (!_settings.Music) return;
            if (m_MusicSource.isPlaying && m_MusicSource.clip == _settings.Music) return;

            m_MusicSource.clip = _settings.Music;
            m_MusicSource.loop = true;
            m_MusicSource.volume = _musicVolume;
            m_MusicSource.pitch = 1f;
            m_MusicSource.Play();
        }

        private void UnlockAudio()
        {
            if (m_Unlocked) return;
            m_Unlocked = true;
        }

        private AudioClip GetClip(SfxId id)
        {
            switch (id)
            {
                case SfxId.BubbleSword: return _settings.BubbleSword;
                case SfxId.DamageHit: return _settings.DamageHit;
                case SfxId.SwordHit: return _settings.SwordHit;
                default: return null;
            }
        }

        private void CreateSources()
        {
            m_SfxSource = gameObject.AddComponent<AudioSource>();
            m_SfxSource.playOnAwake = false;
            m_SfxSource.loop = false;

            m_MusicSource = gameObject.AddComponent<AudioSource>();
            m_MusicSource.playOnAwake = false;
            m_MusicSource.loop = true;
        }

        private void LoadVolumes()
        {
            if (PlayerPrefs.HasKey(PREF_SFX)) _sfxVolume = PlayerPrefs.GetFloat(PREF_SFX);
            if (PlayerPrefs.HasKey(PREF_MUSIC)) _musicVolume = PlayerPrefs.GetFloat(PREF_MUSIC);

            if (m_SfxSource) m_SfxSource.volume = _sfxVolume;
            if (m_MusicSource) m_MusicSource.volume = _musicVolume;
        }
    }
}