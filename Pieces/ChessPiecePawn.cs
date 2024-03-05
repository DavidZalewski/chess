﻿using Chess.Board;

namespace Chess.Pieces
{
    [Serializable]
    public abstract class ChessPiecePawn : ChessPiece
    {
        public bool MovedTwoSquares { get; protected set; }
        public bool IsEnPassantTarget { get; set;}
        protected ChessPiecePawn(Color color, int id, BoardPosition startingPosition) : base(Piece.PAWN, color, id, startingPosition)
        {
            IsEnPassantTarget = false;
        }

        // Used by tests to set state for testing purposes
        internal void SetMovedTwoSquares()
        {
            MovedTwoSquares = true;
        }
    }
}
