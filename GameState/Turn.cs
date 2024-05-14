using Chess.Board;
using Chess.Callbacks;
using Chess.Pieces;

namespace Chess.GameState
{
    [Serializable]
    public class Turn
    {
        public enum Color
        {
            WHITE = 10,
            BLACK = 20
        }

        public int TurnNumber { get; }
        public ChessPiece ChessPiece { get; }
        public BoardPosition PreviousPosition { get; }
        public BoardPosition NewPosition { get; }
        public ChessBoard ChessBoard { get; }
        public Color PlayerTurn { get; }
        public string TurnDescription { get; }
        public List<ChessPiece> ChessPieces { get; }
        public bool IsValidTurn { get; protected set; } = false;
        public string Command { get; set; }

        public Turn(int turnNumber, ChessPiece piece, BoardPosition previousPosition, BoardPosition newPosition, ChessBoard chessBoard)
        {
            TurnNumber = turnNumber;
            ChessPiece = piece;
            PreviousPosition = previousPosition;
            NewPosition = newPosition;
            PlayerTurn = turnNumber % 2 == 0 ? Color.BLACK : Color.WHITE;
            Action = " move ";
            SpecialMovesHandlers.GetActionFromResult = (action) => Action = " " + action + " ";
            ChessBoard = new ChessBoard(chessBoard); // copy state of board
            ChessPieces = ChessBoard.GetActivePieces();
            ChessPiece = ChessPieces.First(p => p.Equals(piece)); // TODO: this should just be an assert
            if (!ChessPiece.IsValidMove(ChessBoard, NewPosition))
            {
                IsValidTurn = false;
                TurnDescription = "Invalid Turn";
            }              
            else
            {
                IsValidTurn = true;
                ChessPiece.Move(ChessBoard, NewPosition); // update the board to reflect latest state - if there is a capture here - update the list of pieces we just copied to reflect the current state of board                     

                // Determine if this is an En Passant capture (IsValidMove will mutate state if it is)
                ChessPiece? enPassantCapturedPiece = ChessPieces.Find(p => p is ChessPiecePawn && (p as ChessPiecePawn).IsEnPassantTarget);

                if (enPassantCapturedPiece != null)
                {
                    Action = " capture [" + enPassantCapturedPiece.GetPieceName() + "] ";
                    ChessBoard.SetPieceAtPosition(enPassantCapturedPiece.GetCurrentPosition(), NoPiece.Instance);
                }

                ChessPieces = ChessBoard.GetActivePieces();
                TurnDescription = ChessPiece.GetPieceName() + " " + PreviousPosition.StringValue + Action + NewPosition.StringValue;
            }
        }

        public Turn(int turnNumber, ChessPiece piece, BoardPosition newPosition, ChessBoard chessBoard)
            : this(turnNumber, piece, piece.GetCurrentPosition(), newPosition, chessBoard) { }

        private string Action { get; set; }
    }
}