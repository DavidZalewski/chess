using Chess.Controller;
using Chess.Interfaces;
using Chess;
using Tests.Services;
using Chess.Board;
using Chess.Callbacks;
using Chess.GameState;
using System.Data;
using Chess.Services;
using Chess.Globals;

namespace Tests
{
    // CANNOT BE RUN ASYNC OR IN PARALLEL
    [Category("CORE")]
    internal class GameManagerTests : TestBase
    {
        private int NumberOfMoves = 0;
        private GameController GetGameController(IConsole console)
        {
            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            ChessStateExplorer explorer = new();
            gameController.SetOnTurnHandler((Turn turn) =>
            {
                ulong _unused = 0;
                List<TurnNode> turns = explorer.GenerateAllPossibleMovesTurnNode(turn, 2, ref _unused);

                // Sort by least number of moves for opponent (Children.Count), then by most number of moves for current player (turns.Count - tn.Children.Count)
                turns = turns.OrderByDescending(tn => tn.TurnDescription.Contains("capture"))
                            .ThenBy(tn => tn.Children.Count())
                            .ThenByDescending(tn => turns.Count - tn.Children.Count)
                            .ToList();

                console.WriteLine($"The possible number of moves at this current move: {turn.TurnDescription} are: {turns.Count}");

                TurnNode bestTurnToMake = turns.First(); // Select the first turn after sorting

                //console.WriteLine($"*************Best turn to make here is: {bestTurnToMake.Command}");

                if (NumberOfMoves > 100)
                {
                    ((MockConsoleService)(console)).Inputs.Enqueue("quit");
                }
                else
                {
                    ((MockConsoleService)(console)).Inputs.Enqueue(bestTurnToMake.Command);
                }

                NumberOfMoves++;
            });

            return gameController;
        }

        [Test(Description = "Tests that the correct build flags are set")]
        public void CompileWithCheckServiceEnabled()
        {
            bool defined = false;
#if COMPILE_WITH_CHECK_SERVICE
            defined = true;
#endif

            Assert.That(defined, Is.True, "Expected macro COMPILE_WITH_CHECK_SERVICE to be enabled for this build");
        }

        [Test(Description = "Tests that you can now capture pieces by simply using: Piece1 Piece2")]
        public void EasierCaptureCommandTest()
        {
            // Arrange
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("Classic"); // init game with all pieces
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("WP4 D4");
            consoleInputs.Enqueue("BP5 E5");
            consoleInputs.Enqueue("WP4 BP5"); // should be valid capture
            consoleInputs.Enqueue("BK1 C6");
            consoleInputs.Enqueue("WP5 E4");
            consoleInputs.Enqueue("BK1 WP4"); // should be valid capture
            consoleInputs.Enqueue("WP5 BK1"); // should be invalid move
            consoleInputs.Enqueue("quit");


            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(gameController.FindChessPieceFromString("BP5"), Is.Null);
            Assert.That(gameController.FindChessPieceFromString("WP4"), Is.Null);
            Assert.That(gameController.FindChessPieceFromString("BK1"), Is.Not.Null);
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("Invalid Move"), Is.True);
        }

        [Test]
        public void NuclearHorseTest()
        {
            // Arrange
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("NuclearHorse"); // nuclear horse ruleset
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("WK1 C3");
            consoleInputs.Enqueue("BP2 B5");
            consoleInputs.Enqueue("BP3 C5");
            consoleInputs.Enqueue("BP4 D5");
            consoleInputs.Enqueue("quit");


            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(gameController.GetLastTurn()?.TurnNumber, Is.EqualTo(1), "Expected last turn to be turn 1");
            Assert.That(((MockConsoleService)consoleService).OutputContainsStringCount("Invalid Move"), Is.EqualTo(3), "Expected 3 Invalid Move messages from Black");
        }

        [Test]
        public void WhiteResignTest()
        {
            // Arrange
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("Classic"); // init game with all pieces
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("WP7 G4");
            consoleInputs.Enqueue("BP5 E5");
            consoleInputs.Enqueue("resign white");
            consoleInputs.Enqueue("y");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("Black wins against White by Resignation!"), Is.True);
        }

        [Test]
        public void BlackResignTest()
        {
            // Arrange
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("Classic"); // init game with all pieces
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("WP7 G4");
            consoleInputs.Enqueue("resign black");
            consoleInputs.Enqueue("y");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("White wins against Black by Resignation!"), Is.True);
        }

        [Test]
        public void FoolsMate()
        {
            // Arrange
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("Classic"); // init game with all pieces
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("WP7 G4");
            consoleInputs.Enqueue("BP5 E5");
            consoleInputs.Enqueue("WP6 F3");
            consoleInputs.Enqueue("BQ1 H4");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
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
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("Classic"); // init game with all pieces
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("WP5 E4"); // 20
            consoleInputs.Enqueue("BP5 E5"); // 29
            consoleInputs.Enqueue("WB2 C4"); // 29
            consoleInputs.Enqueue("BK1 C6"); // 33
            consoleInputs.Enqueue("WQ1 H5"); // 31
            consoleInputs.Enqueue("BK2 F6"); // 43
            consoleInputs.Enqueue("WQ1 F7"); // 30 - wrong - should be zero. Our generateAllPossibleMovesFunction doesnt check if the current turn is in check mate, or even in check. It needs to do different things for different turn states. Are they in check or checkmate?

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("White wins against Black by CheckMate!"), Is.True);
        }

        [Test(Author = "Hikaru", Description = "The mate Hikaru-Bot played on me on chess.com")]
        public void HikaruMate()
        {
            // Arrange
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("Classic"); // init game with all pieces
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("WP5 E4");
            consoleInputs.Enqueue("BP7 G5");
            consoleInputs.Enqueue("WK E2");
            consoleInputs.Enqueue("BP4 D5");
            consoleInputs.Enqueue("WP5 D5");
            consoleInputs.Enqueue("BQ1 D5");
            consoleInputs.Enqueue("WK2 F3");
            consoleInputs.Enqueue("BQ1 E4");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("Black wins against White by CheckMate!"), Is.True);
        }



        [Test(Author ="Hadrian", Description ="This is the mate Hadrian played against me on Chess.com")]
        public void HadriansMate()
        {
            // Arrange
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("Classic"); // init game with all pieces
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("WP5 E4");
            consoleInputs.Enqueue("BP5 E5"); 
            consoleInputs.Enqueue("WK E2");
            consoleInputs.Enqueue("BQ1 H4"); 
            consoleInputs.Enqueue("WK2 F3"); 
            consoleInputs.Enqueue("BQ1 E4");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("Black wins against White by CheckMate!"), Is.True);
        }

        [Test(Author = "Caesar", Description = "This is the mate Caesar played against me on Chess.com")]
        public void CaesarsMate()
        {
            // Arrange
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("Classic"); // Classic game mode
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("WP5 E4");
            consoleInputs.Enqueue("BP3 C5");
            consoleInputs.Enqueue("WK E2");
            consoleInputs.Enqueue("BK2 F6");
            consoleInputs.Enqueue("WK1 C3");
            consoleInputs.Enqueue("BP4 D5");
            consoleInputs.Enqueue("WK1 D5");
            consoleInputs.Enqueue("BK2 D5");
            consoleInputs.Enqueue("WP5 D5");
            consoleInputs.Enqueue("BQ1 D6");
            consoleInputs.Enqueue("WP3 C4");
            consoleInputs.Enqueue("BQ1 E5");
            consoleInputs.Enqueue("WK D3");
            consoleInputs.Enqueue("BB1 F5"); // double check that BB1 is the right bishop being referenced

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            // Assert
            Assert.That(((MockConsoleService)consoleService).OutputContainsString("Black wins against White by CheckMate!"), Is.True);
        }

        [Test]
        public void PawnsOnly_KingInCheck_NoPawnPromotion()
        {
            // Arrange
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("PawnsOnly"); // PawnsOnly game
            consoleInputs.Enqueue("n"); // no to ai
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
            GameController gameController = GetGameController(consoleService);
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
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("PawnsOnly");
            consoleInputs.Enqueue("n"); // no to ai
            // Set up the board for a potential En Passant scenario but then make a different move
            consoleInputs.Enqueue("WP3 C4");
            consoleInputs.Enqueue("BP7 G5");
            consoleInputs.Enqueue("WP3 C5"); // White Pawn is on the En Passant rank
            consoleInputs.Enqueue("BP4 D5"); // Black pawn next to it moves 2 squares
            consoleInputs.Enqueue("WP1 A3"); // Instead of capture, white makes a different move
            consoleInputs.Enqueue("quit");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            var initialPiecesCount = 18;
            game.Start();
            var finalPiecesCount = gameController.GetChessBoard().GetActivePieces().Count;

            // Assert
            Assert.That(finalPiecesCount, Is.EqualTo(initialPiecesCount)); // Assert that no pieces have been removed
        }

        [Test]
        //[Parallelizable(ParallelScope.Self)]
        public void AI_Vs_AI()
        {
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("PawnsOnly");
            consoleInputs.Enqueue("n"); // yes to ai
            consoleInputs.Enqueue("WP4 D4");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            GameController gameController = GetGameController(consoleService);
            GameManager game = new(consoleService, gameController);

            // Act
            game.Start();

            Assert.That(((MockConsoleService)consoleService).Inputs.Count < 11);

            Assert.Fail("Requirements incomplete - the white AI is not implemented yet!");
        }

        [Test]
        public void DebugBadCastleException()
        {
            Queue<string> consoleInputs = new();
            consoleInputs.Enqueue("n"); // no to tutorial
            consoleInputs.Enqueue("Classic");
            consoleInputs.Enqueue("n"); // no to ai
            consoleInputs.Enqueue("BR1 E7");

            IConsole consoleService = new MockConsoleService(consoleInputs);
            string BoardID = "0A00200060000CCC00080A00C0C00000000C000B9B0B00B0B7B00B7050000010";
            ChessBoard cb = new(BoardID);
            GameController gc = new(cb, cb.GetActivePieces());
            gc.TurnNumber = 38;
            GameManager game = new(consoleService, gc);

            // Act
            game.Start();
        }

        [Test(Author = "7OneSeven", Description = "Loads a converted replay from chess.com and plays it out in this engine")]
        public void ReplayChessDotComMatch()
        {
            bool SHOW_OUTPUT_OF_WINNING_GAMES = false;
            // Arrange
            Queue<string> consoleInputs = new Queue<string>();
            string expectedOutcome = "";

            string testReport = "";
            // Read moves from the file
            // Get the project directory path
            string? projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;

            Assert.That(projectDirectory, Is.Not.Null, "Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName should not be null");

            // Define the relative path to the TestInputs directory
            string filePath = Path.Combine(projectDirectory, "TestInputs");

            //string filePath2 = "C:\\Users\\david\\OneDrive\\Documents\\GitHub\\chess\\Tests\\TestInputs"; // Specify your converted_moves file path
            foreach (var file in Directory.GetFiles(filePath))
            {
                Console.WriteLine(file);
                var lines = File.ReadAllLines(file);

                // Initialize game setup (simulating responses for game setup questions)
                consoleInputs.Enqueue("n"); // no to tutorial
                consoleInputs.Enqueue("Classic"); // init game with all pieces
                consoleInputs.Enqueue("n"); // no to ai

                // Enqueue all the moves from the file
                foreach (var line in lines)
                {
                    if (line.StartsWith("Command:"))
                    {
                        var moveCommand = line.Replace("Command: ", "").Trim();
                        consoleInputs.Enqueue(moveCommand);
                    }
                    else if (line.StartsWith("Outcome:"))
                    {
                        expectedOutcome = line.Replace("Outcome: ", "").Trim();
                    }
                }

                // Set up the mock console service and game controller
                IConsole consoleService = new ChessDotComReplayMockConsoleService(consoleInputs, file, expectedOutcome, SHOW_OUTPUT_OF_WINNING_GAMES);
                StaticLogger.SetMetaData(file);
                GameController gameController = GetGameController(consoleService);  
                GameManager game = new GameManager(consoleService, gameController); 

                //if (file.Contains("6302"))
                //    System.Diagnostics.Debugger.Break();
                // Act
                game.Start();  // Start the game which will play through all the moves

                // Assert
                if (!((ChessDotComReplayMockConsoleService)consoleService).VerifyReplayAndLogIfFailed()) {
                    testReport += ("The game outcome did not match the expected checkmate. (Expected Outcome: " + expectedOutcome + ", File: " + file + ")\n");
                }
            }

            if (!String.IsNullOrEmpty(testReport))
            {
                ChessDotComReplayMockConsoleService.OutputTotals();
                Console.WriteLine(testReport);
                Assert.Fail($"One of the files failed to verify against chess engine: {testReport}");
            }
            ChessDotComReplayMockConsoleService.OutputTotals();

            Assert.That(true, Is.True);
        }
    }


}
