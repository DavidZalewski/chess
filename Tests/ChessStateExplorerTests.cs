using Chess;
using Chess.Board;
using Chess.Controller;
using Chess.GameState;
using Chess.Interfaces;
using Chess.Pieces;
using Chess.Services;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Tests.Services;


namespace Tests
{
    [Parallelizable(ParallelScope.All)]
    internal class ChessStateExplorerTests
    {
        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void GenerateAllPossibleMoves_Depth_4_Success()
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

            var generateAllPossibleMovesThread = (Turn t) => stateExplorer.GenerateAllPossibleMoves(t, 3);

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
            Assert.That(possibleMoves.Count, Is.GreaterThanOrEqualTo(207398)); // 207398 moves

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

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public void GenerateAllPossibleMovesTurnNode_Depth_4_Success()
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
            var generateAllPossibleMovesThread = (Turn t) => stateExplorer.GenerateAllPossibleMovesTurnNode(t, 3, ref count);

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
            Console.WriteLine($"The possible number of moves within the first 4 turns of chess is: {count}");
            Assert.That(count, Is.GreaterThanOrEqualTo(400)); // 207398 moves

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


            using (StreamWriter writer = new StreamWriter("possible_moves_first_four_turns_lighter.json"))
            {
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, possibleMoves);
                }
            }
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
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
            Assert.That(ChessStateExplorer.cache.ContainsKey(possibleMoves[0].BoardID));
        }

        [Test]
        public void SaveAndLoadCache_CacheIsSavedAndLoadedCorrectly()
        {
            // Arrange
            ChessStateExplorer explorer = new ChessStateExplorer();
            ChessStateExplorer.cache.AddOrUpdate("test_key", (s) => 123, (s, i) => 123);

            // Act
            ChessStateExplorer.SaveCache();
            ChessStateExplorer.cache = new ConcurrentDictionary<string, ulong>(); // Clear the cache
            explorer = new ChessStateExplorer(); // Load the cache from file

            // Assert
            Assert.That(ChessStateExplorer.cache.ContainsKey("test_key"));
            Assert.That(ChessStateExplorer.cache["test_key"], Is.EqualTo(123));
        }

    }
}
