using UnityEngine;

namespace Assets.F13SDK.Scripts
{
    public class GamePlayState : State
    {
        public OmegaEventManager.GameStateHandler Gameplay_OnEntered;
        public OmegaEventManager.GameStateHandler Gameplay_OnExited;
        public OmegaEventManager.GameStateHandler Gameplay_OnFixedExecuted;
        public OmegaEventManager.GameStateHandler Gameplay_OnExecuted;
        public OmegaEventManager.GameStateHandler Gameplay_OnLateExecuted;

        public static GamePlayState Instance;
        private bool isLevelStarted = false;

        public GamePlayState()
        {
            if(Instance == null) Instance = this;
            OmegaLevelManager.On_LevelStarted += levelStarted;

        }

        private void levelStarted()
        {
            isLevelStarted = true;
        }

        public override void Enter()
        {
            OmegaDebugManager.Instance.PrintDebug("Gameplay state Entered", DebugType.State);
            Gameplay_OnEntered?.Invoke();
            Time.timeScale = 1f;
        }

        public override void FixedExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("Gameplay state fixed executed", DebugType.State);
            Gameplay_OnFixedExecuted?.Invoke();
        }

        public override void Execute()
        {
            if (isLevelStarted)
            {
                Enter();
                isLevelStarted = false;
            }
            Gameplay_OnExecuted?.Invoke();
        }

        public override void LateExecute()
        {
            OmegaDebugManager.Instance.PrintDebug("Gameplay state late executed", DebugType.State);
            Gameplay_OnLateExecuted?.Invoke();
        }

        public override void Exit()
        {
            OmegaDebugManager.Instance.PrintDebug("Gameplay state Exited", DebugType.State);
            Gameplay_OnExited?.Invoke();
        }




    }
}