using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    class PauseState : State
    {
        public OmegaEventManager.GameStateHandler PauseState_OnEntered;
        public OmegaEventManager.GameStateHandler PauseState_OnExited;
        public OmegaEventManager.GameStateHandler PauseState_OnFixedExecuted;
        public OmegaEventManager.GameStateHandler PauseState_OnExecuted;
        public OmegaEventManager.GameStateHandler PauseState_OnLateExecuted;

        public static PauseState Instance;
        public PauseState()
        {
            if (Instance == null) Instance = this;

        }
        public override void Enter()
        {
            OmegaDebugManager.Instance.PrintDebug("PauseState entered", DebugType.State);
            PauseState_OnEntered?.Invoke();
            Time.timeScale = 0.0f;
        }
        public override void FixedExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("PauseState fixed executed", DebugType.State);
            PauseState_OnFixedExecuted?.Invoke();
        }
        public override void Execute()
        {
            PauseState_OnExecuted?.Invoke();
        }
        public override void LateExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("PauseState late executed", DebugType.State);
            PauseState_OnLateExecuted?.Invoke();
        }
        public override void Exit()
        {
            OmegaDebugManager.Instance.PrintDebug("PauseState exited", DebugType.State);
            PauseState_OnExited?.Invoke();
        }
    }
}
