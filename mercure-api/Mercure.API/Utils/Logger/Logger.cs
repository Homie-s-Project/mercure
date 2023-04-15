using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Mercure.API.Utils.Logger;

/// <summary>
/// Logger class to log messages in the console and in a file
/// </summary>
public abstract class Logger
{
    /// <summary>
    /// Log level
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="logTarget"></param>
    /// <param name="message"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void Log(LogLevel logLevel, LogTarget logTarget, string message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        var logText = $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}] ";
        switch (logLevel)
        {
            case LogLevel.Trace:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    logText += "[Trace]";
                }
                else
                {
                    logText += $"\u001b[37m[Trace]\u001b[0m ";
                }
                break;
            case LogLevel.Debug:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    logText += "[Debug]";
                }
                else
                {
                    logText += $"\u001b[34m[Debug]\u001b[0m ";
                }
                break;
            case LogLevel.Info:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    logText += "[Info]";
                }
                else
                {
                    logText += $"\u001b[32m[Info]\u001b[0m ";
                }
                break;
            case LogLevel.Warn:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    logText += "[Warn]";
                }
                else
                {
                    logText += $"\u001b[33m[Warn]\u001b[0m ";
                }

                break;
            case LogLevel.Error:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    logText += "[Error]";
                }
                else
                {
                    logText += $"\u001b[31m[Error]\u001b[0m ";
                }
                break;
            case LogLevel.Critical:
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    logText += "[Critica]";
                }
                else
                {
                    logText += $"\u001b[35m[Critical]\u001b[0m ";
                }
                break;
            case LogLevel.None:
                break;
            default:
                throw new ArgumentException("Invalid log level");
        }

        logText += $"[{logTarget}] {message}";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            switch (logLevel)
            {
                case LogLevel.Error or LogLevel.Critical:
                    WriteColorForWindows(logText, ConsoleColor.Red);
                    break;
                case LogLevel.Warn:
                    WriteColorForWindows(logText, ConsoleColor.Yellow);
                    break;
                default:
                    WriteColorForWindows(logText, ConsoleColor.White);
                    break;
            }
        }
        else
        {
            Console.WriteLine(logText);
        }
        
        // Ajout du log dans le fichier de log
        var folderExist = Directory.Exists("logs");
        if (folderExist)
        {
            var processStarted = Process.GetCurrentProcess().StartTime.ToUniversalTime();
            var logFileName = $"log-{processStarted.ToString("dd-MM-yyyy")}.txt";
            var logFilePath = Path.Combine("logs", logFileName);
            File.AppendAllText(logFilePath, logText + Environment.NewLine);
        }
        else
        {
            Directory.CreateDirectory("logs");
        }
    }

    /// <summary>
    /// Log a message with the Info level
    /// </summary>
    /// <param name="message"></param>
    public static void LogInfo(string message) => Log(LogLevel.Info, LogTarget.File, message);
    /// <summary>
    /// Log a message with the level in parameter
    /// </summary>
    /// <param name="logTarget"></param>
    /// <param name="message"></param>
    public static void LogInfo(LogTarget logTarget, string message) => Log(LogLevel.Info, logTarget, message);

    /// <summary>
    /// Log a message with the Warn level
    /// </summary>
    /// <param name="message"></param>
    public static void LogWarn(string message) => Log(LogLevel.Info, LogTarget.File, message);
    /// <summary>
    /// Log a message with the Warn in parameter
    /// </summary>
    /// <param name="logTarget"></param>
    /// <param name="message"></param>
    public static void LogWarn(LogTarget logTarget, string message) => Log(LogLevel.Info, logTarget, message);

    /// <summary>
    /// Log a message with the Error level
    /// </summary>
    /// <param name="message"></param>
    public static void LogError(string message) => Log(LogLevel.Info, LogTarget.File, message);
    /// <summary>
    /// Log a message with the Error level in parameter
    /// </summary>
    /// <param name="logTarget"></param>
    /// <param name="message"></param>
    public static void LogError(LogTarget logTarget, string message) => Log(LogLevel.Info, logTarget, message);
    
    private static void WriteColorForWindows(string message, ConsoleColor color)
    {
        var pieces = Regex.Split(message, @"(\[[^\]]*\])");

        foreach (var t in pieces)
        {
            var piece = t;
        
            if (piece.StartsWith("[") && piece.EndsWith("]"))
            {
                Console.ForegroundColor = color;
                piece = piece.Substring(1,piece.Length-2);          
            }
        
            Console.Write(piece);
            Console.ResetColor();
        }
    
        Console.WriteLine();
    }
}