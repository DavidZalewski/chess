using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    // How do I want to do this? An abstract ChessPiece class and I create subclasses for
    // each type of piece and then use factory to create multiple instances?

    // or do I handle all of this inside the ChessBoard class, since it knows where each piece will be?
    // Is the ChessBoard class responsible for knowing whether a Knight can make a move? Or is that up to the Knight class?
    // Which class should handle which responsibility?

    // Which is simpler to code and troubleshoot?

    // If user is asking to move WK2 (White Knight 2) to F6, the ChessBoard has to first locate where the White Knight 2
    // is on the board. That means it could possibly iterate 64 times to find the piece.
    // Where-as with a collection of Piece objects, I can just find the piece by searching the collection, which is much smaller,
    // and I'll know where it's position is, because the ChessPiece object stores its position on the board.

    // The only part i cant quite see clearly yet, is the syncing between the ChessPiece position and the piece represented on the
    // ChessBoard. Both need to be in sync with each other at all times.

    // I think I should sketch this out on paper to see which approach is simplest.
    internal abstract class ChessPiece
    {
        private ChessEnums.Piece _piece;
        private ChessEnums.Color _color;
        private BoardPosition _position;
        private BoardPosition _startingPosition;

        public abstract bool IsValidMove(ref ChessBoard board, String position);
        public abstract void Move(ref ChessBoard board, String position);
    }
}
