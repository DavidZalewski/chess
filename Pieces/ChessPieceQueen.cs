using Chess.Board;
using Chess.Globals;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceQueen : ChessPiece
    {
        private ChessPieceRook _chessPieceRook;
        private ChessPieceBishop _chessPieceBishop;

        public ChessPieceQueen(Color color, int id, BoardPosition startingPosition) : base(Piece.QUEEN, color, id, startingPosition)
        {
            StaticLogger.Trace();
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
            _chessPieceBishop = new ChessPieceBishop(color, 5, startingPosition);
            _chessPieceRook = new ChessPieceRook(color, 5, startingPosition);
        }

        public override ChessPiece Clone()
        {
            StaticLogger.Trace();
            ChessPieceQueen copy = new(_color, _id, _startingPosition);
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // need to update these ghost pieces positions everytime this is called
            // need to make a unit test that checks for this (move the piece twice and do 2 IsValidMove tests on it
            _chessPieceRook.SetCurrentPosition(_currentPosition);
            _chessPieceBishop.SetCurrentPosition(_currentPosition);

            // beautifully simple
            bool isValidBishopMove = _chessPieceBishop.IsValidMove(board, position);
            bool isValidRookMove = _chessPieceRook.IsValidMove(board, position);
            return isValidBishopMove || isValidRookMove;
        }

        public override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // does this need to exist?
            return false;
        }
    }
}
