using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Command.Interface
{
    public interface ICommand
    {
        void Execute(IEvent e = null);
        void Complete();
        void Cancel();
        CommandEvent OnCompleted { get; set; }
        CommandEvent OnCanceled { get; set; }


    }
}