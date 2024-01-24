using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Board;
using Chess.Pieces;
using NUnit.Framework;
using static Chess.Pieces.ChessPiece;

namespace Chess
{
    [Serializable]
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
        private List<ChessPiece> _chessPieces = new List<ChessPiece>();

        public Turn(int turnNumber, ChessPiece piece, BoardPosition previousPosition, BoardPosition newPosition, ChessBoard chessBoard, List<ChessPiece> chessPieces)
        {
            _turnNumber = turnNumber;
            _previousPosition = previousPosition;
            _newPosition = newPosition;
            _chessBoard = new ChessBoard(chessBoard); // copy state of board
            _chessPieces = chessPieces.Select(a => (ChessPiece)a.Clone()).ToArray().ToList(); // copy list of pieces
            _piece = _chessPieces.First(p => p.Equals(piece)); // copy constructor here, what if piece is captured later? this reference becomes null
            if (!_piece.IsValidMove(_chessBoard, _newPosition))
                throw new Exception("Cannot construct turn. Invalid Move for piece");
            ChessPiece.SetCastleCallbackFunction(this.CastleCallBackFunction);
            _piece.Move(_chessBoard, _newPosition); // update the board to reflect latest state - if there is a capture here - update the list of pieces we just copied to reflect the current state of board                     
            //if (!_castleCallBackCalled)
            //    _piece.SetCurrentPosition(_newPosition);
            _chessPieces = _chessBoard.PruneCapturedPieces(_chessPieces);

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

        public Turn(int turnNumber, ChessPiece piece, BoardPosition newPosition, ChessBoard chessBoard, List<ChessPiece> chessPieces) 
            : this(turnNumber, piece, piece.GetCurrentPosition(), newPosition, chessBoard, chessPieces) {}

        public int TurnNumber { get { return _turnNumber; } }
        public ChessPiece ChessPiece { get { return _piece; } }
        public BoardPosition PreviousPosition {  get { return _previousPosition; } }
        public BoardPosition NewPosition { get { return _newPosition; } }
        public ChessBoard ChessBoard {  get { return _chessBoard; } }
        public Color PlayerTurn { get { return _playerTurn; } }
        public String TurnDescription { get { return _description; } }  
        public List<ChessPiece> ChessPieces { get { return _chessPieces; } }


        public bool CastleCallBackFunction(ChessBoard chessBoard, BoardPosition boardPosition, ChessPiece king)
        {
            int hv = boardPosition.HorizontalValueAsInt;
            int vv = boardPosition.VerticalValueAsInt;

            ChessPiece? rook = _chessPieces.Find(p => {
                BoardPosition cbp = p.GetCurrentPosition();
                return cbp.HorizontalValueAsInt == hv && cbp.VerticalValueAsInt == vv;
            });

            Assert.That(rook, Is.Not.Null);

            if (rook != null)
            {
                Assert.That(rook.GetPiece(), Is.EqualTo(ChessPiece.Piece.ROOK));

                // is king left of rook, or right of rook?
                int d = king.GetCurrentPosition().HorizontalValueAsInt - rook.GetCurrentPosition().HorizontalValueAsInt;
                BoardPosition.HORIZONTAL kh = king.GetCurrentPosition().HorizontalValue;
                BoardPosition.HORIZONTAL rh = rook.GetCurrentPosition().HorizontalValue;
                BoardPosition.VERTICAL v = king.GetCurrentPosition().VerticalValue;
                // k=4, r=0 4-0=4, k=4, r2=8, k-r2=4-7 = -3
                if (d == 4)
                {
                    // queen side castle
                    // k goes -2 squares
                    // r goes +3 squares
                    kh -= 2;
                    rh += 3;
                }
                else if (d == -3)
                {
                    // king side castle
                    // k goes +2 squares
                    // r goes -2 squares
                    kh += 2;
                    rh -= 2;
                }
                else
                {
                    throw new Exception("Unexpected Horizontal Distance Found when castling. Are you sure this is a valid castle?");
                }

                BoardPosition kingLastPosition = king.GetCurrentPosition();
                BoardPosition rookLastPosition = rook.GetCurrentPosition();

                // set board manually
                _chessBoard.SetBoardValue(kingLastPosition, 0);
                _chessBoard.SetBoardValue(rookLastPosition, 0);

                king.SetCurrentPosition(new(v, kh));
                rook.SetCurrentPosition(new(v, rh));

                _chessBoard.SetBoardValue(king.GetCurrentPosition(), king.GetRealValue());
                _chessBoard.SetBoardValue(rook.GetCurrentPosition(), rook.GetRealValue());
            }

            return true;
        }


    }
}
