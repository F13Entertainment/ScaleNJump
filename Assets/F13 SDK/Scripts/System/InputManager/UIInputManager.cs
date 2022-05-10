using System;
using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    /// <summary>
    /// This class represents UI Inputs from player. 
    /// These inputs are implements here whatever they are. 
    /// </summary>
    /// 
    [RequireComponent(typeof(OmegaUIManager))]
    public class UIInputManager : OmegaInputManager
    {
        public static OmegaEventManager.GameInputHandler On_PlayButton;
        public static OmegaEventManager.GameInputHandler On_PauseButton;
        public static OmegaEventManager.GameInputHandler On_ResumeButton;
        public static OmegaEventManager.GameInputHandler On_HomeButton;
        public static OmegaEventManager.GameInputHandler On_RetryButton;
        public static OmegaEventManager.GameInputHandler On_NextLevelButton;
        public static OmegaEventManager.GameInputHandler On_PreLevelButton;
        public static OmegaEventManager.GameInputHandler On_ResetLevelButton;
        public static OmegaEventManager.GameInputHandler On_RestartLevelButton;
        public static OmegaEventManager.GameInputHandler On_MusicToggle;
        public static OmegaEventManager.GameInputHandler On_SoundToggle;
        public static OmegaEventManager.GameInputHandler On_HapticToggle;


        public void PauseButton()
        {
            OmegaDebugManager.Instance.PrintDebug("Pause button clicked", DebugType.Input);
            On_PauseButton?.Invoke();
        }
        public void PlayButton()
        {
            OmegaDebugManager.Instance.PrintDebug("Play button clicked", DebugType.Input);
            On_PlayButton?.Invoke();
        }
        public void HomeButton()
        {
            OmegaDebugManager.Instance.PrintDebug("Home button clicked", DebugType.Input);
            On_HomeButton?.Invoke();
        }
        public void RetryButton()
        {
            OmegaDebugManager.Instance.PrintDebug("Retry button clicked", DebugType.Input);
            On_RetryButton?.Invoke();
        }
        public void ResumeButton()
        {
            OmegaDebugManager.Instance.PrintDebug("Resume button clicked", DebugType.Input);
            Debug.Log("Resume button clicked");
            On_ResumeButton?.Invoke();
        }
        public void NextLevelButton()
        {
            OmegaDebugManager.Instance.PrintDebug("Next Level button clicked", DebugType.Input);
            On_NextLevelButton?.Invoke();
        }
        public void PreLevelButton()
        {
            OmegaDebugManager.Instance.PrintDebug("Pre Level button clicked", DebugType.Input);
            On_PreLevelButton?.Invoke();
        }
        public void ResetLevelButton()
        {
            OmegaDebugManager.Instance.PrintDebug("Reset Levels button clicked", DebugType.Input);
            On_ResetLevelButton?.Invoke();
        }
        public void RestartLevelButton()
        {
            OmegaDebugManager.Instance.PrintDebug("Restart Level button clicked", DebugType.Input);
            On_RestartLevelButton?.Invoke();
        }
        public void MusicToggle()
        {
            OmegaDebugManager.Instance.PrintDebug("Music Toggle button clicked", DebugType.Input);
            On_MusicToggle?.Invoke();
        }
        public void SoundToggle()
        {
            OmegaDebugManager.Instance.PrintDebug("Sound toggle button clicked", DebugType.Input);
            On_SoundToggle?.Invoke();
        }
        
        public void HapticToggle()
        {
            OmegaDebugManager.Instance.PrintDebug("Haptic toggle button clicked", DebugType.Input);
            On_HapticToggle?.Invoke();
        }
        
    }
}
