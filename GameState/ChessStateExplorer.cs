using Chess.Board;
using Chess.Callbacks;
using Chess.Pieces;
using Chess.Services;
using System.Collections.Concurrent;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chess.GameState
{
    internal class ChessStateExplorer
    {
        private static string cacheFilePath = "chess_cache.bin";
        ConcurrentLogger logger = new ConcurrentLogger("ChessStateExplorer_TurnNode.txt");
        public static ConcurrentDictionary<string, ulong> cache = new(); // TODO: change access later

        static ChessStateExplorer()
        {
            if (File.Exists(cacheFilePath))
            {
                using (FileStream fs = new FileStream(cacheFilePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    cache = (ConcurrentDictionary<string, ulong>)formatter.Deserialize(fs);
                }
            }
            else
            {
                cache = new ConcurrentDictionary<string, ulong>();
            }
        }

        public static void SaveCache()
        {
            using (FileStream fs = new FileStream(cacheFilePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, cache);
            }
        }

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

        public List<TurnNode> GenerateAllPossibleMovesTurnNode(Turn turn, int depth, ref ulong currentCount)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            logger.Log($"ChessStateExplorer - BEGIN: Generating all possible moves for BoardID {turn.ChessBoard.BoardID} at depth {depth}", threadId);
            KingCheckService kingCheckService = new();

            List<TurnNode> possibleMoves = new();

            if (depth == 0) // base case: reached maximum depth
            {
                logger.Log($"ChessStateExplorer - END: BoardID {turn.ChessBoard.BoardID} maximum depth reached, count: {possibleMoves.Count}", threadId);
                return possibleMoves;
            }

            if (kingCheckService.IsCheckMate(turn))
            {
                logger.Log($"ChessStateExplorer - END: BoardID {turn.ChessBoard.BoardID} at depth {depth} has reached CHECKMATE, count: {possibleMoves.Count} ", threadId);
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
                            TurnNode turnNode = new(possibleTurn);
                            ++currentCount;
                            //logger.Log($"ChessStateExplorer - MID: Possible Move Found: {possibleTurn.TurnDescription}, Depth: {depth}, From: {turn.TurnDescription}", threadId);

                            // Recursively generate all possible moves from this new turn
                            if (cache.ContainsKey(turnNode.BoardID))
                            {
                                logger.Log($"ChessStateExplorer - MID: Hitting Cache for BoardID: {turnNode.BoardID}", threadId);
                                ulong innerCount = cache.GetOrAdd(turnNode.BoardID, 0);
                                turnNode.Children = new List<TurnNode>();
                                turnNode.Count = innerCount;
                                possibleMoves.Add(turnNode);
                                currentCount += innerCount;
                                logger.Log($"ChessStateExplorer - MID: Hitting Cache for BoardID: {turnNode.BoardID}, Depth: {depth}, From: {turn.TurnDescription}, ChildrenCount: {innerCount}, MainCount: {currentCount}", threadId);
                            }
                            else
                            {
                                ulong innerCount = 0;
                                turnNode.Children = GenerateAllPossibleMovesTurnNode(possibleTurn, depth - 1, ref innerCount);
                                turnNode.Count = innerCount;
                                possibleMoves.Add(turnNode);
                                cache.AddOrUpdate(turnNode.BoardID, (s) => turnNode.Count, (s, i) => turnNode.Count);
                                currentCount += innerCount;
                                logger.Log($"ChessStateExplorer - MID: BoardID: {turnNode.BoardID}, Depth: {depth}, From: {turn.TurnDescription}, ChildrenCount: {innerCount}, MainCount: {currentCount} - Added to cache", threadId);
                            }
                        }
                    }
                }
            }
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = false;
            // TODO: change the PawnPromotion callback function back to GameManager.HandlePawnPromotion

            logger.Log($"ChessStateExplorer - END: BoardID: {turn.ChessBoard.BoardID}, Depth: {depth}, From: {turn.TurnDescription}, Count: {currentCount}", threadId);

            return possibleMoves;
        }
    }
}
