using Chess.Board;
using Chess.Pieces;
using Chess.Services;

namespace Tests.Board
{
    public class ChessBoardTests
    {
        private ChessBoard chessBoard = new();

        [SetUp]
        public void Setup()
        {
            chessBoard = new ChessBoard();
        }

        [Test]
        public void Test_CopyConstructChessBoard_Success()
        {
            ChessBoard board1 = new();
            ChessBoard board2 = new(board1);

            Assert.Multiple(() =>
            {
                Assert.That(board1, Is.Not.Null);
                Assert.That(board2, Is.Not.Null);
                Assert.That(board1, Is.Not.EqualTo(board2));
                Assert.That(board1.GetBoard(), Is.EqualTo(board2.GetBoard()));
                Assert.That(Object.ReferenceEquals(board1.GetBoard(),board2.GetBoard()), Is.False);
            });
        }

        [Test]
        public void Test_IsPieceAtPosition_Success()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.THREE, FILE.F);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 11);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.True);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColor_Success1()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.THREE, FILE.F);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 11);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.WHITE), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK), Is.False);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColor_Success2()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.SIX, FILE.G);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 21);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.WHITE), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK), Is.True);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColorAndPiece_Success1()
        {
            BoardPosition boardPosition = new("G6");
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 21);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.WHITE), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK, ChessPiece.Piece.PAWN), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK, ChessPiece.Piece.ROOK), Is.False);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColorAndPiece_Success2()
        {
            BoardPosition boardPosition = new("G6");
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 24);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.WHITE), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK, ChessPiece.Piece.PAWN), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK, ChessPiece.Piece.ROOK), Is.True);
        }

        [Test]
        public void Test_PopulateBoard_Success()
        {
            chessBoard.PopulateBoard(ChessPieceFactory.CreateChessPieces());
            int[,] innerBoard = chessBoard.GetBoard();
            int[,] expectedBoard = new int[8, 8]
                {
                    { 24, 22, 23, 25, 26, 23, 22, 24 },
                    { 21, 21, 21, 21, 21, 21, 21, 21 },
                    { 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 11, 11, 11, 11, 11, 11, 11, 11 },
                    { 14, 12, 13, 15, 16, 13, 12, 14 }
                };

            // LINQ to test for 2d array equality
            bool equal = innerBoard.Rank == expectedBoard.Rank &&
                        Enumerable.Range(0, innerBoard.Rank)
                        .All(dimension => innerBoard.GetLength(dimension) == expectedBoard.GetLength(dimension)) &&
                        innerBoard.Cast<int>().SequenceEqual(expectedBoard.Cast<int>());

            Assert.That(equal, Is.True);
        }

        [Test]
        public void Test_PruneCapturedPieces_Success()
        {
            ChessPiece whiteQueenPiece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, new("D1"));
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new("E1"));
            ChessPiece whiteBishopPiece = new ChessPieceBishop(ChessPiece.Color.WHITE, 2, new("F1"));
            ChessPiece whitePawn4Piece = new ChessPieceWhitePawn(4, new("D2"));
            ChessPiece whitePawn5Piece = new ChessPieceWhitePawn(4, new("E2"));
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(4, new("F2"));
            ChessPiece blackBishopPiece = new ChessPieceBishop(ChessPiece.Color.BLACK, 1, new("A5"));
            ChessPiece blackRookPiece = new ChessPieceRook(ChessPiece.Color.BLACK, 1, new("A1"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));
            ChessPiece blackPawn1Piece = new ChessPieceBlackPawn(1, new("F5"));

            List<ChessPiece> chessPiecesOnBoard = new()
            {
                whiteQueenPiece, whiteKingPiece, whiteBishopPiece, whitePawn4Piece,
                whitePawn5Piece, whitePawn6Piece, blackBishopPiece, blackRookPiece
            };

            chessBoard.PopulateBoard(chessPiecesOnBoard);

            List<ChessPiece> allChessPieces = new()
            {
                whiteQueenPiece, whiteKingPiece, whiteBishopPiece, whitePawn4Piece,
                whitePawn5Piece, whitePawn6Piece, blackBishopPiece, blackRookPiece, blackKnightPiece, blackPawn1Piece
            };

            List<ChessPiece> prunedPieces = chessBoard.PruneCapturedPieces(allChessPieces, lcp => true);

            Assert.That(prunedPieces.SequenceEqual(chessPiecesOnBoard), Is.True);
        }

    }
}