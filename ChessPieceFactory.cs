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

        public List<ChessPiece> CreateWhitePawns()
        {
            List<ChessPiece> whitePawns = new List<ChessPiece>();
            for (int i = 0; i < 8; i++)
            {
                whitePawns.Add(new ChessPieceWhitePawn(i + 1, new BoardPosition(BoardPosition.VERTICAL.B, (BoardPosition.HORIZONTAL)i)));
            }

            return whitePawns;
        }
        public List<ChessPiece> CreateChessPieces() { return new List<ChessPiece>(); }
        //public ChessPiece CreateChessPiece() { return new ChessPiece(); }
    }
}
