using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    class LevelFailedPanel: RunnerPanel
    {
        private void On_LevelFailedExited()
        {
            OmegaDebugManager.Instance.PrintDebug("Level failed panel is deactivated", DebugType.UI);
            HidePanel();
        }
        private void On_LevelFailedEntered()
        {
            OmegaDebugManager.Instance.PrintDebug("Level failed panel is activated", DebugType.UI);
            ShowPanel();
        }
        public override void OmegaPanelAwake()
        {
            LevelFailedState.Instance.LevelFailedState_OnEntered += On_LevelFailedEntered;
            LevelFailedState.Instance.LevelFailedState_OnExited += On_LevelFailedExited;
        }
        public override void HidePanel()
        {
            this.gameObject.SetActive(false);
        }
        public override void ShowPanel()
        {
            this.gameObject.SetActive(true);
        }
    }
}
