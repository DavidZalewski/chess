using Chess.Pieces;

namespace Chess.Board
{
    // this technically doesnt need to exist, we can just do this inside ChessBoard. Why define another abstraction of Board?
    // One abstraction per entity as rule of thumb
    public class Square
    {
        public BoardPosition Position { get; set; }
        public ChessPiece Piece { get; set; }
    }
}
