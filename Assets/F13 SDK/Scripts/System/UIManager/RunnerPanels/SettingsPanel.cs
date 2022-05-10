using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Assets.F13SDK.Scripts
{
    public class SettingsPanel: RunnerPanel
    {
        public GameObject AudioImageOn, AudioImageOff, MusicImageOn, MusicImageOff, HapticImageOn, HapticImageOff;
        public override void OmegaPanelAwake()
        {
            UIInputManager.On_SoundToggle += SoundToggleChanged;
            UIInputManager.On_MusicToggle += MusicToggleChanged;
            UIInputManager.On_HapticToggle += HapticToggleChanged;
            GetPlayerPrefSettings();
        }
        private void HapticToggleChanged()
        {
            if (OmegaHapticManager.Instance.isActive)
            {
                HapticImageOff.SetActive(true);
                HapticImageOn.SetActive(false);
            }
            else
            {
                HapticImageOff.SetActive(false);
                HapticImageOn.SetActive(true);
            }
        }

        private void MusicToggleChanged()
        {
            if (!MusicImageOff || !MusicImageOn) return;
            if (OmegaAudioManager.Instance.isMusicPlaying)
            {
                MusicImageOff.SetActive(true);
                MusicImageOn.SetActive(false);
            }
            else
            {
                MusicImageOff.SetActive(false);
                MusicImageOn.SetActive(true);
            }
        }

        private void SoundToggleChanged()
        {
            if (OmegaAudioManager.Instance.isAudioPlaying)
            {
                AudioImageOn.SetActive(false);
                AudioImageOff.SetActive(true);
            }
            else
            {
                AudioImageOn.SetActive(true);
                AudioImageOff.SetActive(false);
            }
        }
        public override void HidePanel()
        {
            OmegaDebugManager.Instance.PrintDebug("Settings panel is deactivated", DebugType.UI);
            this.gameObject.SetActive(false);
        }
        public override void ShowPanel()
        {
            OmegaDebugManager.Instance.PrintDebug("SettingsPanel panel is activated", DebugType.UI);
            this.gameObject.SetActive(true);
        }


        private void GetPlayerPrefSettings()
        {
            if (!OmegaAudioManager.Instance.isAudioPlaying && PlayerPrefsManager.Instance.HasKey("isAudioPlaying"))
            {
                AudioImageOn.SetActive(false);
                AudioImageOff.SetActive(true);
            }
            else
            {
                AudioImageOn.SetActive(true);
                AudioImageOff.SetActive(false);
            }
            if (!OmegaHapticManager.Instance.isActive && PlayerPrefsManager.Instance.HasKey("isHapticActive"))
            {
                HapticImageOff.SetActive(true);
                HapticImageOn.SetActive(false);
            }
            else
            {
                HapticImageOff.SetActive(false);
                HapticImageOn.SetActive(true);
            }

            if (!OmegaAudioManager.Instance.isMusicPlaying && PlayerPrefs.HasKey("isMusicPlaying"))
            {
                MusicImageOff.SetActive(true);
                MusicImageOn.SetActive(false);
            }
            else
            {
                MusicImageOff.SetActive(false);
                MusicImageOn.SetActive(true);
            }
        }

    }
}
