namespace Mercure.API.Utils.Logger;

/// <summary>
/// Logger class to log messages in the console and in a file
/// </summary>
public enum LogTarget
{
    /// <summary>
    /// LogTarget File is used to log messages concerning a file
    /// </summary>
    File,
    /// <summary>
    /// LogTarget Database is used to log messages concerning a database
    /// </summary>
    Database,
    /// <summary>
    /// LogTarget EventLog is used to log messages concerning an event log
    /// </summary>
    EventLog,
}