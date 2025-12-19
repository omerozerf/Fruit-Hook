using _Game._Scripts.GameEvents;
using _Game._Scripts.Patterns.EventBusPattern;
using _Game._Scripts.ScriptableObjects;
using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts.JoystickSystem
{
    public class FloatingJoystick : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;

        [Header("Settings")]
        [SerializeField] private FloatingJoystickSettingsSO _settings;

        [Header("Optional")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _uiCamera;

        private int m_ActiveFingerId = -1;
        private Vector2 m_CenterLocalPos;
        private EventBinding<EndCardShowed> m_EndCardShowedEventBinding;
        private bool m_HintDismissed;
        private float m_HintT;
        private Tween m_HintTween;
        private Vector2 m_Input;
        private bool m_IsActive;

        public float Horizontal => m_Input.x;
        public float Vertical => m_Input.y;
        public Vector2 Direction => m_Input;


        private void Awake()
        {
            if (!_settings)
            {
                Debug.LogError(
                    $"{nameof(FloatingJoystick)} on '{name}' has no {nameof(FloatingJoystickSettingsSO)} assigned.");
                enabled = false;
                return;
            }

            if (!_uiCamera && _canvas && _canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                _uiCamera = _canvas.worldCamera;

            SetVisualActive(false);
            ResetHandle();
        }

        private void Start()
        {
            TryStartHint();
        }

        private void Update()
        {
            UpdatePointerState();
            UpdateHandleAndInput();
        }

        private void OnEnable()
        {
            m_EndCardShowedEventBinding = new EventBinding<EndCardShowed>(HandleEndCardShowed);
            EventBus<EndCardShowed>.Subscribe(m_EndCardShowedEventBinding);
        }

        private void OnDisable()
        {
            KillHintTween();
            EventBus<EndCardShowed>.Unsubscribe(m_EndCardShowedEventBinding);
        }

        private void OnDestroy()
        {
            KillHintTween();
        }

        private void OnValidate()
        {
            if (!_canvas)
                _canvas = GetComponentInParent<Canvas>();
        }


        private void HandleEndCardShowed()
        {
            _canvas.enabled = false;
        }


        private void UpdatePointerState()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DismissHintIfNeeded();

                m_IsActive = true;
                m_ActiveFingerId = -1;
                BeginAtScreenPosition(Input.mousePosition);
            }
            else if (m_IsActive && m_ActiveFingerId < 0 && Input.GetMouseButtonUp(0))
            {
                End();
            }
        }

        private void TryStartHint()
        {
            if (!_settings.ShowHintOnStart)
                return;

            if (m_HintDismissed)
                return;

            if (!_background || !_handle)
                return;

            SetVisualActive(true);
            m_IsActive = false;
            m_ActiveFingerId = -1;
            m_Input = Vector2.zero;
            m_CenterLocalPos = _background.anchoredPosition;

            ResetHandle();
            KillHintTween();

            m_HintT = 0f;
            _handle.anchoredPosition = GetInfinityHintPos(0f);

            m_HintTween = DOTween
                .To(() => m_HintT, x =>
                {
                    m_HintT = x;
                    if (_handle)
                        _handle.anchoredPosition = GetInfinityHintPos(m_HintT);
                }, 1f, Mathf.Max(0.01f, _settings.HintLoopDuration))
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(true);
        }

        private Vector2 GetInfinityHintPos(float t01)
        {
            var a = Mathf.Abs(_settings.HintAmplitude);
            var b = a * Mathf.Max(0f, _settings.HintVerticalScale);

            var theta = t01 * Mathf.PI * 2f;
            var x = a * Mathf.Sin(theta);
            var y = b * Mathf.Sin(theta) * Mathf.Cos(theta);

            return new Vector2(x, y);
        }

        private void DismissHintIfNeeded()
        {
            if (m_HintDismissed)
                return;

            m_HintDismissed = true;
            KillHintTween();

            ResetHandle();
        }

        private void KillHintTween()
        {
            if (m_HintTween != null && m_HintTween.IsActive()) m_HintTween.Kill();

            m_HintTween = null;
        }

        private void UpdateHandleAndInput()
        {
            if (!m_IsActive)
            {
                m_Input = Vector2.zero;
                return;
            }

            if (!_background || !_handle)
            {
                m_Input = Vector2.zero;
                return;
            }

            var screenPos = GetActiveScreenPosition();

            var parent = _background.parent as RectTransform;
            if (!parent || !RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parent,
                    screenPos,
                    _uiCamera,
                    out var currentLocalPos))
            {
                m_Input = Vector2.zero;
                return;
            }

            var delta = currentLocalPos - m_CenterLocalPos;

            var clamped = Vector2.ClampMagnitude(delta, _settings.Radius);
            _handle.anchoredPosition = clamped;

            var raw = clamped / Mathf.Max(1f, _settings.Radius);
            m_Input = raw.magnitude < _settings.DeadZone ? Vector2.zero : raw;
        }

        private Vector2 GetActiveScreenPosition()
        {
            if (Input.touchCount > 0 && m_ActiveFingerId >= 0)
                for (var i = 0; i < Input.touchCount; i++)
                {
                    var t = Input.GetTouch(i);
                    if (t.fingerId == m_ActiveFingerId)
                        return t.position;
                }

            return Input.mousePosition;
        }

        private void BeginAtScreenPosition(Vector2 screenPos)
        {
            if (!_background)
                return;

            SetVisualActive(true);

            var parent = _background.parent as RectTransform;
            if (!parent)
                return;

            if (_settings.FollowTouch)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        parent,
                        screenPos,
                        _uiCamera,
                        out var localPos))
                {
                    _background.anchoredPosition = localPos;
                    m_CenterLocalPos = localPos;
                }
                else
                {
                    m_CenterLocalPos = _background.anchoredPosition;
                }
            }
            else
            {
                m_CenterLocalPos = _background.anchoredPosition;
            }

            ResetHandle();
            m_Input = Vector2.zero;
        }

        private void End()
        {
            m_IsActive = false;
            m_ActiveFingerId = -1;

            m_Input = Vector2.zero;
            ResetHandle();
            SetVisualActive(false);
        }

        private void ResetHandle()
        {
            if (_handle)
                _handle.anchoredPosition = Vector2.zero;
        }

        private void SetVisualActive(bool active)
        {
            if (_background)
                _background.gameObject.SetActive(active);

            if (_handle)
                _handle.gameObject.SetActive(active);
        }
    }
}