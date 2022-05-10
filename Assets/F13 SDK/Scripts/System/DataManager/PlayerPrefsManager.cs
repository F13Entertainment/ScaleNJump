using System;
using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class PlayerPrefsManager: OmegaSingletonManager<PlayerPrefsManager>
    {
        public PlayerData playerData;

        public PlayerPrefsManager()
        {
            playerData = new PlayerData();
        }

        public void AwakePlayerPrefsManager()
        {
            IntiliazePlayerPrefs();
        }

        public void IntiliazePlayerPrefs()
        {
            OmegaAudioManager.Instance.isAudioPlaying = playerData.IsAudioPlaying;
            OmegaAudioManager.Instance.audioMasterVolume = playerData.AudioMasterVolume;
            OmegaAudioManager.Instance.isMusicPlaying = playerData.IsMusicPlaying;
            OmegaAudioManager.Instance.musicMasterVolume = playerData.MusicMasterVolume;
            OmegaHapticManager.Instance.isActive = playerData.isHapticActive;


            OmegaDebugManager.Instance.PrintDebug("isAudioPlaying awake: " + playerData.IsAudioPlaying, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("audioMasterVolume awake: " + playerData.AudioMasterVolume, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("isMusicPlaying awake: " + playerData.IsMusicPlaying, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("musicMasterVolume awake: " + playerData.MusicMasterVolume, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("isHapticActive awake: " + playerData.isHapticActive, DebugType.HapticManager);

        }

        public void SaveAudioPlayerPrefs(bool isAudioPlaying, float volume)
        {
            playerData.IsAudioPlaying = isAudioPlaying;
            playerData.AudioMasterVolume = volume;
            OmegaDebugManager.Instance.PrintDebug("audioMasterVolume save: " + playerData.AudioMasterVolume, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("isAudioPlaying save: " + playerData.IsAudioPlaying, DebugType.AudioManager);
        }

        public void SaveMusicPlayerPrefs(bool isMusicPlaying, float volume)
        {
            playerData.IsMusicPlaying = isMusicPlaying;
            playerData.MusicMasterVolume = volume;
            OmegaDebugManager.Instance.PrintDebug("musicMasterVolume save: " + playerData.MusicMasterVolume, DebugType.AudioManager);
            OmegaDebugManager.Instance.PrintDebug("isMusicPlaying save: " + playerData.IsMusicPlaying, DebugType.AudioManager);
        }
        
        public void SaveHapticManagerPrefs(bool isActive)
        {
            playerData.isHapticActive = isActive;
            OmegaDebugManager.Instance.PrintDebug("isHapticActive save: " + playerData.isHapticActive, DebugType.HapticManager);
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}
