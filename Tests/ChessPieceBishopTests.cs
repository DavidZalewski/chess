using Chess;

namespace Tests
{
    public class ChessPieceBishopTests
    {
        private ChessBoard board;
        private BoardPosition whiteBishop1StartPosition;
        private BoardPosition whiteBishop2StartPosition;
        private BoardPosition blackBishop1StartPosition;
        private BoardPosition blackBishop2StartPosition;

        [SetUp]
        public void Setup()
        {
            whiteBishop1StartPosition = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.THREE);
            whiteBishop2StartPosition = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.SIX);
            blackBishop1StartPosition = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.THREE);
            blackBishop2StartPosition = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.SIX);

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
            BoardPosition newPosition = new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white bishop 1 can move to B4 from A3 with no other pieces on board")]
        public void Test_WhiteBishop1_IsValidMove_StartingMove2()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that white bishop 1 cannot move because there are pieces blocking its path")]
        public void Test_WhiteBishop1_IsValidMove_CannotMoveBlocked()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            List<BoardPosition> positions = new()
            {
                new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.TWO),
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.ONE),
                new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR),
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.SIX),
                new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.SEVEN),
                new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.EIGHT),
            };

            // Set White Pawns on B2 and B4 which block Bishop from moving
            board.SetBoardValue(new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.TWO), 11);
            board.SetBoardValue(new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR), 11);

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
                new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FIVE),
                new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.SIX),
                new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.SEVEN),
                new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.EIGHT),
            };

            // Set Black Pawn on B4 which block Bishop
            board.SetBoardValue(new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR), 21);

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
            BoardPosition d6 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.SIX);

            // Set Black Pawn on D6
            board.SetBoardValue(d6, 21);

            Assert.That(piece.IsValidMove(board, d6), Is.True);
        }


        [Test(Description = "Tests that white bishop can move to all of these valid locations if starting from D4")]
        public void Test_WhiteBishop1_IsValidMove_FromD4()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR));

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

    }
}