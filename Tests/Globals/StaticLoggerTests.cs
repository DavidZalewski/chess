using Chess.Globals;
using System.Collections.Concurrent;

namespace Tests
{
    public class LoggingMock
    {
        public void MockVoidMethod(List<string> stringList, int count, Dictionary<string, object> logParamsDictionary)
        {
            StaticLogger.LogMethod(stringList, count, logParamsDictionary);
        }

        public int MockReturnMethod(int arg1, int arg2, string arg3)
        {
            StaticLogger.LogMethod(arg1, arg2, arg3);
            return 0;
        }

        public static void MockStaticVoidMethod(string arg1, int arg2)
        {
            StaticLogger.LogMethod(arg1, arg2);
        }
    }

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
        public void Log_ShouldLogCorrectlyWithMetaData()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Info;
            StaticLogger.SetMetaData("Some Meta Data");

            // Act
            StaticLogger.Log("Test Message");

            // Assert
            lock (_lock)
            {
                Assert.That(_logEntries.Count, Is.EqualTo(1));
                LogEntry logEntry;
                _logEntries.TryPeek(out logEntry);
                Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Info));
                Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.General));
                Assert.That(logEntry.Message, Is.EqualTo("Some Meta Data - Test Message"));
            }
        }

        [Test]
        public void Log_ShouldLogCorrectlyWithoutMetaData()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Info;
            StaticLogger.SetMetaData("");

            // Act
            StaticLogger.Log("Test Message");

            // Assert
            lock (_lock)
            {
                Assert.That(_logEntries.Count, Is.EqualTo(1));
                LogEntry logEntry;
                _logEntries.TryPeek(out logEntry);
                Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Info));
                Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.General));
                Assert.That(logEntry.Message, Is.EqualTo("Test Message"));
            }
        }

        [Test]
        public void LogMethod_ShouldLogCorrectly_MockVoidMethod()
        {
            // Arrange
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.LoggerConfig.EnableMethodDumps = true;

            var logParamsDictionary = new Dictionary<string, object> { { "key", "value" } };
            var stringList = new List<string> { "item1", "item2" };

            LoggingMock loggingMock = new LoggingMock();

            // Act
            loggingMock.MockVoidMethod(stringList, 42, logParamsDictionary);

            // Assert
            lock (_lock)
            {
                Assert.That(_logEntries.Count, Is.EqualTo(1));
                LogEntry logEntry;
                _logEntries.TryPeek(out logEntry);
                Assert.That(logEntry.LogLevel, Is.EqualTo(LogLevel.Debug));
                Assert.That(logEntry.LogCategory, Is.EqualTo(LogCategory.MethodDump));
                Assert.That(logEntry.Message.Contains("stringList"));
                Assert.That(logEntry.Message.Contains("logParamsDictionary"));
                Assert.That(logEntry.Message.Contains("count"));
                Assert.That(logEntry.Message.Contains("value"));
                Assert.That(logEntry.Message.Contains("42"));
                Assert.That(logEntry.Message.Contains("key"));
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