using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Board;
using Chess.Pieces;

namespace Chess
{
    public class Turn
    {
        public enum Color
        {
            WHITE = 10,
            BLACK = 20
        }

        private int _turnNumber;
        private Color _playerTurn;
        private ChessPiece _piece;
        private BoardPosition _previousPosition;
        private BoardPosition _newPosition;
        private ChessBoard _chessBoard;

        public Turn(int turnNumber, ChessPiece piece, BoardPosition previousPosition, BoardPosition newPosition, ChessBoard chessBoard)
        {
            _turnNumber = turnNumber;
            _piece = piece; // copy constructor here, what if piece is captured later? this reference becomes null
            _previousPosition = previousPosition;
            _newPosition = newPosition;
            _chessBoard = new ChessBoard(chessBoard); // copy state of board
            if (turnNumber % 2 == 0)
            {
                _playerTurn = Color.BLACK;
            }
            else
            {
                _playerTurn = Color.WHITE;
            }
        }

        public int TurnNumber { get { return _turnNumber; } }
        public ChessPiece ChessPiece { get { return _piece; } }
        public BoardPosition PreviousPosition {  get { return _previousPosition; } }
        public BoardPosition NewPosition { get { return _newPosition; } }
        public ChessBoard ChessBoard {  get { return _chessBoard; } }
        public Color PlayerTurn { get { return _playerTurn; } }

    }
}
