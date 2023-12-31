using Chess;

namespace Tests
{
    public class ChessPieceFactoryTests
    {
        [Test]
        public void Test_BuildWhitePawns_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateWhitePawns();

            int i = 1;
            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                    Assert.That(piece.GetPiece(), Is.EqualTo(ChessPiece.Piece.PAWN));
                    Assert.That(piece.GetStartingPosition().VerticalValue, Is.EqualTo(BoardPosition.VERTICAL.TWO));
                    Assert.That(piece.GetStartingPosition().HorizontalValueAsInt, Is.EqualTo(i - 1));
                    Assert.That(piece.GetCurrentPosition().VerticalValue, Is.EqualTo(piece.GetStartingPosition().VerticalValue));
                    Assert.That(piece.GetCurrentPosition().HorizontalValue, Is.EqualTo(piece.GetStartingPosition().HorizontalValue));
                    Assert.That(piece.GetRealValue(), Is.EqualTo(11));
                    Assert.That(piece.GetId(), Is.EqualTo(i));
                });
                i++;
            }
        }

    }
}