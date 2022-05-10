
namespace Assets.F13SDK.Scripts
{
    class LevelSuccessState : State
    {
        public OmegaEventManager.GameStateHandler LevelSuccessState_OnEntered;
        public OmegaEventManager.GameStateHandler LevelSuccessState_OnExited;
        public OmegaEventManager.GameStateHandler LevelSuccessState_OnFixedExecuted;
        public OmegaEventManager.GameStateHandler LevelSuccessState_OnExecuted;
        public OmegaEventManager.GameStateHandler LevelSuccessState_OnLateExecuted;

        public static LevelSuccessState Instance;
        public LevelSuccessState()
        {
            if (Instance == null) Instance = this;

        }
        public override void Enter()
        {
            OmegaDebugManager.Instance.PrintDebug("LevelSuccessState Entered", DebugType.State);
            LevelSuccessState_OnEntered?.Invoke();
        }
        public override void FixedExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("LevelSuccessState fixed executed", DebugType.State);
            LevelSuccessState_OnFixedExecuted?.Invoke();
        }
        public override void Execute()
        {
            LevelSuccessState_OnExecuted?.Invoke();
        }
        public override void LateExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("LevelSuccessState late executed", DebugType.State);
            LevelSuccessState_OnLateExecuted?.Invoke();
        }
        public override void Exit()
        {
            OmegaDebugManager.Instance.PrintDebug("LevelSuccessState exited", DebugType.State);
            LevelSuccessState_OnExited?.Invoke();
        }
    }
}
