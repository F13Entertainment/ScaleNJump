using Assets.F13SDK.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace F13StandardUtils.Scripts.Core
{
    public class RemoteConfigParams
    {
        public float moveZSpeed;
    }
    [System.Serializable] public class LevelEvent:UnityEvent<int>{}
    public class GameController : Singleton<GameController>
    {
        [SerializeField,ReadOnly] private int level;
        [SerializeField,ReadOnly] private bool isLevelInit = false;
        private bool isGamePlayStateInvoked;
        public int Level => level;
        public bool IsLevelInit => isLevelInit;

        public LevelEvent OnBeforeLevelInit=new LevelEvent();
        public LevelEvent OnLevelInit=new LevelEvent();
        public LevelEvent OnBeforeLevelDestroy=new LevelEvent();
        public LevelEvent OnLevelDestroy=new LevelEvent();
        public LevelEvent OnLevelSuccess=new LevelEvent();
        public LevelEvent OnLevelFail=new LevelEvent();
        public UnityEvent OnGameplayEnter = new UnityEvent();
        public UnityEvent OnGameplayExit = new UnityEvent();
        public UnityEvent OnGameplayUpdate = new UnityEvent();
        public UnityEvent OnGameplayFixedUpdate = new UnityEvent();
        public UnityEvent OnGameplayLateUpdate = new UnityEvent();
        public PlayerData PlayerData => PlayerPrefsManager.Instance.playerData;

        [SerializeField] private bool _enableRemoteConfig = false;
        [ReadOnly,ShowIf(nameof(_enableRemoteConfig))] 
        private RemoteConfigParams _remoteConfigParams = new RemoteConfigParams();


        private void OnEnable()
        {
            OmegaLevelManager.On_BeforeLevelInitialized += On_BeforeLevelInitialized;
            OmegaLevelManager.On_LevelInitialized += On_LevelInitialized;
            OmegaLevelManager.On_BeforeLevelDestroy += On_BeforeLevelDestroy;
            OmegaLevelManager.On_LevelDestroy += On_LevelDestroy;
            GamePlayState.Instance.Gameplay_OnEntered += Gameplay_OnEntered;
            GamePlayState.Instance.Gameplay_OnExited += Gameplay_OnExited;
            GamePlayState.Instance.Gameplay_OnExecuted += Gameplay_OnExecuted;
            GamePlayState.Instance.Gameplay_OnFixedExecuted += Gameplay_OnFixedExecuted;
            GamePlayState.Instance.Gameplay_OnLateExecuted += Gameplay_OnLateExecuted;
            
        }
        
        private void OnDisable()
        {
            OmegaLevelManager.On_BeforeLevelInitialized -= On_BeforeLevelInitialized;
            OmegaLevelManager.On_LevelInitialized -= On_LevelInitialized;
            OmegaLevelManager.On_BeforeLevelDestroy -= On_BeforeLevelDestroy;
            OmegaLevelManager.On_LevelDestroy -= On_LevelDestroy;
            GamePlayState.Instance.Gameplay_OnEntered -= Gameplay_OnEntered;
            GamePlayState.Instance.Gameplay_OnExited -= Gameplay_OnExited;
            GamePlayState.Instance.Gameplay_OnExecuted -= Gameplay_OnExecuted;
            GamePlayState.Instance.Gameplay_OnFixedExecuted -= Gameplay_OnFixedExecuted;
            GamePlayState.Instance.Gameplay_OnLateExecuted -= Gameplay_OnLateExecuted;
        }
        
        private void On_BeforeLevelInitialized()
        {
            OnBeforeLevelInitialized();
        }
        
        private void On_LevelInitialized()
        {
            isLevelInit = true;
            isGamePlayStateInvoked = false;
            level = OmegaLevelManager.Instance.currentLevel.levelId;
            OnLevelInitialized();
        }
        
        private void On_BeforeLevelDestroy()
        {
            OnBeforeLevelDestroyed();
        }



        private void On_LevelDestroy()
        {
            isLevelInit = false;
            OnLevelDestroyed();
        }
        
        private void Gameplay_OnEntered()
        {
            if (!isGamePlayStateInvoked)
            {
                Invoke(nameof(OnGamePlayEnter),0.01f);
                isGamePlayStateInvoked = true;

            }
        }
        
        private void Gameplay_OnExited()
        {
            Invoke(nameof(OnGamePlayExit),0.01f);
        }
        
        private void Gameplay_OnExecuted()
        {
            OnUpdate();
        }
        
        private void Gameplay_OnFixedExecuted()
        {
            OnFixedUpdate();
        }

        private void Gameplay_OnLateExecuted()
        {
            OnLateUpdate();
        }
        
        private void OnBeforeLevelInitialized()
        {
            OnBeforeLevelInit.Invoke(level);
            //TODO
        }
        
        private void OnLevelInitialized()
        {
            OnLevelInit.Invoke(level);
            //TODO
        }
        
        private void OnBeforeLevelDestroyed()
        {
            OnBeforeLevelDestroy.Invoke(level);
            //TODO
        }

        private void OnLevelDestroyed()
        {
            OnLevelDestroy.Invoke(level);
            //TODO
        }
        
        private void OnUpdate()
        {
            OnGameplayUpdate.Invoke();
            //TODO
            UpdateRemoteConfigParams();
        }



        private void OnFixedUpdate()
        {
            OnGameplayFixedUpdate.Invoke();
            //TODO
        }

        private void OnLateUpdate()
        {
            OnGameplayLateUpdate.Invoke();
            //TODO
        }
        
        private void OnGamePlayEnter()
        {
            OnGameplayEnter.Invoke();
            //TODO
        }

        private void OnGamePlayExit()
        {
            OnGameplayExit.Invoke();
            //TODO
        }
        
        [Button]
        public void SuccessLevel()
        {
            OmegaLevelManager.Instance.Success();
            OnLevelSuccess.Invoke(level);
        }

        [Button]
        public void FailLevel()
        {
            OmegaLevelManager.Instance.Fail();
            OnLevelFail.Invoke(level);
        }
        
        private void UpdateRemoteConfigParams()
        {
            if(!_enableRemoteConfig) return ;
            // _remoteConfigParams.moveZSpeed =
            //     RemoteConfig.GetInstance().GetFloat(nameof(_remoteConfigParams.moveZSpeed), MoveZ.Instance.VelocityZ);
            MoveZ.Instance.SetVelocityZ(_remoteConfigParams.moveZSpeed);
        }
        
    }
}