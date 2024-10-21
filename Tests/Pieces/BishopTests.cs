using Chess.Board;
using Chess.Pieces;
using Chess.Services;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class BishopTests : TestBase
    {
        private ChessBoard board = new ChessBoard();
        private BoardPosition whiteBishop1StartPosition = new(RANK.THREE, FILE.A);
        private BoardPosition whiteBishop2StartPosition = new(RANK.SIX, FILE.A);
        private BoardPosition blackBishop1StartPosition = new(RANK.THREE, FILE.H);
        private BoardPosition blackBishop2StartPosition = new(RANK.SIX, FILE.H);

        public BishopTests()
        {
            Setup();
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            board = new ChessBoard();
        }

        [Test]
        public void ConstructWhiteBishop_Success()
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
        public void ConstructBlackBishop_Success()
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

        [Test]
        public void CloneWhiteBishop_Success()
        {
            BoardPosition boardPosition = whiteBishop2StartPosition;
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, boardPosition);
            ChessPiece clone = piece.Clone();
       
            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, piece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(clone.GetRealValue(), Is.EqualTo(13));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(clone.GetPiece(), Is.EqualTo(ChessPiece.Piece.BISHOP));
            });
        }

        [Test]
        public void CloneBlackBishop_Success()
        {
            BoardPosition boardPosition = blackBishop2StartPosition;
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.BLACK, 1, boardPosition);
            ChessPiece clone = piece.Clone();

            
            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, piece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(boardPosition));
                Assert.That(clone.GetRealValue(), Is.EqualTo(23));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(clone.GetPiece(), Is.EqualTo(ChessPiece.Piece.BISHOP));
            });
        }

        [Test(Description = "Tests whether the white bishop 1 can move to C1 from A3 with no other pieces on board")]
        public void WhiteBishop1_IsValidMove_StartingMove1()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            BoardPosition newPosition = new(RANK.ONE, FILE.C);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests whether the white bishop 1 can move to B4 from A3 with no other pieces on board")]
        public void WhiteBishop1_IsValidMove_StartingMove2()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            BoardPosition newPosition = new(RANK.FOUR, FILE.B);

            Assert.That(piece.IsValidMove(board, newPosition), Is.True);
        }

        [Test(Description = "Tests that white bishop 1 cannot move because there are pieces blocking its path")]
        public void WhiteBishop1_IsValidMove_CannotMoveBlocked()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            List<BoardPosition> positions = new()
            {
                new(RANK.TWO, FILE.B),
                new(RANK.ONE, FILE.C),
                new(RANK.FOUR, FILE.B),
                new(RANK.FIVE, FILE.C),
                new(RANK.SIX, FILE.D),
                new(RANK.SEVEN, FILE.E),
                new(RANK.EIGHT, FILE.F),
            };

            // Set White Pawns on B2 and B4 which block Bishop from moving

            board.SetBoardValue(new(RANK.TWO, FILE.B), 11);
            board.SetBoardValue(new(RANK.FOUR, FILE.B), 11);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white bishop 1 cannot jump over a black piece on a diagonal")]
        public void WhiteBishop1_IsValidMove_CannotJumpPieces()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            List<BoardPosition> positions = new()
            {
                new(RANK.FIVE, FILE.C),
                new(RANK.SIX, FILE.D),
                new(RANK.SEVEN, FILE.E),
                new(RANK.EIGHT, FILE.F),
            };

            // Set Black Pawn on B4 which block Bishop

            board.SetBoardValue(new(RANK.FOUR, FILE.B), 21);

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in positions)
                {
                    Assert.That(piece.IsValidMove(board, boardPosition), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white bishop 1 can capture")]
        public void WhiteBishop1_IsValidMove_CanCapture()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, whiteBishop1StartPosition);
            BoardPosition d6 = new(RANK.SIX, FILE.D);

            // Set Black Pawn on D6

            board.SetBoardValue(d6, 21);

            Assert.That(piece.IsValidMove(board, d6), Is.True);
        }


        [Test(Description = "Tests that white bishop can move to all of these valid locations if starting from D4")]
        public void WhiteBishop1_IsValidMove_FromD4()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, new BoardPosition(RANK.FOUR, FILE.D));

            // valid positions if bishop on D4
            // C3, B2, A1, C5, B6, A7, E3, F2, G1, E5, F6, G7, H8 
            List<BoardPosition> validPositions = new()
            {
                new(RANK.THREE, FILE.C),
                new(RANK.TWO, FILE.B),
                new(RANK.ONE, FILE.A),
                new(RANK.FIVE, FILE.C),
                new(RANK.SIX, FILE.B),
                new(RANK.SEVEN, FILE.A),
                new(RANK.THREE, FILE.E),
                new(RANK.TWO, FILE.F),
                new(RANK.ONE, FILE.G),
                new(RANK.FIVE, FILE.E),
                new(RANK.SIX, FILE.F),
                new(RANK.SEVEN, FILE.G),
                new(RANK.EIGHT, FILE.H)
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
        public void WhiteBishop1_IsValidMove_FromC1ToG5()
        {
            ChessPiece piece = new ChessPieceBishop(ChessPiece.Color.WHITE, 1, new("C1"));

            board.SetBoardValue(new("D3"), 22);
            board.SetBoardValue(new("E2"), 23);
            Assert.That(piece.IsValidMove(board, new("G5")), Is.True);
        }

        [Test(Description = "Tests that Black Bishop can move from C8 to D7 with pieces on board not in its path")]
        public void BlackBishop1_IsValidMove_FromC8ToD7()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
            board.PopulateBoard(chessPieces);

            ChessPiece blackPawn4 = chessPieces.First(piece => piece.GetPiece().Equals(ChessPiece.Piece.PAWN) &&
                                                              piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetId().Equals(4));

            ChessPiece blackBishop1 = chessPieces.First(piece => piece.GetPiece().Equals(ChessPiece.Piece.BISHOP) &&
                                                              piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetId().Equals(1));

            Assert.That(blackPawn4.HasMoved(), Is.False);
            Assert.That(blackPawn4.IsValidMove(board, new("D6")), Is.True);
            blackPawn4.Move(board, new("D6"));
            Assert.That(blackPawn4.HasMoved(), Is.True);
            Assert.That(blackBishop1.IsValidMove(board, new("D7")), Is.True);
        }

        [Test(Description = "Tests that White Bishop cannot move from C1 to C3")]
        public void WhiteBishop1_IsValidMove_FromC1ToC3()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
            board.PopulateBoard(chessPieces);

            ChessPiece whitePawn2 = chessPieces.First(piece => piece.GetPiece().Equals(ChessPiece.Piece.PAWN) &&
                                                              piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetId().Equals(2));

            ChessPiece whiteBishop1 = chessPieces.First(piece => piece.GetPiece().Equals(ChessPiece.Piece.BISHOP) &&
                                                              piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetId().Equals(1));

            Assert.That(whitePawn2.IsValidMove(board, new("B3")), Is.True);
            whitePawn2.Move(board, new("B3"));
            Assert.That(whitePawn2.HasMoved(), Is.True);
            Assert.That(whiteBishop1.IsValidMove(board, new("C3")), Is.False);
        }

    }
}