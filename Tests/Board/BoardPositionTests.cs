using Chess.Board;

namespace Tests.Board
{
    [Category("CORE")]
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
        public void SplitIntegerGetFirstValue(int value, int expected)
        {
            Assert.That(SplitIntegerGetFirstValue(value), Is.EqualTo(expected));
        }

        [TestCase(73, 3)]
        [TestCase(85, 5)]
        [TestCase(66, 6)]
        [TestCase(59, 9)]
        [TestCase(11, 1)]
        [TestCase(8, 8)]
        public void SplitIntegerGetSecondValue(int value, int expected)
        {
            Assert.That(SplitIntegerGetSecondValue(value), Is.EqualTo(expected));
        }

        [TestCase(RANK.THREE, FILE.F, "F3")]
        [TestCase(RANK.EIGHT, FILE.H, "H8")]
        [TestCase(RANK.ONE, FILE.A, "A1")]
        [TestCase(RANK.FOUR, FILE.G, "G4")]
        public void ConstructBoardPosition_Success(RANK rank, FILE file, string expectedStringValue)
        {
            BoardPosition boardPosition = new(rank, file);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo(expectedStringValue));
        }

        [TestCase("A", FILE.A)]
        [TestCase("B", FILE.B)]
        [TestCase("C", FILE.C)]
        [TestCase("D", FILE.D)]
        [TestCase("E", FILE.E)]
        [TestCase("F", FILE.F)]
        [TestCase("G", FILE.G)]
        [TestCase("H", FILE.H)]
        public void GetFile_Success(char alpha, FILE expected)
        {
            FILE file = BoardPosition.GetFile(alpha);
            Assert.That(file == expected, Is.True);
        }

        [TestCase("X")]
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("9")]
        [TestCase("!")]
        public void GetFile_ThrowsArgumentException(char alpha)
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
        public void GetRank_Success(int num, RANK expected)
        {
            RANK rank = BoardPosition.GetRank(num);
            Assert.That(rank == expected, Is.True);
        }

        [TestCase(0)]
        [TestCase(9)]
        [TestCase(1000)]
        public void GetRank_ThrowsArgumentException(int num)
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
        public void ConstructBoardPositionFromString_Success(string position, int expectedIndexValues)
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
        public void ConstructBoardPosition_ExceptionThrown(string position)
        {
            Assert.Throws<ArgumentException>(() => new BoardPosition(position));
        }

        [Test]
        public void BoardPositions_Are_Equal()
        {
            BoardPosition boardPosition1 = new(RANK.ONE, FILE.A);
            BoardPosition boardPosition2 = new(RANK.ONE, FILE.A);
            Assert.That(boardPosition1 == boardPosition2);
        }

        [Test]
        public void BoardPositions_Are_Equal_Both_Null()
        {
            BoardPosition? boardPosition1 = null;
            BoardPosition? boardPosition2 = null;
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.That(boardPosition1 == boardPosition2);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        [Test]
        public void BoardPositions_Are_Not_Equal()
        {
            BoardPosition boardPosition1 = new(RANK.FIVE, FILE.F);
            BoardPosition boardPosition2 = new(RANK.ONE, FILE.A);
            Assert.That(boardPosition1 != boardPosition2);
        }

        [Test]
        public void BoardPositions_Are_Not_Equal_Null_Handling()
        {
            BoardPosition boardPosition1 = new(RANK.FIVE, FILE.F);
            BoardPosition? boardPosition2 = null;
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.That(boardPosition1 != boardPosition2);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        [Test]
        public void Left_WhenCalled_ReturnsNewBoardPositionOneFileToTheLeft()
        {
            BoardPosition pos = new(RANK.EIGHT, FILE.D);
            BoardPosition newPos = pos.Left();

            Assert.That(newPos.File, Is.EqualTo(FILE.C));
        }

        [Test]
        public void Right_WhenCalled_ReturnsNewBoardPositionOneFileToTheRight()
        {
            BoardPosition pos = new(RANK.EIGHT, FILE.D);
            BoardPosition newPos = pos.Right();

            Assert.That(newPos.File, Is.EqualTo(FILE.E));
        }

        [Test]
        public void Up_WhenCalled_ReturnsNewBoardPositionOneRankUp()
        {
            BoardPosition pos = new(RANK.SEVEN, FILE.D);
            BoardPosition newPos = pos.Up();

            Assert.That(newPos.Rank, Is.EqualTo(RANK.EIGHT));
        }

        [Test]
        public void Down_WhenCalled_ReturnsNewBoardPositionOneRankDown()
        {
            BoardPosition pos = new(RANK.SEVEN, FILE.D);
            BoardPosition newPos = pos.Down();

            Assert.That(newPos.Rank, Is.EqualTo(RANK.SIX));
        }

        [Test]
        public void Offset_WhenCalled_ReturnsNewBoardPositionWithOffsetApplied()
        {
            BoardPosition pos = new(RANK.SEVEN, FILE.D);
            BoardPosition newPos = pos.Offset(-2, 1);

            Assert.That(newPos.Rank, Is.EqualTo(RANK.FIVE));
            Assert.That(newPos.File, Is.EqualTo(FILE.E));
        }

        [Test]
        public void IsDiagonal_WhenCalled_ReturnsTrueIfPositionsAreDiagonal()
        {
            BoardPosition pos1 = new(RANK.SEVEN, FILE.D);
            BoardPosition pos2 = new(RANK.FIVE, FILE.B);

            Assert.That(pos1.IsDiagonal(pos2), Is.True);
        }

        [Test]
        public void IsOnSameFile_WhenCalled_ReturnsTrueIfPositionsAreOnTheSameFile()
        {
            BoardPosition pos1 = new(RANK.SEVEN, FILE.D);
            BoardPosition pos2 = new(RANK.FIVE, FILE.D);

            Assert.That(pos1.IsOnSameFile(pos2), Is.True);
        }

        [Test]
        public void IsOnSameRank_WhenCalled_ReturnsTrueIfPositionsAreOnTheSameRank()
        {
            BoardPosition pos1 = new(RANK.SEVEN, FILE.D);
            BoardPosition pos2 = new(RANK.SEVEN, FILE.B);

            Assert.That(pos1.IsOnSameRank(pos2), Is.True);
        }

    }
}