using System.Collections.Generic;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Interface;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Log
{
    public interface ILoggerService: IService
    {
        Stack<LogHistory> LogHistories();
        void LogTrace(string msg);
        void Log(string msg);
        void LogWarning(string msg);
        void LogError(string msg);
    }
}