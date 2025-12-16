using _Game._Scripts.Patterns.EventBusPattern;

namespace _Game._Scripts.GameEvents
{
    public struct PlayerDiedEvent : IEvent
    {
        public bool isPlayer;
    }
}