using System;
using SharedServices.Files.V1;
using SharedServices.Locator.V1;

namespace SharedServices.Log
{
    public class Log : ILog
    {
        private static LogLevel _logLevel = LogLevel.Debug;
        public static string LogFilePath = "date.log";
        public static bool LogTime = true;
        public static bool LogToFile;
        public static bool LogToUnityConsole = true;
        public static bool LogToSystemConsole;
        private IFileService _fileService;

        public void Initialize()
        {
            ILog.Instance = this;
            LogFilePath = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log";

            try
            {
                _fileService ??= ServiceLocator.Get<IFileService>();
                var config = _fileService.ReadJson<Config>("config.json");
                if (config == null) return;
                if (config.LogLevel.HasValue) _logLevel = config.LogLevel.Value;
                if (config.LogToFile.HasValue) LogToFile = config.LogToFile.Value;
                if (config.LogTime.HasValue) LogTime = config.LogTime.Value;
                if (config.LogToUnityConsole.HasValue) LogToUnityConsole = config.LogToUnityConsole.Value;
                if (config.LogToSystemConsole.HasValue) LogToSystemConsole = config.LogToSystemConsole.Value;
                ILog.Trace("Log settings loaded from config.json.\n" +
                           $"LogLevel: {_logLevel}\n" +
                           $"LogToFile: {LogToFile}\n" +
                           $"LogTime: {LogTime}\n" +
                           $"LogToUnityConsole: {LogToUnityConsole}\n" +
                           $"LogToSystemConsole: {LogToSystemConsole}", this);
            }
            catch
            {
                // ignored
                ILog.Warn("Failed to read config.json, using default log settings.", this);
            }
        }

        public void LogMessage(LogLevel logLevel, string message, object context = null)
        {
            if (logLevel > _logLevel) return;
            var fileLogMessage = $"{logLevel}: {message}";
            var unityLogMessage = $"{message}";
            if (context != null)
            {
                if (LogTime)
                {
                    fileLogMessage = $"[{DateTime.Now:yyyyMMddTHHmmssfff}] {fileLogMessage}";
                }
                if (context is Type type)
                {
                    fileLogMessage = $"[{type.Name}] {fileLogMessage}";
                    unityLogMessage = $"[{type.Name}] {unityLogMessage}";
                }
#if UNITY_EDITOR
                else if (context is UnityEditor.MonoScript monoScript)
                {
                    fileLogMessage = $"[{monoScript.name}] {fileLogMessage}";
                    unityLogMessage = $"[{monoScript.name}] {unityLogMessage}";
                }
#endif
                else
                {
                    fileLogMessage = $"[{context.GetType().Name}] {fileLogMessage}";
                    unityLogMessage = $"[{context.GetType().Name}] {unityLogMessage}";
                }
            }

            if (LogToFile) _fileService.WriteAllText(LogFilePath, fileLogMessage + System.Environment.NewLine);
            if (LogToUnityConsole) LogUnityMessage(logLevel, unityLogMessage, context);
            if (LogToSystemConsole) Console.WriteLine(fileLogMessage);
        }

        private void LogUnityMessage(LogLevel logLevel, string message, object context)
        {
            switch (logLevel)
            {
                case LogLevel.Fatal:
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message, context as UnityEngine.Object);
                    break;
                case LogLevel.Warn:
                    UnityEngine.Debug.LogWarning(message, context as UnityEngine.Object);
                    break;
                case LogLevel.Info:
                case LogLevel.Debug:
                case LogLevel.Trace:
                default:
                    UnityEngine.Debug.Log(message, context as UnityEngine.Object);
                    break;
            }
        }
    }
}