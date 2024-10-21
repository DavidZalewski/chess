using Chess.Board;
using Chess.Globals;

namespace Chess.Pieces
{
    [Serializable]
    public abstract class ChessPiecePawn : ChessPiece
    {
        public bool MovedTwoSquares { get; protected set; } = false;
        public bool IsEnPassantTarget { get; set; } = false;

        protected ChessPiecePawn(Color color, int id, BoardPosition startingPosition) : base(Piece.PAWN, color, id, startingPosition)
        {
            StaticLogger.Trace();
            IsEnPassantTarget = false;
        }

        // Used by tests to set state for testing purposes
        internal void SetMovedTwoSquares()
        {
            StaticLogger.Trace();
            MovedTwoSquares = true;
        }
    }
}
