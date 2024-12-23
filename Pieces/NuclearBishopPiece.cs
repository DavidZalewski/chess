﻿using Chess.Board;
using Chess.Globals;
using System;
using System.Collections.Generic;

namespace Chess.Pieces
{
    [Serializable]
    public class NuclearBishopPiece : ChessPieceBishop
    {
        bool _wasSetOnBoard;
        public NuclearBishopPiece(Color color, int id, BoardPosition startingPosition) : base(color, id, startingPosition)
        {
            StaticLogger.Trace();
            _wasSetOnBoard = false;
        }

        public override ChessPiece Clone()
        {
            StaticLogger.Trace();
            NuclearBishopPiece copy = new(_color, _id, _startingPosition);
            copy._wasSetOnBoard = this._wasSetOnBoard;
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
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

            foreach (Square square in board.Board)
            {
                if (square.Piece is DisabledSquarePiece)
                {
                    if (square.Position.IsDiagonal(position))
                    {
                        square.Piece = NoPiece.Instance; // blast away the disabled squares
                    }
                }
            }
            return false;
        }
    }
}