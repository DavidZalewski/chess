using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Board;
using Chess.Pieces;

namespace Chess.Services
{
    public class KingCheckService
    {
        private List<ChessPiece> _chessPieces;
        private ChessBoard _chessBoard;

        public KingCheckService(ChessBoard chessBoard, List<ChessPiece> chessPieces)
        {
            _chessBoard = chessBoard;
            _chessPieces = chessPieces;
        }

        public bool IsKingInCheck(Turn turnToBeMade)
        {
            ChessPiece chessPieceKing;
            List<ChessPiece> opponentPieces = new();

            if (turnToBeMade.PlayerTurn.Equals(Turn.Color.WHITE))
            {
                opponentPieces = _chessPieces.FindAll(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK));
                chessPieceKing = _chessPieces.Find(piece => piece.GetPiece().Equals(ChessPiece.Piece.KING) && piece.GetColor().Equals(ChessPiece.Color.WHITE));
                if (chessPieceKing == null)
                {
                    throw new Exception("Could not find King piece for IsKingInCheck - something unexpected has happened!");
                }
            }
            else
            {
                opponentPieces = _chessPieces.FindAll(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE));
                chessPieceKing = _chessPieces.Find(piece => piece.GetPiece().Equals(ChessPiece.Piece.KING) && piece.GetColor().Equals(ChessPiece.Color.BLACK));
                if (chessPieceKing == null)
                {
                    throw new Exception("Could not find King piece for IsKingInCheck - something unexpected has happened!");
                }
            }

            BoardPosition positionToCheck;

            // Determine if the king itself has moved, or if another piece has moved that may expose the king to a check
            if (turnToBeMade.ChessPiece.Equals(chessPieceKing))
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
                if (piece.IsValidMove(_chessBoard, positionToCheck))
                    return true; // The King is in check from something
            }

            return false; // The king is not in check
        }
    }
}
