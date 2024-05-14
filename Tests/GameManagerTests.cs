using Chess.Controller;
using Chess.Interfaces;
using Chess;
using Tests.Services;
using Chess.Board;
using Chess.Callbacks;
using Chess.GameState;
using System.Data;
using System.Runtime.InteropServices;

namespace Tests
{
    internal class GameManagerTests
    {
        private GameController GetGameController()
        {
            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            ChessStateExplorer explorer = new();
            //gameController.SetOnTurnHandler((Turn turn) =>
            //{
            //    List<TurnNode> turns = explorer.GenerateAllPossibleMovesTurnNode(turn, 2);
            //    turns = turns.OrderBy((TurnNode tn) => tn.Children.Count()).ToList();

            //    Console.WriteLine($"The possible number of moves at this current move: {turn.TurnDescription} are: {turns.Count}");
            //    //foreach (TurnNode turnNode in turns)
            //    //{
            //    //    Console.WriteLine($"Possible moves for {turnNode.TurnDescription} : {turnNode.Children.Count}");
            //    //}

            //    TurnNode bestTurnToMake = turns.Last();

            //    Console.WriteLine($"*************Best turn to make here is: {bestTurnToMake.Command}");
            //});



            return gameController;
        }
        [Test]
        public void FoolsMate()
        {
            GameController gameController = GetGameController();
            // Arrange
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
            GameController gameController = GetGameController();
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("all"); // init game with all pieces
            consoleInputs.Enqueue("WP5 E4"); // 20
            consoleInputs.Enqueue("BP5 E5"); // 29
            consoleInputs.Enqueue("WB2 C4"); // 29
            consoleInputs.Enqueue("BK1 C6"); // 33
            consoleInputs.Enqueue("WQ1 H5"); // 31
            consoleInputs.Enqueue("BK2 F6"); // 43
            consoleInputs.Enqueue("WQ1 F7"); // 30 - wrong - should be zero. Our generateAllPossibleMovesFunction doesnt check if the current turn is in check mate, or even in check. It needs to do different things for different turn states. Are they in check or checkmate?

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("White wins against Black by CheckMate!"), Is.True);
        }

        [Test]
        public void PawnsOnly_KingInCheck_NoPawnPromotion()
        {
            // Arrange
            GameController gameController = GetGameController();
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("pawns"); // no to tutorial
            consoleInputs.Enqueue("WP3 C4");
            consoleInputs.Enqueue("BP7 G5");
            consoleInputs.Enqueue("WP3 C5");
            consoleInputs.Enqueue("BP4 D5");
            consoleInputs.Enqueue("WP3 D6"); // en passant
            consoleInputs.Enqueue("BP7 G4");
            consoleInputs.Enqueue("WP3 D7");
            consoleInputs.Enqueue("BK F8");
            consoleInputs.Enqueue("WP6 F4");
            consoleInputs.Enqueue("BP7 F3"); // en passant
            consoleInputs.Enqueue("WP1 A3");
            consoleInputs.Enqueue("BP7 F2");
            consoleInputs.Enqueue("WK D1");
            consoleInputs.Enqueue("quit");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameManager game = new GameManager(consoleService, gameController);

            // override what GameManager constructor has registered for callback, with this one instead
            SpecialMovesHandlers.PawnPromotionPromptUser = () =>
            {
                consoleService.WriteLine("Pawn Promoted. Choose type: (Q - Queen, R - Rook, K - Knight, B - Bishop)");
                return "Q";
            };

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("Pawn Promoted."), Is.False);
        }

        [Test]
        public void FalseEnPassant()
        {
            // Arrange
            GameController gameController = GetGameController();
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("pawns");

            // Set up the board for a potential En Passant scenario but then make a different move
            consoleInputs.Enqueue("WP3 C4");
            consoleInputs.Enqueue("BP7 G5");
            consoleInputs.Enqueue("WP3 C5"); // White Pawn is on the En Passant rank
            consoleInputs.Enqueue("BP4 D5"); // Black pawn next to it moves 2 squares
            consoleInputs.Enqueue("WP1 A3"); // Instead of capture, white makes a different move
            consoleInputs.Enqueue("quit");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameManager game = new(consoleService, gameController);

            // Act
            var initialPiecesCount = 18;
            game.Start();
            var finalPiecesCount = gameController.GetChessBoard().GetActivePieces().Count;

            // Assert
            Assert.That(finalPiecesCount, Is.EqualTo(initialPiecesCount)); // Assert that no pieces have been removed
        }
    }
}
