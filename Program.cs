using Chess.Board;
using Chess.Controller;
using Chess.GameState;
using Chess.Interfaces;
using Chess.Pieces;
using Chess.Services;
using System.Collections.Immutable;
using System.Linq;

namespace Chess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChessBoard chessBoard = new();
            GameController gameController = new GameController(chessBoard);
            IConsole consoleService = new ConsoleService();
            GameManager game = new(consoleService, gameController);
            game.Start();
            Console.WriteLine("Program Terminated Successfully");
        }
    }
}
