using _Game._Scripts.Patterns.EventBusPattern;
using UnityEngine;

namespace _Game._Scripts.GameEvents
{
    public struct PlayerDiedEvent : IEvent
    {
        public bool isPlayer;
        public Transform transform;
        public Transform enemyTransform;
    }
}