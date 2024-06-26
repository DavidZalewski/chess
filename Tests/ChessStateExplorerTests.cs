﻿using Chess;
using Chess.Board;
using Chess.Controller;
using Chess.GameState;
using Chess.Interfaces;
using Chess.Pieces;
using Chess.Services;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Tests.Services;


namespace Tests
{
    internal class ChessStateExplorerTests
    {
        [Test]
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

            var generateAllPossibleMovesThread = (Turn t) => stateExplorer.GenerateAllPossibleMovesTurnNode(t, 3);

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
            Console.WriteLine($"The possible number of moves within the first 4 turns of chess is: {possibleMoves.Count}");
            Assert.That(possibleMoves.Count, Is.GreaterThanOrEqualTo(400)); // 207398 moves

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

    }
}
