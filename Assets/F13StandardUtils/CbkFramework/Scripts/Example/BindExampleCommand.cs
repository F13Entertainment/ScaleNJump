using F13StandardUtils.CbkFramework.Scripts.Core.Command;
using F13StandardUtils.CbkFramework.Scripts.Core.Event.Attribute;
using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;

namespace F13StandardUtils.CbkFramework.Scripts.Example
{
    [BindEvent("BindExample")]
    public class BindExampleCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            if (e is ExampleEvent exampleEvent)
            {
                LogWarning("BindExampleCommand OnExecute "+exampleEvent.Message);
            }
        }
    }
}