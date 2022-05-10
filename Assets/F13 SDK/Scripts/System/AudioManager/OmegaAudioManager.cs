using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

namespace Assets.F13SDK.Scripts
{ 
    public class OmegaAudioManager : OmegaSingletonManager<OmegaAudioManager>
    {
        [TabGroup("Audio Variables")]
        public AudioMixer audioMixer;
        [TabGroup("Audio Variables")]
        public bool isAudioPlaying = true;
        [TabGroup("Audio Variables")]
        public float audioMasterVolume = 1;
        [TabGroup("Music Variables")]
        public AudioMixer musicMixer;
        [TabGroup("Music Variables")]
        public bool isMusicPlaying = true;
        [TabGroup("Music Variables")]
        public float musicMasterVolume = 1;
        [InfoBox(
    "Sounds contains all sound components in the game. To use musics, choose Music Mixer instead of mixer option." + 
            "For the other sounds we have Main and SFX Mixer options.\nChoose and drag/drop your sounds." + 
            "\nDon't forget to add name of your sounds. ")]
        [Header("Components")]
        [BoxGroup("Music and Sound")]
        public List<Sound> sounds;


        public void AwakeOmegaAudioManager()
        {
            UIInputManager.On_SoundToggle += AudioToggle;
            UIInputManager.On_MusicToggle += MusicToggle;

            for (int i = 0; i < sounds.Count; i++)
            {
                GameObject soundObject = new GameObject("Sound" + i + "-" + sounds[i].clipName);
                soundObject.transform.SetParent(this.transform);
                sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            }

        }
        public void StartOmegaAudioManager()
        {
            StartPlayOnAwakeAudios();
            audioMixer.SetFloat("myMasterVol", audioMasterVolume);
            musicMixer.SetFloat("myMusicVol", musicMasterVolume);
        }
        private void MusicToggle()
        {
            if (isMusicPlaying) StopMusics();
            else PlayMusics();
        }
        private void AudioToggle()
        {
            if (isAudioPlaying) StopAudios();
            else PlayAudios();
        }
        public void PlayAudio(string audioName)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i].clipName == audioName)
                {
                    sounds[i].Play();
                    return;
                }
            }
        }
        public void StopAudio(string audioName)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i].clipName == audioName)
                {
                    sounds[i].Stop();
                    return;
                }
            }
        }
        public void MuteAudio(string audioName)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i].clipName == audioName)
                {
                    sounds[i].Mute();
                    return;
                }
            }
        }
        public void UnMuteAudio(string audioName)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i].clipName == audioName)
                {
                    sounds[i].UnMute();
                    return;
                }
            }
        }
        public void StartPlayOnAwakeAudios()
        {
            for (int i = 0; i < sounds.Count; i++)
                if (sounds[i].PlayOnAwake)
                    PlayAudio(sounds[i].clipName);
        }

        [Button(ButtonSizes.Large)]
        [FoldoutGroup("Audio Management")]
        [HorizontalGroup("Audio Management/Horizontal", Width = 100)]
        [GUIColor(0.62f, 0.85f, 0.87f)]
        public void PlayAudios()
        {
            audioMixer.SetFloat("myMasterVol", 1f);
            isAudioPlaying = true;
            PlayerPrefsManager.Instance.SaveAudioPlayerPrefs(isAudioPlaying, 1f);
        }
        [Button(ButtonSizes.Large)]
        [FoldoutGroup("Audio Management")]
        [HorizontalGroup("Audio Management/Horizontal", Width = 100)]
        [GUIColor(0.59f, 0.36f, 0.38f)]
        public void StopAudios()
        {
            audioMixer.SetFloat("myMasterVol", 0f);
            isAudioPlaying = false;
            PlayerPrefsManager.Instance.SaveAudioPlayerPrefs(isAudioPlaying, 0f);
        }
        [Button(ButtonSizes.Large)]
        [FoldoutGroup("Audio Management")]
        [HorizontalGroup("Audio Management/Horizontal", Width = 100)]
        [GUIColor(0.62f, 0.85f, 0.87f)]
        public void PlayMusics()
        {
            musicMixer.SetFloat("myMusicVol", 1f);
            isMusicPlaying = true;
            PlayerPrefsManager.Instance.SaveMusicPlayerPrefs(isMusicPlaying, 1f);
        }
        [Button(ButtonSizes.Large)]
        [FoldoutGroup("Audio Management")]
        [HorizontalGroup("Audio Management/Horizontal", Width = 100)]
        [GUIColor(0.59f, 0.36f, 0.38f)]
        public void StopMusics()
        {
            musicMixer.SetFloat("myMusicVol", 0f);
            isMusicPlaying = false;
            PlayerPrefsManager.Instance.SaveMusicPlayerPrefs(isMusicPlaying, 0f);
        }


    }
}

