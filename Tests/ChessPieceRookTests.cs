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

        public ChessPieceRookTests()
        {
            board = new ChessBoard();
            a1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            whiteRook2StartPosition = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.A);
            blackRook1StartPosition = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.H);
            blackRook2StartPosition = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.H);
        }

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
            a1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            whiteRook2StartPosition = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.A);
            blackRook1StartPosition = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.H);
            blackRook2StartPosition = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.H);
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
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.A),
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
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 2, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D)); // D4

            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.D),      
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.D),
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
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 1, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.F)); // F4

            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.H),
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
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 2, new BoardPosition(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.A));

            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.F),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.H),
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
                new BoardPosition(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.C));
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.C)
            };

            // Set White Pawn on c4
            board.SetBoardValue(new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.C), 11);

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
                new BoardPosition(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.C));
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.H)
            };

            // Set White Pawn on c4
            board.SetBoardValue(new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.F), 21);

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
                new BoardPosition(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.C));
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.H),
                new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.B),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.F),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.H),
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
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B));
            BoardPosition b8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B);
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that white rook 1 on B4 canotn capture a piece on B8")]
        public void Test_WhiteRook1_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.WHITE, 1,
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B));
            BoardPosition b8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B);
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

        [Test(Description = "Tests that blak rook 2 on B4 can capture a piece on B8")]
        public void Test_BlackRook2_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.BLACK, 2,
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B));
            BoardPosition b8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B);
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that black rook 2 on B4 cannot capture a piece on B8")]
        public void Test_BlackRook2_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.BLACK, 2,
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B));
            BoardPosition b8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B);
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

    }
}
