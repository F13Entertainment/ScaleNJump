using System;
using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Event
{
    public abstract class MonoEventLayer : MonoServiceLayer, IEventLayer
    {
        private IEventDispatcherService _eventDispatcher;
        
        private IEventDispatcherService GetEventService()
        {
            return _eventDispatcher ??= GetService<IEventDispatcherService>();
        }

        public void Subscribe(string eventName, Action<IEvent> listener)
        {
            GetEventService().Subscribe(eventName,listener);
        }

        public void Unsubscribe(string eventName, Action<IEvent> listener)
        {
            GetEventService().Unsubscribe(eventName,listener);

        }

        public void Fire(string eventName, IEvent e = null)
        {
            GetEventService().Fire(eventName,e);

        }
    }
}