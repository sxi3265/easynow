using System;
using System.Diagnostics;

namespace EasyNow.App.Services
{
    public class Logger : ILogger
    {
        public void Log(string message, params object[] args)
        {
            Debug.WriteLine(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            Debug.WriteLine($"\tERROR: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {message}", args);
        }
    }
}