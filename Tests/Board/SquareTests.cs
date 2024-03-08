using Chess.Board;
using Chess.Pieces;

namespace Tests.Board
{
    internal class SquareTests
    {
        [Test]
        public void SquareIsInitializedCorrectly()
        {
            var square = new Square()
            {
                Position = new BoardPosition(RANK.FOUR, FILE.F),
                Piece = NoPiece.Instance
            };

            Assert.Multiple(() =>
            {
                Assert.That(RANK.FOUR == square.Position.Rank);
                Assert.That(FILE.F == square.Position.File);
                Assert.That(NoPiece.Instance == square.Piece);
            });
        }
    }
}
