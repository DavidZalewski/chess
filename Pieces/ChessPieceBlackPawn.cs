using Chess.Board;
using Chess.Globals;
using Chess.Services;

namespace Chess.Pieces
{
    [Serializable]
    public class ChessPieceBlackPawn : ChessPiecePawn
    {
        public ChessPieceBlackPawn(int id, BoardPosition startingPosition) : base(Color.BLACK, id, startingPosition)
        {
            StaticLogger.Trace();
            _realValue = 21; // could also calculate this in base class by adding the two enums together
        }

        public override ChessPiece Clone()
        {
            StaticLogger.Trace();
            ChessPieceBlackPawn copy = new(_id, _startingPosition);
            copy.IsEnPassantTarget = this.IsEnPassantTarget;
            copy.MovedTwoSquares = this.MovedTwoSquares;
            return Clone(copy);
        }

        public override bool IsValidMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            // get the distance
            //   -2       =                    1                        3
            int verticalDistance = _currentPosition.RankAsInt - position.RankAsInt;

            if (_IsEnPassantCallBackFunction != null)
            {
                bool IsValidEnPassant = _IsEnPassantCallBackFunction.Invoke(board, position, this);
                if (IsValidEnPassant) { return true; }
            }

            if (verticalDistance == -1)
            {
                if (_currentPosition.File == position.File)
                {
                    // is there a piece on this position that is blocking the pawn from moving?
                    if (board.IsPieceAtPosition(position))
                        return false;
                    else
                        return true;
                }
                // if the position to move pawn is on a different horizontal value, check if its a valid capture
                else
                {
                    int horizontalDistance = _currentPosition.FileAsInt - position.FileAsInt;
                    // adjacent and there is a black piece there?
                    if (board.IsPieceAtPosition(position, Color.WHITE) && (horizontalDistance == 1 || horizontalDistance == -1))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            // if the pawn hasn't moved yet, it can jump 2 squares instead of 1
            else if (verticalDistance == -2 && _currentPosition.Equals(_startingPosition))
            {
                if (_currentPosition.File == position.File)
                {
                    BoardPosition previousSquare = new BoardPosition((RANK)position.RankAsInt - 1, position.File);
                    // is there a piece in front of it that it is trying to jump over?
                    if (board.IsPieceAtPosition(previousSquare) || board.IsPieceAtPosition(position))
                        return false;
                    else
                    {
                        if (!SimulationService.IsSimulation)
                        {
                            MovedTwoSquares = true; // We use this to for En Passant
                            LambdaQueue.Enqueue((Chess.Controller.GameController gc) => {
                                ChessPieceBlackPawn? pawn = (ChessPieceBlackPawn?)gc.GetChessBoard().GetActivePieces().Find((p) => p.GetPieceName().Equals(this._pieceName));
                                if (pawn != null)
                                {
                                    StaticLogger.Log($"Closing window of opportunity for En Passant for Pawn {pawn.GetPieceName()}", LogLevel.Debug);
                                    pawn.MovedTwoSquares = false;
                                }
                                else
                                {
                                    //System.Diagnostics.Debugger.Break(); // TEMP
                                    StaticLogger.Log($"Warning - could not find Pawn {this._pieceName} from GameController Active Pieces - looks like it was captured - ignoring", LogLevel.Warn);
                                }
                            }); // Load the LambdaQueue
                        }
                        return true;
                    }
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public override bool ImplementMove(ChessBoard board, BoardPosition position)
        {
            StaticLogger.Trace();
            return false;
        }
    }
}
