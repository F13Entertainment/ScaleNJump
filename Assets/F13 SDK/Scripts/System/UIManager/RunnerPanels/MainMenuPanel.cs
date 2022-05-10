using UnityEngine;
using UnityEngine.UI;

namespace Assets.F13SDK.Scripts
{
    public class MainMenuPanel : RunnerPanel
    {
        public Text dinamicLevelText;
        [SerializeField]
        public SettingsPanel settingsPanel;
        [SerializeField]
        private RunnerTutorialPanel tutorialPanel;
        private void On_MainMenuExited()
        {
            OmegaDebugManager.Instance.PrintDebug("Main Menu panel is deactivated", DebugType.UI);
            HidePanel();
        }
        private void On_MainMenuEntered()
        {
            OmegaDebugManager.Instance.PrintDebug("Main Menu panel is activated", DebugType.UI);
            //dinamicLevelText.text = PlayerPrefs.GetInt("level").ToString();
            ShowPanel();
        }
        public override void OmegaPanelAwake()
        {
            MainMenuState.Instance.MainMenu_OnEntered += On_MainMenuEntered;
            MainMenuState.Instance.MainMenu_OnExited += On_MainMenuExited;
            settingsPanel.OmegaPanelAwake();
            tutorialPanel.OmegaPanelAwake();
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
