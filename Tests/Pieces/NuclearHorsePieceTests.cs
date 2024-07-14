using Chess.Board;
using Chess.Pieces;
using Chess.Services;
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
        public void NuclearHorseCreatesDisabledSquaresEmptyBoard()
        {
            ChessBoard chessBoard = new();
            NuclearHorsePiece nuclearHorse = new(ChessPiece.Color.WHITE, 1, e4); // start on E4
            chessBoard.AddPiece(nuclearHorse);

            // Move the Nuclear Horse to d6
            BoardPosition d6 = new(RANK.SIX, FILE.D);
            Assert.That(nuclearHorse.IsValidMove(chessBoard, d6), Is.True);
            nuclearHorse.Move(chessBoard, d6);

            // Check if the adjacent squares are disabled
            Assert.That(chessBoard.GetSquare(new BoardPosition("D5")).Piece is DisabledSquarePiece, Is.True, "Square D5 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("D4")).Piece is DisabledSquarePiece, Is.True, "Square D4 should be disabled.");

            Assert.That(chessBoard.GetSquare(new BoardPosition("D7")).Piece is DisabledSquarePiece, Is.True, "Square D7 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("D8")).Piece is DisabledSquarePiece, Is.True, "Square D8 should be disabled.");

            Assert.That(chessBoard.GetSquare(new BoardPosition("E6")).Piece is DisabledSquarePiece, Is.True, "Square E6 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("F6")).Piece is DisabledSquarePiece, Is.True, "Square F6 should be disabled.");

            Assert.That(chessBoard.GetSquare(new BoardPosition("B6")).Piece is DisabledSquarePiece, Is.True, "Square B6 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("C6")).Piece is DisabledSquarePiece, Is.True, "Square C6 should be disabled.");
            
            Assert.That(chessBoard.GetSquare(new BoardPosition("B7")).Piece is DisabledSquarePiece, Is.True, "Square B7 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("C7")).Piece is DisabledSquarePiece, Is.True, "Square C7 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("C8")).Piece is DisabledSquarePiece, Is.True, "Square C8 should be disabled.");

            Assert.That(chessBoard.GetSquare(new BoardPosition("E7")).Piece is DisabledSquarePiece, Is.True, "Square E7 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("F7")).Piece is DisabledSquarePiece, Is.True, "Square F7 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("E8")).Piece is DisabledSquarePiece, Is.True, "Square E8 should be disabled.");

        }

        [Test]
        public void NuclearHorseCreatesDisabledSquaresFullBoard()
        {
            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(ChessPieceFactory.CreateChessPiecesNuclearHorse());
            NuclearHorsePiece nuclearHorse = new(ChessPiece.Color.WHITE, 3, e4); // start on E4
            chessBoard.AddPiece(nuclearHorse);

            // Move the Nuclear Horse to d6
            BoardPosition d6 = new(RANK.SIX, FILE.D);
            Assert.That(nuclearHorse.IsValidMove(chessBoard, d6), Is.True);
            nuclearHorse.Move(chessBoard, d6);

            // Check if the adjacent squares are disabled
            Assert.That(chessBoard.GetSquare(new BoardPosition("D5")).Piece is DisabledSquarePiece, Is.True, "Square D5 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("D4")).Piece is DisabledSquarePiece, Is.True, "Square D4 should be disabled.");

            Assert.That(chessBoard.GetSquare(new BoardPosition("D7")).Piece is DisabledSquarePiece, Is.False, "Square D7 should NOT be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("D8")).Piece is DisabledSquarePiece, Is.False, "Square D8 should NOT be disabled.");

            Assert.That(chessBoard.GetSquare(new BoardPosition("E6")).Piece is DisabledSquarePiece, Is.True, "Square E6 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("F6")).Piece is DisabledSquarePiece, Is.True, "Square F6 should be disabled.");

            Assert.That(chessBoard.GetSquare(new BoardPosition("B6")).Piece is DisabledSquarePiece, Is.True, "Square B6 should be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("C6")).Piece is DisabledSquarePiece, Is.True, "Square C6 should be disabled.");

            Assert.That(chessBoard.GetSquare(new BoardPosition("B7")).Piece is DisabledSquarePiece, Is.False, "Square B7 should NOT be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("C7")).Piece is DisabledSquarePiece, Is.False, "Square C7 should NOT be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("C8")).Piece is DisabledSquarePiece, Is.False, "Square C8 should NOT be disabled.");

            Assert.That(chessBoard.GetSquare(new BoardPosition("E7")).Piece is DisabledSquarePiece, Is.False, "Square E7 should NOT be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("F7")).Piece is DisabledSquarePiece, Is.False, "Square F7 should NOT be disabled.");
            Assert.That(chessBoard.GetSquare(new BoardPosition("E8")).Piece is DisabledSquarePiece, Is.False, "Square E8 should NOT be disabled.");

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
