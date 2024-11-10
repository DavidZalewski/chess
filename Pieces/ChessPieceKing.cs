using Chess.Board;
using Chess.Globals;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceKing : ChessPiece
    {
        protected bool _wasInCheck = false;
        public ChessPieceKing(Color color, BoardPosition startingPosition) : base(Piece.KING, color, 1, startingPosition)
        {
            StaticLogger.Trace();
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            if (position == _currentPosition) return false; // Is the current position the same as where its being asked to move?
            if (IsCastleMove(board, position)) return true; // Are we Castling?
            if (board.IsPieceAtPosition(position, _color)) return false; // Is there a friendly piece blocking us here?
            if (board.IsPieceAtPosition(position, Color.NONE)) return false; // Disabled square check

            int vdistance = Math.Abs(_currentPosition.RankAsInt - position.RankAsInt);
            int hdistance = Math.Abs(_currentPosition.FileAsInt - position.FileAsInt);

            return vdistance <= 1 && hdistance <= 1;
        }

        public override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            if (IsCastleMove(board, position))
            {
                // Find the rook that is at this position
                if (_castleEventCallBackFunction != null)
                {
                    _castleEventCallBackFunction.Invoke(board, position, this);
                    return true;
                }
            }
            return false;
        }

        private bool IsCastleMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // Castling Logic
            if (!_hasMoved && !_wasInCheck)
            {
                bool isFriendlyRookOnSquare = board.IsPieceAtPosition(position, _color, Piece.ROOK);
                List<BoardPosition> positionsLeadingToCastle = new();
                BoardPosition A1 = new("A1");
                BoardPosition A8 = new("A8");
                BoardPosition H1 = new("H1");
                BoardPosition H8 = new("H8");
                if (!(_currentPosition.Equals(new("E1")) || _currentPosition.Equals(new("E8"))))
                    return false;

                if (_color.Equals(Color.WHITE))
                {

                    // White King's Queen's Side Castle
                    if (position == A1)
                    {
                        positionsLeadingToCastle.Add(new("B1"));
                        positionsLeadingToCastle.Add(new("C1"));
                        positionsLeadingToCastle.Add(new("D1"));
                    }
                    // White King's Side Castle
                    else if (position == H1)
                    {
                        positionsLeadingToCastle.Add(new("F1"));
                        positionsLeadingToCastle.Add(new("G1"));
                    }
                }
                else
                {
                    // Black King's Queen's Side Castle
                    if (position == A8)
                    {
                        positionsLeadingToCastle.Add(new("B8"));
                        positionsLeadingToCastle.Add(new("C8"));
                        positionsLeadingToCastle.Add(new("D8"));
                    }
                    // Black King's Side Castle
                    else if (position == H8)
                    {
                        positionsLeadingToCastle.Add(new("F8"));
                        positionsLeadingToCastle.Add(new("G8"));
                    }
                }

                if (positionsLeadingToCastle.Count > 0 && isFriendlyRookOnSquare && !_wasInCheck)
                {
                    bool isSquareOpen = true;
                    foreach (BoardPosition boardPosition in positionsLeadingToCastle)
                    {
                        if (board.IsPieceAtPosition(boardPosition))
                        {
                            isSquareOpen = false;
                        }
                    }
                    return isSquareOpen;
                }
            }

            return false;
        }

        public override ChessPiece Clone()
        {
            StaticLogger.Trace();
            ChessPieceKing copy = new(_color, _startingPosition);
            return Clone(copy);
        }

        internal void SetWasInCheck() { StaticLogger.Trace(); _wasInCheck = true; }

        public override List<Square> GetValidSquares(ChessBoard chessBoard)
        {
            List<Square> validSquares = new();

            BoardPosition? upRight = _currentPosition.Up()?.Right();
            BoardPosition? downRight = _currentPosition.Down()?.Right();
            BoardPosition? upLeft = _currentPosition.Up()?.Left();
            BoardPosition? downLeft = _currentPosition.Down()?.Left();
            BoardPosition? up = _currentPosition.Up();
            BoardPosition? down = _currentPosition.Down();
            BoardPosition? left = _currentPosition.Left();
            BoardPosition? right = _currentPosition.Right();

            if (upRight != null)
                validSquares.Add(new Square(upRight, this));

            if (downRight != null)
                validSquares.Add(new Square(downRight, this));

            if (upLeft != null)
                validSquares.Add(new Square(upLeft, this));

            if (downLeft != null)
                validSquares.Add(new Square(downLeft, this));

            if (up != null)
                validSquares.Add(new Square(up, this));

            if (down != null)
                validSquares.Add(new Square(down, this));

            if (left != null)
                validSquares.Add(new Square(left, this));

            if (right != null)
                validSquares.Add(new Square(right, this));

            return validSquares;
        }

    }
}
