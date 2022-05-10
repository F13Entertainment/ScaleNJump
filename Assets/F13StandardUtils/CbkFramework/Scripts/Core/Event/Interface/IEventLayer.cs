using System;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface
{
    public interface IEventLayer
    {
        void Subscribe(string eventName, Action<IEvent> listener);
        void Unsubscribe(string eventName, Action<IEvent> listener);
        void Fire(string eventName, IEvent e = null);
    }
}