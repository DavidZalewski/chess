using Chess;

namespace Tests
{
    public class Tests
    {
        private ChessBoard chessBoard;

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
        [TestCase(59, 5)]
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
        public void Test1(string position, int expectedIndexValues)
        {
            chessBoard.GetBoardPosition()
            Assert.Pass();
        }
    }
}