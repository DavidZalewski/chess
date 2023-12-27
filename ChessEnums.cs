using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class ChessEnums
    {
        enum Piece
        {
            PAWN = 1,
            KNIGHT = 2,
            BISHOP = 3,
            ROOK = 4,
            QUEEN = 5,
            KING = 6
        }

        enum Color
        {
            WHITE = 10,
            BLACK = 20
        }
    }
}
