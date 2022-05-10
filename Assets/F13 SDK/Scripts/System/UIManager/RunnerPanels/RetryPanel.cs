
namespace Assets.F13SDK.Scripts
{
    public class RetryPanel: RunnerPanel
    {
        public override void OmegaPanelAwake()
        {
            RetryState.Instance.Retry_OnEntered += On_RetryStateEntered;
            RetryState.Instance.Retry_OnExited += On_RetryStateExited;
        }

        private void On_RetryStateExited()
        {
            HidePanel();
            OmegaDebugManager.Instance.PrintDebug("Retry panel is deactivated", DebugType.UI);
        }

        private void On_RetryStateEntered()
        {
            ShowPanel();
            OmegaDebugManager.Instance.PrintDebug("Retry panel is activated", DebugType.UI);
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
