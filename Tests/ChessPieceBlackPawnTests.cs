using Chess;

namespace Tests
{
    public class ChessPieceBlackPawnTests
    {
        private ChessBoard board = new ChessBoard();

        public ChessPieceBlackPawnTests()
        {
            Setup();
        }

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
        }

        [Test]
        public void Test_ConstructBlackPawn_Success()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
            Assert.That(piece.GetRealValue(), Is.EqualTo(21));
            Assert.That(piece.GetId(), Is.EqualTo(1));
            Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
            Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.PAWN));
        }

        [Test(Description = "Tests whether the black pawn can move a single square down on its first move")]
        public void Test_BlackPawn_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.F);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.F);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black pawn can move two squares down on its first move")]
        public void Test_BlackPawn_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.E);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that the black pawn cannot move 2 squares if it has already moved")]
        public void Test_BlackPawn_IsValidMove_InvalidMove2Squares()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.G);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            BoardPosition nextPosition = new BoardPosition(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.G);
            Assert.That(piece.IsValidMove(board, nextPosition), Is.True);
            piece.Move(board, nextPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.G);

            Assert.That(piece.IsValidMove(board, newPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move horizontally when there is no capture")]
        public void Test_BlackPawn_IsValidMove_InvalidHorizontalMove1()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.G);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.F);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move horizontally when there is no capture - another variation")]
        public void Test_BlackPawn_IsValidMove_InvalidHorizontalMove2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.G);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.F);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move horizontally without moving forward")]
        public void Test_BlackPawn_IsValidMove_InvalidHorizontalMove3()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.G);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.G);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn can move horizontally as there is a capture")]
        public void Test_BlackPawn_IsValidMove_ValidHorizontalMoveCapture1()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D);
            // Set white pawn at D5
            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the black pawn can move horizontally as there is a capture (variation)")]
        public void Test_BlackPawn_IsValidMove_ValidHorizontalMoveCapture2()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.D);
            // Set white pawn at D3
            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the black pawn cannot capture its own piece")]
        public void Test_BlackPawn_IsValidMove_InvalidCapture1()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.D);
            // Set black pawn at D3
            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot capture a piece parallel to it")]
        public void Test_BlackPawn_IsValidMove_InvalidCapture2()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.E);
            // Set white pawn at E3
            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move backwards")]
        public void Test_BlackPawn_IsValidMove_InvalidMoveBackwards()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.F);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move to erroneous places on board")]
        public void Test_BlackPawn_IsValidMove_InvalidMove1()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.H);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move to erroneous places on board")]
        public void Test_BlackPawn_IsValidMove_InvalidMove2()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move to erroneous places on board")]
        public void Test_BlackPawn_IsValidMove_InvalidMove3()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.E);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }
    }
}