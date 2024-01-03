using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Pieces;

namespace Chess.Services
{
    public class PieceConverterService
    {
        public static ChessPiece? PromotePawn(ChessPiece pawn, Type type)
        {
            // Promoted pieces will have a unique id of current-id + color
            ChessPiece? piece = null;
            int id = pawn.GetId() + (int)pawn.GetColor();
            if (type == typeof(ChessPieceRook))
            {
                piece = new ChessPieceRook(pawn.GetColor(), id, pawn.GetCurrentPosition());
            }
            else if (type == typeof(ChessPieceBishop))
            {
                piece = new ChessPieceBishop(pawn.GetColor(), id, pawn.GetCurrentPosition());
            }
            else if (type == typeof(ChessPieceKnight))
            {
                piece = new ChessPieceKnight(pawn.GetColor(), id, pawn.GetCurrentPosition());
            }
            else if (type == typeof(ChessPieceQueen))
            {
                piece = new ChessPieceQueen(pawn.GetColor(), id, pawn.GetCurrentPosition());
            }

            return piece;
        }
    }
}
