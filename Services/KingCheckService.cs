using Chess.Board;
using Chess.Pieces;

namespace Chess.Services
{
    [Serializable]
    public class KingCheckService
    {
        public KingCheckService() { }

        public bool IsKingInCheck(Turn turnToBeMade)
        {
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
                if (piece.IsValidMove(turnToBeMade.ChessBoard, positionToCheck))
                    return true; // The King is in check from something
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
        public bool IsCheckMate(Turn turn)
        {
            List<ChessPiece> friendlyPieces = new();
            // if the turn passed in was blacks turn, then we should check if its checkmate for white king
            // otherwise if the turn passed in was whites turn, then we should check if its checkmate for black king

            if (turn.PlayerTurn.Equals(Turn.Color.WHITE))
                friendlyPieces = turn.ChessPieces.FindAll(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK));
            else
                friendlyPieces = turn.ChessPieces.FindAll(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE));

            List<Turn> possibleMoves = new();

            foreach (ChessPiece piece in friendlyPieces)
            {
                // iterate over all board positions
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BoardPosition pos = new((RANK)i, (FILE)j);

                        // This may solve the problem
                        ChessPiece.SetCastleCallbackFunction(turn.CastleCallBackFunction);
                        ChessPiece.SetIsEnPassantCallbackFunction(turn.IsEnPassantCallBackFunction);

                        if (piece.IsValidMove(turn.ChessBoard, pos))
                            possibleMoves.Add(new(turn.TurnNumber + 1, piece, piece.GetCurrentPosition(), pos, turn.ChessBoard, turn.ChessPieces));
                    }
                }
            }

            foreach (Turn possibleTurn in possibleMoves)
            {
                if (!IsKingInCheck(possibleTurn))
                    return false;
            }

            return true; // Check Mate
        }
    }
}
