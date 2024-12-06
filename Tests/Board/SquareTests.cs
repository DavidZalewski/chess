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
            // Arrange
            var position = new BoardPosition(RANK.THREE, FILE.E);
            var piece = new ChessPieceKing(ChessPiece.Color.WHITE, position);

            // Act
            var square = new Square(position, piece);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(square.Position.Rank, Is.EqualTo(RANK.THREE));
                Assert.That(square.Position.File, Is.EqualTo(FILE.E));
                Assert.That(square.Piece, Is.EqualTo(piece));
            });
        }

        [Test]
        public void SquareConstructorWithPiece_Success()
        {
            // Arrange
            var position = new BoardPosition(RANK.TWO, FILE.D);
            var piece = new ChessPieceKing(ChessPiece.Color.BLACK, position);

            // Act
            var square = new Square(piece);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(square.Position.Rank, Is.EqualTo(RANK.TWO));
                Assert.That(square.Position.File, Is.EqualTo(FILE.D));
                Assert.That(square.Piece, Is.EqualTo(piece));
            });
        }

        [Test]
        public void SquareConstructorWithOtherSquare_Success()
        {
            // Arrange
            var originalPosition = new BoardPosition(RANK.ONE, FILE.C);
            var originalPiece = new ChessPieceKing(ChessPiece.Color.WHITE, originalPosition);
            var originalSquare = new Square(originalPosition, originalPiece);

            // Act
            var copiedSquare = new Square(originalSquare);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(copiedSquare.Position.Rank, Is.EqualTo(RANK.ONE));
                Assert.That(copiedSquare.Position.File, Is.EqualTo(FILE.C));
                Assert.That(copiedSquare.Piece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(copiedSquare.Piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(copiedSquare.Piece, Is.Not.SameAs(originalPiece)); // Ensure it's a clone
            });
        }

        [Test]
        public void SquareConstructorWithNullOtherSquare_Success()
        {
            // Act
            var square = new Square(null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(square.Position, Is.Null);
                Assert.That(square.Piece, Is.EqualTo(NoPiece.Instance));
            });
        }
    }
}