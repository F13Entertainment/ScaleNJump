using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class GamePlayPanel: RunnerPanel
    {
        public AdminPanel adminPanel;
        private void On_GameplayExited()
        {
            OmegaDebugManager.Instance.PrintDebug("Gameplay panel is deactivated", DebugType.UI);
            HidePanel();
        }
        private void On_GameplayEntered()
        {
            OmegaDebugManager.Instance.PrintDebug("Gameplay panel is activated", DebugType.UI);
            ShowPanel();
        }
        public override void OmegaPanelAwake()
        {
            GamePlayState.Instance.Gameplay_OnEntered += On_GameplayEntered;
            GamePlayState.Instance.Gameplay_OnExited += On_GameplayExited;
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
