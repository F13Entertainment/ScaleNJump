
using System;

namespace Assets.F13SDK.Scripts
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void FixedExecute();
        public abstract void Execute();
        public abstract void LateExecute();
        public abstract void Exit();
    }
}
