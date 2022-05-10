using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace F13StandardUtils.Scripts.Core
{
	public class AsyncManager : Singleton<AsyncManager>
	{
		private readonly List<System.Action> _actionQueuesUpdateFunc = new List<Action>();
		private readonly List<System.Action> _actionCopiedQueueUpdateFunc = new List<System.Action>();
		private volatile bool _noActionQueueToExecuteUpdateFunc = true;
		private readonly List<System.Action> _actionQueuesLateUpdateFunc = new List<Action>();
		private readonly List<System.Action> _actionCopiedQueueLateUpdateFunc = new List<System.Action>();
		private volatile bool _noActionQueueToExecuteLateUpdateFunc = true;
		private readonly List<System.Action> _actionQueuesFixedUpdateFunc = new List<Action>();
		private readonly List<System.Action> _actionCopiedQueueFixedUpdateFunc = new List<System.Action>();
		private volatile bool _noActionQueueToExecuteFixedUpdateFunc = true;
    
		public Coroutine WaitForSecond(float seconds, Action action)
		{			
			if (action == null)
			{
				throw new ArgumentNullException(nameof(AsyncManager)+"."+nameof(WaitForSecond)+"() "+nameof(action)+" is null");
			}
			return StartCoroutine(WaitForSecondCoroutine(seconds, action));
		}
    
		public void ExecuteCoroutine(IEnumerator action)
		{
			ExecuteInUpdate(() => StartCoroutine(action));

		}
	
		public void ExecuteInFixedUpdate(System.Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(AsyncManager)+"."+nameof(ExecuteInFixedUpdate)+"() "+nameof(action)+" is null");
			}

			lock (_actionQueuesFixedUpdateFunc)
			{
				_actionQueuesFixedUpdateFunc.Add(action);
				_noActionQueueToExecuteFixedUpdateFunc = false;
			}
		}
		public void ExecuteInUpdate(System.Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(AsyncManager)+"."+nameof(ExecuteInUpdate)+"() "+nameof(action)+" is null");
			}

			lock (_actionQueuesUpdateFunc)
			{
				_actionQueuesUpdateFunc.Add(action);
				_noActionQueueToExecuteUpdateFunc = false;
			}
		}
	
		public void ExecuteInLateUpdate(System.Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(AsyncManager)+"."+nameof(ExecuteInLateUpdate)+"() "+nameof(action)+" is null");
			}

			lock (_actionQueuesLateUpdateFunc)
			{
				_actionQueuesLateUpdateFunc.Add(action);
				_noActionQueueToExecuteLateUpdateFunc = false;
			}
		}
	
		private IEnumerator WaitForSecondCoroutine(float seconds, Action action)
		{
			yield return new WaitForSeconds(seconds);
			action.Invoke();
		}
	
		private void Update()
		{
			if (_noActionQueueToExecuteUpdateFunc)
			{
				return;
			}
		
			_actionCopiedQueueUpdateFunc.Clear();
			lock (_actionQueuesUpdateFunc)
			{
				_actionCopiedQueueUpdateFunc.AddRange(_actionQueuesUpdateFunc);
				_actionQueuesUpdateFunc.Clear();
				_noActionQueueToExecuteUpdateFunc = true;
			}
		
			for (int i = 0; i < _actionCopiedQueueUpdateFunc.Count; i++)
			{
				_actionCopiedQueueUpdateFunc[i].Invoke();
			}
		}
	
		private void LateUpdate()
		{
			if (_noActionQueueToExecuteLateUpdateFunc)
			{
				return;
			}
		
			_actionCopiedQueueLateUpdateFunc.Clear();
			lock (_actionQueuesLateUpdateFunc)
			{
				_actionCopiedQueueLateUpdateFunc.AddRange(_actionQueuesLateUpdateFunc);
				_actionQueuesLateUpdateFunc.Clear();
				_noActionQueueToExecuteLateUpdateFunc = true;
			}
		
			for (int i = 0; i < _actionCopiedQueueLateUpdateFunc.Count; i++)
			{
				_actionCopiedQueueLateUpdateFunc[i].Invoke();
			}
		}
	
		private void FixedUpdate()
		{
			if (_noActionQueueToExecuteFixedUpdateFunc)
			{
				return;
			}
		
			_actionCopiedQueueFixedUpdateFunc.Clear();
			lock (_actionQueuesFixedUpdateFunc)
			{
				_actionCopiedQueueFixedUpdateFunc.AddRange(_actionQueuesFixedUpdateFunc);
				_actionQueuesFixedUpdateFunc.Clear();
				_noActionQueueToExecuteFixedUpdateFunc = true;
			}
		
			for (int i = 0; i < _actionCopiedQueueFixedUpdateFunc.Count; i++)
			{
				_actionCopiedQueueFixedUpdateFunc[i].Invoke();
			}
		}


	}
}
