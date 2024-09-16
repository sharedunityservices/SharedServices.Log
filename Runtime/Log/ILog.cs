using System;
using SharedServices.V1;

namespace SharedServices.Log
{
    public interface ILog : IService
    {
        private static ILog _instance;
        public static ILog Instance
        {
            get => _instance ??= new Log();
            internal set => _instance = value;
        }
        public static void Exception(Exception exception, object context = null) => Instance.LogException(exception, context);
        public static void Error(string message, object context = null) => Instance.LogMessage(LogLevel.Error, message, context);
        public static void Warn(string message, object context = null) => Instance.LogMessage(LogLevel.Warn, message, context);
        public static void Info(string message, object context = null) => Instance.LogMessage(LogLevel.Info, message, context);
        public static void Debug(string message, object context = null) => Instance.LogMessage(LogLevel.Debug, message, context);
        public static void Trace(string message, object context = null) => Instance.LogMessage(LogLevel.Trace, message, context);
        
        public void LogMessage(LogLevel logLevel, string message, object context = null);
        public void LogException(Exception exception, object context = null);
    }
}