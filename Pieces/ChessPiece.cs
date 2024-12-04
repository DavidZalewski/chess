using Chess.Attributes;
using Chess.Board;
using Chess.Callbacks;
using Chess.Collections;
using Chess.Globals;

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
            NO_PIECE = 0,
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
            BLACK = 20,
            NONE = 0
        }

        protected Piece _piece;
        protected Color _color;
        protected int _id;
        protected int _realValue;
        protected BoardPosition _startingPosition;
        protected BoardPosition _currentPosition;
        protected bool _hasMoved = false;
        protected static Func<ChessBoard, BoardPosition, ChessPiece, bool>? _castleEventCallBackFunction = SpecialMovesHandlers.DoCastleMove;
        protected static Func<ChessBoard, BoardPosition, ChessPiece, bool>? _IsEnPassantCallBackFunction = SpecialMovesHandlers.IsEnPassantMove;
        protected static Action<ChessBoard, BoardPosition, ChessPiece> _pawnPromotedCallBackFunction = SpecialMovesHandlers.PawnPromotion;
        protected string _pieceName;

        public ChessPiece(Piece piece, Color color, int id, BoardPosition startingPosition)
        {
            StaticLogger.Trace();
            StaticLogger.LogMethod(piece, color, id, startingPosition);
            _piece = piece;
            _color = color;
            _id = id;
            _startingPosition = startingPosition;
            _currentPosition = _startingPosition;
            _pieceName = GetPieceName("generate");
        }

        public abstract ChessPiece Clone();

        protected ChessPiece Clone(ChessPiece copy)
        {
            StaticLogger.Trace();
            StaticLogger.LogMethod(copy);
            copy._piece = _piece;
            copy._color = _color;
            copy._id = _id;
            copy._startingPosition = _startingPosition;
            copy._currentPosition = _currentPosition;
            copy._realValue = _realValue;
            copy._hasMoved = _hasMoved;
            copy._pieceName = _pieceName;

            return copy;
        }

        public override bool Equals(object? obj)
        {
            StaticLogger.Trace();
            var item = obj as ChessPiece;

            if (item == null)
            {
                return false;
            }

            return this._color == (item._color) &&
                   this._hasMoved == (item._hasMoved) &&
                   this._piece == (item._piece) &&
                   this._currentPosition == item._currentPosition &&
                   this._startingPosition == item._startingPosition &&
                   this._id == (item._id) &&
                   this._pieceName == (item._pieceName) &&
                   this._realValue == (item._realValue);
        }

        public abstract bool IsValidMove(ChessBoard board, BoardPosition position);
        public abstract bool ImplementMove(ChessBoard board, BoardPosition position);
        public abstract List<BoardPosition> GetPossiblePositions(ChessBoard chessBoard);

        public void Move(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            if (ImplementMove(board, position)) return;

            BoardPosition previousPosition = _currentPosition;
            board.SetPieceAtPosition(position, this);
            if (previousPosition != position)
            {
                _hasMoved = true;
                board.SetPieceAtPosition(previousPosition, NoPiece.Instance);
                _currentPosition = position;
            }

            if (this is ChessPiecePawn)
            {
                if ((_color == Color.WHITE && _currentPosition.Rank == RANK.EIGHT) ||
                    (_color == Color.BLACK && _currentPosition.Rank == RANK.ONE))
                    _pawnPromotedCallBackFunction.Invoke(board, position, this);
            }

        }

        // another way of doing CheckMate calculations - use this method to see if any contain a King in any of the opponent pieces.
        public List<ChessPiece> GetAttackedPieces(ChessBoard chessBoard)
        {
            StaticLogger.Trace();
            SortedTupleBag<ChessPiece.Piece, ChessPiece> results = new();
            foreach (var pos in GetPossiblePositions(chessBoard))
            {
                ChessPiece piece = chessBoard.GetSquare(pos).Piece;
                if (piece is not NoPiece && this.IsValidMove(chessBoard, pos))
                {
                    results.Add(piece.GetPiece(), piece);
                }
            }

            return results.OrderByDescending(x => x.Item1).Select(x => x.Item2).ToList();
        }


        public Piece GetPiece() { StaticLogger.Trace(); return _piece; }
        public Color GetColor() { StaticLogger.Trace(); return _color; }
        public int GetId() { StaticLogger.Trace(); return _id; }
        public int GetRealValue() { StaticLogger.Trace(); return _realValue; }
        public BoardPosition GetStartingPosition() { StaticLogger.Trace(); return _startingPosition; }
        public BoardPosition GetCurrentPosition() { StaticLogger.Trace(); return _currentPosition; }
        public bool HasMoved() { StaticLogger.Trace(); return _hasMoved; }

        public string GetPieceName() { StaticLogger.Trace(); return _pieceName; }
        private string GetPieceName(string dummyArgument)
        {
            StaticLogger.Trace();
            string pieceName = "";
            if (_color.Equals(Color.WHITE))
            {
                pieceName += "White ";
            }
            else if (_color.Equals(Color.BLACK))
            {
                pieceName += "Black ";
            }
            else
            {
                pieceName += "None ";
            }
            switch (_piece)
            {
                case Piece.PAWN: pieceName += "Pawn "; break;
                case Piece.KNIGHT: pieceName += "Knight "; break;
                case Piece.BISHOP: pieceName += "Bishop "; break;
                case Piece.ROOK: pieceName += "Rook "; break;
                case Piece.QUEEN: pieceName += "Queen "; break;
                case Piece.KING: pieceName += "King "; break;
                case Piece.NO_PIECE: pieceName += "Disabled Square "; break;
            }
            pieceName += _id;

            return pieceName;
        }

        // used by ghost pieces
        public void SetCurrentPosition(BoardPosition boardPosition)
        {
            StaticLogger.Trace();
            _currentPosition = boardPosition;
        }

        public static void SetCastleCallbackFunction(Func<ChessBoard, BoardPosition, ChessPiece, bool> callback)
        {
            StaticLogger.Trace();
            _castleEventCallBackFunction = callback;
        }

        public static void SetIsEnPassantCallbackFunction(Func<ChessBoard, BoardPosition, ChessPiece, bool> callback)
        {
            StaticLogger.Trace();
            _IsEnPassantCallBackFunction = callback;
        }

        // used by tests to setup the state of King Piece for certain unit tests"
        internal void SetHasMoved() { StaticLogger.Trace(); _hasMoved = true; }

        public bool Equals(ChessPiece other)
        {
            StaticLogger.Trace();
            return _color.Equals(other._color) &&
                   _id == other._id &&
                   _realValue == other._realValue &&
                   _piece == other._piece &&
                   _startingPosition == other._startingPosition &&
                   _currentPosition == other._currentPosition;
        }

        public override int GetHashCode()
        {
            StaticLogger.Trace();
            return GetHashCode();
        }

    }
}
