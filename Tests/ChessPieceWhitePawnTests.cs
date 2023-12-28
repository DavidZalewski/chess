using Chess;

namespace Tests
{
    public class ChessPieceWhitePawnTests
    {
        private ChessBoard board;

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
        }

        [Test]
        public void Test_ConstructWhitePawn_Success()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.ONE);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
            Assert.That(piece.GetRealValue(), Is.EqualTo(11));
            Assert.That(piece.GetId(), Is.EqualTo(1));
            Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
            Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.PAWN));
        }

        [Test(Description = "Tests whether the white pawn can move a single square up on its first move")]
        public void Test_WhitePawn_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.ONE);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white pawn can move two squares up on its first move")]
        public void Test_WhitePawn_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.ONE);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that the white pawn cannot move 2 squares if it has already moved")]
        public void Test_WhitePawn_IsValidMove_InvalidMove2Squares()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.ONE);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition nextPosition = new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.ONE);
            Assert.That(piece.IsValidMove(board, nextPosition), Is.True);
            piece.Move(board, nextPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move horizontally when there is no capture")]
        public void Test_WhitePawn_IsValidMove_InvalidHorizontalMove1()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.TWO);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move horizontally when there is no capture - another variation")]
        public void Test_WhitePawn_IsValidMove_InvalidHorizontalMove2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FIVE);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move horizontally without moving forward")]
        public void Test_WhitePawn_IsValidMove_InvalidHorizontalMove3()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.B, BoardPosition.HORIZONTAL.SEVEN);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn can move horizontally as there is a capture")]
        public void Test_WhitePawn_IsValidMove_ValidHorizontalMoveCapture1()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FIVE);
            // Set black pawn at E5
            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the white pawn can move horizontally as there is a capture (variation)")]
        public void Test_WhitePawn_IsValidMove_ValidHorizontalMoveCapture2()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.THREE);
            // Set black pawn at E3
            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the white pawn cannot capture its own piece")]
        public void Test_WhitePawn_IsValidMove_InvalidCapture1()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.THREE);
            // Set white pawn at E3
            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot capture a piece parallel to it")]
        public void Test_WhitePawn_IsValidMove_InvalidCapture2()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.THREE);
            // Set black pawn at D3
            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move backwards")]
        public void Test_WhitePawn_IsValidMove_InvalidMoveBackwards()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.C, BoardPosition.HORIZONTAL.FOUR);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to erroneous places on board")]
        public void Test_WhitePawn_IsValidMove_InvalidMove1()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.SEVEN);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to erroneous places on board")]
        public void Test_WhitePawn_IsValidMove_InvalidMove2()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.ONE);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to erroneous places on board")]
        public void Test_WhitePawn_IsValidMove_InvalidMove3()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.EIGHT);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

    }
}