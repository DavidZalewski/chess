using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessPieceKing : ChessPiece
    {
        public ChessPieceKing(Color color, int id, BoardPosition startingPosition) : base(Piece.KING, color, id, startingPosition)
        {
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            throw new NotImplementedException();
        }

        protected override void ImplementMove(ChessBoard board, BoardPosition position)
        {
            throw new NotImplementedException();
        }
    }
}
