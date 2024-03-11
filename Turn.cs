using Chess.Board;
using Chess.Callbacks;
using Chess.Pieces;
using NUnit.Framework;

namespace Chess
{
    [Serializable]
    public class Turn
    {
        // TODO: Consolidate these enums so that only a single Color enum is required
        public enum Color
        {
            WHITE = 10,
            BLACK = 20
        }

        // TODO: convert these into properties
        private int _turnNumber;
        private Color _playerTurn;
        private ChessPiece _piece;
        private BoardPosition _previousPosition;
        private BoardPosition _newPosition;
        private ChessBoard _chessBoard;
        private String _description;
        private List<ChessPiece> _chessPieces = new List<ChessPiece>();
        private string _action;

        public Turn(int turnNumber, ChessPiece piece, BoardPosition previousPosition, BoardPosition newPosition, ChessBoard chessBoard)
        {
            _turnNumber = turnNumber;
            _previousPosition = previousPosition;
            _newPosition = newPosition;
            _action = " move ";
            SpecialMovesHandlers.GetActionFromResult = (string action) => _action = " " + action + " ";
            _chessBoard = new ChessBoard(chessBoard); // copy state of board
            _chessPieces = _chessBoard.GetActivePieces();
            _piece = _chessPieces.First(p => p.Equals(piece));
            if (!_piece.IsValidMove(_chessBoard, _newPosition))
                throw new Exception("Cannot construct turn. Invalid Move for piece");
            _piece.Move(_chessBoard, _newPosition); // update the board to reflect latest state - if there is a capture here - update the list of pieces we just copied to reflect the current state of board                     

            // TODO: Determine if the last move made was an En Passant capture
            // Store another property on ChessPiecePawn.IsEnPassantCaptureMove
            ChessPiece? enPassantCapturedPiece = _chessPieces.Find(p => p is ChessPiecePawn && (p as ChessPiecePawn).IsEnPassantTarget);
            
            if (enPassantCapturedPiece != null)
            {
                _action = " capture [" + enPassantCapturedPiece.GetPieceName() + "] ";
                _chessBoard.SetPieceAtPosition(enPassantCapturedPiece.GetCurrentPosition(), NoPiece.Instance);
            }

            _chessPieces = _chessBoard.GetActivePieces();

            if (turnNumber % 2 == 0)
            {
                _playerTurn = Color.BLACK;
            }
            else
            {
                _playerTurn = Color.WHITE;
            }
            _description = _piece.GetPieceName() + " " + _previousPosition.StringValue + _action + _newPosition.StringValue;
        }

        public Turn(int turnNumber, ChessPiece piece, BoardPosition newPosition, ChessBoard chessBoard)
            : this(turnNumber, piece, piece.GetCurrentPosition(), newPosition, chessBoard) { }

        public int TurnNumber { get { return _turnNumber; } }
        public ChessPiece ChessPiece { get { return _piece; } }
        public BoardPosition PreviousPosition { get { return _previousPosition; } }
        public BoardPosition NewPosition { get { return _newPosition; } }
        public ChessBoard ChessBoard { get { return _chessBoard; } }
        public Color PlayerTurn { get { return _playerTurn; } }
        public String TurnDescription { get { return _description; } }
        public List<ChessPiece> ChessPieces { get { return _chessPieces; } }
    }
}
