using Chess.Globals;
using Chess.Pieces;

namespace Chess.Board
{
    [Serializable]
    public class Square
    {
        public BoardPosition Position { get; set; }
        public ChessPiece Piece { get; set; }

        public Square()
        {
            StaticLogger.Trace();
            Piece = NoPiece.Instance;
        }

        public Square(BoardPosition position, ChessPiece piece)
        {
            StaticLogger.Trace();
            Position = position;
            Piece = piece;
        }

        public Square(ChessPiece piece)
        {
            StaticLogger.Trace();
            Position = piece.GetCurrentPosition();
            Piece = piece;
        }

        public Square(Square? other)
        {
            StaticLogger.Trace();
            if (other != null)
            {
                this.Position = other.Position;
                this.Piece = other.Piece.Clone();
            }
        }
    }
}