using Chess;

namespace Tests
{
    public class BoardPositionTests
    {
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

        [Test]
        public void Test_ConstructBoardPosition_Success()
        {
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.F, "F3");
            Assert.That(boardPosition, Is.Not.Null);
        }

        [Test]
        public void Test_ConstructBoardPosition_Success2()
        {
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.F);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo("F3"));
        }

        [Test]
        public void Test_ConstructBoardPosition_Success3()
        {
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.H);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo("H8"));
        }

        [Test]
        public void Test_ConstructBoardPosition_Success4()
        {
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo("A1"));
        }

        [TestCase("A1", 70)]
        [TestCase("A2", 60)]
        [TestCase("A3", 50)]
        [TestCase("A4", 40)]
        [TestCase("A5", 30)]
        [TestCase("A6", 20)]
        [TestCase("A7", 10)]
        [TestCase("A8", 00)]
        [TestCase("B1", 71)]
        [TestCase("B2", 61)]
        [TestCase("B3", 51)]
        [TestCase("B4", 41)]
        [TestCase("B5", 31)]
        [TestCase("B6", 21)]
        [TestCase("B7", 11)]
        [TestCase("B8", 01)]
        [TestCase("C1", 72)]
        [TestCase("C2", 62)]
        [TestCase("C3", 52)]
        [TestCase("C4", 42)]
        [TestCase("C5", 32)]
        [TestCase("C6", 22)]
        [TestCase("C7", 12)]
        [TestCase("C8", 02)]
        [TestCase("D4", 43)]
        [TestCase("E3", 54)]
        [TestCase("F6", 25)]
        [TestCase("G7", 16)]
        [TestCase("H5", 37)]
        public void Test_ConstructBoardPositionFromString_Success(string position, int expectedIndexValues)
        {
            BoardPosition boardPosition = new(position);
            int firstIndex = SplitIntegerGetFirstValue(expectedIndexValues);
            int secondIndex = SplitIntegerGetSecondValue(expectedIndexValues);

            Assert.Multiple(() =>
            {
                Assert.That(boardPosition.VerticalValue, Is.EqualTo((BoardPosition.VERTICAL)firstIndex));
                Assert.That(boardPosition.HorizontalValue, Is.EqualTo((BoardPosition.HORIZONTAL)secondIndex));
            });
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
            Assert.Throws<Exception>(() => new BoardPosition(position));
        }


        [Test]
        public void Test_CopyConstructBoardPosition_Success()
        {
            BoardPosition boardPosition = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            Assert.That(boardPosition, Is.Not.Null);
            
            BoardPosition boardPosition2 = new(boardPosition);
            Assert.Multiple(() =>
            {
                Assert.That(boardPosition2, Is.Not.Null);
                Assert.That(boardPosition.Equals(boardPosition2), Is.False); // should be a copy, not the same object reference
                Assert.That(boardPosition2.StringValue, Is.EqualTo("A1"));
            });
        }

        [Test]
        public void Test_BoardPositions_Are_Equal()
        {
            BoardPosition boardPosition1 = new BoardPosition(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            BoardPosition boardPosition2 = new BoardPosition(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            Assert.That(boardPosition1.EqualTo(boardPosition2));
        }

        [Test]
        public void Test_BoardPositions_Are_Not_Equal()
        {
            BoardPosition boardPosition1 = new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.F);
            BoardPosition boardPosition2 = new BoardPosition(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            Assert.That(boardPosition1.EqualTo(boardPosition2), Is.False);
        }

    }
}