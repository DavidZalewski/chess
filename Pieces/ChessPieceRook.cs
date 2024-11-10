using Chess.Board;
using Chess.Globals;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceRook : ChessPiece
    {
        public ChessPieceRook(Color color, int id, BoardPosition startingPosition) : base(Piece.ROOK, color, id, startingPosition)
        {
            StaticLogger.Trace();
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
        }

        public override ChessPiece Clone()
        {
            StaticLogger.Trace();
            ChessPieceRook copy = new(_color, _id, _startingPosition);
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // using algorithm deduced from project docs
            int v1 = _currentPosition.RankAsInt;
            int v2 = position.RankAsInt;
            int h1 = _currentPosition.FileAsInt;
            int h2 = position.FileAsInt;

            int vdistance = v1 - v2;
            int hdistance = h1 - h2;

            bool isSquareHorizontalOrVerticalToCurrent = vdistance != 0 && hdistance == 0 || vdistance == 0 && hdistance != 0;
            // a friendly piece cannot be on the destination square
            bool isFriendlyPieceOnSquare = board.IsPieceAtPosition(position, _color);
            bool isDisabledPieceOnSquare = board.IsPieceAtPosition(position, Color.NONE);
            if (!isSquareHorizontalOrVerticalToCurrent || isFriendlyPieceOnSquare || isDisabledPieceOnSquare) { return false; }

            string operation = "";

            // based on what the vdistance and hdistance are, if they are positive numbers, we are decrementing
            // if they are negative numbers, we are incrementing
            // this is how we move diagonally over a 2d array in step wise motion
            if (hdistance == 0 && vdistance < 0)
            {
                operation = "v+"; // this can be done using classes and polymorphism too, but its simpler like this
            }
            else if (hdistance == 0 && vdistance > 0)
            {
                operation = "v-";
            }
            else if (vdistance == 0 && hdistance < 0)
            {
                operation = "h+";
            }
            else if (vdistance == 0 && hdistance > 0)
            {
                operation = "h-";
            }

            bool isPieceBlockingPath(int firstIndex, int secondIndex)
            {
                // these casts should always succeed, given that erroneous positions that dont exist on board are passed into
                // this function
                RANK vVal = (RANK)firstIndex;
                FILE hVal = (FILE)secondIndex;
                BoardPosition pathToPosition = new(vVal, hVal);
                return board.IsPieceAtPosition(pathToPosition);
            }

            bool isCurrentPositionIfSoSkip(int i, int j) => _currentPosition.RankAsInt == i && _currentPosition.FileAsInt == j;

            if (operation == "v-")
            {
                for (int i = v1; i > v2; i--)
                {
                    if (!isCurrentPositionIfSoSkip(i, _currentPosition.FileAsInt) && isPieceBlockingPath(i, _currentPosition.FileAsInt))
                    {
                        return false;
                    }
                }
            }
            else if (operation == "h+")
            {
                for (int j = h1; j < h2; j++)
                {
                    if (!isCurrentPositionIfSoSkip(_currentPosition.RankAsInt, j) && isPieceBlockingPath(_currentPosition.RankAsInt, j))
                    {
                        return false;
                    }
                }
            }
            else if (operation == "v+")
            {
                for (int i = v1; i < v2; i++)
                {
                    if (!isCurrentPositionIfSoSkip(i, _currentPosition.FileAsInt) && isPieceBlockingPath(i, _currentPosition.FileAsInt))
                    {
                        return false;
                    }
                }
            }
            else if (operation == "h-")
            {
                for (int j = h1; j > h2; j--)
                {
                    if (!isCurrentPositionIfSoSkip(_currentPosition.RankAsInt, j) && isPieceBlockingPath(_currentPosition.RankAsInt, j))
                    {
                        return false;
                    }
                }
            }

            // if we didnt return false on any of the branches, then there are no pieces blocking our path
            return true;
        }

        public override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // does this need to exist?
            return false;
        }

        public override List<Square> GetValidSquares(ChessBoard chessBoard)
        {
            List<Square> validSquares = new();

            // Directions for a rook: up, down, left, right
            int[] rankDirections = { -1, 1, 0, 0 };
            int[] fileDirections = { 0, 0, -1, 1 };

            for (int direction = 0; direction < 4; direction++)
            {
                int rank = _currentPosition.RankAsInt;
                int file = _currentPosition.FileAsInt;

                while (true)
                {
                    rank += rankDirections[direction];
                    file += fileDirections[direction];

                    // Check if the new position is within the board boundaries
                    if (rank < 0 || rank > 7 || file < 0 || file > 7)
                    {
                        break;
                    }

                    BoardPosition newPosition = new BoardPosition((RANK)rank, (FILE)file);

                    // Check if the new position is occupied by a friendly piece
                    if (chessBoard.IsPieceAtPosition(newPosition, _color))
                    {
                        break;
                    }

                    validSquares.Add(new Square(newPosition, this));

                    // Check if the new position is occupied by an enemy piece
                    if (chessBoard.IsPieceAtPosition(newPosition))
                    {
                        break;
                    }
                }
            }

            return validSquares;
        }
    }
}
