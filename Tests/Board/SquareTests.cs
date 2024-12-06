using Chess.Board;
using Chess.Pieces;

namespace Tests.Board
{
    [Category("CORE")]
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

        [Test]
        public void SquareConstructorWithPositionAndPiece_Success()
        {
            var position = new BoardPosition(RANK.FOUR, FILE.F);
            var piece = new ChessPieceWhitePawn(1, position);

            var square = new Square(position, piece);

            Assert.Multiple(() =>
            {
                Assert.That(square.Position, Is.EqualTo(position));
                Assert.That(square.Piece, Is.EqualTo(piece));
            });
        }

        [Test]
        public void SquareConstructorWithPiece_Success()
        {
            var position = new BoardPosition(RANK.FOUR, FILE.F);
            var piece = new ChessPieceWhitePawn(1, position);

            var square = new Square(piece);

            Assert.Multiple(() =>
            {
                Assert.That(square.Position, Is.EqualTo(position));
                Assert.That(square.Piece, Is.EqualTo(piece));
            });
        }

        [Test]
        public void SquareConstructorWithOtherSquare_Success()
        {
            var position = new BoardPosition(RANK.FOUR, FILE.F);
            var piece = new ChessPieceWhitePawn(1, position);
            var originalSquare = new Square(position, piece);

            var copiedSquare = new Square(originalSquare);

            Assert.Multiple(() =>
            {
                Assert.That(copiedSquare.Position, Is.EqualTo(originalSquare.Position));
                Assert.That(copiedSquare.Piece, Is.Not.SameAs(originalSquare.Piece)); // Cloned piece should be different object
                Assert.That(copiedSquare.Piece, Is.EqualTo(originalSquare.Piece)); // But should have same properties
            });
        }
    }
}
