using Chess.Globals;
using Chess.Interfaces;

namespace Chess.Services
{
    internal class ConsoleService : IConsole
    {
        string? IConsole.ReadLine()
        {
            StaticLogger.Trace();
            return Console.ReadLine();
        }

        void IConsole.WriteLine(string? message)
        {
            StaticLogger.Trace();
            Console.WriteLine(message);
        }
    }
}
