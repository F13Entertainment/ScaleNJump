using UnityEngine;
using System.Collections.Generic;

namespace Assets.F13SDK.Scripts
{
    /// <summary>
    /// This state machine controls state changes. 
    /// </summary>
    public class StateMachine
    {
        public State _previousState;
        public State _currentState;
        private List<State> _states;
        public OmegaEventManager.GameStateHandler On_StateChanged;
        public StateMachine()
        {
            _states = new List<State>();
            InitiliazeStates();
        }

        /// <summary>
        /// States that you could use:
        /// 1. GameplayState
        /// 2. MainMenuState
        /// 3. PauseState
        /// 4. SettingsState
        /// 5. LevelFailedState
        /// 6. LevelSuccessState
        /// </summary>
        /// <param name="newState"></param>
        public void SetState(State newState)
        {
            if (_currentState != null) _currentState.Exit();
            OmegaDebugManager.Instance.PrintDebug("Current state is settings up", DebugType.State);
            _previousState = _currentState;
            _currentState = newState;
            _currentState.Enter();
            On_StateChanged?.Invoke();
        }

        public void EnterState()
        {
            if (_currentState != null) _currentState.Enter();
        }
        public void UpdateState()
        {
            if (_currentState != null) _currentState.Execute();
        }
        public void FixedUpdateState()
        {
            if (_currentState != null) _currentState.FixedExecute();
        }
        public void LateUpdateState()
        {
            if (_currentState != null) _currentState.LateExecute();
        }
        public void ExitState()
        {
            if (_currentState != null) _currentState.Exit();
            _currentState = _previousState;
        }

        #region State Util Functions

        private void InitiliazeStates()
        {
            _states.Add(new MainMenuState());
            _states.Add(new GamePlayState());
            _states.Add(new PauseState());
            _states.Add(new LevelFailedState());
            _states.Add(new LevelSuccessState());
            _states.Add(new RetryState());
        }
        #endregion

    }
}
