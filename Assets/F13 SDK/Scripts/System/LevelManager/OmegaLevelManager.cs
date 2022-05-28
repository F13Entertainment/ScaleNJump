using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using ElephantSDK;

namespace Assets.F13SDK.Scripts
{ 
        public enum LevelType
        {
            Normal, Bonus, Tutorial
        }

        public class OmegaLevelManager : OmegaSingletonManager<OmegaLevelManager>
        { 
        private OmegaLevelManager() { }
        [BoxGroup("Current Level Info")]
        public Level currentLevel;
        [BoxGroup("Current Level Info")]
        [PreviewField]
        public GameObject currentLevelObject;
        [InfoBox(
"Levels are contains of your all levels. Add their levelIds and prefabs to work with them.\n Levels have 3 different options. Choose the level type. ")]
        public List<Level> levels;
        public static OmegaEventManager.GameLevelHandler On_BeforeLevelInitialized;
        public static OmegaEventManager.GameLevelHandler On_LevelInitialized;
        public static OmegaEventManager.GameLevelHandler On_BeforeLevelDestroy;
        public static OmegaEventManager.GameLevelHandler On_LevelDestroy;
        public static OmegaEventManager.GameLevelHandler On_LevelCompleted;
        public static OmegaEventManager.GameLevelHandler On_LevelStarted;
        public static OmegaEventManager.GameLevelHandler On_LevelFailed;

        public void AwakeOmegaLevelManager()
        {
            UIInputManager.On_NextLevelButton += SetNextLevel;
            UIInputManager.On_PreLevelButton += SetPreLevel;
            UIInputManager.On_ResetLevelButton += ResetLevels;
            UIInputManager.On_RestartLevelButton += RestartLevel;
            if (PlayerPrefs.GetInt("level") == 0) PlayerPrefs.SetInt("level", 1);
            InitiliazeLevel(PlayerPrefs.GetInt("level"));
            currentLevel = levels[PlayerPrefs.GetInt("level")-1];
        }

        [Button(ButtonSizes.Large)]
        [FoldoutGroup("Level Management")]
        [HorizontalGroup("Level Management/Horizontal", Width = 100)]
        [GUIColor(0.95f, 0.9f, 0.65f)]
        private void SetPreLevel()
        {
            if (currentLevel.levelId == 1)
            {
                DestroyGameObject(currentLevelObject);
                On_LevelStarted?.Invoke();
                currentLevel = FindLevel(levels[levels.Count-1].levelId);
                InitiliazeLevel(currentLevel.levelId);
            }
            else
            {
                DestroyGameObject(currentLevelObject);
                On_LevelStarted?.Invoke();
                currentLevel = FindLevel(currentLevel.levelId - 1);
                InitiliazeLevel(currentLevel.levelId);
            }
        }
        [Button(ButtonSizes.Large)]
        [FoldoutGroup("Level Management")]
        [HorizontalGroup("Level Management/Horizontal", Width = 100)]
        [GUIColor(1f, 0.71f, 0.65f)]
        public void RestartLevel()
        {
            DestroyGameObject(currentLevelObject);
            On_LevelStarted?.Invoke();
            InitiliazeLevel(currentLevel.levelId);
        }

        [Button(ButtonSizes.Large)]
        [FoldoutGroup("Level Management")]
        [HorizontalGroup("Level Management/Horizontal", Width = 100)]
        [GUIColor(0.58f, 0.83f, 0.87f)]
        public void SetNextLevel()
        {
            if (currentLevel.levelId == levels.Count)
            {
                DestroyGameObject(currentLevelObject);
                On_LevelStarted?.Invoke();
                currentLevel = FindLevel(1);
                InitiliazeLevel(1);
            }
            else
            {
                DestroyGameObject(currentLevelObject);
                On_LevelStarted?.Invoke();
                currentLevel = FindLevel(currentLevel.levelId + 1);
                InitiliazeLevel(currentLevel.levelId);
            }
        }

        private Level FindLevel(int levelId)
        {
            return levels.ElementAt(levelId-1);
        }

        [Button(ButtonSizes.Large)]
        [FoldoutGroup("Level Management")]
        [HorizontalGroup("Level Management/Horizontal2", Width = 305)]
        [GUIColor(0.72f, 0.71f, 0.71f)]
        public void ResetLevels()
        {
            DestroyGameObject(currentLevelObject);
            currentLevel = levels[0];
            On_LevelStarted?.Invoke();
            InitiliazeLevel(currentLevel.levelId);
        }

        public void Fail()
        {
            if (!currentLevel.IsLevelFailed)
            {
                currentLevel.IsLevelFailed = true;
                On_LevelFailed?.Invoke();
                Elephant.LevelFailed(currentLevel.levelId);
                Debug.Log("ELEPHANT Level failed :" + currentLevel.levelId);
            }

        }
        public void Success()
        {
            if (!currentLevel.IsLevelCompleted)
            {
                currentLevel.IsLevelCompleted = true;
                On_LevelCompleted?.Invoke();
                Elephant.LevelCompleted(currentLevel.levelId);
                Debug.Log("ELEPHANT Level completed :" + currentLevel.levelId);
            }

        }
        private void DestroyGameObject(GameObject gameObject)
        {
            On_BeforeLevelDestroy.Invoke();
            Destroy(gameObject);
            On_LevelDestroy.Invoke();
        }

        private void InitiliazeLevel(int levelId)
        {
            FindLevel(levelId).IsLevelCompleted = false;
            FindLevel(levelId).IsLevelFailed = false;
            On_BeforeLevelInitialized.Invoke();
            currentLevelObject = Instantiate(FindLevel(levelId).levelPrefab);
            PlayerPrefs.SetInt("level", levelId);
            currentLevelObject.SetActive(true);
            On_LevelInitialized.Invoke();
            Elephant.LevelStarted(levelId);
            Debug.Log("ELEPHANT Level start :" + levelId);
        }

    }
}
