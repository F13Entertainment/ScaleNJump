using System.Collections.Generic;
using System.Linq;
using F13StandardUtils.CbkFramework.Scripts.Core.Command.Interface;
using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Command
{
    public class CommandChain : BaseCommand
    {
        private class CommandEventPair
        {
            public ICommand command;
            public IEvent e;
        }
        
        private readonly List<CommandEventPair> _stack=new List<CommandEventPair>();
        
        protected override void OnExecute(IEvent e = null)
        {
            Next();
        }


        public void Add(ICommand command, IEvent e = null)
        {
            command.OnCompleted.AddListener(OnSubCommandCompleted);
            _stack.Add(new CommandEventPair(){command = command,e = e});
        }

        public override void Cancel()
        {
            if(_stack.Any())_stack.First().command.Cancel();
            base.Cancel();
        }

        private void OnSubCommandCompleted(ICommand command)
        {
            command.OnCompleted.RemoveListener(OnSubCommandCompleted);
            _stack.RemoveAll(s => s.command.Equals(command));
            Next();
        }

        private void Next()
        {
            if (_stack.Any())
            {
                var commandEventPair = _stack.FirstOrDefault();
                commandEventPair?.command.Execute(commandEventPair.e);
            }
            else
            {
                Complete();
            }
        }
    }
}