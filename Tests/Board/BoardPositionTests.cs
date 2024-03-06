using Chess.Board;

namespace Tests.Board
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
        public void Test_ConstructBoardPosition_Success2()
        {
            BoardPosition boardPosition = new(RANK.THREE, FILE.F);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo("F3"));
        }

        [Test]
        public void Test_ConstructBoardPosition_Success3()
        {
            BoardPosition boardPosition = new(RANK.EIGHT, FILE.H);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo("H8"));
        }

        [Test]
        public void Test_ConstructBoardPosition_Success4()
        {
            BoardPosition boardPosition = new(RANK.ONE, FILE.A);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo("A1"));
        }

        [TestCase("A", FILE.A)]
        [TestCase("B", FILE.B)]
        [TestCase("C", FILE.C)]
        [TestCase("D", FILE.D)]
        [TestCase("E", FILE.E)]
        [TestCase("F", FILE.F)]
        [TestCase("G", FILE.G)]
        [TestCase("H", FILE.H)]
        public void Test_GetFile_Success(char alpha, FILE expected)
        {
            FILE file = BoardPosition.GetFile(alpha);
            Assert.That(file == expected, Is.True);
        }

        [TestCase("X")]
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("9")]
        [TestCase("!")]
        public void Test_GetFile_ThrowsArgumentException(char alpha)
        {
            Assert.Throws<ArgumentException>(() => BoardPosition.GetFile(alpha));
        }

        [TestCase(1, RANK.ONE)]
        [TestCase(2, RANK.TWO)]
        [TestCase(3, RANK.THREE)]
        [TestCase(4, RANK.FOUR)]
        [TestCase(5, RANK.FIVE)]
        [TestCase(6, RANK.SIX)]
        [TestCase(7, RANK.SEVEN)]
        [TestCase(8, RANK.EIGHT)]
        public void Test_GetRank_Success(int num, RANK expected)
        {
            RANK rank = BoardPosition.GetRank(num);
            Assert.That(rank == expected, Is.True);
        }

        [TestCase(0)]
        [TestCase(9)]
        [TestCase(1000)]
        public void Test_GetRank_ThrowsArgumentException(int num)
        {
            Assert.Throws<ArgumentException>(() => BoardPosition.GetRank(num));
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
                Assert.That(boardPosition.Rank, Is.EqualTo((RANK)firstIndex));
                Assert.That(boardPosition.File, Is.EqualTo((FILE)secondIndex));
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
        public void Test_ConstructBoardPosition_ExceptionThrown(string position)
        {
            Assert.Throws<ArgumentException>(() => new BoardPosition(position));
        }


        [Test]
        public void Test_CopyConstructBoardPosition_Success()
        {
            BoardPosition boardPosition = new(RANK.ONE, FILE.A);
            Assert.That(boardPosition, Is.Not.Null);

            BoardPosition boardPosition2 = new(boardPosition);
            Assert.Multiple(() =>
            {
                Assert.That(boardPosition2, Is.Not.Null);
                Assert.That(ReferenceEquals(boardPosition, boardPosition2), Is.False); // should be a copy, not the same object reference
                Assert.That(boardPosition2.StringValue, Is.EqualTo("A1"));
            });
        }

        [Test]
        public void Test_BoardPositions_Are_Equal()
        {
            BoardPosition boardPosition1 = new(RANK.ONE, FILE.A);
            BoardPosition boardPosition2 = new(RANK.ONE, FILE.A);
            Assert.That(boardPosition1 == boardPosition2);
        }

        [Test]
        public void Test_BoardPositions_Are_Equal_Both_Null()
        {
            BoardPosition? boardPosition1 = null;
            BoardPosition? boardPosition2 = null;
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.That(boardPosition1 == boardPosition2);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        [Test]
        public void Test_BoardPositions_Are_Not_Equal()
        {
            BoardPosition boardPosition1 = new(RANK.FIVE, FILE.F);
            BoardPosition boardPosition2 = new(RANK.ONE, FILE.A);
            Assert.That(boardPosition1 != boardPosition2);
        }

        [Test]
        public void Test_BoardPositions_Are_Not_Equal_Null_Handling()
        {
            BoardPosition boardPosition1 = new(RANK.FIVE, FILE.F);
            BoardPosition? boardPosition2 = null;
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.That(boardPosition1 != boardPosition2);
#pragma warning restore CS8604 // Possible null reference argument.
        }

    }
}