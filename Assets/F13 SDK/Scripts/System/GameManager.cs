using System;
using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private StateMachine _stateMachine;
        [Header("Managers")]
        [SerializeField]
        private OmegaUIManager _omegaUIManager;
        [SerializeField]
        private OmegaLevelManager _omegaLevelManager;
        [SerializeField]
        private OmegaHapticManager _omegaHapticManager;
        [SerializeField]
        private OmegaAudioManager _omegaAudioManager;
        [SerializeField]
        private OmegaDebugManager _omegaDebugManager;
        [SerializeField]
        private PlayerPrefsManager _omegaPlayerPrefsManager;
        [SerializeField]
        private ObjectManager _omegaObjectManager;

        public GameManager()
        {
            _stateMachine = new StateMachine();
        }
        #region Unity Functions
        private void Awake()
        {
            _omegaPlayerPrefsManager.AwakePlayerPrefsManager();
            _omegaUIManager.AwakeOmegaUIManager();
            _omegaHapticManager.AwakeOmageHapticManager();
            _omegaAudioManager.AwakeOmegaAudioManager();
            _omegaLevelManager.AwakeOmegaLevelManager();
            _omegaObjectManager.AwakeObjectManager();
            AddListeners();
        }
        private void Start()
        {
            _omegaDebugManager.PrintDebug("Starting", DebugType.GameManager);
            _stateMachine.SetState(MainMenuState.Instance);
            _omegaAudioManager.StartOmegaAudioManager();
            _omegaObjectManager.StartObjectManager();
        }
        private void FixedUpdate()
        {
            _stateMachine.FixedUpdateState();
        }
        private void Update()
        {
            _stateMachine.UpdateState();
            _omegaUIManager.UpdateOmegaUIManager();
        }
        private void LateUpdate()
        {
            _stateMachine.LateUpdateState();
        }

        #endregion

        #region Custom Event Listeners
        private void AddListeners()
        {
            UIInputManager.On_PlayButton += On_Play;
            UIInputManager.On_PauseButton += On_Paused;
            UIInputManager.On_ResumeButton += On_Resumed;
            UIInputManager.On_HomeButton += On_Homed;
            UIInputManager.On_RetryButton += On_Retryed;
            UIInputManager.On_NextLevelButton += On_NextLevel;
            UIInputManager.On_PreLevelButton += On_PreLevel;
            UIInputManager.On_RestartLevelButton += On_Play;
            OmegaLevelManager.On_LevelCompleted += On_LevelCompleted;
            OmegaLevelManager.On_LevelFailed += On_LevelFailed;
        }

        private void On_Retryed()
        {
            _omegaDebugManager.PrintDebug("Retry level setted", DebugType.GameManager);
            _stateMachine.SetState(RetryState.Instance);
        }

        private void On_PreLevel()
        {
            _omegaDebugManager.PrintDebug("Pre level setted", DebugType.GameManager);
            _stateMachine.SetState(MainMenuState.Instance);
            OmegaLevelManager.Instance.RestartLevel();
        }

        private void On_Resumed()
        {
            _omegaDebugManager.PrintDebug("Game is resumed", DebugType.GameManager);
            _stateMachine.SetState(GamePlayState.Instance);
            _stateMachine.EnterState();
        }
        private void On_Paused()
        {
            _omegaDebugManager.PrintDebug("Game is paused", DebugType.GameManager);
            _stateMachine.SetState(PauseState.Instance);
        }
        private void On_Play()
        {
            _omegaDebugManager.PrintDebug("Game is starting", DebugType.GameManager);
            _stateMachine.SetState(GamePlayState.Instance);

        }
        private void On_Homed()
        {
            _omegaDebugManager.PrintDebug("Going back to home page!", DebugType.GameManager);
            OmegaLevelManager.Instance.RestartLevel();
            _stateMachine.SetState(MainMenuState.Instance);

        }
        private void On_LevelFailed()
        {
            _omegaDebugManager.PrintDebug("Level is failed", DebugType.GameManager);
            _stateMachine.SetState(LevelFailedState.Instance);
        }
        private void On_LevelCompleted()
        {
            _omegaDebugManager.PrintDebug("Level is completed", DebugType.GameManager);
            _stateMachine.SetState(LevelSuccessState.Instance);
        }
        private void On_NextLevel()
        {
            _omegaDebugManager.PrintDebug("Game is starting", DebugType.GameManager);
            _stateMachine.SetState(MainMenuState.Instance);
        }
        #endregion



    }

}
