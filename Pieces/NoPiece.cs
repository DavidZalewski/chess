using Chess.Board;

namespace Chess.Pieces
{
    [Serializable]
    public class NoPiece : ChessPiece
    {
        private static NoPiece _instance;

        // Private constructor for singleton
        private NoPiece() : base(Piece.NO_PIECE, Color.WHITE, 0, null)
        {
            _pieceName = "No Piece";
        }

        public static NoPiece Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NoPiece();
                }
                return _instance;
            }
        }

        public override ChessPiece Clone()
        {
            return this;
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            return false;
        }

        protected override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            return false;
        }
    }
}
