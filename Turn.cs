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
        private String _description;

        public Turn(int turnNumber, ChessPiece piece, BoardPosition previousPosition, BoardPosition newPosition, ChessBoard chessBoard)
        {
            _turnNumber = turnNumber;
            _piece = piece; // copy constructor here, what if piece is captured later? this reference becomes null
            _previousPosition = previousPosition;
            _newPosition = newPosition;
            _chessBoard = new ChessBoard(chessBoard); // copy state of board
            piece.Move(_chessBoard, _newPosition); // update the board to reflect latest state
            if (turnNumber % 2 == 0)
            {
                _playerTurn = Color.BLACK;
            }
            else
            {
                _playerTurn = Color.WHITE;
            }

            String description = _previousPosition.StringValue;

            if (_piece.GetColor().Equals(ChessPiece.Color.WHITE))
                description += " White ";
            else
                description += " Black ";

            switch (_piece.GetPiece())
            {
                case ChessPiece.Piece.PAWN: description += "Pawn " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.ROOK: description += "Rook " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.BISHOP: description += "Bishop " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.KNIGHT: description += "Knight " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.QUEEN: description += "Queen " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.KING: description += "King " + _piece.GetId() + " "; break;
            }

            description += " move " + _newPosition.StringValue;
            _description = description;
        }

        public Turn(int turnNumber, ChessPiece piece, BoardPosition newPosition, ChessBoard chessBoard)
        {
            _turnNumber = turnNumber;
            _piece = piece; // copy constructor here, what if piece is captured later? this reference becomes null
            _previousPosition = piece.GetCurrentPosition();
            _newPosition = newPosition;
            _chessBoard = new ChessBoard(chessBoard); // copy state of board
            piece.Move(_chessBoard, _newPosition); // update the board to reflect current state
            if (turnNumber % 2 == 0)
            {
                _playerTurn = Color.BLACK;
            }
            else
            {
                _playerTurn = Color.WHITE;
            }

            String description = _previousPosition.StringValue;

            if (_piece.GetColor().Equals(ChessPiece.Color.WHITE))
                description += " White ";
            else
                description += " Black ";

            switch (_piece.GetPiece())
            {
                case ChessPiece.Piece.PAWN: description += "Pawn " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.ROOK: description += "Rook " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.BISHOP: description += "Bishop " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.KNIGHT: description += "Knight " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.QUEEN: description += "Queen " + _piece.GetId() + " "; break;
                case ChessPiece.Piece.KING: description += "King " + _piece.GetId() + " "; break;
            }

            description += " move " + _newPosition.StringValue;
            _description = description;
        }

        public int TurnNumber { get { return _turnNumber; } }
        public ChessPiece ChessPiece { get { return _piece; } }
        public BoardPosition PreviousPosition {  get { return _previousPosition; } }
        public BoardPosition NewPosition { get { return _newPosition; } }
        public ChessBoard ChessBoard {  get { return _chessBoard; } }
        public Color PlayerTurn { get { return _playerTurn; } }
        public String TurnDescription { get { return _description; } }  

    }
}
