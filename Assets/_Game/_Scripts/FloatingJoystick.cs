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

        [Header("Optional")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _uiCamera;

        private int m_ActiveFingerId = -1;
        private bool m_IsActive;
        private Vector2 m_Input;
        private Vector2 m_CenterLocalPos;

        public float Horizontal => m_Input.x;
        public float Vertical => m_Input.y;
        public Vector2 Direction => m_Input;

        
        private void Awake()
        {
            if (_canvas == null)
                _canvas = GetComponentInParent<Canvas>();

            if (_uiCamera == null && _canvas != null && _canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                _uiCamera = _canvas.worldCamera;

            SetVisualActive(false);
            ResetHandle();
        }

        private void Update()
        {
            UpdatePointerState();
            UpdateHandleAndInput();
        }

        private void UpdatePointerState()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_IsActive = true;
                m_ActiveFingerId = -1;
                BeginAtScreenPosition(Input.mousePosition);
            }
            else if (m_IsActive && Input.GetMouseButtonUp(0))
            {
                End();
            }
        }

        private void UpdateHandleAndInput()
        {
            if (!m_IsActive)
            {
                m_Input = Vector2.zero;
                return;
            }

            if (_background == null || _handle == null)
            {
                m_Input = Vector2.zero;
                return;
            }

            // We compute input as the delta between the current pointer position and the joystick center.
            // IMPORTANT: Do NOT move the background every frame. If we move it, delta becomes ~0 and input breaks.

            Vector2 screenPos = GetActiveScreenPosition();

            var parent = _background.parent as RectTransform;
            if (parent == null)
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

            // Convert delta into background-local space by using the same units as anchoredPosition.
            var clamped = Vector2.ClampMagnitude(delta, _radius);
            _handle.anchoredPosition = clamped;

            var raw = clamped / Mathf.Max(1f, _radius);
            m_Input = (raw.magnitude < _deadZone) ? Vector2.zero : raw;
        }

        private Vector2 GetActiveScreenPosition()
        {
            if (Input.touchCount > 0 && m_ActiveFingerId >= 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var t = Input.GetTouch(i);
                    if (t.fingerId == m_ActiveFingerId)
                        return t.position;
                }
            }

            return Input.mousePosition;
        }

        private void BeginAtScreenPosition(Vector2 screenPos)
        {
            if (_background == null)
                return;

            SetVisualActive(true);

            var parent = _background.parent as RectTransform;
            if (parent == null)
                return;

            // Determine center in parent-local space.
            // If followTouch is enabled, the joystick center becomes where the user first pressed.
            // If not enabled, we keep the background where it already is.
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

        private void MoveBackgroundToScreenPosition(Vector2 screenPos)
        {
            if (_background == null)
                return;

            var parent = _background.parent as RectTransform;
            if (parent == null)
                return;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parent,
                    screenPos,
                    _uiCamera,
                    out var localPos))
            {
                _background.anchoredPosition = localPos;
            }
        }

        private void ResetHandle()
        {
            if (_handle != null)
                _handle.anchoredPosition = Vector2.zero;
        }

        private void SetVisualActive(bool active)
        {
            if (_background != null)
                _background.gameObject.SetActive(active);

            if (_handle != null)
                _handle.gameObject.SetActive(active);
        }

        private void OnValidate()
        {
            _radius = Mathf.Max(1f, _radius);
            _deadZone = Mathf.Clamp01(_deadZone);
        }
    }
}