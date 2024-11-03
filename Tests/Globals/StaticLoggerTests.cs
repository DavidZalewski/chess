using NUnit.Framework;
using Chess.Globals;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chess.Attributes;
using Chess.Board;
using Chess.Interfaces;
using Chess.Services;
using Tests.Services;
using Chess.Board;
using Chess.Callbacks;
using Chess.GameState;
using Chess.Services;

namespace Tests
{
    [TestFixture]
    [Category("LOGGER")]
    public class StaticLoggerTests
    {
        private static readonly object _lock = new object();
        private static ConcurrentQueue<LogEntry> _logEntries = new ConcurrentQueue<LogEntry>();

        [SetUp]
        public void Setup()
        {
            // Reset the logger configuration and log entries for each test
            StaticLogger.LoggerConfig = new LoggerConfig();
            _logEntries = new ConcurrentQueue<LogEntry>();
            StaticLogger.LogEntries = _logEntries;
        }

        [Test]
        public void Trace_ShouldLogCorrectly()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.LoggerConfig.EnableTrace = true;

            // Act
            StaticLogger.Trace();

            // Assert
            lock (_lock)
            {
                Assert.AreEqual(1, _logEntries.Count);
                var logEntry = _logEntries.Peek();
                Assert.AreEqual(LogLevel.Debug, logEntry.LogLevel);
                Assert.AreEqual(LogCategory.Trace, logEntry.LogCategory);
            }
        }

        [Test]
        public void Log_ShouldLogCorrectly()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Info;

            // Act
            StaticLogger.Log("Test Message");

            // Assert
            lock (_lock)
            {
                Assert.AreEqual(1, _logEntries.Count);
                var logEntry = _logEntries.Peek();
                Assert.AreEqual(LogLevel.Info, logEntry.LogLevel);
                Assert.AreEqual(LogCategory.General, logEntry.LogCategory);
                Assert.AreEqual("Test Message", logEntry.Message);
            }
        }

        [Test]
        public void LogMethod_ShouldLogCorrectly()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.LoggerConfig.EnableMethodDumps = true;

            // Act
            StaticLogger.LogMethod(new object[] { "param1", 2 });

            // Assert
            lock (_lock)
            {
                Assert.AreEqual(1, _logEntries.Count);
                var logEntry = _logEntries.Peek();
                Assert.AreEqual(LogLevel.Debug, logEntry.LogLevel);
                Assert.AreEqual(LogCategory.MethodDump, logEntry.LogCategory);
                Assert.IsTrue(logEntry.Message.Contains("param1"));
                Assert.IsTrue(logEntry.Message.Contains("2"));
            }
        }

        [Test]
        public void DumpLog_ShouldDumpAllLogs()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.LoggerConfig.EnableTrace = true;

            // Act
            StaticLogger.Trace();
            StaticLogger.DumpLog();

            // Assert
            lock (_lock)
            {
                Assert.AreEqual(0, _logEntries.Count);
            }
        }

        [Test]
        public void AddTestName_ShouldAddTestNameCorrectly()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.LoggerConfig.EnableTrace = true;

            // Act
            StaticLogger.AddTestName();
            StaticLogger.Trace();

            // Assert
            lock (_lock)
            {
                Assert.AreEqual(1, _logEntries.Count);
                var logEntry = _logEntries.Peek();
                Assert.AreEqual(TestContext.CurrentContext.Test.MethodName, logEntry.TestName);
            }
        }

        [Test]
        public void LogObject_ShouldLogCorrectly()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.LoggerConfig.EnableObjectDumps = true;

            // Act
            StaticLogger.LogObject(new { Property1 = "Value1", Property2 = 2 });

            // Assert
            lock (_lock)
            {
                Assert.AreEqual(1, _logEntries.Count);
                var logEntry = _logEntries.Peek();
                Assert.AreEqual(LogLevel.Debug, logEntry.LogLevel);
                Assert.AreEqual(LogCategory.ObjectDump, logEntry.LogCategory);
                Assert.IsTrue(logEntry.Message.Contains("Value1"));
                Assert.IsTrue(logEntry.Message.Contains("2"));
            }
        }
    }
}