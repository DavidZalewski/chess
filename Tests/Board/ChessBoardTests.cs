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
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.F);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 11);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.True);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColor_Success1()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.F);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 11);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.WHITE), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK), Is.False);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColor_Success2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.G);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 21);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.WHITE), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, ChessPiece.Color.BLACK), Is.True);
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

    }
}