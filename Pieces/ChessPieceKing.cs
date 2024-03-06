using Chess.Board;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceKing : ChessPiece
    {
        protected bool _wasInCheck = false;
        public ChessPieceKing(Color color, BoardPosition startingPosition) : base(Piece.KING, color, 1, startingPosition)
        {
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            bool isCastling = IsCastleMove(board, position);
            if (isCastling) return true;

            bool isFriendlyPieceOnSquare = board.IsPieceAtPosition(position, _color);
            if (isFriendlyPieceOnSquare) { return false; }

            int v1 = _currentPosition.RankAsInt;
            int v2 = position.RankAsInt;
            int h1 = _currentPosition.FileAsInt;
            int h2 = position.FileAsInt;

            int vdistance = v1 - v2;
            int hdistance = h1 - h2;

            // TODO: Simplify these
            if (vdistance == 0 && hdistance == 1 || vdistance == 0 && hdistance == -1) // moving side to side
            {
                return true;
            }
            else if (vdistance == 1 && hdistance == 0 || vdistance == -1 && hdistance == 0) // moving up / down
            {
                return true;
            }
            else if (vdistance == 1 && hdistance == -1 || vdistance == -1 && hdistance == -1) // moving diagonal
            {
                return true;
            }
            else if (vdistance == -1 && hdistance == 1 || vdistance == -1 && hdistance == 1) // moving diagonal
            {
                return true;
            }
            else if (vdistance == 1 && hdistance == 1) // moving diagonal upright
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
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
            // Castling Logic
            if (!_hasMoved && !_wasInCheck)
            {
                bool isFriendlyRookOnSquare = board.IsPieceAtPosition(position, _color, Piece.ROOK);
                List<BoardPosition> positionsLeadingToCastle = new();

                if (_color.Equals(Color.WHITE))
                {
                    // White King's Queen's Side Castle
                    if (position.EqualTo(new("A1")))
                    {
                        positionsLeadingToCastle.Add(new("B1"));
                        positionsLeadingToCastle.Add(new("C1"));
                        positionsLeadingToCastle.Add(new("D1"));
                    }
                    // White King's Side Castle
                    else if (position.EqualTo(new("H1")))
                    {
                        positionsLeadingToCastle.Add(new("F1"));
                        positionsLeadingToCastle.Add(new("G1"));
                    }
                }
                else
                {
                    // Black King's Queen's Side Castle
                    if (position.EqualTo(new("A8")))
                    {
                        positionsLeadingToCastle.Add(new("B8"));
                        positionsLeadingToCastle.Add(new("C8"));
                        positionsLeadingToCastle.Add(new("D8"));
                    }
                    // Black King's Side Castle
                    else if (position.EqualTo(new("H8")))
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
            ChessPieceKing copy = new(_color, _startingPosition);
            return Clone(copy);
        }

        internal void Test_SetWasInCheck() { _wasInCheck = true; }
    }
}
