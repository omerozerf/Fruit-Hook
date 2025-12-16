using System;

namespace _Game._Scripts.Patterns.EventBusPattern
{
    public interface IEventBinding<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }
}