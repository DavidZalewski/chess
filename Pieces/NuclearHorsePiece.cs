using Chess.Board;
using Chess.Globals;

namespace Chess.Pieces
{
    [Serializable]
    public class NuclearHorsePiece : ChessPieceKnight
    {
        bool _wasSetOnBoard;
        public NuclearHorsePiece(Color color, int id, BoardPosition startingPosition) : base(color, id, startingPosition)
        {
            StaticLogger.Trace();
            _wasSetOnBoard = false;
        }

        public override ChessPiece Clone()
        {
            StaticLogger.Trace();
            NuclearHorsePiece copy = new(_color, _id, _startingPosition);
            copy._wasSetOnBoard = this._wasSetOnBoard;
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // Check if there are any disabled squares in the path
            if (IsPathBlockedByDisabledSquares(board, position))
            {
                return false;
            }

            // Call the base class's IsValidMove method
            return base.IsValidMove(board, position);
        }

        public override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // when setting the board up, do not invoke the nuclear horse logic
            if (!_wasSetOnBoard)
            {
                _wasSetOnBoard = true;
                return false;
            }

            // Create disabled squares around the new position
            List<BoardPosition> adjacentPositions = GetAdjacentPositions(position);
            foreach (BoardPosition adjPos in adjacentPositions)
            {
                if (board.IsPositionWithinBounds(adjPos))
                {
                    if (!board.IsPieceAtPosition(adjPos, Color.WHITE) && !board.IsPieceAtPosition(adjPos, Color.BLACK))
                    {
                        Square disabledSquare = new Square(new DisabledSquarePiece(adjPos));
                        board.SetSquareValue(adjPos, disabledSquare);
                    }

                }
            }
            return false;
        }

        private bool IsPathBlockedByDisabledSquares(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // Calculate the positions the knight would jump over
            int verticalDistance = Math.Abs(_currentPosition.RankAsInt - position.RankAsInt);
            int horizontalDistance = Math.Abs(_currentPosition.FileAsInt - position.FileAsInt);

            if (verticalDistance == 2 && horizontalDistance == 1)
            {
                // Knight moves vertically two squares and horizontally one square
                BoardPosition middlePosition = new((RANK)((_currentPosition.RankAsInt + position.RankAsInt) / 2), _currentPosition.File);
                if (board.GetSquare(middlePosition).Piece is DisabledSquarePiece)
                {
                    return true;
                }
            }
            else if (verticalDistance == 1 && horizontalDistance == 2)
            {
                // Knight moves horizontally two squares and vertically one square
                BoardPosition middlePosition = new(_currentPosition.Rank, (FILE)((_currentPosition.FileAsInt + position.FileAsInt) / 2));
                if (board.GetSquare(middlePosition).Piece is DisabledSquarePiece)
                {
                    return true;
                }
            }

            return false;
        }

        private List<BoardPosition?> GetAdjacentPositions(BoardPosition position)
        {
            StaticLogger.Trace();
            List<BoardPosition?> adjacentPositions = new List<BoardPosition?>
            {
                position?.Left(),
                position?.Left()?.Left(),
                position?.Left()?.Left()?.Up(),
                position?.Left()?.Left()?.Down(),
                position?.Right(),
                position?.Right()?.Right(),
                position?.Right()?.Right()?.Up(),
                position?.Right()?.Right()?.Down(),
                position?.Up(),
                position?.Up()?.Up(),
                position?.Up()?.Up()?.Left(),
                position?.Up()?.Up()?.Right(),
                position?.Down(),
                position?.Down()?.Down(),
                position?.Down()?.Down()?.Left(),
                position?.Down()?.Down()?.Right(),
                position?.Up()?.Left(),
                position?.Up()?.Right(), 
                position?.Down()?.Left(),
                position?.Down()?.Right()
            };
            return adjacentPositions;
        }
    }
}