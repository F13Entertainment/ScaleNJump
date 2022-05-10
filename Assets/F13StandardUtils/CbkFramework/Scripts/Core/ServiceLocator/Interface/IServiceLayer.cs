namespace F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Interface
{
    public interface IServiceLayer
    {
        T GetService<T>() where T : IService;
    }
}