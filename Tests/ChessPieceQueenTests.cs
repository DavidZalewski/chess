using Chess;
using static Chess.ChessPiece;

namespace Tests
{
    public class ChessPieceQueenTests
    {
        private ChessBoard board;
        private BoardPosition a3;
        private BoardPosition a6;
        private BoardPosition h3;
        private BoardPosition h6;
        private BoardPosition a1;

        [SetUp]
        public void Setup()
        {
            a3 = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.THREE);
            a6 = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.SIX);
            h3 = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.THREE);
            h6 = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.SIX);
            a1 = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.ONE);


            board = new ChessBoard();
        }

        [Test]
        public void Test_ConstructWhiteQueen_Success()
        {
            BoardPosition boardPosition = a3;
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(piece.GetRealValue(), Is.EqualTo(15));
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.QUEEN));
            });
        }

        [Test]
        public void Test_ConstructBlackQueen_Success()
        {
            BoardPosition boardPosition = h3;
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(piece.GetRealValue(), Is.EqualTo(25));
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.QUEEN));
            });
        }

        [Test(Description = "Tests whether the white queen 1 can move to C1 from A3 with no other pieces on board")]
        public void Test_WhiteQueen1_IsValidMove_StartingMove1()
        {
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, a3);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black queen 1 can move to B4 from A3 with no other pieces on board")]
        public void Test_BlackQueen1_IsValidMove_StartingMove2()
        {
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 1, a3);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that black queen 1 cannot move because there are pieces blocking its path")]
        public void Test_BlackQueen1_IsValidMove_CannotMoveBlocked()
        {
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 1, a3);
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.TWO),
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.ONE),
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.SIX),
                new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.SEVEN),
                new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.EIGHT),
            };

            // Set Black Pawns on B2 and B4 which block Bishop from moving
            board.SetBoardValue(new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.TWO), 21);
            board.SetBoardValue(new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR), 11);

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
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 1, a3);
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.SIX),
                new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.SEVEN),
                new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.EIGHT),
            };

            // Set Black Pawn on B4 which block Queen
            board.SetBoardValue(new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR), 21);

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
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 1, a3);
            BoardPosition d6 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.SIX);

            // Set White Pawn on D6
            board.SetBoardValue(d6, 11);

            Assert.That(piece.IsValidMove(board, d6), Is.True);
        }


        [Test(Description = "Tests that white queen can move to all of these valid locations if starting from D4")]
        public void Test_WhiteQueen1_IsValidMove_FromD4()
        {
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR));

            // valid positions if bishop on D4
            // C3, B2, A1, C5, B6, A7, E3, F2, G1, E5, F6, G7, H8 
            List<BoardPosition> validPositions = new()
            {
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.THREE),
                new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.TWO),
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.ONE),
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.SIX),
                new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.SEVEN),
                new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.THREE),
                new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.TWO),
                new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.ONE),
                new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.SIX),
                new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.SEVEN),
                new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.EIGHT)
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

        [Test(Description = "Test that black queen 2 on D4 can move horizontally across all D squares")]
        public void Test_Rook_IsValidMove_HorizontalFromD4()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2, new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR)); // D4

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

        [Test(Description = "Test that black queen 1 on F4 can move vertically across all 4 squares (E4, D4, C4, B4, A4, G4, H4)")]
        public void Test_BlackQueen_IsValidMove_VerticalFromF4()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, new BoardPosition(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.FOUR)); // F4

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

        [Test(Description = "Test that black queen 2 on A8 can move vertically across all 8 squares (E8, D8, C8, B8, A8, G8, H8)")]
        public void Test_BlackQueen_IsValidMove_VerticalFromA8()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2, new BoardPosition(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.EIGHT));

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

        [Test(Description = "Tests that black queen 1 on C2 cannot move to C6, or C5 because there is a white piece on C4 blocking its path")]
        public void Test_BlackQueen1_IsValidMove_CannotMoveBlockedByFriendly()
        {
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 1,
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

        [Test(Description = "Tests that black queen 1 on C2 cannot move to G2, or H2 because there is a black piece on F2 blocking its path")]
        public void Test_BlackQueen1_IsValidMove_CannotMoveBlockedByEnemy()
        {
            ChessPiece piece = new ChessPieceRook(ChessPiece.Color.BLACK, 1,
                new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.TWO));
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.TWO),
                new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.TWO)
            };

            // Set White Pawn on c4
            board.SetBoardValue(new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.TWO), 11);

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
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1,
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

        [Test(Description = "Tests that black queen 1 on B4 can capture a piece on B8")]
        public void Test_BlackQueen1_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 1,
                new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR));
            BoardPosition b8 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.EIGHT);
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that black queen 1 on B4 canotn capture a piece on B8")]
        public void Test_BlackQueen1_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 1,
                new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR));
            BoardPosition b8 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.EIGHT);
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

        [Test(Description = "Tests that black queen 2 on B4 can capture a piece on B8")]
        public void Test_BlackQueen2_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 2,
                new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR));
            BoardPosition b8 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.EIGHT);
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that black queen 2 on B4 cannot capture a piece on B8")]
        public void Test_BlackQueen2_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceQueen(ChessPiece.Color.BLACK, 2,
                new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR));
            BoardPosition b8 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.EIGHT);
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

    }
}