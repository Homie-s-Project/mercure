using System;

namespace Mercure.API.Utils.Logger;

public class Logger
{
    public static void Log(LogLevel logLevel, LogTarget logTarget, string message)
    {
        string logText = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] ";
        switch (logLevel)
        {
            case LogLevel.Trace:
                logText += $"\u001b[37m[Trace]\u001b[0m ";
                break;
            case LogLevel.Debug:
                logText += $"\u001b[34m[Debug]\u001b[0m ";
                break;
            case LogLevel.Info:
                logText += $"\u001b[32m[Info]\u001b[0m ";
                break;
            case LogLevel.Warn:
                logText += $"\u001b[33m[Warn]\u001b[0m ";
                break;
            case LogLevel.Error:
                logText += $"\u001b[31m[Error]\u001b[0m ";
                break;
            case LogLevel.Critical:
                logText += $"\u001b[35m[Critical]\u001b[0m ";
                break;
            case LogLevel.None:
                break;
            default:
                throw new ArgumentException("Invalid log level");
        }
        
        logText += $"[{logTarget}] {message}";
        Console.WriteLine(logText);
    }

    public static void LogInfo(string message) => Log(LogLevel.Info, LogTarget.File, message);
    public static void LogInfo(LogTarget logTarget, string message) => Log(LogLevel.Info, logTarget, message);
    
    public static void LogWarn(string message) => Log(LogLevel.Info, LogTarget.File, message);
    public static void LogWarn(LogTarget logTarget, string message) => Log(LogLevel.Info, logTarget, message);
    
    public static void LogError(string message) => Log(LogLevel.Info, LogTarget.File, message);
    public static void LogError(LogTarget logTarget, string message) => Log(LogLevel.Info, logTarget, message);
}