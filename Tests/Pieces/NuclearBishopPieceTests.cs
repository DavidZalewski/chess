using Chess.Board;
using Chess.Pieces;
using NUnit.Framework;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class NuclearBishopPieceTests
    {
        private readonly BoardPosition startingPos = new BoardPosition(RANK.EIGHT, FILE.H);
        private readonly BoardPosition disabledPos = new BoardPosition(RANK.SIX, FILE.F); // Example disabled square position

        private ChessBoard chessBoard;

        private readonly BoardPosition knightStartingPosC3 = new("C3");
        private readonly BoardPosition knightStartingPosF3 = new("F3");
        private readonly BoardPosition knightStartingPosC6 = new("C6");
        private readonly BoardPosition knightStartingPosF6 = new("F6");

        private readonly BoardPosition blackBishopStartingPosH6 = new("H6");
        private readonly BoardPosition blackBishopStartingPosA6 = new("A6");
        private readonly BoardPosition whiteBishopStartingPosH3 = new("H3");
        private readonly BoardPosition whiteBishopStartingPosA3 = new("A3");

        private NuclearHorsePiece? whiteNuclearHorse = null;
        private NuclearHorsePiece? blackNuclearHorse = null;
        private NuclearBishopPiece? whiteNuclearBishop = null;
        private NuclearBishopPiece? blackNuclearBishop = null;

        [SetUp]
        public void Setup()
        {
            whiteNuclearHorse = new(ChessPiece.Color.WHITE, 1, knightStartingPosC3);
            blackNuclearHorse = new(ChessPiece.Color.BLACK, 1, knightStartingPosC6);
            whiteNuclearBishop = new(ChessPiece.Color.WHITE, 1, whiteBishopStartingPosH3);
            blackNuclearBishop = new(ChessPiece.Color.BLACK, 1, blackBishopStartingPosH6);
            chessBoard = new ChessBoard();
        }

        [Test]
        public void NuclearBishopCannotMoveToDisabledSquare()
        {
            // Arrange
            NuclearBishopPiece nuclearBishop = new NuclearBishopPiece(ChessPiece.Color.WHITE, 1, startingPos);
            DisabledSquarePiece disabledSquare = new DisabledSquarePiece(disabledPos);
            chessBoard.AddPiece(nuclearBishop);
            chessBoard.AddPiece(disabledSquare);

            // Act & Assert
            Assert.That(nuclearBishop.IsValidMove(chessBoard, disabledPos), Is.False, "Nuclear Bishop should not be able to move to a disabled square.");
        }

        [Test]
        public void NuclearBishopCanMoveToEmptySquare()
        {
            // Arrange
            NuclearBishopPiece nuclearBishop = new NuclearBishopPiece(ChessPiece.Color.WHITE, 1, startingPos);
            chessBoard.AddPiece(nuclearBishop);

            // Act & Assert
            BoardPosition validMovePos = new BoardPosition(RANK.SEVEN, FILE.G); // Example valid move position
            Assert.That(nuclearBishop.IsValidMove(chessBoard, validMovePos), Is.True, "Nuclear Bishop should be able to move to an empty square.");
        }

        [Test]
        public void NuclearBishopBlastsDisabledSquaresOnMove()
        {
            // Arrange
            NuclearBishopPiece nuclearBishop = new(ChessPiece.Color.WHITE, 1, startingPos); // H8
            chessBoard.AddPiece(nuclearBishop);
            chessBoard.AddPiece(new DisabledSquarePiece(new("C7")));
            chessBoard.AddPiece(new DisabledSquarePiece(new("G3")));
            chessBoard.AddPiece(new DisabledSquarePiece(new("B2")));

            // Act
            BoardPosition movePos = new(RANK.FIVE, FILE.E); // E5
            nuclearBishop.Move(chessBoard, movePos);

            // Assert
            Assert.That(chessBoard.GetSquare(new BoardPosition("C7")).Piece is NoPiece, Is.True, "Disabled square should be blasted away after Nuclear Bishop moves.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("G3")).Piece is NoPiece, Is.True, "Disabled square should be blasted away after Nuclear Bishop moves.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("B2")).Piece is NoPiece, Is.True, "Disabled square should be blasted away after Nuclear Bishop moves.");
        }
    }
}
