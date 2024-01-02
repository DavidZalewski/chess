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
                whitePawns.Add(new ChessPieceWhitePawn(i + 1, new BoardPosition(BoardPosition.VERTICAL.TWO, (BoardPosition.HORIZONTAL)i)));
            }

            return whitePawns;
        }

        public static List<ChessPiece> CreateBlackPawns()
        {
            List<ChessPiece> blackPawns = new();
            for (int i = 0; i < 8; i++)
            {
                blackPawns.Add(new ChessPieceBlackPawn(i + 1, new BoardPosition(BoardPosition.VERTICAL.SEVEN, (BoardPosition.HORIZONTAL)i)));
            }

            return blackPawns;
        }

        public static List<ChessPiece> CreateWhiteKnights()
        {
            List<ChessPiece> whiteKnights = new();
            BoardPosition b1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.B);
            BoardPosition g1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.G);
            whiteKnights.Add(new ChessPieceKnight(ChessPiece.Color.WHITE, 1, b1));
            whiteKnights.Add(new ChessPieceKnight(ChessPiece.Color.WHITE, 2, g1));
            return whiteKnights;
        }

        public static List<ChessPiece> CreateBlackKnights()
        {
            List<ChessPiece> blackKnights = new();
            BoardPosition b8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.B);
            BoardPosition g8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.G);
            blackKnights.Add(new ChessPieceKnight(ChessPiece.Color.BLACK, 1, b8));
            blackKnights.Add(new ChessPieceKnight(ChessPiece.Color.BLACK, 2, g8));
            return blackKnights;
        }

        public static List<ChessPiece> CreateWhiteBishops()
        {
            List<ChessPiece> whiteBishops = new();
            BoardPosition c1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.C);
            BoardPosition f1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.F);
            whiteBishops.Add(new ChessPieceBishop(ChessPiece.Color.WHITE, 1, c1));
            whiteBishops.Add(new ChessPieceBishop(ChessPiece.Color.WHITE, 2, f1));
            return whiteBishops;
        }

        public static List<ChessPiece> CreateBlackBishops()
        {
            List<ChessPiece> blackBishops = new();
            BoardPosition c8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.C);
            BoardPosition f8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.F);
            blackBishops.Add(new ChessPieceBishop(ChessPiece.Color.BLACK, 1, c8));
            blackBishops.Add(new ChessPieceBishop(ChessPiece.Color.BLACK, 2, f8));
            return blackBishops;
        }

        public static List<ChessPiece> CreateWhiteRooks()
        {
            List<ChessPiece> whiteRooks = new();
            BoardPosition a1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.A);
            BoardPosition h1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.H);
            whiteRooks.Add(new ChessPieceRook(ChessPiece.Color.WHITE, 1, a1));
            whiteRooks.Add(new ChessPieceRook(ChessPiece.Color.WHITE, 2, h1));
            return whiteRooks;
        }

        public static List<ChessPiece> CreateBlackRooks()
        {
            List<ChessPiece> blackRooks = new();
            BoardPosition a8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.A);
            BoardPosition h8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.H);
            blackRooks.Add(new ChessPieceRook(ChessPiece.Color.BLACK, 1, a8));
            blackRooks.Add(new ChessPieceRook(ChessPiece.Color.BLACK, 2, h8));
            return blackRooks;
        }

        public static List<ChessPiece> CreateWhiteQueenAndKing()
        {
            List<ChessPiece> chessPieces = new();
            BoardPosition d1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.D);
            BoardPosition e1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.E);
            chessPieces.Add(new ChessPieceQueen(ChessPiece.Color.WHITE, 1, d1)); // queen needs id exposed in case of pawn promotion
            chessPieces.Add(new ChessPieceKing(ChessPiece.Color.WHITE, e1));
            return chessPieces;
        }

        public static List<ChessPiece> CreateBlackQueenAndKing()
        {
            List<ChessPiece> chessPieces = new();
            BoardPosition d8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.D);
            BoardPosition e8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.E);
            chessPieces.Add(new ChessPieceQueen(ChessPiece.Color.BLACK, 1, d8)); // queen needs id exposed in case of pawn promotion
            chessPieces.Add(new ChessPieceKing(ChessPiece.Color.BLACK, e8));
            return chessPieces;
        }

        public static List<ChessPiece> CreateWhiteChessPieces()
        {
            List<ChessPiece> chessPieces = new();

            chessPieces.AddRange(CreateWhitePawns());
            chessPieces.AddRange(CreateWhiteKnights());
            chessPieces.AddRange(CreateWhiteBishops());
            chessPieces.AddRange(CreateWhiteRooks());
            chessPieces.AddRange(CreateWhiteQueenAndKing());

            return chessPieces;
        }

        public static List<ChessPiece> CreateBlackChessPieces()
        {
            List<ChessPiece> chessPieces = new();

            chessPieces.AddRange(CreateBlackPawns());
            chessPieces.AddRange(CreateBlackKnights());
            chessPieces.AddRange(CreateBlackBishops());
            chessPieces.AddRange(CreateBlackRooks());
            chessPieces.AddRange(CreateBlackQueenAndKing());

            return chessPieces;
        }

        public static List<ChessPiece> CreateChessPieces() 
        {
            List<ChessPiece> chessPieces = new();
            chessPieces.AddRange(CreateWhiteChessPieces());
            chessPieces.AddRange(CreateBlackChessPieces());
            return chessPieces;
        }
    }
}
