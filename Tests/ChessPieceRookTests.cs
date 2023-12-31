using Chess;
using static Chess.ChessPiece;

namespace Tests
{
    public class ChessPieceRookTests
    {
        private ChessBoard board;
        private BoardPosition a1;
        private BoardPosition whiteRook2StartPosition;
        private BoardPosition blackRook1StartPosition;
        private BoardPosition blackRook2StartPosition;

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
            a1 = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.ONE);
            whiteRook2StartPosition = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.EIGHT);
            blackRook1StartPosition = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.ONE);
            blackRook2StartPosition = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.EIGHT);
        }

        [Test]
        public void Test_ConstructWhiteRook_Success()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1, a1);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(a1));
                Assert.That(piece.GetRealValue(), Is.EqualTo(14)); // 14 is rook
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.ROOK));
            });
        }

        [Test(Description = "Test that white rook 1 on A1 can move horizontally across all A squares")]
        public void Test_Rook_IsValidMove_HorizontalFromA1()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1, a1); // A1

            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.TWO),
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.THREE),
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.FOUR),
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.SIX),
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.SEVEN),
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.EIGHT),
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition position in positions)
                {
                    Assert.That(piece.IsValidMove(board, position), Is.True);
                }
            });
        }

        [Test(Description = "Test that black rook 2 on D4 can move horizontally across all D squares")]
        public void Test_Rook_IsValidMove_HorizontalFromD4()
        {
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 2, new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR)); // D4

            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.ONE),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.TWO),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.THREE),      
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.SIX),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.SEVEN),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.EIGHT),
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition position in positions)
                {
                    Assert.That(piece.IsValidMove(board, position), Is.True);
                }
            });
        }

        [Test(Description = "Test that black rook 1 on F4 can move vertically across all 4 squares (E4, D4, C4, B4, A4, G4, H4)")]
        public void Test_Rook_IsValidMove_VerticalFromF4()
        {
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 1, new BoardPosition(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.FOUR)); // F4

            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.FOUR),
                new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR),
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FOUR),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR),
                new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR),
                new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.FOUR),
                new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.FOUR),
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition position in positions)
                {
                    Assert.That(piece.IsValidMove(board, position), Is.True);
                }
            });
        }

        [Test(Description = "Test that white rook 2 on A8 can move vertically across all 8 squares (E8, D8, C8, B8, A8, G8, H8)")]
        public void Test_Rook_IsValidMove_VerticalFromA8()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 2, new BoardPosition(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.EIGHT));

            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.EIGHT),
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.EIGHT),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.EIGHT),
                new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.EIGHT),
                new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.EIGHT),
                new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.EIGHT),
                new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.EIGHT),
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition position in positions)
                {
                    Assert.That(piece.IsValidMove(board, position), Is.True);
                }
            });
        }

        [Test(Description = "Tests that white rook 1 on C2 cannot move to C6, or C5 because there is a white piece on C4 blocking its path")]
        public void Test_WhiteRook1_IsValidMove_CannotMoveBlockedByFriendly()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.WHITE, 1, 
                new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.TWO));
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.SIX)
            };

            // Set White Pawn on c4
            board.SetBoardValue(new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FOUR), 11);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white rook 1 on C2 cannot move to G2, or H2 because there is a black piece on F2 blocking its path")]
        public void Test_WhiteRook1_IsValidMove_CannotMoveBlockedByEnemy()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.WHITE, 1,
                new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.TWO));
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.TWO),
                new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.TWO)
            };

            // Set White Pawn on c4
            board.SetBoardValue(new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.TWO), 21);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that rook on C2 cannot move to these squares as they are invalid")]
        public void Test_WhiteRook1_IsValidMove_InvalidMoves()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.WHITE, 1,
                new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.TWO));
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.THREE),
                new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.ONE),
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.ONE),
                new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.THREE),
                new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR),
                new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.SIX),
                new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.SEVEN),
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white rook 1 on B4 can capture a piece on B8")]
        public void Test_WhiteRook1_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.WHITE, 1,
                new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR));
            BoardPosition b8 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.EIGHT);
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that white rook 1 on B4 canotn capture a piece on B8")]
        public void Test_WhiteRook1_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.WHITE, 1,
                new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR));
            BoardPosition b8 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.EIGHT);
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

        [Test(Description = "Tests that blak rook 2 on B4 can capture a piece on B8")]
        public void Test_BlackRook2_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.BLACK, 2,
                new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR));
            BoardPosition b8 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.EIGHT);
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that black rook 2 on B4 cannot capture a piece on B8")]
        public void Test_BlackRook2_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.BLACK, 2,
                new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR));
            BoardPosition b8 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.EIGHT);
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

    }
}
