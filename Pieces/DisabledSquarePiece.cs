using Chess.Board;

namespace Chess.Pieces
{
    [Serializable]
    public class DisabledSquarePiece : ChessPiece
    {
        public DisabledSquarePiece(BoardPosition startingPosition) : base(Piece.NO_PIECE, Color.NONE, 0, startingPosition)
        {
            _realValue = -1;
        }

        public override ChessPiece Clone()
        {
            DisabledSquarePiece copy = new(_startingPosition);
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            return true; // always produces valid move result
        }

        protected override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            return false;
        }
    }

}
