using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Chess.Board;
using Newtonsoft.Json;

namespace Chess.Pieces
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
    [Serializable]
    public abstract class ChessPiece
    {
        public enum Piece
        {
            PAWN = 1,
            KNIGHT = 2,
            BISHOP = 3,
            ROOK = 4,
            QUEEN = 5,
            KING = 6
        }

        public enum Color
        {
            WHITE = 10,
            BLACK = 20
        }

        protected Piece _piece;
        protected Color _color;
        protected int _id;
        protected int _realValue;
        protected BoardPosition _startingPosition;
        protected BoardPosition _currentPosition;
        protected bool _hasMoved = false;
        protected static Func<ChessBoard, BoardPosition, ChessPiece, bool>? _castleEventCallBackFunction = null;

        public ChessPiece(Piece piece, Color color, int id, BoardPosition startingPosition)
        {
            _piece = piece;
            _color = color;
            _id = id;
            _startingPosition = startingPosition;
            _currentPosition = _startingPosition;
        }

        public abstract ChessPiece Clone();

        protected ChessPiece Clone(ChessPiece copy)
        {
            copy._piece = _piece;
            copy._color = _color;
            copy._id = _id;
            copy._startingPosition = _startingPosition;
            copy._currentPosition = _currentPosition;
            copy._realValue = _realValue;
            copy._hasMoved = _hasMoved;

            return copy;
        }

        public override bool Equals(object? obj)
        {
            var item = obj as ChessPiece;

            if (item == null)
            {
                return false;
            }

            return this._color.Equals(item._color) &&
                   this._hasMoved.Equals(item._hasMoved) &&
                   this._piece.Equals(item._piece) &&
                   this._currentPosition.EqualTo(item._currentPosition) &&
                   this._startingPosition.EqualTo(item._startingPosition) &&
                   this._id.Equals(item._id) &&
                   this._realValue.Equals(item._realValue);
        }

        public abstract bool IsValidMove(ChessBoard board, BoardPosition position);
        protected abstract bool ImplementMove(ChessBoard board, BoardPosition position);

        public void Move(ChessBoard board, BoardPosition position)
        {
            if (ImplementMove(board, position)) return;

            BoardPosition previousPosition = _currentPosition;
            board.SetBoardValue(position, _realValue);
            if (!previousPosition.EqualTo(position))
            {
                _hasMoved = true;
                board.SetBoardValue(previousPosition, 0); // empty the previous square
                _currentPosition = position; // breaks too many test cases
            }     
        }

        public Piece GetPiece() { return _piece; }
        public Color GetColor() { return _color; }
        public int GetId() { return _id; }
        public int GetRealValue() { return _realValue; }
        public BoardPosition GetStartingPosition() { return _startingPosition; }
        public BoardPosition GetCurrentPosition() { return _currentPosition; }
        public bool HasMoved() {  return _hasMoved; }

        // used by ghost pieces
        public void SetCurrentPosition(BoardPosition boardPosition)
        {
            _currentPosition = boardPosition;
        }

        public static void SetCastleCallbackFunction(Func<ChessBoard, BoardPosition, ChessPiece, bool> callback) { 
            _castleEventCallBackFunction = callback; 
        }

        // used by tests to setup the state of King Piece for certain unit tests
        internal void SetHasMoved() { _hasMoved = true; }

        public bool Equals(ChessPiece other)
        {
            return _color.Equals(other._color) && 
                   _id == other._id && 
                   _realValue == other._realValue && 
                   _piece == other._piece &&
                   _startingPosition.EqualTo(other._startingPosition) && 
                   _currentPosition.EqualTo(other._currentPosition);
        }
    }
}
