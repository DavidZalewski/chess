using Chess.Board;
using Chess.Pieces;
using Chess.Services;

namespace Tests.Services
{
    public class PieceConverterServiceTests
    {
        [Test]
        public void Test_PromoteWhitePawnIntoWhiteQueen_Success()
        {
            BoardPosition h2 = new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.H);
            ChessPiece piece = new ChessPieceWhitePawn(1, h2);
            ChessPiece? queen = PieceConverterService.PromotePawn(piece, typeof(ChessPieceQueen));
            Assert.That(queen, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(queen.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(queen.GetCurrentPosition().EqualTo(h2), Is.True);
                Assert.That(queen.GetPiece(), Is.EqualTo(ChessPiece.Piece.QUEEN));
                Assert.That(queen.GetStartingPosition().EqualTo(h2), Is.True);
                Assert.That(queen.GetId(), Is.EqualTo(11));
                Assert.That(queen.GetRealValue(), Is.EqualTo(15));
            });
        }

        [Test]
        public void Test_PromoteBlackPawnIntoBlackRook_Success()
        {
            BoardPosition a4 = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.A);
            ChessPiece piece = new ChessPieceBlackPawn(5, a4); // pawn 5
            ChessPiece? rook = PieceConverterService.PromotePawn(piece, typeof(ChessPieceRook));
            Assert.That(rook, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(rook.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(rook.GetCurrentPosition().EqualTo(a4), Is.True);
                Assert.That(rook.GetPiece(), Is.EqualTo(ChessPiece.Piece.ROOK));
                Assert.That(rook.GetStartingPosition().EqualTo(a4), Is.True);
                Assert.That(rook.GetId(), Is.EqualTo(25)); // color + id (20 + 5) == 25
                Assert.That(rook.GetRealValue(), Is.EqualTo(24));
            });
        }

        [Test]
        public void Test_PromoteBlackPawnIntoBlackKnight_Success()
        {
            BoardPosition a4 = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.A);
            ChessPiece piece = new ChessPieceBlackPawn(7, a4);
            ChessPiece? knight = PieceConverterService.PromotePawn(piece, typeof(ChessPieceKnight));
            Assert.That(knight, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(knight.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(knight.GetCurrentPosition().EqualTo(a4), Is.True);
                Assert.That(knight.GetPiece(), Is.EqualTo(ChessPiece.Piece.KNIGHT));
                Assert.That(knight.GetStartingPosition().EqualTo(a4), Is.True);
                Assert.That(knight.GetId(), Is.EqualTo(27)); // COLOR + Id = (20 + 7) == 27
                Assert.That(knight.GetRealValue(), Is.EqualTo(22));
            });
        }

        [Test]
        public void Test_PromoteWhitePawnIntoWhiteBishop_Success()
        {
            BoardPosition h4 = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.H);
            ChessPiece piece = new ChessPieceWhitePawn(6, h4);
            ChessPiece? bishop = PieceConverterService.PromotePawn(piece, typeof(ChessPieceBishop));
            Assert.That(bishop, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(bishop.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(bishop.GetCurrentPosition().EqualTo(h4), Is.True);
                Assert.That(bishop.GetPiece(), Is.EqualTo(ChessPiece.Piece.BISHOP));
                Assert.That(bishop.GetStartingPosition().EqualTo(h4), Is.True);
                Assert.That(bishop.GetId(), Is.EqualTo(16)); // COLOR + Id = (10 + 6) == 16
                Assert.That(bishop.GetRealValue(), Is.EqualTo(13));
            });
        }

        [Test]
        public void Test_PromoteWhitePawnIntoUnknownType_ReturnsNull()
        {
            BoardPosition h4 = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.H);
            ChessPiece piece = new ChessPieceWhitePawn(6, h4);
            ChessPiece? unknown = PieceConverterService.PromotePawn(piece, typeof(double));
            Assert.That(unknown, Is.Null);
        }
    }
}
