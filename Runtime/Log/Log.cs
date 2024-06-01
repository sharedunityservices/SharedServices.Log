using System;
using SharedServices.Files.V1;

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
                var config = _fileService.ReadJson<Config>("config.json");
                if (config == null) return;
                if (config.LogLevel.HasValue) _logLevel = config.LogLevel.Value;
                if (config.LogToFile.HasValue) LogToFile = config.LogToFile.Value;
                if (config.LogTime.HasValue) LogTime = config.LogTime.Value;
                if (config.LogToUnityConsole.HasValue) LogToUnityConsole = config.LogToUnityConsole.Value;
                if (config.LogToSystemConsole.HasValue) LogToSystemConsole = config.LogToSystemConsole.Value;
            }
            catch
            {
                // ignored
            }
        }

        public void LogMessage(LogLevel logLevel, string message, UnityEngine.Object context = null)
        {
            if (logLevel > _logLevel) return;
            var logMessage = LogTime
                ? $"[{DateTime.Now:yyyyMMddTHHmmssfff}] {logLevel}: {message}"
                : $"{logLevel}: {message}";
            if (LogToFile) _fileService.WriteAllText(LogFilePath, logMessage + System.Environment.NewLine);
            if (LogToUnityConsole) LogUnityMessage(logLevel, message, context);
            if (LogToSystemConsole) Console.WriteLine(logMessage);
        }

        private void LogUnityMessage(LogLevel logLevel, string message, UnityEngine.Object context)
        {
            switch (logLevel)
            {
                case LogLevel.Fatal:
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message, context);
                    break;
                case LogLevel.Warn:
                    UnityEngine.Debug.LogWarning(message, context);
                    break;
                case LogLevel.Info:
                case LogLevel.Debug:
                case LogLevel.Trace:
                default:
                    UnityEngine.Debug.Log(message, context);
                    break;
            }
        }
    }
}