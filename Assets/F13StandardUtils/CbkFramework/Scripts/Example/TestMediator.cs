using F13StandardUtils.CbkFramework.Scripts.Core.Event.Interface;
using F13StandardUtils.CbkFramework.Scripts.Core.View;

namespace F13StandardUtils.CbkFramework.Scripts.Example
{
    public class TestMediator : BaseMediator
    {
        private void OnEnable()
        {
            Subscribe("BindExample",OnProcess);
        }

        private void OnDisable()
        {
            Unsubscribe("BindExample",OnProcess);

        }

        private void OnProcess(IEvent e)
        {
            LogWarning("TestMediator.OnProcess");
        }
    }
}
