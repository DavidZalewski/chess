using Chess.Board;
using Chess.Pieces;
using static Chess.Pieces.ChessPiece;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class RookTests : TestBase
    {
        private ChessBoard board;
        private BoardPosition a1;
        private BoardPosition whiteRook2StartPosition;
        private BoardPosition blackRook1StartPosition;
        private BoardPosition blackRook2StartPosition;

        public RookTests()
        {
            board = new ChessBoard();
            a1 = new(RANK.ONE, FILE.A);
            whiteRook2StartPosition = new(RANK.EIGHT, FILE.A);
            blackRook1StartPosition = new(RANK.ONE, FILE.H);
            blackRook2StartPosition = new(RANK.EIGHT, FILE.H);
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            board = new ChessBoard();
            a1 = new(RANK.ONE, FILE.A);
            whiteRook2StartPosition = new(RANK.EIGHT, FILE.A);
            blackRook1StartPosition = new(RANK.ONE, FILE.H);
            blackRook2StartPosition = new(RANK.EIGHT, FILE.H);
        }

        [Test]
        public void ConstructWhiteRook_Success()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1, a1);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(a1));
                Assert.That(piece.GetRealValue(), Is.EqualTo(14)); // 14 is rook
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(Color.WHITE));
                Assert.That(piece.GetPiece(), Is.EqualTo(Piece.ROOK));
            });
        }

        [Test]
        public void CloneWhiteRook_Success()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1, a1);
            ChessPiece clone = piece.Clone();
    
            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, piece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(a1));
                Assert.That(clone.GetRealValue(), Is.EqualTo(14)); // 14 is rook
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(Color.WHITE));
                Assert.That(clone.GetPiece(), Is.EqualTo(Piece.ROOK));
            });
        }

        [Test(Description = "Test that white rook 1 on A1 can move horizontally across all A squares")]
        public void Rook_IsValidMove_HorizontalFromA1()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1, a1); // A1

            List<BoardPosition> positions = new()
            {
                new(RANK.TWO, FILE.A),
                new(RANK.THREE, FILE.A),
                new(RANK.FOUR, FILE.A),
                new(RANK.FIVE, FILE.A),
                new(RANK.SIX, FILE.A),
                new(RANK.SEVEN, FILE.A),
                new(RANK.EIGHT, FILE.A),
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
        public void Rook_IsValidMove_HorizontalFromD4()
        {
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 2, new BoardPosition(RANK.FOUR, FILE.D)); // D4

            List<BoardPosition> positions = new()
            {
                new(RANK.ONE, FILE.D),
                new(RANK.TWO, FILE.D),
                new(RANK.THREE, FILE.D),
                new(RANK.FIVE, FILE.D),
                new(RANK.SIX, FILE.D),
                new(RANK.SEVEN, FILE.D),
                new(RANK.EIGHT, FILE.D),
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
        public void Rook_IsValidMove_VerticalFromF4()
        {
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 1, new BoardPosition(RANK.FOUR, FILE.F)); // F4

            List<BoardPosition> positions = new()
            {
                new(RANK.FOUR, FILE.A),
                new(RANK.FOUR, FILE.B),
                new(RANK.FOUR, FILE.C),
                new(RANK.FOUR, FILE.D),
                new(RANK.FOUR, FILE.E),
                new(RANK.FOUR, FILE.G),
                new(RANK.FOUR, FILE.H),
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
        public void Rook_IsValidMove_VerticalFromA8()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 2, new BoardPosition(RANK.EIGHT, FILE.A));

            List<BoardPosition> positions = new()
            {
                new(RANK.EIGHT, FILE.B),
                new(RANK.EIGHT, FILE.C),
                new(RANK.EIGHT, FILE.D),
                new(RANK.EIGHT, FILE.E),
                new(RANK.EIGHT, FILE.F),
                new(RANK.EIGHT, FILE.G),
                new(RANK.EIGHT, FILE.H),
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
        public void WhiteRook1_IsValidMove_CannotMoveBlockedByFriendly()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1,
                new BoardPosition(RANK.TWO, FILE.C));
            List<BoardPosition> positions = new()
            {
                new(RANK.FIVE, FILE.C),
                new(RANK.SIX, FILE.C)
            };

            // Set White Pawn on c4

            board.SetBoardValue(new(RANK.FOUR, FILE.C), 11);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white rook 1 on C2 cannot move to G2, or H2 because there is a black piece on F2 blocking its path")]
        public void WhiteRook1_IsValidMove_CannotMoveBlockedByEnemy()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1,
                new BoardPosition(RANK.TWO, FILE.C));
            List<BoardPosition> positions = new()
            {
                new(RANK.TWO, FILE.G),
                new(RANK.TWO, FILE.H)
            };

            // Set White Pawn on c4

            board.SetBoardValue(new(RANK.TWO, FILE.F), 21);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that rook on C2 cannot move to these squares as they are invalid")]
        public void WhiteRook1_IsValidMove_InvalidMoves()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1,
                new BoardPosition(RANK.TWO, FILE.C));
            List<BoardPosition> positions = new()
            {
                new(RANK.THREE, FILE.G),
                new(RANK.ONE, FILE.H),
                new(RANK.ONE, FILE.A),
                new(RANK.THREE, FILE.B),
                new(RANK.FOUR, FILE.E),
                new(RANK.FIVE, FILE.F),
                new(RANK.SIX, FILE.G),
                new(RANK.SEVEN, FILE.H),
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
        public void WhiteRook1_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1,
                new BoardPosition(RANK.FOUR, FILE.B));
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);

            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that white rook 1 on B4 canotn capture a piece on B8")]
        public void WhiteRook1_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceRook(Color.WHITE, 1,
                new BoardPosition(RANK.FOUR, FILE.B));
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);

            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

        [Test(Description = "Tests that blak rook 2 on B4 can capture a piece on B8")]
        public void BlackRook2_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 2,
                new BoardPosition(RANK.FOUR, FILE.B));
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);

            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that black rook 2 on B4 cannot capture a piece on B8")]
        public void BlackRook2_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 2,
                new BoardPosition(RANK.FOUR, FILE.B));
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);

            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

    }
}
