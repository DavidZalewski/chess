using Chess.Board;
using Chess.Globals;
using Chess.Pieces;

namespace Chess.Services
{
    public class ChessPieceFactory
    {
        public ChessPieceFactory() { StaticLogger.Trace(); }

        public static List<ChessPiece> CreateWhitePawns()
        {
            StaticLogger.Trace();
            List<ChessPiece> whitePawns = new();
            for (int i = 0; i < 8; i++)
            {
                whitePawns.Add(new ChessPieceWhitePawn(i + 1, new BoardPosition(RANK.TWO, (FILE)i)));
            }

            return whitePawns;
        }

        public static List<ChessPiece> CreateBlackPawns()
        {
            StaticLogger.Trace();
            List<ChessPiece> blackPawns = new();
            for (int i = 0; i < 8; i++)
            {
                blackPawns.Add(new ChessPieceBlackPawn(i + 1, new BoardPosition(RANK.SEVEN, (FILE)i)));
            }

            return blackPawns;
        }

        public static List<ChessPiece> CreateWhiteKnights()
        {
            StaticLogger.Trace();
            List<ChessPiece> whiteKnights = new();
            BoardPosition b1 = new(RANK.ONE, FILE.B);
            BoardPosition g1 = new(RANK.ONE, FILE.G);
            whiteKnights.Add(new ChessPieceKnight(ChessPiece.Color.WHITE, 1, b1));
            whiteKnights.Add(new ChessPieceKnight(ChessPiece.Color.WHITE, 2, g1));
            return whiteKnights;
        }

        public static List<ChessPiece> CreateWhiteNuclearHorses()
        {
            StaticLogger.Trace();
            List<ChessPiece> whiteKnights = new();
            BoardPosition b1 = new(RANK.ONE, FILE.B);
            BoardPosition g1 = new(RANK.ONE, FILE.G);
            whiteKnights.Add(new NuclearHorsePiece(ChessPiece.Color.WHITE, 1, b1));
            whiteKnights.Add(new NuclearHorsePiece(ChessPiece.Color.WHITE, 2, g1));
            return whiteKnights;
        }

        public static List<ChessPiece> CreateBlackKnights()
        {
            StaticLogger.Trace();
            List<ChessPiece> blackKnights = new();
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);
            BoardPosition g8 = new(RANK.EIGHT, FILE.G);
            blackKnights.Add(new ChessPieceKnight(ChessPiece.Color.BLACK, 1, b8));
            blackKnights.Add(new ChessPieceKnight(ChessPiece.Color.BLACK, 2, g8));
            return blackKnights;
        }

        public static List<ChessPiece> CreateBlackNuclearHorses()
        {
            StaticLogger.Trace();
            List<ChessPiece> blackKnights = new();
            BoardPosition b8 = new(RANK.EIGHT, FILE.B);
            BoardPosition g8 = new(RANK.EIGHT, FILE.G);
            blackKnights.Add(new NuclearHorsePiece(ChessPiece.Color.BLACK, 1, b8));
            blackKnights.Add(new NuclearHorsePiece(ChessPiece.Color.BLACK, 2, g8));
            return blackKnights;
        }

        public static List<ChessPiece> CreateWhiteBishops()
        {
            StaticLogger.Trace();
            List<ChessPiece> whiteBishops = new();
            BoardPosition c1 = new(RANK.ONE, FILE.C);
            BoardPosition f1 = new(RANK.ONE, FILE.F);
            whiteBishops.Add(new ChessPieceBishop(ChessPiece.Color.WHITE, 1, c1));
            whiteBishops.Add(new ChessPieceBishop(ChessPiece.Color.WHITE, 2, f1));
            return whiteBishops;
        }

        public static List<ChessPiece> CreateWhiteNuclearBishops()
        {
            StaticLogger.Trace();
            List<ChessPiece> whiteBishops = new();
            BoardPosition c1 = new(RANK.ONE, FILE.C);
            BoardPosition f1 = new(RANK.ONE, FILE.F);
            whiteBishops.Add(new NuclearBishopPiece(ChessPiece.Color.WHITE, 1, c1));
            whiteBishops.Add(new NuclearBishopPiece(ChessPiece.Color.WHITE, 2, f1));
            return whiteBishops;
        }

        public static List<ChessPiece> CreateBlackBishops()
        {
            StaticLogger.Trace();
            List<ChessPiece> blackBishops = new();
            BoardPosition c8 = new(RANK.EIGHT, FILE.C);
            BoardPosition f8 = new(RANK.EIGHT, FILE.F);
            blackBishops.Add(new ChessPieceBishop(ChessPiece.Color.BLACK, 1, c8));
            blackBishops.Add(new ChessPieceBishop(ChessPiece.Color.BLACK, 2, f8));
            return blackBishops;
        }

        public static List<ChessPiece> CreateBlackNuclearBishops()
        {
            StaticLogger.Trace();
            List<ChessPiece> blackBishops = new();
            BoardPosition c8 = new(RANK.EIGHT, FILE.C);
            BoardPosition f8 = new(RANK.EIGHT, FILE.F);
            blackBishops.Add(new NuclearBishopPiece(ChessPiece.Color.BLACK, 1, c8));
            blackBishops.Add(new NuclearBishopPiece(ChessPiece.Color.BLACK, 2, f8));
            return blackBishops;
        }

        public static List<ChessPiece> CreateWhiteRooks()
        {
            StaticLogger.Trace();
            List<ChessPiece> whiteRooks = new();
            BoardPosition a1 = new(RANK.ONE, FILE.A);
            BoardPosition h1 = new(RANK.ONE, FILE.H);
            whiteRooks.Add(new ChessPieceRook(ChessPiece.Color.WHITE, 1, a1));
            whiteRooks.Add(new ChessPieceRook(ChessPiece.Color.WHITE, 2, h1));
            return whiteRooks;
        }

        public static List<ChessPiece> CreateBlackRooks()
        {
            StaticLogger.Trace();
            List<ChessPiece> blackRooks = new();
            BoardPosition a8 = new(RANK.EIGHT, FILE.A);
            BoardPosition h8 = new(RANK.EIGHT, FILE.H);
            blackRooks.Add(new ChessPieceRook(ChessPiece.Color.BLACK, 1, a8));
            blackRooks.Add(new ChessPieceRook(ChessPiece.Color.BLACK, 2, h8));
            return blackRooks;
        }

        public static List<ChessPiece> CreateWhiteQueenAndKing()
        {
            StaticLogger.Trace();
            List<ChessPiece> chessPieces = new();
            BoardPosition d1 = new(RANK.ONE, FILE.D);
            BoardPosition e1 = new(RANK.ONE, FILE.E);
            chessPieces.Add(new ChessPieceQueen(ChessPiece.Color.WHITE, 1, d1)); // queen needs id exposed in case of pawn promotion
            chessPieces.Add(new ChessPieceKing(ChessPiece.Color.WHITE, e1));
            return chessPieces;
        }

        public static List<ChessPiece> CreateBlackQueenAndKing()
        {
            StaticLogger.Trace();
            List<ChessPiece> chessPieces = new();
            BoardPosition d8 = new(RANK.EIGHT, FILE.D);
            BoardPosition e8 = new(RANK.EIGHT, FILE.E);
            chessPieces.Add(new ChessPieceQueen(ChessPiece.Color.BLACK, 1, d8)); // queen needs id exposed in case of pawn promotion
            chessPieces.Add(new ChessPieceKing(ChessPiece.Color.BLACK, e8));
            return chessPieces;
        }

        public static List<ChessPiece> CreateWhiteChessPieces()
        {
            StaticLogger.Trace();
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
            StaticLogger.Trace();
            List<ChessPiece> chessPieces = new();

            chessPieces.AddRange(CreateBlackPawns());
            chessPieces.AddRange(CreateBlackKnights());
            chessPieces.AddRange(CreateBlackBishops());
            chessPieces.AddRange(CreateBlackRooks());
            chessPieces.AddRange(CreateBlackQueenAndKing());

            return chessPieces;
        }

        public static List<ChessPiece> CreateChessPiecesClassic()
        {
            StaticLogger.Trace();
            List<ChessPiece> chessPieces = new();
            chessPieces.AddRange(CreateWhiteChessPieces());
            chessPieces.AddRange(CreateBlackChessPieces());
            StaticLogger.Log(chessPieces.ToDetailedString(), LogLevel.Debug, LogCategory.ObjectDump);
            return chessPieces;
        }

        public static List<ChessPiece> CreateChessPiecesNuclearHorse()
        {
            StaticLogger.Trace();
            List<ChessPiece> chessPieces = new();
            chessPieces.AddRange(CreateWhitePawns());
            chessPieces.AddRange(CreateWhiteNuclearHorses());
            chessPieces.AddRange(CreateWhiteNuclearBishops());
            chessPieces.AddRange(CreateWhiteRooks());
            chessPieces.AddRange(CreateWhiteQueenAndKing());

            chessPieces.AddRange(CreateBlackPawns());
            chessPieces.AddRange(CreateBlackNuclearHorses());
            chessPieces.AddRange(CreateBlackNuclearBishops());
            chessPieces.AddRange(CreateBlackRooks());
            chessPieces.AddRange(CreateBlackQueenAndKing());

            StaticLogger.Log(chessPieces.ToDetailedString(), LogLevel.Debug, LogCategory.ObjectDump);

            return chessPieces;
        }

        internal static ChessPiece CreatePieceFromInt(BoardPosition position, int value)
        {
            StaticLogger.Trace();
            StaticLogger.LogMethod(position, value);
            return value switch
            {
                -1 => new DisabledSquarePiece(position),
                0 => NoPiece.Instance,
                11 => new ChessPieceWhitePawn(1, position),
                12 => new ChessPieceKnight(ChessPiece.Color.WHITE, 1, position),
                13 => new ChessPieceBishop(ChessPiece.Color.WHITE, 1, position),
                14 => new ChessPieceRook(ChessPiece.Color.WHITE, 1, position),
                15 => new ChessPieceQueen(ChessPiece.Color.WHITE, 1, position),
                16 => new ChessPieceKing(ChessPiece.Color.WHITE, position),
                21 => new ChessPieceBlackPawn(1, position),
                22 => new ChessPieceKnight(ChessPiece.Color.BLACK, 1, position),
                23 => new ChessPieceBishop(ChessPiece.Color.BLACK, 1, position),
                24 => new ChessPieceRook(ChessPiece.Color.BLACK, 1, position),
                25 => new ChessPieceQueen(ChessPiece.Color.BLACK, 1, position),
                26 => new ChessPieceKing(ChessPiece.Color.BLACK, position),
                _ => throw new ArgumentException("Invalid piece value"),
            };
        }
    }
}
