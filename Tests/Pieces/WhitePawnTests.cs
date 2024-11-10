using Chess.Board;
using Chess.Pieces;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class WhitePawnTests : TestBase
    {
        private ChessBoard board;

        public WhitePawnTests()
        {
            board = new ChessBoard();
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            board = new ChessBoard();
        }

        [Test]
        public void ConstructWhitePawn_Success()
        {
            BoardPosition boardPosition = new(RANK.ONE, FILE.A);
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
        public void CloneWhitePawn_Success()
        {
            BoardPosition boardPosition = new(RANK.ONE, FILE.A);
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
        public void WhitePawn_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.TWO, FILE.C);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(RANK.THREE, FILE.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white pawn can move two squares up on its first move")]
        public void WhitePawn_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.TWO, FILE.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition newPosition = new BoardPosition(RANK.FOUR, FILE.D);

            Assert.Multiple(() =>
            {
                Assert.That(piece.IsValidMove(board, newPosition), Is.True);
                Assert.That(piece is ChessPiecePawn, Is.True);
                Assert.That((piece as ChessPiecePawn).MovedTwoSquares, Is.True);
            });
        }

        [Test(Description = "Tests that the white pawn cannot move 2 squares if it has already moved")]
        public void WhitePawn_IsValidMove_InvalidMove2Squares()
        {
            BoardPosition boardPosition = new(RANK.TWO, FILE.B);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition nextPosition = new(RANK.THREE, FILE.B);
            Assert.That(piece.IsValidMove(board, nextPosition), Is.True);
            Assert.That(piece.HasMoved(), Is.False);
            piece.Move(board, nextPosition);
            Assert.That(piece.HasMoved(), Is.True);
            BoardPosition newPosition = new(RANK.FIVE, FILE.B);

            Assert.That(piece.IsValidMove(board, newPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move horizontally when there is no capture")]
        public void WhitePawn_IsValidMove_InvalidHorizontalMove1()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.B);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(RANK.TWO, FILE.C);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move horizontally when there is no capture - another variation")]
        public void WhitePawn_IsValidMove_InvalidHorizontalMove2()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.B);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(RANK.FIVE, FILE.C);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move horizontally without moving forward")]
        public void WhitePawn_IsValidMove_InvalidHorizontalMove3()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.B);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badHorizontalPosition = new BoardPosition(RANK.SEVEN, FILE.B);
            Assert.That(piece.IsValidMove(board, badHorizontalPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn can move horizontally as there is a capture")]
        public void WhitePawn_IsValidMove_ValidHorizontalMoveCapture1()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(RANK.FIVE, FILE.E);
            // Set black pawn at E5
            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the white pawn can move horizontally as there is a capture (variation)")]
        public void WhitePawn_IsValidMove_ValidHorizontalMoveCapture2()
        {
            // Create White Pawn at D3
            BoardPosition boardPosition = new BoardPosition(RANK.THREE, FILE.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(RANK.FOUR, FILE.E);
            // Set black pawn at E4

            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.True);
        }

        [Test(Description = "Tests that the white pawn cannot capture its own piece")]
        public void WhitePawn_IsValidMove_InvalidCapture1()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(RANK.THREE, FILE.E);
            // Set white pawn at E3

            board.SetBoardValue(capturePosition, 11);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot capture a piece parallel to it")]
        public void WhitePawn_IsValidMove_InvalidCapture2()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition capturePosition = new BoardPosition(RANK.THREE, FILE.D);
            // Set black pawn at D3

            board.SetBoardValue(capturePosition, 21);
            Assert.That(piece.IsValidMove(board, capturePosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move backwards")]
        public void WhitePawn_IsValidMove_InvalidMoveBackwards()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(RANK.FOUR, FILE.C);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to erroneous places on board")]
        public void WhitePawn_IsValidMove_InvalidMove1()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(RANK.SEVEN, FILE.G);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to erroneous places on board")]
        public void WhitePawn_IsValidMove_InvalidMove2()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(RANK.ONE, FILE.A);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to erroneous places on board")]
        public void WhitePawn_IsValidMove_InvalidMove3()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new BoardPosition(RANK.FOUR, FILE.D);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);

            BoardPosition badPosition = new BoardPosition(RANK.EIGHT, FILE.D);
            Assert.That(piece.IsValidMove(board, badPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move forward if there is a white piece in front of it")]
        public void WhitePawn_IsValidMove_BlockedByWhite()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition F4 = new(RANK.FOUR, FILE.F);
            // set white bishop at F4, directly in front of white pawn at E4

            board.SetBoardValue(F4, 13);
            Assert.That(piece.IsValidMove(board, F4), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move forward if there is a black piece in front of it")]
        public void WhitePawn_IsValidMove_BlockedByBlack()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            BoardPosition F4 = new(RANK.FOUR, FILE.F);
            // set black bishop at F4, directly in front of white pawn at E4
            board.SetBoardValue(F4, 23);
            Assert.That(piece.IsValidMove(board, F4), Is.False);
        }

        [Test(Description = "Tests that the white pawn cannot move to the same position it is already at")]
        public void WhitePawn_IsValidMove_SamePosition()
        {
            // Create White Pawn at D4
            BoardPosition boardPosition = new(RANK.FOUR, FILE.E);
            ChessPiece piece = new ChessPieceWhitePawn(1, boardPosition);
            Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
        }

        [Test(Description = "Tests that the white pawn on E2 can capture Black Knight on F3")]
        public void WhitePawn_IsValidMove_CanCaptureKnightOnF3()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn5Piece = new ChessPieceWhitePawn(5, new("E2"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn5Piece, blackKnightPiece });

            Assert.That(whitePawn5Piece.IsValidMove(board, new("F3")), Is.True);
        }

        [Test(Description = "Tests that the white pawn on F2 cannot capture Black Knight on F3")]
        public void WhitePawn_IsValidMove_CannotCaptureKnightOnF3()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(5, new("F2"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn6Piece, blackKnightPiece });

            Assert.That(whitePawn6Piece.IsValidMove(board, new("F3")), Is.False);
        }

        [Test(Description = "Tests that the white pawn on F2 cannot jump over Black Knight on F3")]
        public void WhitePawn_IsValidMove_CannotJumpOverKnightOnF3()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(5, new("F2"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn6Piece, blackKnightPiece });

            Assert.That(whitePawn6Piece.IsValidMove(board, new("F4")), Is.False);
        }

        [Test(Description = "Tests that the White Pawn on C2 cannot capture a Black Pawn on C4")]
        public void WhitePawn_IsValidMove_CannotCaptureBPOnC4()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn3Piece = new ChessPieceWhitePawn(3, new("C2"));
            ChessPiece blackPawnPiece = new ChessPieceBlackPawn(3, new("C4"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn3Piece, blackPawnPiece });

            Assert.That(whitePawn3Piece.IsValidMove(board, new("C4")), Is.False);
        }

        [Test(Description = "Tests that the White Pawn on D4 can capture a Black Pawn on E5")]
        public void WhitePawn_IsValidMove_PawnOnD4CanCapturePawnOnE5()
        {
            // Create White Pawn at D4
            ChessPiece whitePawn3Piece = new ChessPieceWhitePawn(4, new("D4"));
            ChessPiece blackPawnPiece = new ChessPieceBlackPawn(5, new("E5"));
            board.PopulateBoard(new List<ChessPiece>() { whitePawn3Piece, blackPawnPiece });

            Assert.That(whitePawn3Piece.IsValidMove(board, new("E5")), Is.True);
        }

        [TestCase("B2")]
        [TestCase("B3")]
        [TestCase("G2")]
        [TestCase("G3")]
        public void WhitePawn_GetValidSquares_AlwaysReturns4_FromThesePositions(string startPos)
        {
            BoardPosition startingPosition = new(startPos);
            ChessPiece piece = new ChessPieceWhitePawn(1, startingPosition);
            ChessBoard chessBoard = new();
            chessBoard.AddPiece(piece);

            List<Square> squares = piece.GetValidSquares(chessBoard);

            Assert.That(squares.Count, Is.EqualTo(4));
        }

        [TestCase("A7")]
        [TestCase("H7")]
        public void WhitePawn_GetValidSquares_AlwaysReturns2_FromThesePositions(string startPos)
        {
            BoardPosition startingPosition = new(startPos);
            ChessPiece piece = new ChessPieceWhitePawn(1, startingPosition);
            ChessBoard chessBoard = new();
            chessBoard.AddPiece(piece);

            List<Square> squares = piece.GetValidSquares(chessBoard);

            Assert.That(squares.Count, Is.EqualTo(2));
        }

    }
}