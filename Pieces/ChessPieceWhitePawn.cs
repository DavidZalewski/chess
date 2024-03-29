﻿using Chess.Board;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceWhitePawn : ChessPiecePawn
    {
        public ChessPieceWhitePawn(int id, BoardPosition startingPosition) : base(Color.WHITE, id, startingPosition)
        {
            _realValue = 11; // could also calculate this in base class by adding the two enums together
        }

        public override ChessPiece Clone()
        {
            ChessPieceWhitePawn copy = new(_id, _startingPosition);
            copy.IsEnPassantTarget = this.IsEnPassantTarget;
            copy.MovedTwoSquares = this.MovedTwoSquares;
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            // get the distance
            //   2       =                    6                        4
            int verticalDistance = _currentPosition.RankAsInt - position.RankAsInt;

            if (_IsEnPassantCallBackFunction != null)
            {
                bool IsValidEnPassant = _IsEnPassantCallBackFunction.Invoke(board, position, this);
                if (IsValidEnPassant) { return true; }
            }
            // TODO: handle promotions

            if (verticalDistance == 1)
            {
                if (_currentPosition.File == position.File)
                {
                    // is there a piece on this position that is blocking the pawn from moving?
                    if (board.IsPieceAtPosition(position))
                        return false;
                    else
                        return true;
                }
                // if the position to move pawn is on a different horizontal value, check if its a valid capture
                else
                {
                    int horizontalDistance = _currentPosition.FileAsInt - position.FileAsInt;
                    // adjacent and there is a black piece there?
                    if (board.IsPieceAtPosition(position, Color.BLACK) && (horizontalDistance == 1 || horizontalDistance == -1))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            // if the pawn hasn't moved yet, it can jump 2 squares instead of 1
            else if (verticalDistance == 2 && _currentPosition.Equals(_startingPosition))
            {
                if (_currentPosition.File == position.File)
                {
                    BoardPosition previousSquare = new BoardPosition((RANK)position.RankAsInt + 1, position.File);
                    // is there a piece in front of it that it is trying to jump over?
                    if (board.IsPieceAtPosition(previousSquare) || board.IsPieceAtPosition(position))
                        return false;
                    else
                    {
                        MovedTwoSquares = true; // We use this to for En Passant
                        return true;
                    }
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        protected override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            // does this need to exist?
            return false;
        }
    }
}
