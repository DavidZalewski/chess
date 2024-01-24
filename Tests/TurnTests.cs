using Chess;
using Chess.Board;
using Chess.Pieces;
using Chess.Services;

namespace Tests
{
    public class TurnTests
    {
        [Test]
        public void Test_ConstructTurn_Success()
        {
            ChessBoard board = new();
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();
            ChessPiece whitePawn = chessPieces.First(pieces => pieces.GetColor() == ChessPiece.Color.WHITE &&
                                                     pieces.GetPiece() == ChessPiece.Piece.PAWN);
            BoardPosition previousPosition = new(whitePawn.GetCurrentPosition());
            BoardPosition newPosition = new(previousPosition.VerticalValue - 2, previousPosition.HorizontalValue);
            board.PopulateBoard(chessPieces);

            Assert.That(whitePawn.IsValidMove(board, newPosition), Is.True);

            Turn turn1 = new(1, whitePawn, previousPosition, newPosition, board, chessPieces);

            Assert.That(turn1, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(turn1.ChessPiece.GetPiece().Equals(whitePawn.GetPiece()) && turn1.ChessPiece.GetId().Equals(whitePawn.GetId()), Is.True);
                Assert.That(turn1.NewPosition, Is.EqualTo(newPosition));
                Assert.That(turn1.TurnNumber, Is.EqualTo(1));
                Assert.That(turn1.PreviousPosition, Is.EqualTo(previousPosition));
                Assert.That(turn1.ChessBoard, Is.Not.Null);
                Assert.That(turn1.ChessBoard.Equals(board), Is.False); // should be a copy, not the same object reference
                Assert.That(turn1.PlayerTurn, Is.EqualTo(Turn.Color.WHITE));
            });
        }
    }
}
