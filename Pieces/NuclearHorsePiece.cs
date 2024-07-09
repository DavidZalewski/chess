using Chess.Board;
using System;
using System.Collections.Generic;

namespace Chess.Pieces
{
    [Serializable]
    public class NuclearHorsePiece : ChessPieceKnight
    {
        bool _wasSetOnBoard = false;
        public NuclearHorsePiece(Color color, int id, BoardPosition startingPosition) : base(color, id, startingPosition)
        {
        }

        public override ChessPiece Clone()
        {
            NuclearHorsePiece copy = new(_color, _id, _startingPosition);
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            // Check if there are any disabled squares in the path
            if (IsPathBlockedByDisabledSquares(board, position))
            {
                return false;
            }

            // Call the base class's IsValidMove method
            return base.IsValidMove(board, position);
        }

        protected override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
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
                        ChessPiece disabledSquare = new DisabledSquarePiece(adjPos);
                        board.GetSquare(adjPos).Piece = disabledSquare;
                    }

                }
            }
            return false;
        }

        private bool IsPathBlockedByDisabledSquares(ChessBoard board, BoardPosition position)
        {
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

        private List<BoardPosition> GetAdjacentPositions(BoardPosition position)
        {
            List<BoardPosition> adjacentPositions = new List<BoardPosition>
            {
                new BoardPosition(position.Rank + 1, position.File),
                new BoardPosition(position.Rank - 1, position.File),
                new BoardPosition(position.Rank, position.File + 1),
                new BoardPosition(position.Rank, position.File - 1),
                new BoardPosition(position.Rank + 1, position.File + 1),
                new BoardPosition(position.Rank + 1, position.File - 1),
                new BoardPosition(position.Rank - 1, position.File + 1),
                new BoardPosition(position.Rank - 1, position.File - 1)
            };
            return adjacentPositions;
        }
    }
}