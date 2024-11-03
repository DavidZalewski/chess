using Chess.Attributes;
using Chess.Globals;
using Chess.Pieces;
using NUnit.Framework;
using System.Collections.Generic;

namespace Chess.GameState.Tests
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
        private Dictionary<(ChessPiece.Color, string), (int initial, int next)> testData = new Dictionary<(ChessPiece.Color, string), (int initial, int next)>
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
        [TestNeeded]
        public void GetNextID_ReturnsCorrectIDAndIncrementsCounter((ChessPiece.Color, string) piece, int expectedInitialID, int expectedNextID)
        {
            // Arrange
            (ChessPiece.Color color, string pieceType) = piece;
            string key = $"{(color == ChessPiece.Color.WHITE ? "W" : "B")}{pieceType}";
            int initialID = promotionTracker.GetNextID(color, pieceType);

            // Act
            int nextID = promotionTracker.GetNextID(color, pieceType);

            // Assert
            Assert.AreEqual(expectedInitialID, initialID, "Initial ID is incorrect");
            Assert.AreEqual(expectedNextID, promotionTracker.promotionCounters[key], "Next ID is incorrect");
        }

        private IEnumerable<TestCaseData> GetTestCases()
        {
            foreach (var (piece, (initial, next)) in testData)
            {
                yield return new TestCaseData(piece, initial, next).SetName($"GetNextID_{piece.Item1}_{piece.Item2}_ShouldReturnCorrectID");
            }
        }
    }
}