using Chess.Globals;
using System.Collections.Concurrent;

namespace Tests
{
    [TestFixture]
    [Category("LOGGER")]
    public class StaticLoggerTests : TestBase
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
                Assert.That(_logEntries.Count, Is.EqualTo(1));
                LogEntry logEntry;
                _logEntries.TryPeek(out logEntry);
                Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Debug));
                Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.Trace));
            }
        }

        [Test]
        public void Log_ShouldLogCorrectly()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Info;

            // Act
            StaticLogger.Log("Test Message");
            string expectedStringValue = " - Test Message";
            // Assert
            lock (_lock)
            {
                Assert.That(_logEntries.Count, Is.EqualTo(1));
                LogEntry logEntry;
                _logEntries.TryPeek(out logEntry);
                Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Info));
                Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.General));
                Assert.That(logEntry.Message, Is.EqualTo(expectedStringValue));
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
                Assert.That(_logEntries.Count, Is.EqualTo(1));
                LogEntry logEntry;
                _logEntries.TryPeek(out logEntry);
                Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Debug));
                Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.MethodDump));
                Assert.That(logEntry.Message.Contains("param1"));
                Assert.That(logEntry.Message.Contains("2"));
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
                Assert.That(_logEntries.Count, Is.EqualTo(0));
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
                Assert.That(_logEntries.Count, Is.EqualTo(1));
                LogEntry logEntry;
                _logEntries.TryPeek(out logEntry);
                Assert.That(logEntry.TestName, Is.EqualTo(TestContext.CurrentContext.Test.MethodName));
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
                Assert.That(_logEntries.Count, Is.EqualTo(1));
                LogEntry logEntry;
                _logEntries.TryPeek(out logEntry);
                Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Debug));
                Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.ObjectDump));
                Assert.That(logEntry.Message.Contains("Value1"));
                Assert.That(logEntry.Message.Contains("2"));
            }
        }
    }
}