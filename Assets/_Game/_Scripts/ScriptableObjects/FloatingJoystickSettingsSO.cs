using UnityEngine;

namespace _Game._Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "FloatingJoystickSettingsSO",
        menuName = "Game/Input/Floating Joystick Settings",
        order = 0)]
    public class FloatingJoystickSettingsSO : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private float _radius = 80f;
        [SerializeField, Range(0f, 1f)] private float _deadZone = 0.05f;
        [SerializeField] private bool _followTouch = true;

        [Header("Tutorial Hint")]
        [SerializeField] private bool _showHintOnStart = true;
        [SerializeField] private float _hintAmplitude = 45f;
        [SerializeField] private float _hintVerticalScale = 0.5f;
        [SerializeField] private float _hintLoopDuration = 1.2f;

        public float Radius => _radius;
        public float DeadZone => _deadZone;
        public bool FollowTouch => _followTouch;

        public bool ShowHintOnStart => _showHintOnStart;
        public float HintAmplitude => _hintAmplitude;
        public float HintVerticalScale => _hintVerticalScale;
        public float HintLoopDuration => _hintLoopDuration;

        private void OnValidate()
        {
            _radius = Mathf.Max(1f, _radius);
            _deadZone = Mathf.Clamp01(_deadZone);

            _hintAmplitude = Mathf.Max(0f, _hintAmplitude);
            _hintVerticalScale = Mathf.Max(0f, _hintVerticalScale);
            _hintLoopDuration = Mathf.Max(0.01f, _hintLoopDuration);
        }
    }
}