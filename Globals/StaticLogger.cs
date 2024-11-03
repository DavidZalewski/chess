using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using Chess.Attributes;
using Chess.Board;
using Chess.Interfaces;
using Chess.Services;
using NUnit.Framework;

namespace Chess.Globals
{
    // TODO: Separate these classes into separate files. group them under folder Logger
    public enum LogLevel
    {
        Fatal,
        Error,
        Warn,
        Info,
        Debug,
    }

    public enum LogCategory
    {
        General,
        ObjectDump,
        MethodDump,
        StateChange,
        Trace
    }

    // IMPORTANT NOTE: Not thread safe. Don't try and change these properties in between tests... it will break everything
    // Set the config up once in a single place, and dont touch it in tests
    public class LoggerConfig
    {
        public bool EnableObjectDumps { get; set; } = true;
        public bool EnableMethodDumps { get; set; } = true;
        public bool EnableStateChanges { get; set; } = true;
        public bool EnableTrace { get; set; } = true;
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Info;
        public List<Type> TypesToSkipForLogging { get; } = new();

        public List<Type> WhitelistTypesToLog { get; } = new();
        public void AddTypeToSkipObjectDumps(Type objectType)
        {
            ToStringTrait.AddTypeToSkip(objectType);
        }

        public void AddTypeToSkipForLogging(Type type)
        {
            if (!TypesToSkipForLogging.Contains(type))
            {
                TypesToSkipForLogging.Add(type);
            }
        }

        public void AddTypeToWhiteList(Type type)
        {
            if (!WhitelistTypesToLog.Contains(type))
            {
                WhitelistTypesToLog.Add(type);
            }
        }
    }

    public struct LogEntry
    {
        public DateTime DateTime { get; init; }
        public string? TestName { get; init; }
        public string? Thread { get; init; }
        public string? CallerClassName { get; init; }
        public string? CallerMethodName { get; init; }
        public LogLevel LogLevel { get; init; }
        public LogCategory LogCategory { get; init; }
        public string? Message { get; init; }
    }

    public struct LogParameter
    {
        public string? Name { get; init; }
        public object? Value { get; init; }
    }

    public class StaticLogger
    {
        static public LoggerConfig LoggerConfig = new();
        static public IConsole Console { get; set; } = new ConsoleService();
        static public ConcurrentQueue<LogEntry> LogEntries = new();
        static public ConcurrentDictionary<string, string> ThreadsAndTests = new();
        static public string MetaData = "";

        [TestNeeded]
        static public void Trace()
        {
            if (LoggerConfig.MinimumLogLevel < LogLevel.Debug)
                return;

            if (!LoggerConfig.EnableTrace)
                return;

            var method = GetStackTraceInfo();
            var callerMethodName = method?.Name;
            var callerClassName = method?.DeclaringType?.FullName ?? "UnknownClass";

            // If a whitelist is defined, it takes precedence over filtered types
            if (LoggerConfig.WhitelistTypesToLog.Count > 0)
            {
                // If the type is not found in the whitelist, skip
                if (!LoggerConfig.WhitelistTypesToLog.Any(t => t.FullName == callerClassName))
                    return;
            }
            else
            {
                // Skip logging if the source class is in the skip list
                if (LoggerConfig.TypesToSkipForLogging.Any(t => t.FullName == callerClassName))
                {
                    return; // Skip this log entry
                }
            }

            string? testName;
            ThreadsAndTests.TryGetValue(Thread.CurrentThread?.Name ?? "", out testName);
            LogEntries.Enqueue(new()
            {
                DateTime = DateTime.Now,
                Thread = Thread.CurrentThread?.Name,
                TestName = testName ?? "",
                CallerClassName = callerMethodName,
                CallerMethodName = callerClassName,
                Message = $"{MetaData}",
                LogLevel = LogLevel.Debug,
                LogCategory = LogCategory.Trace
            });

        }

        [TestNeeded]
        static public void Log(string? message, LogLevel logLevel = LogLevel.Info, LogCategory logCategory = LogCategory.General)
        {
            if (LoggerConfig.MinimumLogLevel < logLevel)
                return;

            // if the Log is trying to do an Object Dump, and this setting is disabled - skip
            if (!LoggerConfig.EnableObjectDumps && logCategory == LogCategory.ObjectDump)
                return;

            if (!LoggerConfig.EnableStateChanges && logCategory == LogCategory.StateChange)
                return;

            // Retrieve the stack trace to get the caller information
            var method = GetStackTraceInfo();
            var callerMethodName = method?.Name;
            var callerClassName = method?.DeclaringType?.FullName ?? "UnknownClass";

            // If a whitelist is defined, it takes precedence over filtered types
            if (LoggerConfig.WhitelistTypesToLog.Count > 0)
            {
                // If the type is not found in the whitelist, skip
                if (!LoggerConfig.WhitelistTypesToLog.Any(t => t.FullName == callerClassName))
                    return;
            }
            else
            {
                // Skip logging if the source class is in the skip list
                if (LoggerConfig.TypesToSkipForLogging.Any(t => t.FullName == callerClassName))
                {
                    return; // Skip this log entry
                }
            }

            string? testName;
            ThreadsAndTests.TryGetValue(Thread.CurrentThread?.Name ?? "", out testName);

            LogEntries.Enqueue(new()
            {
                DateTime = DateTime.Now,
                Thread = Thread.CurrentThread?.Name,
                TestName = testName ?? "",
                CallerClassName = callerClassName,
                CallerMethodName = callerMethodName,
                Message = $"{MetaData} - {message}",
                LogLevel = logLevel,
                LogCategory = logCategory
            });
        }

        [TestNeeded]
        static public void LogMethod(params object[] parameterValues)
        {
            if (LoggerConfig.MinimumLogLevel < LogLevel.Debug)
                return;

            if (!LoggerConfig.EnableMethodDumps)
                return;

            // Retrieve the stack trace to get the caller information
            var method = GetStackTraceInfo();
            var callerMethodName = method?.Name;
            var callerClassName = method?.DeclaringType?.FullName ?? "UnknownClass";

            // If a whitelist is defined, it takes precedence over filtered types
            if (LoggerConfig.WhitelistTypesToLog.Count > 0)
            {
                // If the type is not found in the whitelist, skip
                if (!LoggerConfig.WhitelistTypesToLog.Any(t => t.FullName == callerClassName))
                    return;
            }
            else
            {
                // Skip logging if the source class is in the skip list
                if (LoggerConfig.TypesToSkipForLogging.Any(t => t.FullName == callerClassName))
                {
                    return; // Skip this log entry
                }
            }

            var parameters = method?.GetParameters();

            // Collect parameter names and values
            List<LogParameter> args = new();
            for (int i = 0; i < parameters.Length; i++)
            {
                var paramName = parameters[i].Name;
                var paramValue = i < parameterValues.Length ? parameterValues[i] : "Unknown";
                args.Add(new LogParameter { Name = paramName, Value = paramValue });
            }

            string? testName;
            ThreadsAndTests.TryGetValue(Thread.CurrentThread?.Name ?? "", out testName);

            LogEntries.Enqueue(new()
            {
                DateTime = DateTime.Now,
                Thread = Thread.CurrentThread?.Name,
                TestName = testName,
                CallerClassName = callerClassName,
                CallerMethodName = callerMethodName,
                LogLevel = LogLevel.Debug,
                LogCategory = LogCategory.MethodDump,
                Message = $"{MetaData} - Parameters: {args?.ToDetailedString()}"
            });
        }

        [TestNeeded]
        static public void DumpLog()
        {
            while(LogEntries.Count > 0)
            {
                LogEntry entry;
                if (LogEntries.TryDequeue(out entry))
                {
                    string formattedMessage = $@"
{{
    DateTime = {{ {entry.DateTime} }},
    Thread = {{ {entry.Thread} }},
    Test = {{ {entry.TestName} }}, 
    Source = {{ {entry.CallerClassName}.{entry.CallerMethodName} }},
    Level = {{ {entry.LogLevel} }},
    Category = {{ {entry.LogCategory} }},
    Message = {{ {entry.Message} }}
}}";
                    Console.WriteLine(formattedMessage);
                }
                else
                {
                    Console.WriteLine("Fatal Logging Exception - unable to Dequeue LogEntries from StaticLogger!");
                }
            }
        }


        private static MethodBase? GetStackTraceInfo()
        {
            // Retrieve the stack trace to get the caller information
            var stackTrace = new StackTrace(2, true); // Skip the first two frames (this method, and the one above it)
            var frame = stackTrace.GetFrame(0); // Get the calling method's frame
            return frame?.GetMethod();
        }

        [TestNeeded]
        public static void AddTestName()
        {
            var currentTestName = TestContext.CurrentContext.Test.MethodName;

            ThreadsAndTests.AddOrUpdate(Thread.CurrentThread?.Name ?? "", (v) => currentTestName, (k, v) => currentTestName);
        }

        public static void SetMetaData(string metadata)
        {
            MetaData = metadata;
        }

        [TestNeeded]
        public static void LogObject(object? obj, string message = "")
        {
            if (LoggerConfig.MinimumLogLevel < LogLevel.Debug)
                return;

            // if the Log is trying to do an Object Dump, and this setting is disabled - skip
            if (!LoggerConfig.EnableObjectDumps)
                return;


            // Retrieve the stack trace to get the caller information
            var method = GetStackTraceInfo();
            var callerMethodName = method?.Name;
            var callerClassName = method?.DeclaringType?.FullName ?? "UnknownClass";

            // If a whitelist is defined, it takes precedence over filtered types
            if (LoggerConfig.WhitelistTypesToLog.Count > 0)
            {
                // If the type is not found in the whitelist, skip
                if (!LoggerConfig.WhitelistTypesToLog.Any(t => t.FullName == callerClassName))
                    return;
            }
            else
            {
                // Skip logging if the source class is in the skip list
                if (LoggerConfig.TypesToSkipForLogging.Any(t => t.FullName == callerClassName))
                {
                    return; // Skip this log entry
                }
            }

            string? testName;
            ThreadsAndTests.TryGetValue(Thread.CurrentThread?.Name ?? "", out testName);

            string formattedMessage = $"{MetaData} - {message} - {obj?.ToDetailedString()}";

            LogEntries.Enqueue(new()
            {
                DateTime = DateTime.Now,
                Thread = Thread.CurrentThread?.Name,
                TestName = testName ?? "",
                CallerClassName = callerClassName,
                CallerMethodName = callerMethodName,
                Message = formattedMessage,
                LogLevel = LogLevel.Debug,
                LogCategory = LogCategory.ObjectDump
            });

        }
    }
}
