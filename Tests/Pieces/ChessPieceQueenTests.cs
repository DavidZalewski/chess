using Chess.Board;
using Chess.Pieces;
using static Chess.Pieces.ChessPiece;

namespace Tests.Pieces
{
    public class ChessPieceQueenTests
    {
        private ChessBoard board = new ChessBoard();
        private BoardPosition a3 = new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.A);
        private BoardPosition a6 = new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.A);
        private BoardPosition h3 = new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.H);
        private BoardPosition h6 = new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.H);
        private BoardPosition a1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);

        public ChessPieceQueenTests()
        {
            Setup();
        }

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
        }

        [Test]
        public void Test_ConstructWhiteQueen_Success()
        {
            BoardPosition boardPosition = a3;
            ChessPiece piece = new ChessPieceQueen(Color.WHITE, 1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(piece.GetRealValue(), Is.EqualTo(15));
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(Color.WHITE));
                Assert.That(piece.GetPiece(), Is.EqualTo(Piece.QUEEN));
            });
        }

        [Test]
        public void Test_ConstructBlackQueen_Success()
        {
            BoardPosition boardPosition = h3;
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(piece.GetRealValue(), Is.EqualTo(25));
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(Color.BLACK));
                Assert.That(piece.GetPiece(), Is.EqualTo(Piece.QUEEN));
            });
        }

        [Test(Description = "Tests whether the white queen 1 can move to C1 from A3 with no other pieces on board")]
        public void Test_WhiteQueen1_IsValidMove_StartingMove1()
        {
            ChessPiece piece = new ChessPieceQueen(Color.WHITE, 1, a3);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black queen 1 can move to B4 from A3 with no other pieces on board")]
        public void Test_BlackQueen1_IsValidMove_StartingMove2()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a3);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that black queen 1 cannot move because there are pieces blocking its path")]
        public void Test_BlackQueen1_IsValidMove_CannotMoveBlocked()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a3);
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.B),
                new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.F),
            };

            // Set Black Pawns on B2 and B4 which block Bishop from moving
            board.SetBoardValue(new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.B), 21);
            board.SetBoardValue(new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B), 11);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that black queen 1 cannot jump over a black piece on a diagonal")]
        public void Test_BlackQueen1_IsValidMove_CannotJumpPieces()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a3);
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.F),
            };

            // Set Black Pawn on B4 which block Queen
            board.SetBoardValue(new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B), 21);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that black queen 1 can capture")]
        public void Test_WhiteQueen1_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a3);
            BoardPosition d6 = new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.D);

            // Set White Pawn on D6
            board.SetBoardValue(d6, 11);

            Assert.That(piece.IsValidMove(board, d6), Is.True);
        }


        [Test(Description = "Tests that white queen can move to all of these valid locations if starting from D4")]
        public void Test_WhiteQueen1_IsValidMove_FromD4()
        {
            ChessPiece piece = new ChessPieceQueen(Color.WHITE, 1, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D));

            // valid positions if bishop on D4
            // C3, B2, A1, C5, B6, A7, E3, F2, G1, E5, F6, G7, H8 
            List<BoardPosition> validPositions = new()
            {
                new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.B),
                new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.B),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.F),
                new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.F),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.H)
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in validPositions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.True);
                }
            });
        }

        [Test(Description = "Test that black queen 1 on A1 can move horizontally across all A squares")]
        public void Test_BlackQueen_IsValidMove_HorizontalFromA1()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a1); // A1

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

        [Test(Description = "Test that black queen 2 on D4 can move horizontally across all D squares")]
        public void Test_Rook_IsValidMove_HorizontalFromD4()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D)); // D4

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

        [Test(Description = "Test that black queen 1 on F4 can move vertically across all 4 squares (E4, D4, C4, B4, A4, G4, H4)")]
        public void Test_BlackQueen_IsValidMove_VerticalFromF4()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.F)); // F4

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

        [Test(Description = "Test that black queen 2 on A8 can move vertically across all 8 squares (E8, D8, C8, B8, A8, G8, H8)")]
        public void Test_BlackQueen_IsValidMove_VerticalFromA8()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2, new BoardPosition(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.A));

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

        [Test(Description = "Tests that black queen 1 on C2 cannot move to C6, or C5 because there is a white piece on C4 blocking its path")]
        public void Test_BlackQueen1_IsValidMove_CannotMoveBlockedByFriendly()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1,
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

        [Test(Description = "Tests that black queen 1 on C2 cannot move to G2, or H2 because there is a black piece on F2 blocking its path")]
        public void Test_BlackQueen1_IsValidMove_CannotMoveBlockedByEnemy()
        {
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 1,
                new BoardPosition(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.C));
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.H)
            };

            // Set White Pawn on c4
            board.SetBoardValue(new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.F), 11);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white queen on C2 cannot move to these squares as they are invalid")]
        public void Test_WhiteQueen1_IsValidMove_InvalidMoves()
        {
            ChessPiece piece = new ChessPieceQueen(Color.WHITE, 1,
                new BoardPosition(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.C));
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.H),
                new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.F),
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.G),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.H),
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that black queen 1 on B4 can capture a piece on B8")]
        public void Test_BlackQueen1_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1,
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B));
            BoardPosition b8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B);
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that black queen 1 on B4 canotn capture a piece on B8")]
        public void Test_BlackQueen1_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1,
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B));
            BoardPosition b8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B);
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

        [Test(Description = "Tests that black queen 2 on B4 can capture a piece on B8")]
        public void Test_BlackQueen2_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2,
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B));
            BoardPosition b8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B);
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that black queen 2 on B4 cannot capture a piece on B8")]
        public void Test_BlackQueen2_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2,
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B));
            BoardPosition b8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B);
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

    }
}