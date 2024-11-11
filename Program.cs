using Chess.Board;
using Chess.Controller;
using Chess.GameState;
using Chess.Globals;
using Chess.Interfaces;
using Chess.Services;

//#if COMPILE_WITH_CHECK_SERVICE
//#error "COMPILE_WITH_CHECK_SERVICE is defined"
//#else
//#error "COMPILE_WITH_CHECK_SERVICE is NOT defined"
//#endif

namespace Chess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StaticLogger.Trace();
            ChessBoard chessBoard = new();
            GameController gameController = new GameController(chessBoard);
            IConsole consoleService = new ConsoleService();
            GameManager game = new(consoleService, gameController);
            game.Start();
            Console.WriteLine("Program Terminated Successfully");
        }
    }
}
