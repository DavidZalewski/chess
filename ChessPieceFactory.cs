using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessPieceFactory
    {
        public ChessPieceFactory() { }

        public static List<ChessPiece> CreateWhitePawns()
        {
            List<ChessPiece> whitePawns = new();
            for (int i = 0; i < 8; i++)
            {
                whitePawns.Add(new ChessPieceWhitePawn(i + 1, new BoardPosition(BoardPosition.VERTICAL.B, (BoardPosition.HORIZONTAL)i)));
            }

            return whitePawns;
        }

        public static List<ChessPiece> CreateBlackPawns()
        {
            List<ChessPiece> blackPawns = new();
            for (int i = 0; i < 8; i++)
            {
                blackPawns.Add(new ChessPieceBlackPawn(i + 1, new BoardPosition(BoardPosition.VERTICAL.G, (BoardPosition.HORIZONTAL)i)));
            }

            return blackPawns;
        }
        public static List<ChessPiece> CreateChessPieces() { return new List<ChessPiece>(); }
        //public ChessPiece CreateChessPiece() { return new ChessPiece(); }
    }
}
