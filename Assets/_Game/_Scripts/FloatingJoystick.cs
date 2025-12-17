using System;
using _Game._Scripts.GameEvents;
using _Game._Scripts.Patterns.EventBusPattern;
using DG.Tweening;
using UnityEngine;

namespace _Game._Scripts
{
    public class FloatingJoystick : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;

        [Header("Settings")]
        [SerializeField] private float _radius = 80f;
        [SerializeField] private float _deadZone = 0.05f;
        [SerializeField] private bool _followTouch = true;

        [Header("Tutorial Hint")]
        [SerializeField] private bool _showHintOnStart = true;
        [SerializeField] private float _hintAmplitude = 45f;
        [SerializeField] private float _hintVerticalScale = 0.5f;
        [SerializeField] private float _hintLoopDuration = 1.2f;

        [Header("Optional")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _uiCamera;

        private int m_ActiveFingerId = -1;
        private Vector2 m_CenterLocalPos;
        private bool m_HintDismissed;
        private float m_HintT;
        private Tween m_HintTween;
        private Vector2 m_Input;
        private bool m_IsActive;
        private EventBinding<EndCardShowed> m_EndCardShowedEventBinding;

        public float Horizontal => m_Input.x;
        public float Vertical => m_Input.y;
        public Vector2 Direction => m_Input;


        private void Awake()
        {
            if (!_uiCamera && _canvas && _canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                _uiCamera = _canvas.worldCamera;

            SetVisualActive(false);
            ResetHandle();
        }

        private void OnEnable()
        {
            m_EndCardShowedEventBinding = new EventBinding<EndCardShowed>(HandleEndCardShowed);
            EventBus<EndCardShowed>.Subscribe(m_EndCardShowedEventBinding);
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
            
            _radius = Mathf.Max(1f, _radius);
            _deadZone = Mathf.Clamp01(_deadZone);

            _hintAmplitude = Mathf.Max(0f, _hintAmplitude);
            _hintVerticalScale = Mathf.Max(0f, _hintVerticalScale);
            _hintLoopDuration = Mathf.Max(0.01f, _hintLoopDuration);
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
            if (!_showHintOnStart)
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
                }, 1f, Mathf.Max(0.01f, _hintLoopDuration))
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(true);
        }

        private Vector2 GetInfinityHintPos(float t01)
        {
            var a = Mathf.Abs(_hintAmplitude);
            var b = a * Mathf.Max(0f, _hintVerticalScale);

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
            if (!parent)
            {
                m_Input = Vector2.zero;
                return;
            }

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parent,
                    screenPos,
                    _uiCamera,
                    out var currentLocalPos))
            {
                m_Input = Vector2.zero;
                return;
            }

            var delta = currentLocalPos - m_CenterLocalPos;

            var clamped = Vector2.ClampMagnitude(delta, _radius);
            _handle.anchoredPosition = clamped;

            var raw = clamped / Mathf.Max(1f, _radius);
            m_Input = raw.magnitude < _deadZone ? Vector2.zero : raw;
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

            if (_followTouch)
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