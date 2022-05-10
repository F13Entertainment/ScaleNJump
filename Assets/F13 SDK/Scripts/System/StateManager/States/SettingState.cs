using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    class SettingState : State
    {
        public OmegaEventManager.GameStateHandler SettingsState_OnEntered;
        public OmegaEventManager.GameStateHandler SettingsState_OnExited;

        public static SettingState Instance;
        public SettingState()
        {
            if (Instance == null) Instance = this;

        }
        public override void Enter()
        {
            OmegaDebugManager.Instance.PrintDebug("SettingsState entered", DebugType.State);
            SettingsState_OnEntered?.Invoke();
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            OmegaDebugManager.Instance.PrintDebug("SettingState exited", DebugType.State);
            SettingsState_OnExited?.Invoke();
        }

        public override void FixedExecute()
        {
           
        }

        public override void LateExecute()
        {
            
        }
    }
}
