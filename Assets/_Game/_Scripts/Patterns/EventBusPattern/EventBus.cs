using System.Collections.Generic;

namespace _Game._Scripts.Patterns.EventBusPattern
{
    public static class EventBus<T> where T : IEvent
    {
        private static readonly HashSet<IEventBinding<T>> BINDINGS = new();


        private static void Clear()
        {
            BINDINGS.Clear();
        }


        public static void Subscribe(EventBinding<T> binding)
        {
            BINDINGS.Add(binding);
        }

        public static void Unsubscribe(EventBinding<T> binding)
        {
            BINDINGS.Remove(binding);
        }

        public static void Publish(T eventToRaise)
        {
            foreach (var binding in BINDINGS)
            {
                binding.OnEvent.Invoke(eventToRaise);
                binding.OnEventNoArgs.Invoke();
            }
        }
    }
}