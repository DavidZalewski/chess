﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public abstract class ChessPiecePawn : ChessPiece
    {
        protected ChessPiecePawn(Color color, int id, BoardPosition startingPosition) : base(Piece.PAWN, color, id, startingPosition)
        {
        }
    }
}