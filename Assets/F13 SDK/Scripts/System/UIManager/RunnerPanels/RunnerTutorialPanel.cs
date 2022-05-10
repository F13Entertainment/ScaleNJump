using UnityEngine;
using UnityEngine.Events;

namespace Assets.F13SDK.Scripts
{
    public class RunnerTutorialPanel: RunnerPanel
    {
        public UnityEvent On_MainMenuStart;
        public GameObject DragToMove, TapAndHold, Swipe, Swerve, TapToStart, Sling;
        private UI_INPUTTYPE _input;

        private void InputSwich()
        {
            switch (_input)
            {
                case UI_INPUTTYPE.DragToMove:
                    if (DragToMove)
                        DragToMove.SetActive(true);
                    else
                    {
                        OmegaDebugManager.Instance.PrintDebug("DragToMove Tutorial Panel is null", DebugType.UI);
                    }
                    break;
                case UI_INPUTTYPE.TapAndHold:
                    if (TapAndHold)
                        TapAndHold.SetActive(true);
                    else
                    {
                        OmegaDebugManager.Instance.PrintDebug("TapAndHold Tutorial Panel is null", DebugType.UI);
                    }
                    break;
                case UI_INPUTTYPE.Swipe:
                    if (Swipe)
                        Swipe.SetActive(true);
                    else
                    {
                        OmegaDebugManager.Instance.PrintDebug("Swipe Tutorial Panel is null", DebugType.UI);
                    }
                    break;
                case UI_INPUTTYPE.Swerve:
                    if (Swerve)
                        Swerve.SetActive(true);
                    else
                    {
                        OmegaDebugManager.Instance.PrintDebug("Swerve Tutorial Panel is null", DebugType.UI);
                    }
                    break;
                case UI_INPUTTYPE.TapToStart:
                    if (TapToStart)
                        TapToStart.SetActive(true);
                    else
                    {
                        OmegaDebugManager.Instance.PrintDebug("TapToStart Tutorial Panel is null", DebugType.UI);
                    }
                    break;
                case UI_INPUTTYPE.Sling:
                    if (Sling)
                        Sling.SetActive(true);
                    else
                    {
                        OmegaDebugManager.Instance.PrintDebug("Sling Tutorial Panel is null", DebugType.UI);
                    }
                    break;
            }
        }

        public override void OmegaPanelAwake()
        {
            MainMenuState.Instance.MainMenu_OnExecuted += OnMainMenuExecuted;
            _input = OmegaUIManager.Instance._input;
            InputSwich();
        }

        private void OnMainMenuExecuted()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (InputRaycastUtils.IsPointerOverUIElement())
                    return;
                else
                    On_MainMenuStart?.Invoke();
            }
        }
    }
}
