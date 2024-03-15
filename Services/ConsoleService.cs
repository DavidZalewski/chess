using Chess.Interfaces;

namespace Chess.Services
{
    internal class ConsoleService : IConsole
    {
        string? IConsole.ReadLine()
        {
            return Console.ReadLine();
        }

        void IConsole.WriteLine(string? message)
        {
            Console.WriteLine(message);
        }
    }
}
