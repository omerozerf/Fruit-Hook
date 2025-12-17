using _Game._Scripts.Patterns.EventBusPattern;
using UnityEngine;

namespace _Game._Scripts.GameEvents
{
    public struct SwordBubbleTaken : IEvent
    {
        public Transform transform;
    }
}