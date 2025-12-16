using UnityEngine;
using UnityEngine.UI;

namespace _Game._Scripts
{
    public class PlayableEndCardController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _root;          
        [SerializeField] private Button _ctaButton;         
        [SerializeField] private Button _backgroundButton;
        [SerializeField] private Canvas _floatingJoystickCanvas;

        [Header("Settings")]
        [SerializeField] private bool _backgroundAlsoClicksCta = false;
        [SerializeField] private string _editorFallbackUrl = "";

        private bool m_IsShown;

        private void Awake()
        {
            if (_ctaButton)
                _ctaButton.onClick.AddListener(OnCtaClicked);

            if (_backgroundButton)
                _backgroundButton.onClick.AddListener(OnBackgroundClicked);

            HideImmediate();
        }

        public void Show()
        {
            if (m_IsShown) return;
            m_IsShown = true;

            _floatingJoystickCanvas.enabled = false;
            if (_root) _root.SetActive(true);

#if UNITY_WEBGL && !UNITY_EDITOR
            Luna.Unity.LifeCycle.GameEnded();
#endif
        }

        public void Hide()
        {
            m_IsShown = false;
            if (_root) _root.SetActive(false);
        }

        private void HideImmediate()
        {
            m_IsShown = false;
            if (_root) _root.SetActive(false);
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