using Chess.Board;
using Chess.Callbacks;
using Chess.Pieces;
using Chess.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.GameState
{
    internal class ChessStateExplorer
    {
        ConcurrentLogger logger = new ConcurrentLogger("ChessStateExplorer.txt");
        
        public List<Turn> GenerateAllPossibleMoves(Turn turn, int depth)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            logger.Log($"ChessStateExplorer - BEGIN: Generating all possible moves for turn {turn.TurnDescription} at depth {depth}", threadId);
            KingCheckService kingCheckService = new KingCheckService();

            List<Turn> possibleMoves = new();

            if (depth == 0) // base case: reached maximum depth
            {
                logger.Log($"ChessStateExplorer - END: Turn {turn.TurnDescription} maximum depth reached, count: {possibleMoves.Count}", threadId);
                return possibleMoves;
            }

            if (kingCheckService.IsCheckMate(turn))
            {
                logger.Log($"ChessStateExplorer - END: Turn {turn.TurnDescription} at depth {depth} has reached CHECKMATE, count: {possibleMoves.Count} ", threadId);
                return possibleMoves;
            }

            List<ChessPiece> currentSidePieces = turn.ChessPieces.FindAll(piece => !piece.GetColor().Equals((ChessPiece.Color)turn.PlayerTurn));

            // TODO: Set the PawnPromotion callback function to just return 'Q' each time
            // Simulated future turns assume a pawn is always promoted to queen
            // iterate over all board positions
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = true;
            foreach (ChessPiece piece in currentSidePieces)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BoardPosition pos = new((RANK)i, (FILE)j);
                        Turn possibleTurn = new(turn.TurnNumber + 1, piece, piece.GetCurrentPosition(), pos, turn.ChessBoard);
                        if (possibleTurn.IsValidTurn && !kingCheckService.IsKingInCheck(possibleTurn))
                        {
                            logger.Log($"ChessStateExplorer - MID: Possible Move Found: {possibleTurn.TurnDescription}, Depth: {depth}, From: {turn.TurnDescription}", threadId);
                            possibleMoves.Add(possibleTurn);
                            // Recursively generate all possible moves from this new turn
                            List<Turn> subMoves = GenerateAllPossibleMoves(possibleTurn, depth - 1);
                            logger.Log($"ChessStateExplorer - MID: Turn: {possibleTurn.TurnDescription}, Depth: {depth}, From: {turn.TurnDescription}, SubCount: {subMoves.Count},", threadId);
                            possibleMoves.AddRange(subMoves);
                            logger.Log($"ChessStateExplorer - MID: Turn: {possibleTurn.TurnDescription}, Depth: {depth}, From: {turn.TurnDescription}, MainCount: {possibleMoves.Count},", threadId);
                        }
                    }
                }
            }
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = false;
            // TODO: change the PawnPromotion callback function back to GameManager.HandlePawnPromotion

            logger.Log($"ChessStateExplorer - END: Turn: {turn.TurnDescription}, Depth: {depth}, MainCount: {possibleMoves.Count},", threadId);

            return possibleMoves;
        }

        public List<TurnNode> GenerateAllPossibleMovesTurnNode(Turn turn, int depth)
        {
            KingCheckService kingCheckService = new KingCheckService();

            List<TurnNode> possibleMoves = new();

            if (depth == 0) // base case: reached maximum depth
                return possibleMoves;

            if (kingCheckService.IsCheckMate(turn))
            {
                return possibleMoves;
            }

            List<ChessPiece> currentSidePieces = turn.ChessPieces.FindAll(piece => !piece.GetColor().Equals((ChessPiece.Color)turn.PlayerTurn));

            // TODO: Set the PawnPromotion callback function to just return 'Q' each time
            // Simulated future turns assume a pawn is always promoted to queen
            // iterate over all board positions
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = true;
            foreach (ChessPiece piece in currentSidePieces)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BoardPosition pos = new((RANK)i, (FILE)j);
                        Turn possibleTurn = new(turn.TurnNumber + 1, piece, piece.GetCurrentPosition(), pos, turn.ChessBoard);
                        if (possibleTurn.IsValidTurn && !kingCheckService.IsKingInCheck(possibleTurn))
                        {
                            TurnNode turnNode = new TurnNode(possibleTurn);
                            // Recursively generate all possible moves from this new turn
                            turnNode.Children = GenerateAllPossibleMovesTurnNode(possibleTurn, depth - 1);
                            possibleMoves.Add(turnNode);
                        }
                    }
                }
            }
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = false;
            // TODO: change the PawnPromotion callback function back to GameManager.HandlePawnPromotion


            return possibleMoves;
        }
    }
}
