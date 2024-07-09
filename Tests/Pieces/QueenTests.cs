using Chess.Board;
using Chess.Pieces;
using static Chess.Pieces.ChessPiece;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class QueenTests
    {
        private ChessBoard board = new ChessBoard();
        private BoardPosition a3 = new(RANK.THREE, FILE.A);
        private BoardPosition a6 = new(RANK.SIX, FILE.A);
        private BoardPosition h3 = new(RANK.THREE, FILE.H);
        private BoardPosition h6 = new(RANK.SIX, FILE.H);
        private BoardPosition a1 = new(RANK.ONE, FILE.A);

        public QueenTests()
        {
            Setup();
        }

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
        }

        [Test]
        public void ConstructWhiteQueen_Success()
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
        public void CloneWhiteQueen_Success()
        {
            BoardPosition boardPosition = a3;
            ChessPiece piece = new ChessPieceQueen(Color.WHITE, 1, boardPosition);
            ChessPiece clone = piece.Clone();
            
            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, piece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(clone.GetRealValue(), Is.EqualTo(15));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(Color.WHITE));
                Assert.That(clone.GetPiece(), Is.EqualTo(Piece.QUEEN));
            });
        }

        [Test]
        public void ConstructBlackQueen_Success()
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

        [Test]
        public void CloneBlackQueen_Success()
        {
            BoardPosition boardPosition = h3;
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, boardPosition);
            ChessPiece clone = piece.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, piece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(clone.GetRealValue(), Is.EqualTo(25));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(Color.BLACK));
                Assert.That(clone.GetPiece(), Is.EqualTo(Piece.QUEEN));
            });
        }

        [Test(Description = "Tests whether the white queen 1 can move to C1 from A3 with no other pieces on board")]
        public void WhiteQueen1_IsValidMove_StartingMove1()
        {
            ChessPiece piece = new ChessPieceQueen(Color.WHITE, 1, a3);
            BoardPosition newPosition = new(RANK.ONE, FILE.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black queen 1 can move to B4 from A3 with no other pieces on board")]
        public void BlackQueen1_IsValidMove_StartingMove2()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a3);
            BoardPosition newPosition = new(RANK.FOUR, FILE.B);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that black queen 1 cannot move because there are pieces blocking its path")]
        public void BlackQueen1_IsValidMove_CannotMoveBlocked()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a3);
            List<BoardPosition> positions = new()
            {
                new(RANK.TWO, FILE.B),
                new(RANK.ONE, FILE.C),
                new(RANK.FIVE, FILE.C),
                new(RANK.SIX, FILE.D),
                new(RANK.SEVEN, FILE.E),
                new(RANK.EIGHT, FILE.F),
            };

            // Set Black Pawns on B2 and B4 which block Bishop from moving
            // TODO: Implement Later
            board.SetBoardValue(new(RANK.TWO, FILE.B), 21);
            board.SetBoardValue(new(RANK.FOUR, FILE.B), 11);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that black queen 1 cannot jump over a black piece on a diagonal")]
        public void BlackQueen1_IsValidMove_CannotJumpPieces()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a3);
            List<BoardPosition> positions = new()
            {
                new(RANK.FIVE, FILE.C),
                new(RANK.SIX, FILE.D),
                new(RANK.SEVEN, FILE.E),
                new(RANK.EIGHT, FILE.F),
            };

            // Set Black Pawn on B4 which block Queen
            // TODO: Implement Later
            board.SetBoardValue(new(RANK.FOUR, FILE.B), 21);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that black queen 1 can capture")]
        public void WhiteQueen1_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a3);
            BoardPosition d6 = new(RANK.SIX, FILE.D);

            // Set White Pawn on D6
            // TODO: Implement Later
            board.SetBoardValue(d6, 11);

            Assert.That(piece.IsValidMove(board, d6), Is.True);
        }


        [Test(Description = "Tests that white queen can move to all of these valid locations if starting from D4")]
        public void WhiteQueen1_IsValidMove_FromD4()
        {
            ChessPiece piece = new ChessPieceQueen(Color.WHITE, 1, new BoardPosition(RANK.FOUR, FILE.D));

            // valid positions if bishop on D4
            // C3, B2, A1, C5, B6, A7, E3, F2, G1, E5, F6, G7, H8 
            List<BoardPosition> validPositions = new()
            {
                new(RANK.THREE, FILE.C),
                new(RANK.TWO, FILE.B),
                new(RANK.ONE, FILE.A),
                new(RANK.FIVE, FILE.C),
                new(RANK.SIX, FILE.B),
                new(RANK.SEVEN, FILE.A),
                new(RANK.THREE, FILE.E),
                new(RANK.TWO, FILE.F),
                new(RANK.ONE, FILE.G),
                new(RANK.FIVE, FILE.E),
                new(RANK.SIX, FILE.F),
                new(RANK.SEVEN, FILE.G),
                new(RANK.EIGHT, FILE.H)
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
        public void BlackQueen_IsValidMove_HorizontalFromA1()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, a1); // A1

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

        [Test(Description = "Test that black queen 2 on D4 can move horizontally across all D squares")]
        public void Rook_IsValidMove_HorizontalFromD4()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2, new BoardPosition(RANK.FOUR, FILE.D)); // D4

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

        [Test(Description = "Test that black queen 1 on F4 can move vertically across all 4 squares (E4, D4, C4, B4, A4, G4, H4)")]
        public void BlackQueen_IsValidMove_VerticalFromF4()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1, new BoardPosition(RANK.FOUR, FILE.F)); // F4

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

        [Test(Description = "Test that black queen 2 on A8 can move vertically across all 8 squares (E8, D8, C8, B8, A8, G8, H8)")]
        public void BlackQueen_IsValidMove_VerticalFromA8()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2, new BoardPosition(RANK.EIGHT, FILE.A));

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

        [Test(Description = "Tests that black queen 1 on C2 cannot move to C6, or C5 because there is a white piece on C4 blocking its path")]
        public void BlackQueen1_IsValidMove_CannotMoveBlockedByFriendly()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1,
                new BoardPosition(RANK.TWO, FILE.C));
            List<BoardPosition> positions = new()
            {
                new(RANK.FIVE, FILE.C),
                new(RANK.SIX, FILE.C)
            };

            // Set White Pawn on c4
            // TODO: Implement Later
            board.SetBoardValue(new(RANK.FOUR, FILE.C), 11);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that black queen 1 on C2 cannot move to G2, or H2 because there is a black piece on F2 blocking its path")]
        public void BlackQueen1_IsValidMove_CannotMoveBlockedByEnemy()
        {
            ChessPiece piece = new ChessPieceRook(Color.BLACK, 1,
                new BoardPosition(RANK.TWO, FILE.C));
            List<BoardPosition> positions = new()
            {
                new(RANK.TWO, FILE.G),
                new(RANK.TWO, FILE.H)
            };

            // Set White Pawn on c4
            // TODO: Implement Later
            board.SetBoardValue(new(RANK.TWO, FILE.F), 11);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white queen on C2 cannot move to these squares as they are invalid")]
        public void WhiteQueen1_IsValidMove_InvalidMoves()
        {
            ChessPiece piece = new ChessPieceQueen(Color.WHITE, 1,
                new BoardPosition(RANK.TWO, FILE.C));
            List<BoardPosition> positions = new()
            {
                new(RANK.THREE, FILE.G),
                new(RANK.ONE, FILE.H),
                new(RANK.ONE, FILE.A),
                new(RANK.FOUR, FILE.B),
                new(RANK.SEVEN, FILE.E),
                new(RANK.FOUR, FILE.F),
                new(RANK.FIVE, FILE.G),
                new(RANK.EIGHT, FILE.H),
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
        public void BlackQueen1_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1,
                new BoardPosition(RANK.FOUR, FILE.B));
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);
            // TODO: Implement Later
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that black queen 1 on B4 canotn capture a piece on B8")]
        public void BlackQueen1_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 1,
                new BoardPosition(RANK.FOUR, FILE.B));
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);
            // TODO: Implement Later
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

        [Test(Description = "Tests that black queen 2 on B4 can capture a piece on B8")]
        public void BlackQueen2_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2,
                new BoardPosition(RANK.FOUR, FILE.B));
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);
            // TODO: Implement Later
            board.SetBoardValue(b8, 12); // white knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.True);
        }

        [Test(Description = "Tests that black queen 2 on B4 cannot capture a piece on B8")]
        public void BlackQueen2_IsValidMove_CannotCapture()
        {
            ChessPiece piece = new ChessPieceQueen(Color.BLACK, 2,
                new BoardPosition(RANK.FOUR, FILE.B));
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);
            // TODO: Implement Later
            board.SetBoardValue(b8, 22); // black knight on B8
            Assert.That(piece.IsValidMove(board, b8), Is.False);
        }

    }
}