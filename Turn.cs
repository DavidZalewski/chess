using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Turn
    {
        private int _turnNumber;
        private ChessPiece _piece;
        private BoardPosition _previousPosition;
        private BoardPosition _newPosition;
        private ChessBoard _chessBoard;

        public Turn(int turnNumber, ChessPiece piece, BoardPosition previousPosition, BoardPosition newPosition, ChessBoard chessBoard)
        {
            _turnNumber = turnNumber;
            _piece = piece;
            _previousPosition = previousPosition;
            _newPosition = newPosition;
            _chessBoard = new ChessBoard(chessBoard); // copy state of board
        }

        public int TurnNumber { get { return _turnNumber; } }
        public ChessPiece ChessPiece { get { return _piece; } }
        public BoardPosition PreviousPosition {  get { return _previousPosition; } }
        public BoardPosition NewPosition { get { return _newPosition; } }
        public ChessBoard ChessBoard {  get { return _chessBoard; } }

    }
}
