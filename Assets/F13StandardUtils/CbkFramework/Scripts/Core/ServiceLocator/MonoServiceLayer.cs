using F13StandardUtils.CbkFramework.Scripts.Core.Log;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Interface;
using UnityEngine;

namespace F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator
{
    public abstract class MonoServiceLayer : MonoBehaviour,IServiceLayer
    {
        private ILoggerService _logger;
        
        private ILoggerService GetLogger()
        {
            return _logger ??= GetService<ILoggerService>();
        }
        
        public void LogTrace(string msg)
        {
            GetLogger().LogTrace(msg);
        }
        
        public void Log(string msg)
        {
            GetLogger().Log(msg);
        }

        public void LogWarning(string msg)
        {
            GetLogger().LogWarning(msg);
        }

        public void LogError(string msg)
        {
            GetLogger().LogError(msg);
        }
        public T GetService<T>() where T : IService
        {
            return ServiceLocator.Instance.GetService<T>();
        }
    }
}