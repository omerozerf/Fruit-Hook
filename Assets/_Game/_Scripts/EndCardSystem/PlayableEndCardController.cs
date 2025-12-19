using _Game._Scripts.GameEvents;
using _Game._Scripts.Patterns.EventBusPattern;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class PlayableEndCardController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _ctaButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Settings")]
        [SerializeField] private string _editorFallbackUrl = "";

        private int m_DeadAiCount;
        private bool m_IsShown;
        private EventBinding<PlayerDiedEvent> m_PlayerDiedEventBinding;


        private void Awake()
        {
            if (_ctaButton)
                _ctaButton.onClick.AddListener(OnCtaClicked);

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

        private void NotifyGameEnded()
        {
#if !UNITY_EDITOR
            // Required by some ad networks to properly mark the playable as finished.
            Luna.Unity.LifeCycle.GameEnded();
#endif
        }

        private void RequestInstall()
        {
#if UNITY_EDITOR
            // Required by some ad networks for CTA/install tracking.
            Luna.Unity.Playable.InstallFullGame();
#else
            // Outside a Playworks/Luna runtime, this API is not implemented; use a normal URL for testing.
            if (!string.IsNullOrWhiteSpace(_editorFallbackUrl))
            {
                Application.OpenURL(_editorFallbackUrl);
                return;
            }

            Debug.LogWarning(
                "CTA clicked, but Luna InstallFullGame is only implemented in the Playworks WebGL export." +
                "Provide _editorFallbackUrl for local testing.");
#endif
        }


        private void Show()
        {
            if (m_IsShown) return;
            m_IsShown = true;

            if (_canvas) _canvas.enabled = true;

            if (_canvasGroup)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;
                _canvasGroup.DOFade(1f, 0.3f);
            }

            NotifyGameEnded();
            EventBus<EndCardShowed>.Publish(new EndCardShowed());
        }

        private void HideImmediate()
        {
            m_IsShown = false;
            if (_canvasGroup)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;
            }

            if (_canvas) _canvas.enabled = false;
        }

        private void OnCtaClicked()
        {
            // Store click / CTA
            RequestInstall();
        }
    }
}