using Chess.Controller;
using Chess.Interfaces;
using Chess;
using Tests.Services;
using Chess.Board;

namespace Tests
{
    internal class GameManagerTests
    {
        [Test]
        public void FoolsMate()
        {
            // Arrange
            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("all"); // init game with all pieces
            consoleInputs.Enqueue("WP7 G4");
            consoleInputs.Enqueue("BP5 E5");
            consoleInputs.Enqueue("WP6 F3");
            consoleInputs.Enqueue("BQ1 H4");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("Black wins against White by CheckMate!"), Is.True);
        }

        [Test]
        public void ScholarsMate()
        {
            // Arrange
            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("all"); // init game with all pieces
            consoleInputs.Enqueue("WP5 E4");
            consoleInputs.Enqueue("BP5 E5");
            consoleInputs.Enqueue("WB2 C4");
            consoleInputs.Enqueue("BK1 C6");
            consoleInputs.Enqueue("WQ1 H5");
            consoleInputs.Enqueue("BK2 F6");
            consoleInputs.Enqueue("WQ1 F7");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("White wins against Black by CheckMate!"), Is.True);
        }

    }
}
