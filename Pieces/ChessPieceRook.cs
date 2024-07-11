using Chess.Board;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceRook : ChessPiece
    {
        public ChessPieceRook(Color color, int id, BoardPosition startingPosition) : base(Piece.ROOK, color, id, startingPosition)
        {
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
        }

        public override ChessPiece Clone()
        {
            ChessPieceRook copy = new(_color, _id, _startingPosition);
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
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
            // does this need to exist?
            return false;
        }

    }
}
