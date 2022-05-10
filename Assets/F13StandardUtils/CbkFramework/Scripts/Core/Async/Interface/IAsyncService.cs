using System;
using System.Collections;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Interface;
using UnityEngine;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Async.Interface
{
    public interface IAsyncService : IService
    {
        Coroutine WaitForSecond(float seconds, Action onComplete);
        void ExecuteCoroutine(IEnumerator action);
        void ExecuteInFixedUpdate(System.Action action);
        void ExecuteInUpdate(System.Action action);
        void ExecuteInLateUpdate(System.Action action);
	    
    }
}