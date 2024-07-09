using Chess.Board;
using Chess.Pieces;
using NUnit.Framework;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class NuclearHorsePieceTests
    {
        private readonly BoardPosition d4 = new(RANK.FOUR, FILE.D);
        private readonly BoardPosition e4 = new(RANK.FOUR, FILE.E);
        private readonly BoardPosition c4 = new(RANK.FOUR, FILE.C);

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NuclearHorseCreatesDisabledSquares()
        {
            ChessBoard chessBoard = new ChessBoard();
            NuclearHorsePiece nuclearHorse = new NuclearHorsePiece(ChessPiece.Color.WHITE, 1, e4); // start on E4
            chessBoard.AddPiece(nuclearHorse);

            // Move the Nuclear Horse to d6
            BoardPosition d6 = new BoardPosition(RANK.SIX, FILE.D);
            Assert.That(nuclearHorse.IsValidMove(chessBoard, d6), Is.True);
            nuclearHorse.Move(chessBoard, d6);

            ChessPiece p = chessBoard.GetSquare(new BoardPosition(RANK.SEVEN, FILE.D)).Piece;

            Console.WriteLine(p.ToString);
            // Check if the adjacent squares are disabled
            Assert.That(chessBoard.GetSquare(new BoardPosition(RANK.SEVEN, FILE.D)).Piece is DisabledSquarePiece, Is.True, "Square d7 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition(RANK.FIVE, FILE.D)).Piece is DisabledSquarePiece, Is.True, "Square d5 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition(RANK.SIX, FILE.C)).Piece is DisabledSquarePiece, Is.True, "Square c6 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition(RANK.SIX, FILE.E)).Piece is DisabledSquarePiece, Is.True, "Square e6 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition(RANK.SEVEN, FILE.C)).Piece is DisabledSquarePiece, Is.True, "Square c7 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition(RANK.SEVEN, FILE.E)).Piece is DisabledSquarePiece, Is.True, "Square e7 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition(RANK.FIVE, FILE.C)).Piece is DisabledSquarePiece, Is.True, "Square c5 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition(RANK.FIVE, FILE.E)).Piece is DisabledSquarePiece, Is.True, "Square e5 should be disabled.");
        }

        [Test]
        public void NuclearHorseCannotMoveToDisabledSquare()
        {
            ChessBoard chessBoard = new ChessBoard();
            NuclearHorsePiece nuclearHorse = new NuclearHorsePiece(ChessPiece.Color.WHITE, 1, e4);
            DisabledSquarePiece disabledSquarePiece = new DisabledSquarePiece(d4);
            chessBoard.AddPiece(nuclearHorse);
            chessBoard.AddPiece(disabledSquarePiece);

            Assert.That(nuclearHorse.IsValidMove(chessBoard, d4), Is.False, "Nuclear Horse should not be able to move to a disabled square.");
        }

        [Test]
        public void NuclearHorseCannotMoveOverDisabledSquare()
        {
            ChessBoard chessBoard = new();
            NuclearHorsePiece nuclearHorse = new(ChessPiece.Color.WHITE, 1, new BoardPosition(RANK.SIX, FILE.E));
            DisabledSquarePiece disabledSquarePiece1 = new(new BoardPosition(RANK.FIVE, FILE.D));
            DisabledSquarePiece disabledSquarePiece2 = new(new BoardPosition(RANK.FIVE, FILE.E));
            chessBoard.AddPiece(nuclearHorse);
            chessBoard.AddPiece(disabledSquarePiece1);
            chessBoard.AddPiece(disabledSquarePiece2);

            Assert.That(nuclearHorse.IsValidMove(chessBoard, d4), Is.False, "Nuclear Horse should not be able to jump over a disabled square.");
        }
    }
}
