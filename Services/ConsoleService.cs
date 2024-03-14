using Chess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
