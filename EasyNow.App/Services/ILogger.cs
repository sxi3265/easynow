namespace EasyNow.App.Services
{
    public interface ILogger
    {
        void Log(string message, params object[] args);
        void LogError(string message, params object[] args);
    }
}