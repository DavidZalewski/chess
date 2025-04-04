using Chess.Board;
using Chess.Pieces;
using static Chess.Pieces.ChessPiece;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class KnightTests : TestBase
    {
        private ChessBoard board = new ChessBoard();
        private BoardPosition whiteKnight1StartPosition = new(RANK.TWO, FILE.A);
        private BoardPosition whiteKnight2StartPosition = new(RANK.SEVEN, FILE.A);
        private BoardPosition blackKnight1StartPosition = new(RANK.TWO, FILE.H);
        private BoardPosition blackKnight2StartPosition = new(RANK.SEVEN, FILE.H);

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            board = new ChessBoard();
        }

        [Test]
        public void ConstructWhiteKnight_Success()
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
        public void CloneWhiteKnight_Success()
        {
            BoardPosition boardPosition = whiteKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, boardPosition);
            ChessPiece clone = piece.Clone();
            
            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, piece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(clone.GetRealValue(), Is.EqualTo(12));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(clone.GetPiece(), Is.EqualTo(ChessPiece.Piece.KNIGHT));
            });
        }

        [Test]
        public void ConstructBlackKnight_Success()
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

        [Test]
        public void CloneBlackKnight_Success()
        {
            BoardPosition boardPosition = blackKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, boardPosition);
            ChessPiece clone = piece.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, piece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(clone.GetRealValue(), Is.EqualTo(22));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(clone.GetPiece(), Is.EqualTo(ChessPiece.Piece.KNIGHT));
            });
        }

        [Test(Description = "Tests whether the white knight 1 can move to C1 on its first move")]
        public void WhiteKnight1_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = whiteKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, boardPosition);
            BoardPosition newPosition = new(RANK.ONE, FILE.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white knight 1 can move to C3 on its first move")]
        public void WhiteKnight1_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = whiteKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, boardPosition);
            BoardPosition newPosition = new(RANK.THREE, FILE.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white knight 2 can move to C6 on its first move")]
        public void WhiteKnight2_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = whiteKnight2StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 2, boardPosition);
            BoardPosition newPosition = new(RANK.SIX, FILE.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white knight 2 can move to C8 on its first move")]
        public void WhiteKnight2_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = whiteKnight2StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 2, boardPosition);
            BoardPosition newPosition = new(RANK.EIGHT, FILE.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black knight 1 can move to F1 on its first move")]
        public void BlackKnight1_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = blackKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, boardPosition);
            BoardPosition newPosition = new(RANK.ONE, FILE.F);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black knight 1 can move to F3 on its first move")]
        public void BlackKnight1_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = blackKnight1StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, boardPosition);
            BoardPosition newPosition = new(RANK.THREE, FILE.F);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black knight 2 can move to F6 on its first move")]
        public void BlackKnight2_IsValidMove_StartingMove1()
        {
            BoardPosition boardPosition = blackKnight2StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 2, boardPosition);
            BoardPosition newPosition = new(RANK.SIX, FILE.F);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the black knight 2 can move to F8 on its first move")]
        public void BlackKnight2_IsValidMove_StartingMove2()
        {
            BoardPosition boardPosition = blackKnight2StartPosition;
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 2, boardPosition);
            BoardPosition newPosition = new(RANK.EIGHT, FILE.F);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that white knight cannot capture its own piece")]
        public void WhiteKnight1_IsValidMove_InvalidCapture()
        {
            BoardPosition boardPosition = whiteKnight1StartPosition;
            BoardPosition c3 = new(RANK.THREE, FILE.C);
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, boardPosition);
            // Set White Pawn on C3
            board.SetBoardValue(c3, 11);
            Assert.That(piece.IsValidMove(board, c3), Is.False);
        }

        [Test(Description = "Tests that black knight cannot capture its own piece")]
        public void BlackKnight2_IsValidMove_InvalidCapture()
        {
            BoardPosition boardPosition = blackKnight2StartPosition;
            BoardPosition f3 = new(RANK.THREE, FILE.F);
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 2, boardPosition);
            // Set Black Pawn on F3

            board.SetBoardValue(f3, 21);
            Assert.That(piece.IsValidMove(board, f3), Is.False);
        }

        [Test(Description = "Tests that white knight can move to all of these valid locations if starting from C3")]
        public void WhiteKnight1_IsValidMove_FromC3()
        {
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new BoardPosition(RANK.THREE, FILE.C));

            // valid positions if knight on C3
            // A2, A4, B1, D1, E2, E4, D5, B5,  
            BoardPosition a2 = new(RANK.TWO, FILE.A);
            BoardPosition a4 = new(RANK.FOUR, FILE.A);
            BoardPosition b1 = new(RANK.ONE, FILE.B);
            BoardPosition d1 = new(RANK.ONE, FILE.D);
            BoardPosition e2 = new(RANK.TWO, FILE.E);
            BoardPosition e4 = new(RANK.FOUR, FILE.E);
            BoardPosition d5 = new(RANK.FIVE, FILE.D);
            BoardPosition b5 = new(RANK.FIVE, FILE.B);

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
        public void WhiteKnight1_IsValidMove_FromC3OnlySome()
        {
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new BoardPosition(RANK.THREE, FILE.C));

            // valid positions if knight on C3
            // A2, A4, B1, D1, E2, E4, D5, B5,  
            BoardPosition a2 = new(RANK.TWO, FILE.A);
            BoardPosition a4 = new(RANK.FOUR, FILE.A);

            BoardPosition b1 = new(RANK.ONE, FILE.B);
            BoardPosition b5 = new(RANK.FIVE, FILE.B);

            BoardPosition d1 = new(RANK.ONE, FILE.D);
            BoardPosition d5 = new(RANK.FIVE, FILE.D);

            BoardPosition e2 = new(RANK.TWO, FILE.E);
            BoardPosition e4 = new(RANK.FOUR, FILE.E);

            // Set White Pawn at B1 and B5, and D5

            board.SetBoardValue(b1, 11);
            board.SetBoardValue(b5, 11);
            board.SetBoardValue(d5, 11);
            //// Set White King at A4
            board.SetBoardValue(a4, 15);
            //// Set Black Pawn at E2 and E4
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
        public void BlackKnight1_IsValidMove_FromF3OnlySome()
        {
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new BoardPosition(RANK.THREE, FILE.F));

            // valid positions if knight on F3
            // H2, H4, G1, E1, D2, D4, E5, G5,  
            BoardPosition h2 = new(RANK.TWO, FILE.H);
            BoardPosition h4 = new(RANK.FOUR, FILE.H);

            BoardPosition g1 = new(RANK.ONE, FILE.G);
            BoardPosition g5 = new(RANK.FIVE, FILE.G);

            BoardPosition e1 = new(RANK.ONE, FILE.E);
            BoardPosition e5 = new(RANK.FIVE, FILE.E);

            BoardPosition d2 = new(RANK.TWO, FILE.D);
            BoardPosition d4 = new(RANK.FOUR, FILE.D);

            // Set Black Pawn at G1 and G5, and E5

            board.SetBoardValue(g1, 21);
            board.SetBoardValue(g5, 21);
            board.SetBoardValue(e5, 21);
            //// Set Black King at H4
            board.SetBoardValue(h4, 25);
            //// Set White Pawn at D2 and D4
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
        public void Knight1_IsValidMove_InvalidMovesFromD4()
        {
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new BoardPosition(RANK.FOUR, FILE.D));

            BoardPosition h2 = new(RANK.TWO, FILE.H);
            BoardPosition h4 = new(RANK.FOUR, FILE.H);

            BoardPosition g1 = new(RANK.ONE, FILE.G);
            BoardPosition g5 = new(RANK.FIVE, FILE.G);

            BoardPosition e1 = new(RANK.ONE, FILE.E);
            BoardPosition e5 = new(RANK.FIVE, FILE.E);

            BoardPosition d2 = new(RANK.TWO, FILE.D);
            BoardPosition d4 = new(RANK.FOUR, FILE.D);

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

        // Tests a knight in various positions on the board returns 8 possible squares each time
        [TestCase("C3")]
        [TestCase("D3")]
        [TestCase("E3")]
        [TestCase("F3")]
        [TestCase("C6")]
        [TestCase("D6")]
        [TestCase("E6")]
        [TestCase("F6")]
        public void Knight_GetValidSquares_AlwaysReturns8_FromThesePositions(string startPos)
        {
            BoardPosition startingPosition = new(startPos);
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, startingPosition);
            ChessBoard chessBoard = new();
            chessBoard.AddPiece(piece);

            List<BoardPosition> results = piece.GetPossiblePositions(chessBoard);

            Assert.That(results.Count, Is.EqualTo(8));
        }

        [TestCase("B3")]
        [TestCase("B4")]
        [TestCase("B5")]
        [TestCase("B6")]
        [TestCase("C2")]
        [TestCase("D2")]
        [TestCase("E2")]
        [TestCase("F2")]
        [TestCase("G3")]
        [TestCase("G4")]
        [TestCase("G5")]
        [TestCase("G6")]
        [TestCase("C7")]
        [TestCase("D7")]
        [TestCase("E7")]
        [TestCase("F7")]
        public void Knight_GetValidSquares_AlwaysReturns6_FromThesePositions(string startPos)
        {
            BoardPosition startingPosition = new(startPos);
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, startingPosition);
            ChessBoard chessBoard = new();
            chessBoard.AddPiece(piece);

            List<BoardPosition> results = piece.GetPossiblePositions(chessBoard);

            Assert.That(results.Count, Is.EqualTo(6));
        }

        [TestCase("B2")]
        [TestCase("B7")]
        [TestCase("G2")]
        [TestCase("G7")]
        public void Knight_GetValidSquares_AlwaysReturns4_FromThesePositions(string startPos)
        {
            BoardPosition startingPosition = new(startPos);
            ChessPiece piece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, startingPosition);
            ChessBoard chessBoard = new();
            chessBoard.AddPiece(piece);

            List<BoardPosition> results = piece.GetPossiblePositions(chessBoard);

            Assert.That(results.Count, Is.EqualTo(4));
        }

        [Test]
        public void Knight_GetAttackedPieces_Returns_Fork()
        {
            ChessPiece blackKing = new ChessPieceKing(Color.BLACK, new("D4"));
            ChessPiece blackRook1 = new ChessPieceRook(Color.BLACK, 1, new("E3"));
            ChessPiece blackRook2 = new ChessPieceRook(Color.BLACK, 2, new("A3"));
            ChessPiece blackQueen = new ChessPieceQueen(Color.BLACK, 1, new("B4"));
            ChessPiece whiteKnight = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new("E1"));
            ChessBoard chessBoard = new();
            chessBoard.AddPiece(blackKing);
            chessBoard.AddPiece(blackRook1);
            chessBoard.AddPiece(blackRook2);
            chessBoard.AddPiece(blackQueen);
            chessBoard.AddPiece(whiteKnight);

            whiteKnight.Move(chessBoard, new("C2"));

            List<ChessPiece> knightAttacking = whiteKnight.GetAttackedPieces(chessBoard);
            List<ChessPiece> blackAttacking = blackRook1.GetAttackedPieces(chessBoard);
            blackAttacking.AddRange(blackRook2.GetAttackedPieces(chessBoard));
            blackAttacking.AddRange(blackQueen.GetAttackedPieces(chessBoard));
            blackAttacking.AddRange(blackKing.GetAttackedPieces(chessBoard));

            Assert.Multiple(() =>
            {
                Assert.That(knightAttacking.Count == 4, "Expected White Knight to fork all Black Pieces");
                Assert.That(blackAttacking.Count == 0, "None of Black Pieces can attack this knight");
                Assert.That(knightAttacking[3] is ChessPieceRook, "Expected the Rook to be 4th element");
                Assert.That(knightAttacking[2] is ChessPieceRook, "Expected the Rook to be 3rd element");
                Assert.That(knightAttacking[1] is ChessPieceQueen, "Expected the Queen to be 2nd element");
                Assert.That(knightAttacking[0] is ChessPieceKing, "Expected the King to be 1st element");
            });
        }
    }
}