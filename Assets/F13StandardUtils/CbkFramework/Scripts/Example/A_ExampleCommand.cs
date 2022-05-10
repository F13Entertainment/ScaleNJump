using F13StandardUtils.CbkFramework.Scripts.Core.Command;
using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;

namespace F13StandardUtils.CbkFramework.Scripts.Example
{
    public class A_ExampleCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            LogWarning("A_ExampleCommand OnExecute");
            Fire("BindExample",new ExampleEvent());
            Complete();
        }
    }
}