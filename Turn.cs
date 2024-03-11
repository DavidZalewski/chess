using Chess.Board;
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
            ChessPiece.SetCastleCallbackFunction(this.CastleCallBackFunction);
            ChessPiece.SetIsEnPassantCallbackFunction(this.IsEnPassantCallBackFunction); // THIS IS THE PROBLEM
            _chessBoard = new ChessBoard(chessBoard); // copy state of board
            _chessPieces = _chessBoard.GetActivePieces();
            _piece = _chessPieces.First(p => p.Equals(piece));
            Console.WriteLine("Turn Ctor: Piece: " + _piece.GetPieceName());
            Console.WriteLine("Ctor: Turn._chessPieces.Count(): " + _chessPieces.Count);
            if (!_piece.IsValidMove(_chessBoard, _newPosition))
                throw new Exception("Cannot construct turn. Invalid Move for piece");
            Console.WriteLine("After _piece.IsValidMove(): Turn._chessPieces.Count(): " + _chessPieces.Count);
            _piece.Move(_chessBoard, _newPosition); // update the board to reflect latest state - if there is a capture here - update the list of pieces we just copied to reflect the current state of board                     
            Console.WriteLine("After _piece.Move(): Turn._chessPieces.Count(): " + _chessPieces.Count);
            Console.WriteLine("After _chessPieces.ForEach: Turn._chessPieces.Count(): " + _chessPieces.Count);

            // TODO: Determine if the last move made was an En Passant capture
            // Store another property on ChessPiecePawn.IsEnPassantCaptureMove
            ChessPiece enPassantCapturedPiece = _chessPieces.Find(p => p is ChessPiecePawn && (p as ChessPiecePawn).IsEnPassantTarget);
            
            if (enPassantCapturedPiece != null)
            {
                _action = " capture [" + enPassantCapturedPiece.GetPieceName() + "] ";
                chessBoard.SetPieceAtPosition(enPassantCapturedPiece.GetCurrentPosition(), NoPiece.Instance);
            }
            Console.WriteLine("After _chessPieces.RemoveAll: Turn._chessPieces.Count(): " + _chessPieces.Count);

            _chessPieces = _chessBoard.GetActivePieces();

            Console.WriteLine("After _chessBoard.RemovedCapturedPieces: Turn._chessPieces.Count(): " + _chessPieces.Count);

            if (turnNumber % 2 == 0)
            {
                _playerTurn = Color.BLACK;
            }
            else
            {
                _playerTurn = Color.WHITE;
            }
            _description = _piece.GetPieceName() + " " + _previousPosition.StringValue + _action + _newPosition.StringValue;
            Console.WriteLine("Turn CTOR End: Description: " + _description);
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


        // TODO: review this method signature: chessBoard arg is never used in preference to member _chessBoard
        // Can this method signature be shortened?
        public bool CastleCallBackFunction(ChessBoard chessBoard, BoardPosition boardPosition, ChessPiece king)
        {
            int hv = boardPosition.FileAsInt;
            int vv = boardPosition.RankAsInt;

            ChessPiece? rook = _chessPieces.Find(p =>
            {
                BoardPosition cbp = p.GetCurrentPosition();
                return cbp.FileAsInt == hv && cbp.RankAsInt == vv;
            });

            Assert.That(rook, Is.Not.Null);

            if (rook != null)
            {
                Assert.That(rook.GetPiece(), Is.EqualTo(ChessPiece.Piece.ROOK));

                // is king left of rook, or right of rook?
                int d = king.GetCurrentPosition().FileAsInt - rook.GetCurrentPosition().FileAsInt;
                FILE kh = king.GetCurrentPosition().File;
                FILE rh = rook.GetCurrentPosition().File;
                RANK v = king.GetCurrentPosition().Rank;
                // k=4, r=0 4-0=4, k=4, r2=8, k-r2=4-7 = -3
                if (d == 4)
                {
                    // queen side castle
                    // k goes -2 squares
                    // r goes +3 squares
                    _action = " Castle (Queen Side) ";
                    kh -= 2;
                    rh += 3;
                }
                else if (d == -3)
                {
                    // king side castle
                    // k goes +2 squares
                    // r goes -2 squares
                    _action = " Castle (King Side) ";
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
                _chessBoard.SetPieceAtPosition(kingLastPosition, NoPiece.Instance);
                _chessBoard.SetPieceAtPosition(rookLastPosition, NoPiece.Instance);
                _chessBoard.SetPieceAtPosition(new(v, kh), king);
                _chessBoard.SetPieceAtPosition(new(v, rh), rook);
            }

            return true;
        }

        public bool IsEnPassantCallBackFunction(ChessBoard chessBoard, BoardPosition boardPosition, ChessPiece pawnAttemptingEnPassant)
        {
            Console.WriteLine("IsEnPassantCallBackFunction invoked for Turn: " + _description);
            RANK enPassantRow;
            ChessPiece.Color opponentColor;
            int enPassantOffSet = 0;

            if (pawnAttemptingEnPassant.GetColor().Equals(ChessPiece.Color.WHITE))
            {
                enPassantRow = RANK.FIVE;
                opponentColor = ChessPiece.Color.BLACK;
                enPassantOffSet = -1;
            }
            else
            {
                enPassantRow = RANK.FOUR;
                opponentColor = ChessPiece.Color.WHITE;
                enPassantOffSet = +1;
            }

            BoardPosition pawnPos = pawnAttemptingEnPassant.GetCurrentPosition();

            // Is the pawn in the correct Row to do this? En Passant is only possible if a pawn is on a specific row on the board
            if (pawnPos.Rank != enPassantRow)
                return false;

            // Are there opponent pieces to its immediate left or right?

            // TODO: Provide better constructors for these kinds of operations
            BoardPosition bpl = new(pawnPos.Rank, (FILE)pawnPos.FileAsInt - 1);
            BoardPosition bpr = new(pawnPos.Rank, (FILE)pawnPos.FileAsInt + 1);

            foreach (BoardPosition bpToCheck in new List<BoardPosition>() { bpl, bpr })
            {
                // Is there an opponent piece at this position?
                if (chessBoard.IsPieceAtPosition(bpToCheck, opponentColor))
                {
                    ChessPiece? opponentPiece = _chessPieces.Find((ChessPiece cp) => cp.GetCurrentPosition() == bpToCheck);
                    Assert.That(opponentPiece, Is.Not.Null, "This assertion failed. If ChessBoard.IsPieceAtPosition returns true, the piece must exist in the collection");

                    // Is the opponent piece a pawn?
                    if (opponentPiece is ChessPiecePawn)
                    {
                        // Did that pawn move 2 squares?
                        if ((opponentPiece as ChessPiecePawn).MovedTwoSquares)
                        {
                            // is the position a capture position?
                            // Get the opponent pawn position, and get the position that is 1 square behind it
                            BoardPosition oppPos = opponentPiece.GetCurrentPosition();
                            // this operation needs to support both + 1 (for black) and -1 (for white)
                            BoardPosition enPassantCapturePos = new((RANK)oppPos.RankAsInt + enPassantOffSet, oppPos.File);
                            if (enPassantCapturePos == boardPosition)
                            {
                                (opponentPiece as ChessPiecePawn).IsEnPassantTarget = true;
                                return true; // This is a valid En Passant capture move
                            }
                        }
                    }
                }

            }

            return false;
        }

    }
}
