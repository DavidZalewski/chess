using Chess.Attributes;
using Chess.Globals;

namespace Chess.Services
{
    [ToDo("Move this out of Services namespace/folder and put into Logger namespace/folder")]
    public class ConcurrentLogger
    {
        private static int _counter = 0;
        private readonly string _filePath;
        private readonly object _lock = new object();

        public ConcurrentLogger(string filePath)
        {
            StaticLogger.Trace();
            _filePath = filePath + _counter + ".log";
            ++_counter;
        }

        public virtual void Log(string message, int threadId)
        {
            StaticLogger.Trace();
            lock (_lock)
            {
                using (StreamWriter writer = File.AppendText(_filePath))
                {
                    writer.WriteLine($"Thread {threadId}: {message}");
                    writer.Flush();
                }
            }
        }
    }

    public class NoOpConcurrentLogger : ConcurrentLogger
    {
        public NoOpConcurrentLogger(string filePath) : base(filePath)
        {
        }

        public override void Log(string message, int threadId)
        {

        }
    }
}
