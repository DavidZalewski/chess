using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessPieceKing : ChessPiece
    {
        public ChessPieceKing(Color color, BoardPosition startingPosition) : base(Piece.KING, color, 1, startingPosition)
        {
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            bool isFriendlyPieceOnSquare = board.IsPieceAtPosition(position, _color);
            if (isFriendlyPieceOnSquare) { return false; }

            int v1 = _currentPosition.VerticalValueAsInt;
            int v2 = position.VerticalValueAsInt;
            int h1 = _currentPosition.HorizontalValueAsInt;
            int h2 = position.HorizontalValueAsInt;

            int vdistance = v1 - v2;
            int hdistance = h1 - h2;

            // TODO: Simplify these
            if ((vdistance == 0 && hdistance == 1) || (vdistance == 0 && hdistance == -1)) // moving side to side
            {
                return true;
            }
            else if ((vdistance == 1 && hdistance == 0) || (vdistance == -1 && hdistance == 0)) // moving up / down
            {
                return true;
            }
            else if ((vdistance == 1 && hdistance == -1) || (vdistance == -1 && hdistance == -1)) // moving diagonal
            {
                return true;
            }
            else if ((vdistance == -1 && hdistance == 1) || (vdistance == -1 && hdistance == 1)) // moving diagonal
            {
                return true;
            }
            else if (vdistance == 1 && hdistance == 1) // moving diagonal upright
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void ImplementMove(ChessBoard board, BoardPosition position)
        {
            throw new NotImplementedException();
        }
    }
}
