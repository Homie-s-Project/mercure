using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Mercure.API.Utils.Logger;

public class Logger
{
    public static void Log(LogLevel logLevel, LogTarget logTarget, string message)
    {
        string logText = $"[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}] ";
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

        logText += $" [{logTarget}] {message}";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (logLevel == LogLevel.Error || logLevel == LogLevel.Critical)
            {
                WriteColorForWindows(logText, ConsoleColor.Red);
            }
            else if (logLevel == LogLevel.Warn)
            {
                WriteColorForWindows(logText, ConsoleColor.Yellow);
            }
            else
            {
                WriteColorForWindows(logText, ConsoleColor.White);
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
            using (StreamWriter streamWriter = File.AppendText(logFilePath))
            {
                streamWriter.WriteLine(logText);
            }
        }
        else
        {
            Directory.CreateDirectory("logs");
        }
    }

    public static void LogInfo(string message) => Log(LogLevel.Info, LogTarget.File, message);
    public static void LogInfo(LogTarget logTarget, string message) => Log(LogLevel.Info, logTarget, message);

    public static void LogWarn(string message) => Log(LogLevel.Warn, LogTarget.File, message);
    public static void LogWarn(LogTarget logTarget, string message) => Log(LogLevel.Warn, logTarget, message);

    public static void LogError(string message) => Log(LogLevel.Error, LogTarget.File, message);
    public static void LogError(LogTarget logTarget, string message) => Log(LogLevel.Error, logTarget, message);

    private static void WriteColorForWindows(string message, ConsoleColor color)
    {
        var pieces = Regex.Split(message, @"(\[[^\]]*\])");

        for (int i = 0; i < pieces.Length; i++)
        {
            string piece = pieces[i];

            if (piece.StartsWith("[") && piece.EndsWith("]"))
            {
                Console.ForegroundColor = color;
                piece = piece.Substring(1, piece.Length - 2);
            }

            Console.Write(piece);
            Console.ResetColor();
        }

        Console.WriteLine();
    }
}