namespace Mercure.API.Utils.Logger;

/// <summary>
/// LogLevel enum to define the log level
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Trace log level is the lowest level of logging. It is used to log all messages.
    /// </summary>
    Trace = 0,
    /// <summary>
    /// Debug log level is used to log messages that are useful for debugging.
    /// </summary>
    Debug = 1,
    /// <summary>
    /// Info log level is used to log messages that are useful for the user.
    /// </summary>
    Info = 2,
    /// <summary>
    /// Warn log level is used to log messages that are useful for the user but that may indicate a problem.
    /// </summary>
    Warn = 3,
    /// <summary>
    /// Error log level is used to log messages that indicate a problem.
    /// </summary>
    Error = 4,
    /// <summary>
    /// Critical log level is used to log messages that indicate a critical problem.
    /// </summary>
    Critical = 5,
    /// <summary>
    /// None log level is used to disable logging.
    /// </summary>
    None = 6,
}