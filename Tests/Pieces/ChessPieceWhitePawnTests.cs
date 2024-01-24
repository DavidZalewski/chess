using Chess.Board;
using Chess.Pieces;

namespace Tests.Pieces
{
    public class ChessPieceWhitePawnTests
    {
        private ChessBoard board;

        public ChessPieceWhitePawnTests()
        {
            board = new ChessBoard();
        }

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
        }

        [Test]
        public void Test_ConstructWhitePawn_Success()
        {
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(piece.GetRealValue(), Is.EqualTo(11));
                Assert.That(piece.GetId(), Is.EqualTo(1));
                Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.PAWN));
            });
        }

        [Test]
        public void Test_CloneWhitePawn_Success()
        {
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            ChessPiece clone = piece.Clone();
            
            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, piece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(clone.GetRealValue(), Is.EqualTo(11));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(clone.GetPiece(), Is.EqualTo(ChessPiece.Piece.PAWN));
            });
        }

        [Test(Description = "Tests whether the white pawn can move a single square up on its first move")]
        public void Test_WhitePawn_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.C);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white pawn can move two squares up on its first move")]
        public void Test_WhitePawn_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that the white pawn cannot move 2 squares if it has already moved")]
        public void Test_WhitePawn_IsValidMove_InvalidMove2Squares()
        {
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.B);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition nextPosition = new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.B);
            Assert.That(piece.IsValidMove(board, nextPosition), Is.True);
            Assert.That(piece.HasMoved(), Is.False);
            piece.Move(board, nextPosition);
            Assert.That(piece.HasMoved(), Is.True);
            BoardPosition newPosition = new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.B);

            Assert.That(piece.IsValidMove(board, newPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move horizontally when there is no capture")]
        public void Test_WhitePawn_IsValidMove_InvalidHorizontalMove1()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.C);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move horizontally when there is no capture - another variation")]
        public void Test_WhitePawn_IsValidMove_InvalidHorizontalMove2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.C);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move horizontally without moving forward")]
        public void Test_WhitePawn_IsValidMove_InvalidHorizontalMove3()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.B);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.B);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn can move horizontally as there is a capture")]
        public void Test_WhitePawn_IsValidMove_ValidHorizontalMoveCapture1()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.E);
            // Set black pawn at E5
            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the white pawn can move horizontally as there is a capture (variation)")]
        public void Test_WhitePawn_IsValidMove_ValidHorizontalMoveCapture2()
        {
            // Create White Pawn at D3
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            // Set black pawn at E4
            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the white pawn cannot capture its own piece")]
        public void Test_WhitePawn_IsValidMove_InvalidCapture1()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.E);
            // Set white pawn at E3
            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot capture a piece parallel to it")]
        public void Test_WhitePawn_IsValidMove_InvalidCapture2()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.D);
            // Set black pawn at D3
            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move backwards")]
        public void Test_WhitePawn_IsValidMove_InvalidMoveBackwards()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.C);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to erroneous places on board")]
        public void Test_WhitePawn_IsValidMove_InvalidMove1()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.G);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to erroneous places on board")]
        public void Test_WhitePawn_IsValidMove_InvalidMove2()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to erroneous places on board")]
        public void Test_WhitePawn_IsValidMove_InvalidMove3()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.D);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move forward if there is a white piece in front of it")]
        public void Test_WhitePawn_IsValidMove_BlockedByWhite()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition F4 = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.F);
            // set white bishop at F4, directly in front of white pawn at E4
            board.SetBoardValue(F4, 13);
            Assert.That(piece.IsValidMove(board, F4), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move forward if there is a black piece in front of it")]
        public void Test_WhitePawn_IsValidMove_BlockedByBlack()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition F4 = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.F);
            // set black bishop at F4, directly in front of white pawn at E4
            board.SetBoardValue(F4, 23);
            Assert.That(piece.IsValidMove(board, F4), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to the same position it is already at")]
        public void Test_WhitePawn_IsValidMove_SamePosition()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn on E2 can capture Black Knight on F3")]
        public void Test_WhitePawn_IsValidMove_CanCaptureKnightOnF3()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn5Piece = new ChessPieceWhitePawn(5, new("E2"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn5Piece, blackKnightPiece });

            Assert.That(whitePawn5Piece.IsValidMove(board, new("F3")), Is.True);
        }

        [Test(Description = "Tests that the white pawn on F2 cannot capture Black Knight on F3")]
        public void Test_WhitePawn_IsValidMove_CannotCaptureKnightOnF3()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(5, new("F2"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn6Piece, blackKnightPiece });

            Assert.That(whitePawn6Piece.IsValidMove(board, new("F3")), Is.False);
        }

        [Test(Description = "Tests that the white pawn on F2 cannot jump over Black Knight on F3")]
        public void Test_WhitePawn_IsValidMove_CannotJumpOverKnightOnF3()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(5, new("F2"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn6Piece, blackKnightPiece });

            Assert.That(whitePawn6Piece.IsValidMove(board, new("F4")), Is.False);
        }

        [Test(Description = "Tests that the White Pawn on C2 cannot capture a Black Pawn on C4")]
        public void Test_WhitePawn_IsValidMove_CannotCaptureBPOnC4()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn3Piece = new ChessPieceWhitePawn(3, new("C2"));
            ChessPiece blackPawnPiece = new ChessPieceBlackPawn(3, new("C4"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn3Piece, blackPawnPiece });

            Assert.That(whitePawn3Piece.IsValidMove(board, new("C4")), Is.False);
        }

        [Test(Description = "Tests that the White Pawn on D4 can capture a Black Pawn on E5")]
        public void Test_WhitePawn_IsValidMove_PawnOnD4CanCapturePawnOnE5()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn3Piece = new ChessPieceWhitePawn(4, new("D4"));
            ChessPiece blackPawnPiece = new ChessPieceBlackPawn(5, new("E5"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn3Piece, blackPawnPiece });

            Assert.That(whitePawn3Piece.IsValidMove(board, new("E5")), Is.True);
        }
    }
}