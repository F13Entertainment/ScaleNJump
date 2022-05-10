using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class AdminPanel: RunnerPanel
    {
        public bool isActive;
        public override void HidePanel()
        {
            OmegaDebugManager.Instance.PrintDebug("Admin panel is deactivated", DebugType.UI);
            this.gameObject.SetActive(false);
        }
        public override void ShowPanel()
        {
            OmegaDebugManager.Instance.PrintDebug("Admin panel is activated", DebugType.UI);
            this.gameObject.SetActive(true);
        }
    }
}
