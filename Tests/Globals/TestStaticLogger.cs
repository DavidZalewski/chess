using Chess.Board;
using Chess.Controller;
using Chess.GameState;
using Chess.Globals;
using Chess.Interfaces;
using Chess.Pieces;
using Chess.Services;
using Tests.Services;

namespace Tests.Globals
{
    [Category("CORE")]
    // TODO: The Parellizable attribute breaks the logger
    // Because the tests are running on independent threads, there is a race condition on the StaticLogger
    // So we need to make sure the StaticLogger is also thread safe, so that the output makes sense and is readable
    // This is where a collection of sub loggers would be useful. because then that way each thread has its own logger
    // the Logger can collect all the log lines, and print them out in a nice order at the end, reducing the difficulty of trying to read 
    // a log file with multiple threads interweaving together
    //[Parallelizable(ParallelScope.All)]
    public class TestStaticLogger
    {
        private string _currentTestName;
        [SetUp]
        public void SetUp()
        {
            IConsole mockConsole = new MockConsoleService();
            StaticLogger.Console = mockConsole;

            _currentTestName = TestContext.CurrentContext.Test.MethodName;

            StaticLogger.TestName = _currentTestName;

            Console.WriteLine($"Starting test {_currentTestName}");
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine($"Ending test {_currentTestName}");
            StaticLogger.TestName = "";
            _currentTestName = "";
        }

        [Test]
        public void Example_TestName_ConstructTurn_Success()
        {
            ChessBoard board = new();
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
            ChessPiece whitePawn = chessPieces.First(pieces => pieces.GetColor() == ChessPiece.Color.WHITE &&
                                                     pieces.GetPiece() == ChessPiece.Piece.PAWN);
            BoardPosition previousPosition = whitePawn.GetCurrentPosition();
            BoardPosition newPosition = new(previousPosition.Rank - 2, previousPosition.File);
            board.PopulateBoard(chessPieces);


            Turn turn1 = new(1, whitePawn, previousPosition, newPosition, board);
        }

        [Test]
        public void Example_TestName_ChessPieceFactoryWorksAsExpected()
        {
            ChessBoard board = new();
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
        }

    }
}
