using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessBoard
    {
        // the last inner array is assumed to be the starting position
        // looking at diagrams where the board is labelled, white is always on the bottom
        // therefore first value in array [0,0] is A8
        // A1 is found at [7,0]
        private int[,] _board = new int[8, 8] 
        {
            { 0 /*A8*/, 0, 0, 0, 0, 0, 0, 0 /*H8*/},
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0 /*A1*/, 0, 0, 0, 0, 0, 0, 0 /*H1*/},
        };

        public ChessBoard() { }

        public ChessBoard(ChessBoard other)
        {
            _board = other._board;
        }

        public int[,] GetBoard() { return _board; }

        public bool PopulateBoard(List<ChessPiece> chessPieces)
        {
            foreach (ChessPiece piece in chessPieces)
            {
                piece.Move(this, piece.GetStartingPosition());
            }

            return true;
        }
        public bool SetBoardValue(BoardPosition position, int value)
        {
            _board[position.VerticalValueAsInt, position.HorizontalValueAsInt] = value;
            return true;
        }

        public bool IsPieceAtPosition(BoardPosition position)
        {
            return _board[position.VerticalValueAsInt, position.HorizontalValueAsInt] > 0;
        }

        public bool IsPieceAtPosition(BoardPosition position, ChessPiece.Color color)
        {
            int value = _board[position.VerticalValueAsInt, position.HorizontalValueAsInt];
            if (color == ChessPiece.Color.WHITE)
                return value > 0 && value < 20;
            else
                return value > 20 && value < 30;
        }

        internal void InternalTestOnly_SetBoard(int[,] boardValue)
        {
            _board = boardValue;
        }
    }
}
