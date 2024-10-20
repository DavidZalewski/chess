using Chess.Board;
using Chess.Controller;
using Chess.GameState;
using Chess.Globals;
using Chess.Services;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using System.Collections.Concurrent;


namespace Tests
{
    [Category("CORE")]
    internal class ToStringTraitTests
    {
        [Test]
        public void TestDetailedStringDump()
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

            string dump1 = chessBoard.ToDetailedString();
            Console.WriteLine("DUMP1:");
            Console.WriteLine(dump1);
            string dump2 = gameController.ToDetailedString();
            Console.WriteLine("DUMP2:");
            Console.WriteLine(dump2);
            string dump3 = startingTurns.ToDetailedString();
            Console.WriteLine("DUMP3:");
            Console.WriteLine(dump3);
            string dump4 = startingMoves.ToDetailedString();
            Console.WriteLine("DUMP4:");
            Console.WriteLine(dump4);

            Assert.That(dump1, Is.Not.Empty);
            Assert.That(dump2, Is.Not.Empty);
            Assert.That(dump3, Is.Not.Empty);
            Assert.That(dump4, Is.Not.Empty);
        }
    }
}