
namespace Assets.F13SDK.Scripts
{
    class LevelFailedState : State
    {
        public OmegaEventManager.GameStateHandler LevelFailedState_OnEntered;
        public OmegaEventManager.GameStateHandler LevelFailedState_OnExited;
        public OmegaEventManager.GameStateHandler LevelFailedState_OnFixedExecuted;
        public OmegaEventManager.GameStateHandler LevelFailedState_OnExecuted;
        public OmegaEventManager.GameStateHandler LevelFailedState_OnLateExecuted;

        public static LevelFailedState Instance;
        public LevelFailedState()
        {
            if (Instance == null) Instance = this;

        }
        public override void Enter()
        {
            OmegaDebugManager.Instance.PrintDebug("Level Failed state Entered", DebugType.State);
            LevelFailedState_OnEntered?.Invoke();
        }
        public override void FixedExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("Level Failed fixed executed", DebugType.State);
            LevelFailedState_OnFixedExecuted?.Invoke();
        }
        public override void Execute()
        {
            LevelFailedState_OnExecuted?.Invoke();
        }
        public override void LateExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("Level Failed late executed", DebugType.State);
            LevelFailedState_OnLateExecuted?.Invoke();
        }
        public override void Exit()
        {
            OmegaDebugManager.Instance.PrintDebug("Level failed state Exited", DebugType.State);
            LevelFailedState_OnExited?.Invoke();
        }



    }
}
