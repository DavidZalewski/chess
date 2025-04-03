using Chess.Board;
using Chess.Collections;
using Chess.Controller;
using Chess.GameState;
using Chess.Pieces;
using Chess.Services;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using NUnit.Framework.Legacy;
using System.Collections.Concurrent;
using System.Security;


namespace Tests.GameState
{
    [Category("PERFORMANCE")]
    internal class ChessStateExplorerTests : TestBase
    {
        [Test(Description = "206583 results in 5.1 min for depth 4")]
        public void GenerateAllPossibleMoves_Depth_3_Success()
        {
            // Arrange
            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            gameController.StartGame();
            List<string> startingMoves = new List<string>()
            { "WP1 A3", "WP1 A4", "WP2 B3", "WP2 B4", "WP3 C3", "WP3 C4", "WP4 D3", "WP4 D4",
              "WP5 E3", "WP5 E4", "WP6 F3", "WP6 F4", "WP7 G3", "WP7 G4", "WP8 H3", "WP8 H4",
              "WK1 A3", "WK1 C3", "WK2 F3", "WK2 H3"
            };
            List<Turn> startingTurns = new List<Turn>();
            foreach (string move in startingMoves)
            {
                Turn? turn = gameController.GetTurnFromCommand(move);
                Assert.That(turn, Is.Not.Null);
                startingTurns.Add(turn);
            }

            ChessStateExplorer stateExplorer = new();

            var generateAllPossibleMovesThread = (Turn t) => stateExplorer.GenerateAllPossibleMoves(t, 2);

            List<Thread> threads = new List<Thread>();
            List<Turn> possibleMoves = new List<Turn>();
            object mutex = new();

            foreach (Turn turn in startingTurns)
            {
                Thread thread = new(() =>
                {
                    List<Turn> threadPossibleMoves = generateAllPossibleMovesThread(turn);
                    lock (mutex)
                    {
                        possibleMoves.AddRange(threadPossibleMoves);
                    }
                });
                threads.Add(thread);
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Assert.That(possibleMoves, Is.Not.Empty);
            Console.WriteLine($"The possible number of moves within the first 4 turns of chess is: {possibleMoves.Count}");
            Assert.That(possibleMoves.Count, Is.GreaterThanOrEqualTo(1000)); // 207398 moves

            /*
System.OutOfMemoryException : Exception of type 'System.OutOfMemoryException' was thrown.
Stack Trace: 
StringBuilder.ToString()
JsonConvert.SerializeObjectInternal(Object value, Type type, JsonSerializer jsonSerializer)
JsonConvert.SerializeObject(Object value, Type type, JsonSerializerSettings settings)
JsonConvert.SerializeObject(Object value)
ChessStateExplorerTests.GenerateAllPossibleMoves_Depth_4_Success() line 69
RuntimeMethodHandle.InvokeMethod(Object target, Void** arguments, Signature sig, Boolean isConstructor)
MethodInvoker.Invoke(Object obj, IntPtr* args, BindingFlags invokeAttr)
             */
            //string json = JsonConvert.SerializeObject(possibleMoves);
            //File.WriteAllText("possible_moves_first_four_turns.json", json);


            using (StreamWriter writer = new StreamWriter("possible_moves_first_four_turns.json"))
            {
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, possibleMoves);
                }
            }
        }

        [Test(Description = "3106735 results in 7.5min for depth 5")]
        public void GenerateAllPossibleMovesTurnNode_Depth_3_Success()
        {
            // Arrange
            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            gameController.StartGame();
            List<string> startingMoves = new List<string>()
            { "WP1 A3", "WP1 A4", "WP2 B3", "WP2 B4", "WP3 C3", "WP3 C4", "WP4 D3", "WP4 D4",
              "WP5 E3", "WP5 E4", "WP6 F3", "WP6 F4", "WP7 G3", "WP7 G4", "WP8 H3", "WP8 H4",
              "WK1 A3", "WK1 C3", "WK2 F3", "WK2 H3"
            };
            List<Turn> startingTurns = new List<Turn>();
            foreach (string move in startingMoves)
            {
                Turn? turn = gameController.GetTurnFromCommand(move);
                Assert.That(turn, Is.Not.Null);
                startingTurns.Add(turn);
            }

            ChessStateExplorer stateExplorer = new();
            ulong count = 0;
            var generateAllPossibleMovesThread = (Turn t) => stateExplorer.GenerateAllPossibleMovesTurnNode(t, 2, ref count);

            List<Thread> threads = new List<Thread>();
            List<TurnNode> possibleMoves = new List<TurnNode>();
            object mutex = new();

            foreach (Turn turn in startingTurns)
            {
                Thread thread = new(() =>
                {
                    List<TurnNode> threadPossibleMoves = generateAllPossibleMovesThread(turn);
                    lock (mutex)
                    {
                        possibleMoves.AddRange(threadPossibleMoves);
                    }
                });
                threads.Add(thread);
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Assert.That(possibleMoves, Is.Not.Empty);
            Console.WriteLine($"The possible number of moves within the first 3 turns of chess is: {count}");
            Assert.That(count, Is.GreaterThanOrEqualTo(300)); // 3106735 moves

            using (StreamWriter writer = new StreamWriter("possible_moves_first_three_turns_lighter.json"))
            {
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, possibleMoves);
                }
            }
        }

        [Test(Description = "[TPL-Managed: 14.2min], [WithDegreeOfParallelism(20): 18.2min], [Caching-Removed: 25.4min")]
        [Ignore("Too much time")]
        public void GenerateAllPossibleMoves_LighterMemory_Depth_7_Success()
        {
            // Arrange
            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            gameController.StartGame();
            List<string> startingMoves = new()
    { "WP1 A3", "WP1 A4", "WP2 B3", "WP2 B4", "WP3 C3", "WP3 C4", "WP4 D3", "WP4 D4",
      "WP5 E3", "WP5 E4", "WP6 F3", "WP6 F4", "WP7 G3", "WP7 G4", "WP8 H3", "WP8 H4",
      "WK1 A3", "WK1 C3", "WK2 F3", "WK2 H3"
    };
            List<Turn> startingTurns = new();
            foreach (string move in startingMoves)
            {
                Turn? turn = gameController.GetTurnFromCommand(move);
                Assert.That(turn, Is.Not.Null);
                startingTurns.Add(turn);
            }

            ChessStateExplorer stateExplorer = new();
            ConcurrentLogger logger = new("GenerateAllPossibleMoves_LighterMemory_Depth_7_Success.txt");

            var possibleMoves = startingTurns.AsParallel()
                //.WithDegreeOfParallelism(20) // Limit the degree of parallelism to 20
                .Select((turn) =>
                {
                    int threadId = Thread.CurrentThread.ManagedThreadId;
                    ulong count = 0;
                    logger.Log($"Starting to process turn {turn.TurnDescription}", threadId);
                    List<TurnNode> moves = stateExplorer.GenerateAllPossibleMovesTurnNode(turn, 6, ref count);
                    logger.Log($"Finished processing turn {turn.TurnDescription} with {count} possible moves", threadId);
                    return (long)count;
                })
                .Sum();

            logger.Log($"The possible number of moves within the first 7 turns of chess is: {possibleMoves}", 0);
            Assert.That(possibleMoves, Is.LessThanOrEqualTo(2557005815)); // 2496503021
            // when caching with TurnID :
            //           2,557,005,815 moves at turn 7 / 11.6s
            //          47,702,789,199 moves at turn 8 / 23.7s
            //       1,347,926,109,040 moves at turn 9 / 27.1s
            //      24,375,292,174,409 moves at turn 10 / 30.5s
            //     842,251,828,564,222 moves at turn 11 / 33.4s
            //  13,152,484,339,525,676 moves at turn 12 / 34.4s
            // 377,644,534,572,086,014 moves at turn 13 / 38.9s
        }

        [Test]
        [Ignore("Too Time Consuming")]
        public void GenerateAllPossibleMovesTurnNode_UsesBoardIDAsCacheKey()
        {
            // Arrange
            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            gameController.StartGame();
            List<string> startingMoves = new()
    {
      "WP1 A3", "WP1 A4", "WP2 B3", "WP2 B4", "WP3 C3", "WP3 C4", "WP4 D3", "WP4 D4",
      "WP5 E3", "WP5 E4", "WP6 F3", "WP6 F4", "WP7 G3", "WP7 G4", "WP8 H3", "WP8 H4",
      "WK1 A3", "WK1 C3", "WK2 F3", "WK2 H3"
    };
            List<Turn> startingTurns = new();
            foreach (string move in startingMoves)
            {
                Turn? turn = gameController.GetTurnFromCommand(move);
                Assert.That(turn, Is.Not.Null);
                startingTurns.Add(turn);
            }

            ChessStateExplorer explorer = new ChessStateExplorer();
            int depth = 2;
            ulong currentCount = 0;

            // Act
            List<TurnNode> possibleMoves = explorer.GenerateAllPossibleMovesTurnNode(startingTurns.First(), depth, ref currentCount);

            // Assert
            Assert.That(explorer.cache.TryGetValue(possibleMoves[0].BoardID, out CacheItem cacheItem));
        }

        [Test]
        [Ignore("Cache Saving Not Implemented Yet!")]
        public void SaveAndLoadCache_CacheIsSavedAndLoadedCorrectly()
        {
            // Arrange
            ChessStateExplorer explorer = new ChessStateExplorer();
            explorer.cache.AddOrUpdate("test_key", new(123));

            // Act
            //ChessStateExplorer.SaveCache();
            explorer.cache = new MultiDimensionalCache<CacheItem>(8); // 8 is hardcoded var in chess state explorer
            explorer = new ChessStateExplorer(); // Load the cache from file

            // Assert
            Assert.That(explorer.cache.TryGetValue("test_key", out CacheItem cacheItem));
            Assert.That(cacheItem.Value, Is.EqualTo(123));
        }

        [Test]
        public void PrintTopCacheItems_Top25_Success()
        {
            // Arrange
            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            gameController.StartGame();
            List<string> startingMoves = new()
    {
      "WP1 A3", "WP1 A4", "WP2 B3", "WP2 B4", "WP3 C3", "WP3 C4", "WP4 D3", "WP4 D4",
      "WP5 E3", "WP5 E4", "WP6 F3", "WP6 F4", "WP7 G3", "WP7 G4", "WP8 H3", "WP8 H4",
      "WK1 A3", "WK1 C3", "WK2 F3", "WK2 H3"
    };
            List<Turn> startingTurns = new();
            foreach (string move in startingMoves)
            {
                Turn? turn = gameController.GetTurnFromCommand(move);
                Assert.That(turn, Is.Not.Null);
                startingTurns.Add(turn);
            }

            ChessStateExplorer explorer = new ChessStateExplorer();
            int depth = 2;
            ulong currentCount = 0;

            // Act
            List<TurnNode> possibleMoves = explorer.GenerateAllPossibleMovesTurnNode(startingTurns.First(), depth, ref currentCount);


            // Act
            explorer.PrintTopCacheItems(250);

            foreach ((string key, CacheItem cii) ci in explorer.GetTopNCachedItems(25))
                Console.WriteLine($"Cache (Partition: {ci.key}, Cache Value: {ci.cii.Value}, Access Count: {ci.cii.AccessCount}");

            // Assert
            Assert.That(1 == 1);
        }

        [Test(Description = "Provides analysis of all pieces that are threatening other pieces")]
        public void GetAllAttacks_Success()
        {
            // Arrange
            ChessBoard chessBoard = new();

            ChessPiece whiteKing = new ChessPieceKing(ChessPiece.Color.WHITE, new("G1"));
            ChessPiece whiteRook1 = new ChessPieceRook(ChessPiece.Color.WHITE, 1, new("A1"));
            ChessPiece whiteRook2 = new ChessPieceRook(ChessPiece.Color.WHITE, 2, new("F1"));
            ChessPiece whiteKnight1 = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new("B1"));
            ChessPiece whiteKnight2 = new ChessPieceKnight(ChessPiece.Color.WHITE, 2, new("H3"));
            ChessPiece whitePawn1 = new ChessPieceWhitePawn(1, new("A3"));
            ChessPiece whitePawn2 = new ChessPieceWhitePawn(2, new("B4"));
            ChessPiece whitePawn5 = new ChessPieceWhitePawn(5, new("E3"));
            ChessPiece whitePawn6 = new ChessPieceWhitePawn(6, new("F2"));
            ChessPiece whitePawn7 = new ChessPieceWhitePawn(7, new("G2"));
            ChessPiece whitePawn8 = new ChessPieceWhitePawn(8, new("H2"));

            ChessPiece blackKing = new ChessPieceKing(ChessPiece.Color.BLACK, new("G8"));
            ChessPiece blackRook1 = new ChessPieceRook(ChessPiece.Color.BLACK, 1, new("F8"));
            ChessPiece blackRook2 = new ChessPieceRook(ChessPiece.Color.BLACK, 2, new("A8"));
            ChessPiece blackKnight1 = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("D7"));
            ChessPiece blackKnight2 = new ChessPieceKnight(ChessPiece.Color.BLACK, 2, new("D5")); 
            ChessPiece blackPawn1 = new ChessPieceBlackPawn(1, new("A4"));
            ChessPiece blackPawn2 = new ChessPieceBlackPawn(2, new("B7"));
            ChessPiece blackPawn5 = new ChessPieceBlackPawn(5, new("E4"));
            ChessPiece blackPawn6 = new ChessPieceBlackPawn(6, new("F5"));
            ChessPiece blackPawn7 = new ChessPieceBlackPawn(7, new("G5"));
            ChessPiece blackPawn8 = new ChessPieceBlackPawn(8, new("H6"));

            chessBoard.AddPiece(whiteKing);
            chessBoard.AddPiece(whiteRook1);
            chessBoard.AddPiece(whiteRook2);
            chessBoard.AddPiece(whiteKnight1);
            chessBoard.AddPiece(whiteKnight2);
            chessBoard.AddPiece(whitePawn1);
            chessBoard.AddPiece(whitePawn2);
            chessBoard.AddPiece(whitePawn5);
            chessBoard.AddPiece(whitePawn6);
            chessBoard.AddPiece(whitePawn7);
            chessBoard.AddPiece(whitePawn8);

            chessBoard.AddPiece(blackKing);
            chessBoard.AddPiece(blackRook1);
            chessBoard.AddPiece(blackRook2);
            chessBoard.AddPiece(blackKnight1);
            chessBoard.AddPiece(blackKnight2);
            chessBoard.AddPiece(blackPawn1);
            chessBoard.AddPiece(blackPawn2);
            chessBoard.AddPiece(blackPawn5);
            chessBoard.AddPiece(blackPawn6);
            chessBoard.AddPiece(blackPawn7);
            chessBoard.AddPiece(blackPawn8);

            ChessStateExplorer chessStateExplorer = new ChessStateExplorer();

            SortedTupleBag<string, List<ChessPiece>> results = chessStateExplorer.GetAllAttacks(chessBoard);

            Console.WriteLine(chessBoard.DisplayBoard());

            var expectedResults = new Dictionary<string, List<string>>
            {
                { "Black Knight 2", new List<string> { "White Pawn 5", "White Pawn 2" } },
                { "White Knight 2", new List<string> { "Black Pawn 7" } }
            };

            foreach (var keyValuePair in results)
            {
                var pieceName = keyValuePair.Item1;
                var threatenedPieces = keyValuePair.Item2.Select(p => p.GetPieceName()).ToList();

                string threatsList = "[";
                foreach (string threatenedPiece in threatenedPieces)
                {
                    threatsList += threatenedPiece;
                    threatsList += ",";
                }
                threatsList += "]";
                Console.WriteLine($"Piece: {keyValuePair.Item1}, Threatens: {threatsList}");
                Assert.That(expectedResults.ContainsKey(pieceName), $"Unexpected piece: {pieceName}");
                CollectionAssert.AreEquivalent(expectedResults[pieceName], threatenedPieces, $"Threats for {pieceName} do not match expected results.");
            }
        }

        [Test(Description = "Provides analysis of all pieces that are threatening other pieces for all possible moves up to depth n")]
        public void GetAllAttacksForAllPossibleMovesForDepth_Success()
        {
            // Arrange
            ChessBoard chessBoard = new();

            ChessPiece whiteKing = new ChessPieceKing(ChessPiece.Color.WHITE, new("G1"));
            ChessPiece whiteRook1 = new ChessPieceRook(ChessPiece.Color.WHITE, 1, new("A1"));
            ChessPiece whiteRook2 = new ChessPieceRook(ChessPiece.Color.WHITE, 2, new("F1"));
            ChessPiece whiteKnight1 = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new("B1"));
            ChessPiece whiteKnight2 = new ChessPieceKnight(ChessPiece.Color.WHITE, 2, new("H3"));
            ChessPiece whitePawn1 = new ChessPieceWhitePawn(1, new("A2")); // A3
            ChessPiece whitePawn2 = new ChessPieceWhitePawn(2, new("B4"));
            ChessPiece whitePawn5 = new ChessPieceWhitePawn(5, new("E3"));
            ChessPiece whitePawn6 = new ChessPieceWhitePawn(6, new("F2"));
            ChessPiece whitePawn7 = new ChessPieceWhitePawn(7, new("G2"));
            ChessPiece whitePawn8 = new ChessPieceWhitePawn(8, new("H2"));

            ChessPiece blackKing = new ChessPieceKing(ChessPiece.Color.BLACK, new("G8"));
            ChessPiece blackRook1 = new ChessPieceRook(ChessPiece.Color.BLACK, 1, new("F8"));
            ChessPiece blackRook2 = new ChessPieceRook(ChessPiece.Color.BLACK, 2, new("A8"));
            ChessPiece blackKnight1 = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("D7"));
            ChessPiece blackKnight2 = new ChessPieceKnight(ChessPiece.Color.BLACK, 2, new("D5"));
            ChessPiece blackPawn1 = new ChessPieceBlackPawn(1, new("A4"));
            ChessPiece blackPawn2 = new ChessPieceBlackPawn(2, new("B7"));
            ChessPiece blackPawn5 = new ChessPieceBlackPawn(5, new("E4"));
            ChessPiece blackPawn6 = new ChessPieceBlackPawn(6, new("F5"));
            ChessPiece blackPawn7 = new ChessPieceBlackPawn(7, new("G5"));
            ChessPiece blackPawn8 = new ChessPieceBlackPawn(8, new("H6"));

            chessBoard.AddPiece(whiteKing);
            chessBoard.AddPiece(whiteRook1);
            chessBoard.AddPiece(whiteRook2);
            chessBoard.AddPiece(whiteKnight1);
            chessBoard.AddPiece(whiteKnight2);
            chessBoard.AddPiece(whitePawn1);
            chessBoard.AddPiece(whitePawn2);
            chessBoard.AddPiece(whitePawn5);
            chessBoard.AddPiece(whitePawn6);
            chessBoard.AddPiece(whitePawn7);
            chessBoard.AddPiece(whitePawn8);

            chessBoard.AddPiece(blackKing);
            chessBoard.AddPiece(blackRook1);
            chessBoard.AddPiece(blackRook2);
            chessBoard.AddPiece(blackKnight1);
            chessBoard.AddPiece(blackKnight2);
            chessBoard.AddPiece(blackPawn1);
            chessBoard.AddPiece(blackPawn2);
            chessBoard.AddPiece(blackPawn5);
            chessBoard.AddPiece(blackPawn6);
            chessBoard.AddPiece(blackPawn7);
            chessBoard.AddPiece(blackPawn8);

            ChessStateExplorer chessStateExplorer = new ChessStateExplorer();

            Turn turn = new(44, whitePawn1, whitePawn1.GetCurrentPosition(), new("A3"), chessBoard);

            whitePawn1.Move(chessBoard, new("A3"));

            Console.WriteLine(chessBoard.DisplayBoard());

            ChessAnalysisResult chessAnalysisResult = chessStateExplorer.GetAllAttacksForAllPossibleMovesForDepth(turn, 2);

            foreach (var attackInfo in chessAnalysisResult.Attacks)
            {
                Console.WriteLine($"Attacking Piece: {attackInfo.Attacker.GetPieceName()}");
                foreach(var attacked in attackInfo.Targets)
                {
                    Console.WriteLine($"Attacks: {attacked.GetPieceName()}");
                }
            }
        }

    }
}
