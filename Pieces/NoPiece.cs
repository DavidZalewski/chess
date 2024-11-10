using Chess.Board;
using Chess.Globals;

namespace Chess.Pieces
{
    [Serializable]
    public class NoPiece : ChessPiece
    {
        private static NoPiece _instance;

        // Private constructor for singleton
        private NoPiece() : base(Piece.NO_PIECE, Color.WHITE, 0, null)
        {
            StaticLogger.Trace();
            _pieceName = "No Piece";
        }

        public static NoPiece Instance
        {
            get
            {
                StaticLogger.Trace();
                if (_instance == null)
                {
                    _instance = new NoPiece();
                }
                return _instance;
            }
        }

        public override ChessPiece Clone()
        {
            StaticLogger.Trace();
            return this;
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            return false;
        }

        public override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            return false;
        }

        public override List<Square> GetValidSquares(ChessBoard chessBoard)
        {
            return new List<Square>();
        }
    }
}
