using Chess.Attributes;
using Chess.GameState;
using Chess.Globals;
using Chess.Pieces;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class PromotionTrackerTests
    {
        private PromotionTracker promotionTracker;

        [SetUp]
        public void Setup()
        {
            promotionTracker = new PromotionTracker();
        }

        // Define a dictionary to hold expected results for each piece type and color
        private static Dictionary<(ChessPiece.Color, string), (int initial, int next)> testData = new Dictionary<(ChessPiece.Color, string), (int initial, int next)>
        {
            { (ChessPiece.Color.WHITE, "Q"), (2, 3) },
            { (ChessPiece.Color.WHITE, "R"), (3, 4) },
            { (ChessPiece.Color.WHITE, "K"), (3, 4) },
            { (ChessPiece.Color.WHITE, "B"), (3, 4) },
            { (ChessPiece.Color.BLACK, "Q"), (2, 3) },
            { (ChessPiece.Color.BLACK, "R"), (3, 4) },
            { (ChessPiece.Color.BLACK, "K"), (3, 4) },
            { (ChessPiece.Color.BLACK, "B"), (3, 4) },
        };

        [TestCaseSource(nameof(GetTestCases))]
        public void GetNextID_ReturnsCorrectIDAndIncrementsCounter((ChessPiece.Color, string) piece, int expectedInitialID, int expectedNextID)
        {
            // Arrange
            (ChessPiece.Color color, string pieceType) = piece;
            string key = $"{(color == ChessPiece.Color.WHITE ? "W" : "B")}{pieceType}";
            int initialID = promotionTracker.GetNextID(color, pieceType);

            // Act
            int nextID = promotionTracker.GetNextID(color, pieceType);

            // Assert
            Assert.That(initialID, Is.EqualTo(expectedInitialID), "Initial ID is incorrect");
            Assert.That(nextID, Is.EqualTo(expectedNextID), "Next ID is incorrect");
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            foreach (var (piece, (initial, next)) in testData)
            {
                yield return new TestCaseData(piece, initial, next).SetName($"GetNextID_{piece.Item1}_{piece.Item2}_ShouldReturnCorrectID");
            }
        }
    }
}