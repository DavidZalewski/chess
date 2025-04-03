using Chess.Board;
using Chess.Globals;

namespace Chess.Pieces
{
    [Serializable]
    public class DisabledSquarePiece : ChessPiece
    {
        public DisabledSquarePiece(BoardPosition startingPosition) : base(Piece.NO_PIECE, Color.NONE, 0, startingPosition)
        {
            StaticLogger.Trace();
            _realValue = -1;
        }

        public override ChessPiece Clone()
        {
            StaticLogger.Trace();
            DisabledSquarePiece copy = new(_startingPosition);
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            return true; // always produces valid move result
        }

        public override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            return false;
        }

        public override List<BoardPosition> GetPossiblePositions(ChessBoard chessBoard)
        {
            StaticLogger.Trace();
            return new();  
        }
    }

}
