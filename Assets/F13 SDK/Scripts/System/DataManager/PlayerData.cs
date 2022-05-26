using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class PlayerData
    {
        private bool _isAudioPlaying = true;
        private float _audioMasterVolume = 1f;
        private bool _isMusicPlaying = true;
        private bool _isTutorialCompleted = false;
        private float _musicMasterVolume = 1f;
        private bool _isHapticActive = true;
        private int _level = 1;
        private int _money = 0;


        #region Properties
        public bool IsAudioPlaying
        {
            get
            {
                if (!PlayerPrefs.HasKey("isAudioPlaying")) return _isAudioPlaying;
                return PlayerPrefs.GetInt("isAudioPlaying") == 1 ? true : false;
            }
            set
            {
                _isAudioPlaying = value;
                PlayerPrefs.SetInt("isAudioPlaying", _isAudioPlaying ? 1 : 0); 
            }
        }
        public float AudioMasterVolume
        {
            get
            {
                if (!PlayerPrefs.HasKey("audioMasterVolume")) return _audioMasterVolume;
                return PlayerPrefs.GetFloat("audioMasterVolume");
            }
            set
            {
                _audioMasterVolume = value;
                PlayerPrefs.SetFloat("audioMasterVolume", _audioMasterVolume);
            }
        }
        public bool IsMusicPlaying
        {
            get
            {
                if (!PlayerPrefs.HasKey("isMusicPlaying")) return _isMusicPlaying;
                return PlayerPrefs.GetInt("isMusicPlaying") == 1 ? true : false;
            }
            set
            {
                _isMusicPlaying = value;
                PlayerPrefs.SetInt("isMusicPlaying", _isMusicPlaying ? 1 : 0);
            }
        }

        public bool IsTutorialCompleted
        {
            get
            {
                if (!PlayerPrefs.HasKey("isTutorialCompleted")) return _isTutorialCompleted;
                return PlayerPrefs.GetInt("isTutorialCompleted") == 1 ? true : false;
            }
            set
            {
                _isMusicPlaying = value;
                PlayerPrefs.SetInt("isTutorialCompleted", _isTutorialCompleted ? 1 : 0);
            }
        }

        public float MusicMasterVolume
        {
            get
            {
                if (!PlayerPrefs.HasKey("musicMasterVolume")) return _musicMasterVolume;
                return PlayerPrefs.GetFloat("musicMasterVolume");
            }
            set
            {
                _musicMasterVolume = value;
                PlayerPrefs.SetFloat("musicMasterVolume", _musicMasterVolume);
            }
        }
        public bool isHapticActive
        {
            get
            {
                if (!PlayerPrefs.HasKey("isHapticActive")) return _isHapticActive;
                return PlayerPrefs.GetInt("isHapticActive") == 1 ? true : false;
            }
            set
            {
                _isHapticActive = value;
                PlayerPrefs.SetInt("isHapticActive", _isHapticActive ? 1: 0);
            }
        }
        public int Level
        {
            get
            {
                if (!PlayerPrefs.HasKey("level")) return _level;
                return PlayerPrefs.GetInt("level");
            }
            set
            {
                _level = value;
                PlayerPrefs.SetInt("level", _level);
            }
        }

        public int Money
        {
            get
            {
                if (!PlayerPrefs.HasKey("money")) return _money;
                return PlayerPrefs.GetInt("money");
            }
            set
            {
                _money = value;
                PlayerPrefs.SetInt("money", _money);
            }
        }
        #endregion
    }
}
