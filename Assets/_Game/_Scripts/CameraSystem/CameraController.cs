using _Game._Scripts.GameEvents;
using _Game._Scripts.Patterns.EventBusPattern;
using UnityEngine;

namespace _Game._Scripts.CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        private Camera m_Camera;
        private EventBinding<PlayerDiedEvent> m_PlayerDiedEventBinding;

        private int m_EnemyDeathCount;

        
        private void OnEnable()
        {
            m_PlayerDiedEventBinding = new EventBinding<PlayerDiedEvent>(HandlePlayerDied);
            EventBus<PlayerDiedEvent>.Subscribe(m_PlayerDiedEventBinding);
        }

        private void OnDisable()
        {
            EventBus<PlayerDiedEvent>.Unsubscribe(m_PlayerDiedEventBinding);
        }

        private void OnValidate()
        {
            if (!m_Camera) m_Camera = GetComponent<Camera>();
        }


        private void HandlePlayerDied(PlayerDiedEvent obj)
        {
            var isPlayer = obj.isPlayer;

            if (isPlayer)
                transform.SetParent(null, true);
            else
            {
                m_EnemyDeathCount++;
                if (m_EnemyDeathCount >= 3)
                    transform.SetParent(null, true);
            }
        }
    }
}