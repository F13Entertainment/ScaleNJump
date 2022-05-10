using System;
using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class RetryState : State
    {
        public OmegaEventManager.GameStateHandler Retry_OnEntered;
        public OmegaEventManager.GameStateHandler Retry_OnExited;
        public OmegaEventManager.GameStateHandler Retry_OnFixedExecuted;
        public OmegaEventManager.GameStateHandler Retry_OnExecuted;
        public OmegaEventManager.GameStateHandler Retry_OnLateExecuted;

        public static RetryState Instance;

        public RetryState()
        {
            if (Instance == null) Instance = this;
        }

        public override void Enter()
        {
            OmegaDebugManager.Instance.PrintDebug("RetryState entered", DebugType.State);
            Retry_OnEntered?.Invoke();
            Time.timeScale = 0.0f;
        }
        public override void FixedExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("RetryState fixed executed", DebugType.State);
            Retry_OnFixedExecuted?.Invoke();
        }
        public override void Execute()
        {
            Retry_OnExecuted?.Invoke();
        }
        public override void LateExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("RetryState late executed", DebugType.State);
            Retry_OnLateExecuted?.Invoke();
        }
        public override void Exit()
        {
            OmegaDebugManager.Instance.PrintDebug("RetryState exited", DebugType.State);
            Retry_OnExited?.Invoke();
        }
    }
}
