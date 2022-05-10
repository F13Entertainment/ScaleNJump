using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    class MainMenuState : State
    {
        public OmegaEventManager.GameStateHandler MainMenu_OnEntered;
        public OmegaEventManager.GameStateHandler MainMenu_OnExited;
        public OmegaEventManager.GameStateHandler MainMenu_OnFixedExecuted;
        public OmegaEventManager.GameStateHandler MainMenu_OnLateExecuted;
        public OmegaEventManager.GameStateHandler MainMenu_OnExecuted;

        public static MainMenuState Instance;
        public MainMenuState()
        {
            if (Instance == null) Instance = this;

        }
        public override void Enter()
        {
            OmegaDebugManager.Instance.PrintDebug("MainMenuState entered", DebugType.State);
            MainMenu_OnEntered?.Invoke();
            Time.timeScale = 1f;
        }
        public override void FixedExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("MainMenuState fixed executed", DebugType.State);
            MainMenu_OnFixedExecuted?.Invoke();
        }
        public override void Execute()
        {
            MainMenu_OnExecuted?.Invoke();
        }
        public override void LateExecute()
        {

            OmegaDebugManager.Instance.PrintDebug("MainMenuState late executed", DebugType.State);
            MainMenu_OnLateExecuted?.Invoke();
        }
        public override void Exit()
        {
            OmegaDebugManager.Instance.PrintDebug("MainMenuState exited", DebugType.State);
            MainMenu_OnExited?.Invoke();
        }
    }
}
