using Chess;

namespace Tests
{
    public class ChessPieceKnightTests
    {
        private ChessBoard board;
        private BoardPosition whiteKnight1StartPosition;
        private BoardPosition whiteKnight2StartPosition;
        private BoardPosition blackKnight1StartPosition;
        private BoardPosition blackKnight2StartPosition;

        [SetUp]
        public void Setup()
        {
            whiteKnight1StartPosition = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.TWO);
            whiteKnight2StartPosition = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.SEVEN);
            blackKnight1StartPosition = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.TWO);
            blackKnight2StartPosition = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.SEVEN);

            board = new ChessBoard();
        }

        [Test]
        public void Test_ConstructWhiteKnight_Success()
        {
            BoardPosition boardPosition = whiteKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(piece.GetRealValue(), Is.EqualTo(12));
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.KNIGHT));
            });
        }

        [Test]
        public void Test_ConstructBlackKnight_Success()
        {
            BoardPosition boardPosition = blackKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(piece.GetRealValue(), Is.EqualTo(22));
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.KNIGHT));
            });
        }

        [Test(Description = "Tests whether the white knight 1 can move to C1 on its first move")]
        public void Test_WhiteKnight1_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = whiteKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, boardPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white knight 1 can move to C3 on its first move")]
        public void Test_WhiteKnight1_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = whiteKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, boardPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.THREE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white knight 2 can move to C6 on its first move")]
        public void Test_WhiteKnight2_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = whiteKnight2StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 2, boardPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.SIX);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white knight 2 can move to C8 on its first move")]
        public void Test_WhiteKnight2_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = whiteKnight2StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 2, boardPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.EIGHT);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black knight 1 can move to F1 on its first move")]
        public void Test_BlackKnight1_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = blackKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, boardPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black knight 1 can move to F3 on its first move")]
        public void Test_BlackKnight1_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = blackKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, boardPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.THREE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black knight 2 can move to F6 on its first move")]
        public void Test_BlackKnight2_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = blackKnight2StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 2, boardPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.SIX);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black knight 2 can move to F8 on its first move")]
        public void Test_BlackKnight2_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = blackKnight2StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 2, boardPosition);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.EIGHT);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that white knight cannot capture its own piece")]
        public void Test_WhiteKnight1_IsValidMove_InvalidCapture()
        {
            BoardPosition boardPosition = whiteKnight1StartPosition;
            BoardPosition c3 = new(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.THREE);
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, boardPosition);
            // Set White Pawn on C3
            board.SetBoardValue(c3, 11);
            Assert.That(piece.IsValidMove(board, c3), Is.False);
        }

        [Test(Description = "Tests that black knight cannot capture its own piece")]
        public void Test_BlackKnight2_IsValidMove_InvalidCapture()
        {
            BoardPosition boardPosition = blackKnight2StartPosition;
            BoardPosition f3 = new(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.THREE);
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 2, boardPosition);
            // Set Black Pawn on F3
            board.SetBoardValue(f3, 21);
            Assert.That(piece.IsValidMove(board, f3), Is.False);
        }

        [Test(Description = "Tests that white knight can move to all of these valid locations if starting from C3")]
        public void Test_WhiteKnight1_IsValidMove_FromC3()
        {
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.THREE));

            // valid positions if knight on C3
            // A2, A4, B1, D1, E2, E4, D5, B5,  
            BoardPosition a2 = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.TWO);
            BoardPosition a4 = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.FOUR);
            BoardPosition b1 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.ONE);
            BoardPosition d1 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.ONE);
            BoardPosition e2 = new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.TWO);
            BoardPosition e4 = new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);
            BoardPosition d5 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FIVE);
            BoardPosition b5 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FIVE);

            Assert.Multiple(() =>
            {
                Assert.That(piece.IsValidMove(board, a2), Is.True);
                Assert.That(piece.IsValidMove(board, a4), Is.True);
                Assert.That(piece.IsValidMove(board, b1), Is.True);
                Assert.That(piece.IsValidMove(board, d1), Is.True);
                Assert.That(piece.IsValidMove(board, e2), Is.True);
                Assert.That(piece.IsValidMove(board, e4), Is.True);
                Assert.That(piece.IsValidMove(board, d5), Is.True);
                Assert.That(piece.IsValidMove(board, b5), Is.True);
            });
        }

        [Test(Description = "Tests that white knight can move to some of these valid locations if starting from C3")]
        public void Test_WhiteKnight1_IsValidMove_FromC3OnlySome()
        {
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.THREE));

            // valid positions if knight on C3
            // A2, A4, B1, D1, E2, E4, D5, B5,  
            BoardPosition a2 = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.TWO);
            BoardPosition a4 = new(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.FOUR);

            BoardPosition b1 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.ONE);
            BoardPosition b5 = new(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FIVE);

            BoardPosition d1 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.ONE);
            BoardPosition d5 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FIVE);

            BoardPosition e2 = new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.TWO);
            BoardPosition e4 = new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);

            // Set White Pawn at B1 and B5, and D5
            board.SetBoardValue(b1, 11);
            board.SetBoardValue(b5, 11);
            board.SetBoardValue(d5, 11);
            // Set White King at A4
            board.SetBoardValue(a4, 15);
            // Set Black Pawn at E2 and E4
            board.SetBoardValue(e2, 21);
            board.SetBoardValue(e4, 21);

            Assert.Multiple(() =>
            {
                Assert.That(piece.IsValidMove(board, a2), Is.True);
                Assert.That(piece.IsValidMove(board, a4), Is.False);
                Assert.That(piece.IsValidMove(board, b1), Is.False);
                Assert.That(piece.IsValidMove(board, d1), Is.True);
                Assert.That(piece.IsValidMove(board, e2), Is.True);
                Assert.That(piece.IsValidMove(board, e4), Is.True);
                Assert.That(piece.IsValidMove(board, d5), Is.False);
                Assert.That(piece.IsValidMove(board, b5), Is.False);
            });
        }

        [Test(Description = "Tests that black knight can move to some of these valid locations if starting from F3")]
        public void Test_BlackKnight1_IsValidMove_FromF3OnlySome()
        {
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new BoardPosition(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.THREE));

            // valid positions if knight on F3
            // H2, H4, G1, E1, D2, D4, E5, G5,  
            BoardPosition h2 = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.TWO);
            BoardPosition h4 = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.FOUR);

            BoardPosition g1 = new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.ONE);
            BoardPosition g5 = new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.FIVE);

            BoardPosition e1 = new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.ONE);
            BoardPosition e5 = new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FIVE);

            BoardPosition d2 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.TWO);
            BoardPosition d4 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);

            // Set Black Pawn at G1 and G5, and E5
            board.SetBoardValue(g1, 21);
            board.SetBoardValue(g5, 21);
            board.SetBoardValue(e5, 21);
            // Set Black King at H4
            board.SetBoardValue(h4, 25);
            // Set White Pawn at D2 and D4
            board.SetBoardValue(d2, 11);
            board.SetBoardValue(d4, 11);

            Assert.Multiple(() =>
            {
                Assert.That(piece.IsValidMove(board, h2), Is.True);
                Assert.That(piece.IsValidMove(board, h4), Is.False);
                Assert.That(piece.IsValidMove(board, g1), Is.False);
                Assert.That(piece.IsValidMove(board, e1), Is.True);
                Assert.That(piece.IsValidMove(board, d2), Is.True);
                Assert.That(piece.IsValidMove(board, d4), Is.True);
                Assert.That(piece.IsValidMove(board, e5), Is.False);
                Assert.That(piece.IsValidMove(board, g5), Is.False);
            });
        }

        [Test(Description = "Tests that these are invalid moves for knight at starting position D4")]
        public void Test_Knight1_IsValidMove_InvalidMovesFromD4()
        {
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR));

            BoardPosition h2 = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.TWO);
            BoardPosition h4 = new(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.FOUR);

            BoardPosition g1 = new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.ONE);
            BoardPosition g5 = new(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.FIVE);

            BoardPosition e1 = new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.ONE);
            BoardPosition e5 = new(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FIVE);

            BoardPosition d2 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.TWO);
            BoardPosition d4 = new(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);

            Assert.Multiple(() =>
            {
                Assert.That(piece.IsValidMove(board, h2), Is.False);
                Assert.That(piece.IsValidMove(board, h4), Is.False);
                Assert.That(piece.IsValidMove(board, g1), Is.False);
                Assert.That(piece.IsValidMove(board, e1), Is.False);
                Assert.That(piece.IsValidMove(board, d2), Is.False);
                Assert.That(piece.IsValidMove(board, d4), Is.False);
                Assert.That(piece.IsValidMove(board, e5), Is.False);
                Assert.That(piece.IsValidMove(board, g5), Is.False);
            });

        }
    }
}