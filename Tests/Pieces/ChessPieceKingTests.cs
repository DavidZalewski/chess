using Chess.Board;
using Chess.Pieces;

namespace Tests.Pieces
{
    public class ChessPieceKingTests
    {
        private readonly BoardPosition e1 = new(RANK.ONE, FILE.E);
        private readonly BoardPosition e8 = new(RANK.EIGHT, FILE.E);

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test_ConstructWhiteKing_Success()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(RANK.ONE, FILE.E));

            Assert.That(whiteKingPiece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(whiteKingPiece.GetCurrentPosition(), Is.EqualTo(e1));
                Assert.That(whiteKingPiece.GetRealValue(), Is.EqualTo(16));
                Assert.That(whiteKingPiece.GetId(), Is.EqualTo(1));
                Assert.That(whiteKingPiece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(whiteKingPiece.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(whiteKingPiece.GetStartingPosition().EqualTo(whiteKingPiece.GetCurrentPosition()), Is.True);
            });
        }

        [Test]
        public void Test_CloneWhiteKing_Success()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(RANK.ONE, FILE.E));
            ChessPiece clone = whiteKingPiece.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, whiteKingPiece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(e1));
                Assert.That(clone.GetRealValue(), Is.EqualTo(16));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(clone.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(clone.GetStartingPosition().EqualTo(whiteKingPiece.GetCurrentPosition()), Is.True);
            });
        }

        [Test]
        public void Test_ConstructBlackKing_Success()
        {
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(RANK.EIGHT, FILE.E));
            Assert.That(blackKingPiece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(blackKingPiece.GetCurrentPosition().EqualTo(e8), Is.True);
                Assert.That(blackKingPiece.GetRealValue(), Is.EqualTo(26));
                Assert.That(blackKingPiece.GetId(), Is.EqualTo(1));
                Assert.That(blackKingPiece.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(blackKingPiece.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(blackKingPiece.GetStartingPosition().EqualTo(blackKingPiece.GetCurrentPosition()), Is.True);
            });
        }

        [Test]
        public void Test_CloneBlackKing_Success()
        {
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(RANK.EIGHT, FILE.E));
            ChessPiece clone = blackKingPiece.Clone();
            
            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, blackKingPiece), Is.False);
                Assert.That(clone.GetCurrentPosition().EqualTo(e8), Is.True);
                Assert.That(clone.GetRealValue(), Is.EqualTo(26));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(clone.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(clone.GetStartingPosition().EqualTo(blackKingPiece.GetCurrentPosition()), Is.True);
            });
        }

        private ChessBoard InitializeFullBoard()
        {
            ChessBoard chessBoard = new();
            int[,] boardValue = new int[8, 8]
            {
                { 24 /*A8*/, 22, 23, 25, 26, 23, 22, 24 /*H8*/},
                { 21, 21, 21, 21, 21, 21, 21, 21 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 11, 11, 11, 11, 11, 11, 11, 11 },
                { 14 /*A1*/, 12, 13, 15, 16, 13, 12, 11 /*H1*/},
            };
            chessBoard.InternalTestOnly_SetBoard(boardValue);

            return chessBoard;
        }

        [Test(Description = "Tests that the white king has no valid moves it can make at the start of the game")]
        public void Test_WhiteKing_IsValidMove_NoValidMovesFromStart()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(RANK.ONE, FILE.E));
            ChessBoard chessBoard = InitializeFullBoard();
            List<BoardPosition> possibleMoves = new();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possibleMoves.Add(new BoardPosition((RANK)i, (FILE)j));
                }
            }

            Assert.Multiple(() =>
            {
                foreach (BoardPosition possibleMove in possibleMoves)
                {
                    Assert.That(whiteKingPiece.IsValidMove(chessBoard, possibleMove), Is.False, possibleMove.StringValue);
                }
            });
        }

        [Test(Description = "Tests that the black king has no valid moves it can make at the start of the game")]
        public void Test_BlackKing_IsValidMove_NoValidMovesFromStart()
        {
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(RANK.EIGHT, FILE.E));
            ChessBoard chessBoard = InitializeFullBoard();
            List<BoardPosition> possibleMoves = new();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possibleMoves.Add(new BoardPosition((RANK)i, (FILE)j));
                }
            }

            Assert.Multiple(() =>
            {
                foreach (BoardPosition possibleMove in possibleMoves)
                {
                    Assert.That(blackKingPiece.IsValidMove(chessBoard, possibleMove), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white or black king on empty board starting on D4 can move to all valid positions")]
        public void Test_King_IsValidMove_ValidMovesFromD4()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(RANK.FOUR, FILE.D));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(RANK.FOUR, FILE.D));
            ChessBoard chessBoard = new(); // blank chess board with 2 kings on it
            List<BoardPosition> validMoves = new()
            {
                new BoardPosition(RANK.THREE, FILE.D), // D3
                new BoardPosition(RANK.THREE, FILE.C), // C3
                new BoardPosition(RANK.THREE, FILE.E), // E3
                new BoardPosition(RANK.FOUR, FILE.C), // C4
                new BoardPosition(RANK.FOUR, FILE.E), // E4
                new BoardPosition(RANK.FIVE, FILE.D), // D5
                new BoardPosition(RANK.FIVE, FILE.C), // C5
                new BoardPosition(RANK.FIVE, FILE.E), // E5
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in validMoves)
                {
                    Assert.That(whiteKingPiece.IsValidMove(chessBoard, boardPosition), Is.True);
                    Assert.That(blackKingPiece.IsValidMove(chessBoard, boardPosition), Is.True);
                }
            });
        }

        [Test(Description = "Tests that white or black king on empty board starting on D4 cannot move to these positions")]
        public void Test_King_IsValidMove_InvalidMovesFromD4()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(RANK.FOUR, FILE.D));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(RANK.FOUR, FILE.D));
            ChessBoard chessBoard = new();
            List<BoardPosition> validMoves = new()
            {
                new BoardPosition(RANK.THREE, FILE.D), // D3
                new BoardPosition(RANK.THREE, FILE.C), // C3
                new BoardPosition(RANK.THREE, FILE.E), // E3
                new BoardPosition(RANK.FOUR, FILE.C), // C4
                new BoardPosition(RANK.FOUR, FILE.E), // E4
                new BoardPosition(RANK.FIVE, FILE.D), // D5
                new BoardPosition(RANK.FIVE, FILE.C), // C5
                new BoardPosition(RANK.FIVE, FILE.E), // E5
            };
            List<BoardPosition> possibleMoves = new();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possibleMoves.Add(new BoardPosition((RANK)i, (FILE)j));
                }
            }

            Assert.Multiple(() =>
            {
                int assertsInvoked = 0;
                foreach (BoardPosition position in possibleMoves)
                {
                    if (!validMoves.Any(vm => vm.File == position.File && vm.Rank == position.Rank))
                    {
                        Assert.That(whiteKingPiece.IsValidMove(chessBoard, position), Is.False);
                        Assert.That(blackKingPiece.IsValidMove(chessBoard, position), Is.False);
                        assertsInvoked++;
                    }
                }
                Assert.That(assertsInvoked, Is.GreaterThan(0));
            });
        }





    }
}
