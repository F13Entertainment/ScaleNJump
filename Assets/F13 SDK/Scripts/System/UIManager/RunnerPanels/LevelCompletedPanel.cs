using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    class LevelCompletedPanel: RunnerPanel
    {
        private void On_LevelCompletedExited()
        {
            OmegaDebugManager.Instance.PrintDebug("Level completed panel is deactivated", DebugType.UI);
            HidePanel();
        }
        private void On_LevelCompletedEntered()
        {
            OmegaDebugManager.Instance.PrintDebug("Level completed panel is activated", DebugType.UI);
            ShowPanel();
        }
        public override void OmegaPanelAwake()
        {
            LevelSuccessState.Instance.LevelSuccessState_OnEntered += On_LevelCompletedEntered;
            LevelSuccessState.Instance.LevelSuccessState_OnExited += On_LevelCompletedExited;
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
