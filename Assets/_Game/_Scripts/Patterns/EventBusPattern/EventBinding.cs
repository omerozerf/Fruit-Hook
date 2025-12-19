using System;

namespace _Game._Scripts.Patterns.EventBusPattern
{
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        private Action<T> m_OnEvent = _ => { };
        private Action m_OnEventNoArgs = () => { };


        public EventBinding(Action<T> onEvent)
        {
            m_OnEvent = onEvent;
        }

        public EventBinding(Action onEventNoArgs)
        {
            m_OnEventNoArgs = onEventNoArgs;
        }


        Action<T> IEventBinding<T>.OnEvent
        {
            get => m_OnEvent;
            set => m_OnEvent = value;
        }

        Action IEventBinding<T>.OnEventNoArgs
        {
            get => m_OnEventNoArgs;
            set => m_OnEventNoArgs = value;
        }


        public void Add(Action<T> onEvent)
        {
            m_OnEvent += onEvent;
        }

        public void Remove(Action<T> onEvent)
        {
            m_OnEvent -= onEvent;
        }

        public void Add(Action onEvent)
        {
            m_OnEventNoArgs += onEvent;
        }

        public void Remove(Action onEvent)
        {
            m_OnEventNoArgs -= onEvent;
        }
    }
}