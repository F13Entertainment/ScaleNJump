using System;
using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    class PausePanel: RunnerPanel
    {
        public override void OmegaPanelAwake()
        {
            PauseState.Instance.PauseState_OnEntered += On_PauseStateEntered;
            PauseState.Instance.PauseState_OnExited += On_PauseStateExited;

        }
        private void On_PauseStateExited()
        {
            HidePanel();
            OmegaDebugManager.Instance.PrintDebug("Pause panel is deactivated", DebugType.UI);
        }
        private void On_PauseStateEntered()
        {
            ShowPanel();
            OmegaDebugManager.Instance.PrintDebug("Pause panel ise activated", DebugType.UI);
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
