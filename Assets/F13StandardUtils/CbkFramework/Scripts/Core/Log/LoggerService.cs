using System.Collections.Generic;
using F13StandardUtils.CbkFramework.Scripts.Core.ServiceLocator.Attribute;
using UnityEngine;

namespace F13StandardUtils.CbkFramework.Scripts.Core.Log
{
    [Service(false)]
    public class LoggerService: MonoBehaviour, ILoggerService
    {
        [SerializeField] private LogLevel logLevel = LogLevel.Info;
        private readonly Stack<LogHistory> _logHistories=new Stack<LogHistory>();
        public void Initialize()
        {
            
        }

        public Stack<LogHistory> LogHistories()
        {
            return _logHistories;
        }

        public void LogTrace(string msg)
        {
            RecordToHistory(msg);
            if(logLevel>=LogLevel.Verbose)
                Debug.Log(msg);
        }
        
        public void Log(string msg)
        {
            RecordToHistory(msg);
            if(logLevel>=LogLevel.Info)
                Debug.Log(msg);
        }

        public void LogWarning(string msg)
        {
            RecordToHistory(msg);
            if(logLevel>=LogLevel.WarningAndErrors)
                Debug.LogWarning(msg);
        }

        public void LogError(string msg)
        {
            RecordToHistory(msg);
            if(logLevel>=LogLevel.ErrorOnly)
                Debug.LogError(msg);
        }
        
#if UNITY_EDITOR
        [SerializeField] private List<LogHistory> logList=new List<LogHistory>();
#endif
        private void RecordToHistory(string msg)
        {
            var history = new LogHistory() {level = logLevel, msg = msg, time = Time.time};
            _logHistories.Push(history);
#if UNITY_EDITOR
            logList.Insert(0,history);
#endif
        }

    }
}