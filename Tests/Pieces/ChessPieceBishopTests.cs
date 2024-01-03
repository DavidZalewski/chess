using Chess.Board;
using Chess.Pieces;

namespace Tests.Pieces
{
    public class ChessPieceBishopTests
    {
        private ChessBoard board = new ChessBoard();
        private BoardPosition whiteBishop1StartPosition = new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.A);
        private BoardPosition whiteBishop2StartPosition = new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.A);
        private BoardPosition blackBishop1StartPosition = new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.H);
        private BoardPosition blackBishop2StartPosition = new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.H);

        public ChessPieceBishopTests()
        {
            Setup();
        }

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
        }

        [Test]
        public void Test_ConstructWhiteBishop_Success()
        {
            BoardPosition boardPosition = whiteBishop1StartPosition;
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(piece.GetRealValue(), Is.EqualTo(13));
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.BISHOP));
            });
        }

        [Test]
        public void Test_ConstructBlackBishop_Success()
        {
            BoardPosition boardPosition = blackBishop1StartPosition;
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.BLACK, 1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(piece.GetRealValue(), Is.EqualTo(23));
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.BISHOP));
            });
        }

        [Test(Description = "Tests whether the white bishop 1 can move to C1 from A3 with no other pieces on board")]
        public void Test_WhiteBishop1_IsValidMove_StartingMove1()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white bishop 1 can move to B4 from A3 with no other pieces on board")]
        public void Test_WhiteBishop1_IsValidMove_StartingMove2()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that white bishop 1 cannot move because there are pieces blocking its path")]
        public void Test_WhiteBishop1_IsValidMove_CannotMoveBlocked()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.B),
                new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B),
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.F),
            };

            // Set White Pawns on B2 and B4 which block Bishop from moving
            board.SetBoardValue(new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.B), 11);
            board.SetBoardValue(new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B), 11);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white bishop 1 cannot jump over a black piece on a diagonal")]
        public void Test_WhiteBishop1_IsValidMove_CannotJumpPieces()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.C),
                new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.D),
                new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.E),
                new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.F),
            };

            // Set Black Pawn on B4 which block Bishop
            board.SetBoardValue(new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B), 21);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white bishop 1 can capture")]
        public void Test_WhiteBishop1_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            BoardPosition d6 = new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.D);

            // Set Black Pawn on D6
            board.SetBoardValue(d6, 21);

            Assert.That(piece.IsValidMove(board, d6), Is.True);
        }


        [Test(Description = "Tests that white bishop can move to all of these valid locations if starting from D4")]
        public void Test_WhiteBishop1_IsValidMove_FromD4()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D));

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

        [Test(Description = "Tests that white bishop can move from C1 to G5 with pieces on board not in its path")]
        public void Test_WhiteBishop1_IsValidMove_FromC1ToG5()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, new("C1"));
            board.SetBoardValue(new("D3"), 22);
            board.SetBoardValue(new("E2"), 23);
            Assert.That(piece.IsValidMove(board, new("G5")), Is.True);
        }

    }
}