using Chess;

namespace Tests
{
    public class ChessBoardTests
    {
        private ChessBoard chessBoard = new();

        [SetUp]
        public void Setup()
        {
            chessBoard = new ChessBoard();
        }

        public int SplitIntegerGetFirstValue(int value)
        {
            return value / 10;
        }

        public int SplitIntegerGetSecondValue(int value)
        {
            return value % 10;
        }

        [TestCase(73, 7)]
        [TestCase(85, 8)]
        [TestCase(66, 6)]
        [TestCase(59, 5)]
        [TestCase(11, 1)]
        [TestCase(8, 0)]
        public void Test_SplitIntegerGetFirstValue(int value, int expected)
        {
            Assert.That(SplitIntegerGetFirstValue(value), Is.EqualTo(expected));
        }

        [TestCase(73, 3)]
        [TestCase(85, 5)]
        [TestCase(66, 6)]
        [TestCase(59, 9)]
        [TestCase(11, 1)]
        [TestCase(8, 8)]
        public void Test_SplitIntegerGetSecondValue(int value, int expected)
        {
            Assert.That(SplitIntegerGetSecondValue(value), Is.EqualTo(expected));
        }

        [TestCase("A1", 70)]
        [TestCase("A2", 71)]
        [TestCase("A3", 72)]
        [TestCase("A4", 73)]
        [TestCase("A5", 74)]
        [TestCase("A6", 75)]
        [TestCase("A7", 76)]
        [TestCase("A8", 77)]
        [TestCase("B1", 60)]
        [TestCase("B2", 61)]
        [TestCase("B3", 62)]
        [TestCase("B4", 63)]
        [TestCase("B5", 64)]
        [TestCase("B6", 65)]
        [TestCase("B7", 66)]
        [TestCase("B8", 67)]
        [TestCase("C1", 50)]
        [TestCase("C2", 51)]
        [TestCase("C3", 52)]
        [TestCase("C4", 53)]
        [TestCase("C5", 54)]
        [TestCase("C6", 55)]
        [TestCase("C7", 56)]
        [TestCase("C8", 57)]
        [TestCase("D4", 43)]
        [TestCase("E3", 32)]
        [TestCase("F6", 25)]
        [TestCase("G7", 16)]
        [TestCase("H5", 04)]
        public void Test_GetBoardPositionSuccess(string position, int expectedIndexValues)
        {
            BoardPosition boardPosition = chessBoard.GetBoardPosition(position);
            int firstIndex = SplitIntegerGetFirstValue(expectedIndexValues);
            int secondIndex = SplitIntegerGetSecondValue(expectedIndexValues);

            Assert.That(boardPosition.VerticalValue, Is.EqualTo((BoardPosition.VERTICAL)firstIndex));
            Assert.That(boardPosition.HorizontalValue, Is.EqualTo((BoardPosition.HORIZONTAL)secondIndex));
        }


        [TestCase("X2")]
        [TestCase("")]
        [TestCase("__")]
        [TestCase("99")]
        [TestCase("AA")]
        [TestCase("11")]
        [TestCase("1A")]
        [TestCase("A")]
        [TestCase("1")]
        public void Test_GetBoardPositionExceptionThrown(string position)
        {
            Assert.Throws<Exception>(() => chessBoard.GetBoardPosition(position));
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

    }
}