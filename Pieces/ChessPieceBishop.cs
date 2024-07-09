using Chess.Board;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceBishop : ChessPiece
    {
        public ChessPieceBishop(Color color, int id, BoardPosition startingPosition) : base(Piece.BISHOP, color, id, startingPosition)
        {
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
        }

        public override ChessPiece Clone()
        {
            ChessPieceBishop copy = new(_color, _id, _startingPosition);
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

            bool isSquareAValidDiagonal = v1 + hdistance == v2 || v2 + hdistance == v1;
            // a friendly piece cannot be on the destination square
            bool isFriendlyPieceOnSquare = board.IsPieceAtPosition(position, _color);
            bool isDisabledPieceOnSquare = board.IsPieceAtPosition(position, Color.NONE); // PART OF NEW RULESETS
            if (!isSquareAValidDiagonal || isFriendlyPieceOnSquare || isDisabledPieceOnSquare) { return false; }

            string operation = "";


            // based on what the vdistance and hdistance are, if they are positive numbers, we are decrementing
            // if they are negative numbers, we are incrementing
            // this is how we move diagonally over a 2d array in step wise motion
            if (vdistance < 0)
            {
                operation = "+"; // this can be done using classes and polymorphism too, but its simpler like this
                if (hdistance < 0)
                {
                    operation += "+";
                }
                else if (hdistance > 0)
                {
                    operation += "-";
                }
            }
            else if (vdistance > 0)
            {
                operation = "-";
                if (hdistance < 0)
                {
                    operation += "+";
                }
                else if (hdistance > 0)
                {
                    operation += "-";
                }
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

            if (operation == "--")
            {
                for (int i = v1, j = h1; i > v2; i--, j--)
                {
                    if (!isCurrentPositionIfSoSkip(i, j) && isPieceBlockingPath(i, j))
                    {
                        return false;
                    }
                }
            }
            else if (operation == "-+")
            {
                for (int i = v1, j = h1; i > v2; i--, j++)
                {
                    if (!isCurrentPositionIfSoSkip(i, j) && isPieceBlockingPath(i, j))
                    {
                        return false;
                    }
                }
            }
            else if (operation == "++")
            {
                for (int i = v1, j = h1; i < v2; i++, j++)
                {
                    if (!isCurrentPositionIfSoSkip(i, j) && isPieceBlockingPath(i, j))
                    {
                        return false;
                    }
                }
            }
            else if (operation == "+-")
            {
                for (int i = v1, j = h1; i < v2; i++, j--)
                {
                    if (!isCurrentPositionIfSoSkip(i, j) && isPieceBlockingPath(i, j))
                    {
                        return false;
                    }
                }
            }

            // if we didnt return false on any of the branches, then there are no pieces blocking our path
            return true;
        }

        protected override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            // does this need to exist?
            return false;
        }
    }
}
