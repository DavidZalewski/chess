using Chess.Attributes;
using Chess.Globals;
using Chess.Pieces;

namespace Chess.Board
{
    // this technically doesnt need to exist, we can just do this inside ChessBoard. Why define another abstraction of Board?
    // One abstraction per entity as rule of thumb
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

        [TestNeeded]
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
            };
        }
    }
}
