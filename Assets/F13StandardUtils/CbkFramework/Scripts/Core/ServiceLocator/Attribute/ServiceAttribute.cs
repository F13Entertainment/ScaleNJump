namespace F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Attribute
{
    public class ServiceAttribute : System.Attribute
    {
        public bool IsLazy { get; }

        public ServiceAttribute(bool isLazy = false)
        {
            this.IsLazy = isLazy;
        }
    }
}