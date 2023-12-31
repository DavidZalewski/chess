using Chess;

namespace Tests
{
    public class BoardPositionTests
    {
        [Test]
        public void Test_ConstructBoardPosition_Success()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.F, "F3");
            Assert.That(boardPosition, Is.Not.Null);
        }

        [Test]
        public void Test_ConstructBoardPosition_Success2()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.F);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo("F3"));
        }

        [Test]
        public void Test_ConstructBoardPosition_Success3()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.H);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo("H8"));
        }

        [Test]
        public void Test_ConstructBoardPosition_Success4()
        {
            BoardPosition boardPosition = new BoardPosition(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition.StringValue, Is.EqualTo("A1"));
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