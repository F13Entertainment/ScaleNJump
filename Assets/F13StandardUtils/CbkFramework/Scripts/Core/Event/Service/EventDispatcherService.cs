using System;
using System.Collections.Generic;
using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Attribute;
using UnityEngine.Events;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Event.Service
{
    [Service]
    public class EventDispatcherService: IEventDispatcherService
    {
        [System.Serializable] private class GameEvent:UnityEvent<IEvent>{}

        private Dictionary<string, GameEvent> _eventGroups=new Dictionary<string, GameEvent>();
        public void Initialize()
        {
            
        }
        
        public void Subscribe(string eventName, Action<IEvent> listener)
        {
            if(!_eventGroups.ContainsKey(eventName))
            {
                _eventGroups.Add(eventName,new GameEvent());
            }
            _eventGroups[eventName].AddListener(listener.Invoke);

        }

        public void Unsubscribe(string eventName, Action<IEvent> listener)
        {
            if(_eventGroups.ContainsKey(eventName))
            {
                _eventGroups[eventName].RemoveListener(listener.Invoke);
            }
            else
            {
                throw new Exception(nameof(EventDispatcherService) +" : " + nameof(Unsubscribe)+" event error! There is no event group called like : "+eventName);
            }
        }

        public void Fire(string eventName, IEvent e = null)
        {
            if(_eventGroups.ContainsKey(eventName))
            {
                _eventGroups[eventName].Invoke(e);
            }
            else
            {
                throw new Exception(nameof(EventDispatcherService) + " : " + nameof(Fire) +" event error! There is no event group called like : "+eventName);
            }
        }
        
    }
}