using Chess;

namespace Tests
{
    public class ChessPieceBlackPawnTests
    {
        private ChessBoard board;

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
        }

        [Test]
        public void Test_ConstructBlackPawn_Success()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.ONE);
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
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.ONE);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black pawn can move two squares down on its first move")]
        public void Test_BlackPawn_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.ONE);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that the black pawn cannot move 2 squares if it has already moved")]
        public void Test_BlackPawn_IsValidMove_InvalidMove2Squares()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.ONE);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            BoardPosition nextPosition = new BoardPosition(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.ONE);
            Assert.That(piece.IsValidMove(board, nextPosition), Is.True);
            piece.Move(board, nextPosition);
            BoardPosition newPosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.ONE);

            Assert.That(piece.IsValidMove(board, newPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move horizontally when there is no capture")]
        public void Test_BlackPawn_IsValidMove_InvalidHorizontalMove1()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.TWO);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move horizontally when there is no capture - another variation")]
        public void Test_BlackPawn_IsValidMove_InvalidHorizontalMove2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.FIVE);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move horizontally without moving forward")]
        public void Test_BlackPawn_IsValidMove_InvalidHorizontalMove3()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(BoardPosition.VERTICAL.G, BoardPosition.HORIZONTAL.SEVEN);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn can move horizontally as there is a capture")]
        public void Test_BlackPawn_IsValidMove_ValidHorizontalMoveCapture1()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.FIVE);
            // Set white pawn at D5
            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the black pawn can move horizontally as there is a capture (variation)")]
        public void Test_BlackPawn_IsValidMove_ValidHorizontalMoveCapture2()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.THREE);
            // Set white pawn at D3
            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the black pawn cannot capture its own piece")]
        public void Test_BlackPawn_IsValidMove_InvalidCapture1()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.D, BoardPosition.HORIZONTAL.THREE);
            // Set black pawn at D3
            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot capture a piece parallel to it")]
        public void Test_BlackPawn_IsValidMove_InvalidCapture2()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.THREE);
            // Set white pawn at E3
            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move backwards")]
        public void Test_BlackPawn_IsValidMove_InvalidMoveBackwards()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.F, BoardPosition.HORIZONTAL.FOUR);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move to erroneous places on board")]
        public void Test_BlackPawn_IsValidMove_InvalidMove1()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.EIGHT);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move to erroneous places on board")]
        public void Test_BlackPawn_IsValidMove_InvalidMove2()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.A, BoardPosition.HORIZONTAL.ONE);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move to erroneous places on board")]
        public void Test_BlackPawn_IsValidMove_InvalidMove3()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(BoardPosition.VERTICAL.E, BoardPosition.HORIZONTAL.EIGHT);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn can be successfully promoted into a black queen piece")]
        public void Test_BlackPawn_PromotePawn_Success()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.H, BoardPosition.HORIZONTAL.FOUR);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            Assert.That(false, Is.True); // not implemented test, requires creation of additional piece sub classes
            Assert.That(piece.PromotePawn<ChessPiece>((ChessPieceBlackPawn)piece, piece), Is.False);
        }

    }
}