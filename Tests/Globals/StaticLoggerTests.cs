using Chess.Globals;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class StaticLoggerTests
    {
        private static void ClearLogEntries()
        {
            StaticLogger.LogEntries.Clear();
            StaticLogger.ThreadsAndTests.Clear();
        }

        [Test]
        public void Trace_ShouldLogWhenEnabled()
        {
            // Arrange
            ClearLogEntries();
            StaticLogger.LoggerConfig.EnableTrace = true;
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.AddTestName();

            // Act
            StaticLogger.Trace();

            // Assert
            Assert.That(StaticLogger.LogEntries.Count, Is.EqualTo(1));
            var logEntry = StaticLogger.LogEntries.Dequeue();
            Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.Trace));
            Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Debug));
        }

        [Test]
        public void Log_ShouldLogWhenConditionsMet()
        {
            // Arrange
            ClearLogEntries();
            StaticLogger.LoggerConfig.EnableObjectDumps = true;
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Info;
            StaticLogger.AddTestName();

            // Act
            StaticLogger.Log("Test message", LogLevel.Info);

            // Assert
            Assert.That(StaticLogger.LogEntries.Count, Is.EqualTo(1));
            var logEntry = StaticLogger.LogEntries.Dequeue();
            Assert.That(logEntry.Message, Is.EqualTo("Test message"));
            Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Info));
            Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.General));
        }

        [Test]
        public void LogMethod_ShouldLogMethodParameters()
        {
            // Arrange
            ClearLogEntries();
            StaticLogger.LoggerConfig.EnableMethodDumps = true;
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.AddTestName();

            // Act
            StaticLogger.LogMethod("param1", 42);

            // Assert
            Assert.That(StaticLogger.LogEntries.Count, Is.EqualTo(1));
            var logEntry = StaticLogger.LogEntries.Dequeue();
            Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.MethodDump));
            Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Debug));
            Assert.That(logEntry.Message, Contains.Substring("Parameters: Name = param1, Value = param1"));
        }

        [Test]
        public void DumpLog_ShouldDumpAllLogEntries()
        {
            // Arrange
            ClearLogEntries();
            StaticLogger.LoggerConfig.EnableTrace = true;
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.AddTestName();
            StaticLogger.Trace();
            StaticLogger.Log("Test message", LogLevel.Info);

            // Act
            var output = StaticLogger.DumpLog();

            // Assert
            Assert.That(output, Is.Not.Empty);
            Assert.That(StaticLogger.LogEntries.Count, Is.EqualTo(0));
        }

        [Test]
        public void LogObject_ShouldLogObjectDetails()
        {
            // Arrange
            ClearLogEntries();
            StaticLogger.LoggerConfig.EnableObjectDumps = true;
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.AddTestName();
            var testObject = new { Name = "Test", Value = 42 };

            // Act
            StaticLogger.LogObject(testObject, "Logging object details");

            // Assert
            Assert.That(StaticLogger.LogEntries.Count, Is.EqualTo(1));
            var logEntry = StaticLogger.LogEntries.Dequeue();
            Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.ObjectDump));
            Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Debug));
            Assert.That(logEntry.Message, Contains.Substring("Logging object details - Name = Test, Value = 42"));
        }
    }
}