using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessPieceQueen : ChessPiece
    {
        private ChessPieceRook _chessPieceRook;
        private ChessPieceBishop _chessPieceBishop;

        public ChessPieceQueen(Color color, int id, BoardPosition startingPosition) : base(Piece.QUEEN, color, id, startingPosition)
        {
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
            _chessPieceBishop = new ChessPieceBishop(color, 5, startingPosition);
            _chessPieceRook = new ChessPieceRook(color, 5, startingPosition);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            // need to update these ghost pieces positions everytime this is called
            // need to make a unit test that checks for this (move the piece twice and do 2 IsValidMove tests on it
            _chessPieceRook.SetCurrentPosition(_currentPosition);
            _chessPieceBishop.SetCurrentPosition(_currentPosition);

            // beautifully simple
            bool isValidBishopMove = _chessPieceBishop.IsValidMove(board, position);
            bool isValidRookMove = _chessPieceRook.IsValidMove(board, position);
            return isValidBishopMove || isValidRookMove;
        }

        protected override void ImplementMove(ChessBoard board, BoardPosition position)
        {
            // does this need to exist?
            
        }
    }
}
