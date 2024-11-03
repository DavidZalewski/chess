using NUnit.Framework;
using Chess.GameState;
using Chess.Pieces;

namespace Chess.Tests
{
    public class PromotionTrackerTests
    {
        [TestCase(ChessPiece.Color.WHITE, "Q", 2)]
        [TestCase(ChessPiece.Color.BLACK, "Q", 2)]
        [TestCase(ChessPiece.Color.WHITE, "R", 3)]
        [TestCase(ChessPiece.Color.BLACK, "R", 3)]
        [TestCase(ChessPiece.Color.WHITE, "K", 3)]
        [TestCase(ChessPiece.Color.BLACK, "K", 3)]
        [TestCase(ChessPiece.Color.WHITE, "B", 3)]
        [TestCase(ChessPiece.Color.BLACK, "B", 3)]
        public void GetNextID_ShouldReturnCorrectIdAndIncrementCounter(ChessPiece.Color color, string pieceType, int initialCount)
        {
            // Arrange
            var promotionTracker = new PromotionTracker();
            string key = $"{(color == ChessPiece.Color.WHITE ? "W" : "B")}{pieceType}";

            // Act
            int firstId = promotionTracker.GetNextID(color, pieceType);
            int secondId = promotionTracker.GetNextID(color, pieceType);

            // Assert
            Assert.That(firstId, Is.EqualTo(initialCount), "The first ID should match the initial count.");
            Assert.That(secondId, Is.EqualTo(initialCount + 1), "The second ID should be incremented by 1.");
        }
    }
}