using SharedServices.V1;
using UnityEngine;

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
        public static void Fatal(string message, Object context = null) => Instance.LogMessage(LogLevel.Fatal, message, context);
        public static void Error(string message, Object context = null) => Instance.LogMessage(LogLevel.Error, message, context);
        public static void Warn(string message, Object context = null) => Instance.LogMessage(LogLevel.Warn, message, context);
        public static void Info(string message, Object context = null) => Instance.LogMessage(LogLevel.Info, message, context);
        public static void Debug(string message, Object context = null) => Instance.LogMessage(LogLevel.Debug, message, context);
        public static void Trace(string message, Object context = null) => Instance.LogMessage(LogLevel.Trace, message, context);
        public void LogMessage(LogLevel logLevel, string message, Object context = null);
    }
}