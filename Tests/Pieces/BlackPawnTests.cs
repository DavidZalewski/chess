using Chess.Board;
using Chess.Pieces;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class BlackPawnTests
    {
        private ChessBoard board = new ChessBoard();

        public BlackPawnTests()
        {
            Setup();
        }

        [SetUp]
        public void Setup()
        {
            board = new ChessBoard();
        }

        [Test]
        public void ConstructBlackPawn_Success()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.ONE, FILE.A);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            Assert.That(piece, Is.Not.Null);
            Assert.That(piece.GetCurrentPosition(), Is.EqualTo(boardPosition));
            Assert.That(piece.GetRealValue(), Is.EqualTo(21));
            Assert.That(piece.GetId(), Is.EqualTo(1));
            Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
            Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.PAWN));
        }

        [Test(Description = "Tests whether the black pawn can move a single square down on its first move")]
        public void BlackPawn_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.SEVEN, FILE.F);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(RANK.SIX, FILE.F);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black pawn can move two squares down on its first move")]
        public void BlackPawn_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.SEVEN, FILE.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(RANK.FIVE, FILE.E);

            Assert.Multiple(() =>
            {
                Assert.That(piece.IsValidMove(board, newPosition), Is.True);
                Assert.That(piece is ChessPiecePawn, Is.True);
                Assert.That((piece as ChessPiecePawn).MovedTwoSquares, Is.True);
            });
        }

        [Test(Description = "Tests that the black pawn cannot move 2 squares if it has already moved")]
        public void BlackPawn_IsValidMove_InvalidMove2Squares()
        {
            BoardPosition boardPosition = new("G7");
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);
            BoardPosition nextPosition = new("G6");
            Assert.That(piece.IsValidMove(board, nextPosition), Is.True);
            Assert.That(piece.HasMoved(), Is.False);
            piece.Move(board, nextPosition);
            Assert.That(piece.HasMoved(), Is.True);
            BoardPosition newPosition = new(RANK.FOUR, FILE.G);

            Assert.That(piece.IsValidMove(board, newPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move horizontally when there is no capture")]
        public void BlackPawn_IsValidMove_InvalidHorizontalMove1()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.G);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(RANK.TWO, FILE.F);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move horizontally when there is no capture - another variation")]
        public void BlackPawn_IsValidMove_InvalidHorizontalMove2()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.G);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(RANK.FIVE, FILE.F);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move horizontally without moving forward")]
        public void BlackPawn_IsValidMove_InvalidHorizontalMove3()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.G);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(RANK.SEVEN, FILE.G);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn can move horizontally as there is a capture")]
        public void BlackPawn_IsValidMove_ValidHorizontalMoveCapture1()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(RANK.FIVE, FILE.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(RANK.FOUR, FILE.D);
            // Set white pawn at D5

            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the black pawn can move horizontally as there is a capture (variation)")]
        public void BlackPawn_IsValidMove_ValidHorizontalMoveCapture2()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(RANK.THREE, FILE.D);
            // Set white pawn at D3

            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the black pawn cannot capture its own piece")]
        public void BlackPawn_IsValidMove_InvalidCapture1()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(RANK.THREE, FILE.D);
            // Set black pawn at D3

            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot capture a piece parallel to it")]
        public void BlackPawn_IsValidMove_InvalidCapture2()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(RANK.THREE, FILE.E);
            // Set white pawn at E3

            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move backwards")]
        public void BlackPawn_IsValidMove_InvalidMoveBackwards()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(RANK.FOUR, FILE.F);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move to erroneous places on board")]
        public void BlackPawn_IsValidMove_InvalidMove1()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(RANK.EIGHT, FILE.H);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move to erroneous places on board")]
        public void BlackPawn_IsValidMove_InvalidMove2()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(RANK.ONE, FILE.A);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn cannot move to erroneous places on board")]
        public void BlackPawn_IsValidMove_InvalidMove3()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(RANK.EIGHT, FILE.E);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the black pawn 2 on A7 cannot move to C5")]
        public void BlackPawn_IsValidMove_InvalidMove4()
        {
            // Create Black Pawn at E4
            BoardPosition boardPosition = new BoardPosition(RANK.SEVEN, FILE.A);
            ChessPiece piece = new ChessPieceBlackPawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(RANK.FIVE, FILE.C);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }
    }
}