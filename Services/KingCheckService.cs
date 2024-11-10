using Chess.Attributes;
using Chess.Board;
using Chess.Callbacks;
using Chess.GameState;
using Chess.Globals;
using Chess.Pieces;

namespace Chess.Services
{
    [Serializable]
    public class KingCheckService
    {
        public KingCheckService() { }

        public bool IsKingInCheck(ChessPiece.Color color, ChessBoard chessBoard)
        {
            StaticLogger.Trace();
            List<ChessPiece> chessPieces = chessBoard.GetActivePieces();
            if (chessPieces.Count == 0) { return false; }
            ChessPiece chessPieceKing = chessPieces.First(p => p.GetPiece().Equals(ChessPiece.Piece.KING) && p.GetColor().Equals(color));
            if (chessPieceKing == null)
                return false;
            if (chessPieceKing.GetColor().Equals(ChessPiece.Color.WHITE))
            {
                bool IsInCheck = chessPieces.Any(p => p.GetColor().Equals(ChessPiece.Color.BLACK) && p.IsValidMove(chessBoard, chessPieceKing.GetCurrentPosition()));
                return IsInCheck;
            }
            else
            {
                bool IsInCheck = chessPieces.Any(p => p.GetColor().Equals(ChessPiece.Color.WHITE) && p.IsValidMove(chessBoard, chessPieceKing.GetCurrentPosition()));
                return IsInCheck;
            }
        }

        public bool IsKingInCheck(Turn turnToBeMade)
        {
            StaticLogger.Trace();
            ChessPiece chessPieceKing;
            List<ChessPiece> opponentPieces = new();

            if (turnToBeMade.PlayerTurn.Equals(Turn.Color.WHITE))
            {
                opponentPieces = turnToBeMade.ChessPieces.FindAll(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK)); // && turnToBeMade.ChessBoard.IsPieceAtPosition(piece));
                chessPieceKing = turnToBeMade.ChessPieces.Find(piece => piece.GetPiece().Equals(ChessPiece.Piece.KING) && piece.GetColor().Equals(ChessPiece.Color.WHITE));
                if (chessPieceKing == null)
                {
                    throw new Exception("Could not find King piece for IsKingInCheck - something unexpected has happened!");
                }
            }
            else
            {
                opponentPieces = turnToBeMade.ChessPieces.FindAll(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE)); // && turnToBeMade.ChessBoard.IsPieceAtPosition(piece));
                chessPieceKing = turnToBeMade.ChessPieces.Find(piece => piece.GetPiece().Equals(ChessPiece.Piece.KING) && piece.GetColor().Equals(ChessPiece.Color.BLACK));
                if (chessPieceKing == null)
                {
                    throw new Exception("Could not find King piece for IsKingInCheck - something unexpected has happened!");
                }
            }

            BoardPosition positionToCheck;

            // Determine if the king itself has moved, or if another piece has moved that may expose the king to a check
            if (turnToBeMade.ChessPiece.GetRealValue().Equals(chessPieceKing.GetRealValue()))
            {
                // The king has moved, would this new position put it in check?
                positionToCheck = turnToBeMade.NewPosition;
            }
            else
            {
                // Another piece has moved, does this expose the king to a check?
                positionToCheck = chessPieceKing.GetCurrentPosition();
            }

            // iterate over all Opponent Pieces and call IsValidMove(position) 
            // if one of these pieces returns true (this is a valid move that piece can make)
            // then the king would be put in check if it moved to this position        
            foreach (ChessPiece piece in opponentPieces)
            {
                SimulationService.BeginSimulation();
                if (piece.IsValidMove(turnToBeMade.ChessBoard, positionToCheck))
                {
                    SimulationService.EndSimulation();
                    return true; // The King is in check from something
                }
                SimulationService.EndSimulation();
            }
            
            return false; // The king is not in check
        }

        // there is going to be duplicate logic here, but we need to go step further and
        // identify all pieces that put the king in check
        // identify all possible moves (including blocking with another piece) - blocking is not possible against knight
        // brute force method - generate all 64 boardpositions - iterate over all pieces over all boardpositions - identify all valid moves for each piece
        // for each valid move a piece can make, call IsKingInCheck() on it
        // if there is a valid move a piece can make that makes IsKingInCheck() false, then its not checkmate

        // at its highest load - 64 * 16 pieces * 8 iterations (bishop/rook) = 8192 iterations (ChessBoard Copy Constructs, IsValidMove(), etc)
        // at 6 pieces left - 64 * 6 pieces = 384 iterations
        // 8000~ iterations is not super heavy performance wise given it can be checked per turn
        public bool IsCheckMate(Turn turn, out bool isStaleMate)
        {
            StaticLogger.Trace();
            List<ChessPiece> friendlyPieces = turn.PlayerTurn.Equals(Turn.Color.WHITE)
                ? turn.ChessPieces.FindAll(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK))
                : turn.ChessPieces.FindAll(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE));

            List<Turn> possibleMoves = new();

            foreach (ChessPiece piece in friendlyPieces)
            {
                SpecialMovesHandlers.ByPassPawnPromotionPromptUser = true;
                SimulationService.BeginSimulation();
                foreach (BoardPosition pos in piece.GetPossiblePositions(turn.ChessBoard))
                {
                    Turn possibleTurn = new(turn.TurnNumber + 1, piece, piece.GetCurrentPosition(), pos, turn.ChessBoard);
                    if (possibleTurn.IsValidTurn)
                        possibleMoves.Add(possibleTurn);
                }
                SpecialMovesHandlers.ByPassPawnPromotionPromptUser = false;
                SimulationService.EndSimulation();
            }

            foreach (Turn possibleTurn in possibleMoves)
            {
                if (!IsKingInCheck(possibleTurn))
                {
                    isStaleMate = false;
                    return false;
                }
            }

            ChessPiece.Color colorToCheck = turn.PlayerTurn.Equals(Turn.Color.WHITE) ? ChessPiece.Color.BLACK : ChessPiece.Color.WHITE;
            if (!IsKingInCheck(colorToCheck, turn.ChessBoard))
            {
                isStaleMate = true;
                return false;
            }
            else
            {
                isStaleMate = false;
                return true; // Check Mate
            }
        }
    }
}
