using _Game._Scripts.PlayerSystem;
using UnityEngine;

namespace _Game._Scripts.EnemyMoveSystem
{
    public class ExternalMoveInputSource : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;

        private void Awake()
        {
            if (!_movement)
                _movement = GetComponent<PlayerMovement>();
        }

        private void OnValidate()
        {
            if (!_movement)
                _movement = GetComponent<PlayerMovement>();
        }

        public void PushInput(Vector2 input)
        {
            if (_movement)
                _movement.SetMoveInput(input);
        }

        public void Stop()
        {
            if (_movement)
                _movement.SetMoveInput(Vector2.zero);
        }
    }
}