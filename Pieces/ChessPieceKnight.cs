﻿using Chess.Board;
using Chess.Globals;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceKnight : ChessPiece
    {
        public ChessPieceKnight(Color color, int id, BoardPosition startingPosition) : base(Piece.KNIGHT, color, id, startingPosition)
        {
            StaticLogger.Trace();
            _realValue = (int)_piece + (int)_color; // could also calculate this in base class by adding the two enums together
        }

        public override ChessPiece Clone()
        {
            StaticLogger.Trace();
            ChessPieceKnight copy = new(_color, _id, _startingPosition);
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // get the distance
            //   2       =                                  7                             5
            int verticalDistance = _currentPosition.RankAsInt - position.RankAsInt;
            int horizontalDistance = _currentPosition.FileAsInt - position.FileAsInt;

            if (verticalDistance == 1 || verticalDistance == -1)
            {
                if (horizontalDistance == 2 || horizontalDistance == -2)
                {
                    // is there a friendly piece on this position that is blocking the knight from moving?
                    if (board.IsPieceAtPosition(position, _color) || board.IsPieceAtPosition(position, Color.NONE))
                        return false;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            }
            else if (verticalDistance == 2 || verticalDistance == -2)
            {
                if (horizontalDistance == 1 || horizontalDistance == -1)
                {
                    // is there a friendly piece on this position that is blocking the knight from moving?
                    if (board.IsPieceAtPosition(position, _color) || board.IsPieceAtPosition(position, Color.NONE))
                        return false;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // does this need to exist?
            return false;
        }
    }
}
