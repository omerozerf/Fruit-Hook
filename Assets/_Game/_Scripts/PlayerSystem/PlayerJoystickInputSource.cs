using System;
using _Game._Scripts.GameEvents;
using _Game._Scripts.Patterns.EventBusPattern;
using UnityEngine;

namespace _Game._Scripts.PlayerSystem
{
    public class PlayerJoystickInputSource : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private FloatingJoystick _floatingJoystick;

        [Header("Settings")]
        [SerializeField] private bool _isEnabled = true;

        private EventBinding<EndCardShowed> m_EndCardShowedEventBinding;
        
        private void OnEnable()
        {
            m_EndCardShowedEventBinding = new EventBinding<EndCardShowed>(HandleEndCardShowed);
            EventBus<EndCardShowed>.Subscribe(m_EndCardShowedEventBinding);
        }

        private void Update()
        {
            if (!_isEnabled)
                return;

            var input = ReadJoystick();
            if (_movement)
                _movement.SetMoveInput(input);
        }

        private void OnDisable()
        {
            EventBus<EndCardShowed>.Unsubscribe(m_EndCardShowedEventBinding);
        }

        private void OnValidate()
        {
            if (!_movement)
                _movement = GetComponent<PlayerMovement>();
        }
        
        
        private void HandleEndCardShowed()
        { 
            _isEnabled = false;
            _movement.SetMoveInput(Vector2.zero);
        }
        

        private Vector2 ReadJoystick()
        {
            if (!_floatingJoystick)
                return Vector2.zero;

            return new Vector2(
                _floatingJoystick.Horizontal,
                _floatingJoystick.Vertical
            );
        }

        public void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;

            if (!_isEnabled && _movement)
                _movement.SetMoveInput(Vector2.zero);
        }
    }
}