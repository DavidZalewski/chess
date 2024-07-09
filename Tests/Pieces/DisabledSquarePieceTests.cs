using Chess.Board;
using Chess.Pieces;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class DisabledSquarePieceTests
    {
        private readonly BoardPosition d4 = new(RANK.FOUR, FILE.D);
        private readonly BoardPosition e4 = new(RANK.FOUR, FILE.E);
        private readonly BoardPosition c4 = new(RANK.FOUR, FILE.C);

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConstructDisabledSquarePiece_Success()
        {
            ChessPiece disabledSquarePiece = new DisabledSquarePiece(d4);

            Assert.That(disabledSquarePiece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(disabledSquarePiece.GetCurrentPosition(), Is.EqualTo(d4));
                Assert.That(disabledSquarePiece.GetRealValue(), Is.EqualTo(-1));
                Assert.That(disabledSquarePiece.GetColor(), Is.EqualTo(ChessPiece.Color.NONE));
                Assert.That(disabledSquarePiece.GetPiece(), Is.EqualTo(ChessPiece.Piece.NO_PIECE));
            });
        }

        [Test]
        public void PlaceDisabledSquarePiece_OnBoard()
        {
            ChessBoard chessBoard = new();
            ChessPiece disabledSquarePiece = new DisabledSquarePiece(d4);
            chessBoard.AddPiece(disabledSquarePiece);

            Assert.That(chessBoard.GetSquare(d4).Piece, Is.EqualTo(disabledSquarePiece));
        }

        [Test]
        public void PiecesCannotMoveToDisabledSquare()
        {
            ChessBoard chessBoard = new ChessBoard();
            ChessPiece disabledSquarePiece = new DisabledSquarePiece(d4);
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, e4);
            chessBoard.AddPiece(disabledSquarePiece);
            chessBoard.AddPiece(whiteKingPiece);

            Assert.That(whiteKingPiece.IsValidMove(chessBoard, d4), Is.False, "White King should not be able to move to a disabled square.");
        }

        [Test]
        public void PiecesCannotMoveOverDisabledSquareQueen()
        {
            ChessBoard chessBoard = new ChessBoard();
            ChessPiece disabledSquarePiece = new DisabledSquarePiece(d4);
            ChessPiece whiteQueenPiece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, c4);
            chessBoard.AddPiece(disabledSquarePiece);
            chessBoard.AddPiece(whiteQueenPiece);
            Assert.That(whiteQueenPiece.IsValidMove(chessBoard, new("G4")), Is.False, "White Queen should not be able to move over a disabled square.");
        }

        [Test]
        public void PiecesCannotMoveOverDisabledSquarePawn()
        {
            BoardPosition d2 = new("D2");
            BoardPosition d5 = new("D5");
            ChessBoard chessBoard = new ChessBoard();
            ChessPiece disabledSquarePiece = new DisabledSquarePiece(d4);
            ChessPiece whitePawn = new ChessPieceWhitePawn(1, d2);
            ChessPiece blackPawn = new ChessPieceBlackPawn(1, d5);
            chessBoard.AddPiece(disabledSquarePiece);
            chessBoard.AddPiece(whitePawn);
            chessBoard.AddPiece(blackPawn);

            Assert.That(whitePawn.IsValidMove(chessBoard, d4), Is.False, "White Pawn should not be able to move over a disabled square.");
            Assert.That(blackPawn.IsValidMove(chessBoard, d4), Is.False, "Black Pawn should not be able to move over a disabled square.");
        }

        [Test]
        public void PiecesCannotMoveOverDisabledSquarePawnDoubleJump()
        {
            BoardPosition d2 = new("D2");
            BoardPosition d3 = new("D3");
            BoardPosition e7 = new("E7");
            BoardPosition e6 = new("E6");
     
            ChessPiece disabledSquarePiece1 = new DisabledSquarePiece(d3);
            ChessPiece disabledSquarePiece2 = new DisabledSquarePiece(e6);
            ChessPiece whitePawn = new ChessPieceWhitePawn(1, d2);
            ChessPiece blackPawn = new ChessPieceBlackPawn(1, e7);

            ChessBoard chessBoard = new();
            chessBoard.AddPiece(disabledSquarePiece1);
            chessBoard.AddPiece(disabledSquarePiece2);
            chessBoard.AddPiece(whitePawn);
            chessBoard.AddPiece(blackPawn);

            Assert.That(whitePawn.IsValidMove(chessBoard, d4), Is.False, "White Pawn should not be able to double jump over a disabled square.");
            Assert.That(blackPawn.IsValidMove(chessBoard, new("E5")), Is.False, "Black Pawn should not be able to double jump over a disabled square.");
        }

        [Test]
        public void PiecesCannotMoveOverDisabledSquarePawnDoubleJump2()
        {
            BoardPosition d2 = new("D2");
            BoardPosition d4 = new("D4");
            BoardPosition e7 = new("E7");
            BoardPosition e5 = new("E5");

            ChessPiece disabledSquarePiece1 = new DisabledSquarePiece(d4);
            ChessPiece disabledSquarePiece2 = new DisabledSquarePiece(e5);
            ChessPiece whitePawn = new ChessPieceWhitePawn(1, d2);
            ChessPiece blackPawn = new ChessPieceBlackPawn(1, e7);

            ChessBoard chessBoard = new();
            chessBoard.AddPiece(disabledSquarePiece1);
            chessBoard.AddPiece(disabledSquarePiece2);
            chessBoard.AddPiece(whitePawn);
            chessBoard.AddPiece(blackPawn);

            Assert.That(whitePawn.IsValidMove(chessBoard, d4), Is.False, "White Pawn should not be able to double jump over a disabled square.");
            Assert.That(blackPawn.IsValidMove(chessBoard, new("E5")), Is.False, "Black Pawn should not be able to double jump over a disabled square.");
        }


        [Test]
        public void PiecesCannotMoveOverDisabledSquareRook()
        {
            ChessBoard chessBoard = new ChessBoard();
            ChessPiece disabledSquarePiece1 = new DisabledSquarePiece(d4);
            ChessPiece disabledSquarePiece2 = new DisabledSquarePiece(new ("C3"));
            ChessPiece whiteRookPiece = new ChessPieceRook(ChessPiece.Color.WHITE, 1, c4);
            chessBoard.AddPiece(disabledSquarePiece1);
            chessBoard.AddPiece(disabledSquarePiece2);
            chessBoard.AddPiece(whiteRookPiece);

            BoardPosition d5 = new(RANK.FIVE, FILE.D); // A position white rook would move to if d4 wasn't disabled.

            Assert.That(whiteRookPiece.IsValidMove(chessBoard, d5), Is.False, "White Rook should not be able to move over a disabled square.");
        }

        [Test]
        public void PiecesCannotMoveOverDisabledSquareBishop()
        {
            ChessBoard chessBoard = new();
            ChessPiece disabledSquarePiece1 = new DisabledSquarePiece(new BoardPosition("D3"));
            ChessPiece disabledSquarePiece2 = new DisabledSquarePiece(new BoardPosition("B5"));

            ChessPiece whiteBishopPiece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, c4);
            chessBoard.AddPiece(disabledSquarePiece1);
            chessBoard.AddPiece(disabledSquarePiece2);
            chessBoard.AddPiece(whiteBishopPiece);

            Assert.That(whiteBishopPiece.IsValidMove(chessBoard, new("E2")), Is.False, "White Bishop should not be able to move over a disabled square.");
            Assert.That(whiteBishopPiece.IsValidMove(chessBoard, new("A6")), Is.False, "White Bishop should not be able to move over a disabled square.");
        }

        [Test]
        public void PiecesCannotMoveToDisabledSquareKnight()
        {
            ChessBoard chessBoard = new();
            ChessPiece disabledSquarePiece = new DisabledSquarePiece(d4);
            ChessPiece whiteKnightPiece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new("E6"));
            chessBoard.AddPiece(disabledSquarePiece);
            chessBoard.AddPiece(whiteKnightPiece);

            Assert.That(whiteKnightPiece.IsValidMove(chessBoard, d4), Is.False, "White Knight should not be able to move to a disabled square.");
        }

        [Test]
        [Ignore("This behavior is implemented in NuclearHorsePiece instead")]
        public void PiecesCannotMoveToDisabledSquareKnightCannotJumpOver()
        {
            ChessBoard chessBoard = new();
            ChessPiece disabledSquarePiece1 = new DisabledSquarePiece(new("D5"));
            ChessPiece disabledSquarePiece2 = new DisabledSquarePiece(new("E5"));

            ChessPiece whiteKnightPiece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new("E6"));
            chessBoard.AddPiece(disabledSquarePiece1);
            chessBoard.AddPiece(disabledSquarePiece2);
            chessBoard.AddPiece(whiteKnightPiece);

            Assert.That(whiteKnightPiece.IsValidMove(chessBoard, d4), Is.False, "White Knight should not be able to jump over a disabled square.");
        }

    }
}