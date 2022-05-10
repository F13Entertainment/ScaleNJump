using System.Collections.Generic;
using F13StandardUtils.CbkFramework.Scripts.Core.Command.Interface;
using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Command
{
    public class CommandGroup : BaseCommand
    {
        private class CommandEventPair
        {
            public ICommand command;
            public IEvent e;
        }
        
        private readonly List<CommandEventPair> _stack=new List<CommandEventPair>();
        private int _completedCount = 0;
        
        protected override void OnExecute(IEvent e = null)
        {
            foreach (var p in _stack)
            {
                p.command.Execute(p.e);
            }
        }

        public void Add(ICommand command, IEvent e = null)
        {
            command.OnCompleted.AddListener(OnSubCommandCompleted);
            _stack.Add(new CommandEventPair(){command = command,e = e});
        }

        private void OnSubCommandCompleted(ICommand command)
        {
            command.OnCompleted.RemoveListener(OnSubCommandCompleted);
            _completedCount += 1;
            if(_completedCount == _stack.Count) Complete();
        }
    }
}