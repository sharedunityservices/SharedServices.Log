using System;

namespace SharedServices.Log
{
    [Serializable]
    public class Config
    {
        public LogLevel? LogLevel;
        public bool? LogTime;
        public bool? LogToFile;
        public bool? LogToUnityConsole;
        public bool? LogToSystemConsole;
    }
}