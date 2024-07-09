namespace Chess.Services
{
    public class ConcurrentLogger
    {
        private static int _counter = 0;
        private readonly string _filePath;
        private readonly object _lock = new object();

        public ConcurrentLogger(string filePath)
        {
            _filePath = filePath + _counter + ".log";
            ++_counter;
        }

        public void Log(string message, int threadId)
        {
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
}
