using System;
using _Game._Scripts.GameEvents;
using _Game._Scripts.Patterns.EventBusPattern;
using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class PlayableEndCardController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;          
        [SerializeField] private Button _ctaButton;         
        [SerializeField] private Button _backgroundButton;
        [SerializeField] private Canvas _floatingJoystickCanvas;

        [Header("Settings")]
        [SerializeField] private bool _backgroundAlsoClicksCta = false;
        [SerializeField] private string _editorFallbackUrl = "";

        private bool m_IsShown;
        private EventBinding<PlayerDiedEvent> m_PlayerDiedEventBinding;
        private int m_DeadAiCount;

        private void Awake()
        {
            if (_ctaButton)
                _ctaButton.onClick.AddListener(OnCtaClicked);

            if (_backgroundButton)
                _backgroundButton.onClick.AddListener(OnBackgroundClicked);

            HideImmediate();
        }

        private void OnEnable()
        {
            m_PlayerDiedEventBinding = new EventBinding<PlayerDiedEvent>(HandlePlayerDied);
            EventBus<PlayerDiedEvent>.Subscribe(m_PlayerDiedEventBinding);
        }

        private void OnDisable()
        {
            EventBus<PlayerDiedEvent>.Unsubscribe(m_PlayerDiedEventBinding);
        }

        private void HandlePlayerDied(PlayerDiedEvent playerDiedEvent)
        {
            var isPlayer = playerDiedEvent.isPlayer;

            switch (isPlayer)
            {
                case true:
                {
                    Show();
                    break;
                }
                case false:
                {
                    m_DeadAiCount++;
                    if (m_DeadAiCount >= 3)
                        Show();
                    break;
                }
            }
        }

        public void Show()
        {
            if (m_IsShown) return;
            m_IsShown = true;

            _floatingJoystickCanvas.enabled = false;
            if (_canvas) _canvas.enabled = true;

#if UNITY_WEBGL && !UNITY_EDITOR
            Luna.Unity.LifeCycle.GameEnded();
#endif
        }

        public void Hide()
        {
            m_IsShown = false;
            if (_canvas) _canvas.enabled = false;
        }

        private void HideImmediate()
        {
            m_IsShown = false;
            if (_canvas) _canvas.enabled = false;
        }

        public void OnCtaClicked()
        {
            // Store click / CTA
#if UNITY_WEBGL && !UNITY_EDITOR
            Luna.Unity.Playable.InstallFullGame();
#else
            // Outside of a Playworks/Luna runtime this API is not implemented; use a normal URL for testing.
            if (!string.IsNullOrWhiteSpace(_editorFallbackUrl))
            {
                Application.OpenURL(_editorFallbackUrl);
                return;
            }

            Debug.LogWarning("CTA clicked, but Luna InstallFullGame is only implemented in the Playworks WebGL export. Provide _editorFallbackUrl for local testing.");
#endif
        }

        private void OnBackgroundClicked()
        {
            if (!_backgroundAlsoClicksCta) return;
            OnCtaClicked();
        }
    }
}