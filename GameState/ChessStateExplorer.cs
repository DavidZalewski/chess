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
        private const int REPARTITION_THRESHOLD = 1000; // adjust this value as needed
        private const int PARTITION_SIZE = 8; // adjust this value as needed
        private Dictionary<string, ConcurrentDictionary<string, CacheItem>> partitions = new();
        public static ConcurrentDictionary<string, CacheItem> cache = new(); // TODO: change access later
        private static ulong cache_hits = 0; // TODO

        //static ChessStateExplorer()
        //{
        //    if (File.Exists(cacheFilePath))
        //    {
        //        using (FileStream fs = new FileStream(cacheFilePath, FileMode.Open))
        //        {
        //            BinaryFormatter formatter = new BinaryFormatter();
        //            cache = (ConcurrentDictionary<string, ulong>)formatter.Deserialize(fs);
        //        }
        //    }
        //    else
        //    {
        //        cache = new ConcurrentDictionary<string, ulong>();
        //    }
        //}

        //public static void SaveCache()
        //{
        //    using (FileStream fs = new FileStream(cacheFilePath, FileMode.Create))
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(fs, cache);
        //    }
        //}

        public void RepartitionCache()
        {
            if (cache.Count < REPARTITION_THRESHOLD) return;

            int threadId = Thread.CurrentThread.ManagedThreadId;

            var topItems = cache.OrderByDescending(x => x.Value.AccessCount).Take(PARTITION_SIZE);
            var commonPrefixes = topItems.Select(x => x.Key.Substring(0, PARTITION_SIZE)).Distinct();

            logger.Log($"ChessStateExplorer - RepartitionCache called - {commonPrefixes.Count()} unique prefix partitions found", threadId);

            foreach (var prefix in commonPrefixes)
            {
                var partition = new ConcurrentDictionary<string, CacheItem>();
                foreach (var item in cache.Where(x => x.Key.StartsWith(prefix)))
                {
                    partition.TryAdd(item.Key, item.Value);
                }
                partitions.TryAdd(prefix, partition);
            }

            logger.Log($"ChessStateExplorer - Repartition Successful - {partitions.Count} partitions generated for cache", threadId);

            cache.Clear();
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

                            if (cache.ContainsKey(turnNode.BoardID))
                            {
                                CacheItem cacheItem = cache[turnNode.BoardID];
                                cacheItem.InterlockedIncrement();
                                ulong innerCount = cacheItem.Value;
                                turnNode.Children = new List<TurnNode>();
                                turnNode.Count = innerCount;
                                possibleMoves.Add(turnNode);
                                currentCount += innerCount;
                                RepartitionCache();
                                logger.Log($"ChessStateExplorer - MID: Hitting Cache for BoardID: {turnNode.BoardID}, Depth: {depth}, From: {turn.ChessBoard.BoardID}, ChildrenCount: {innerCount}, MainCount: {currentCount}", threadId);
                            }
                            else
                            {
                                ulong innerCount = 0;
                                turnNode.Children = GenerateAllPossibleMovesTurnNode(possibleTurn, depth - 1, ref innerCount);
                                turnNode.Count = innerCount;
                                possibleMoves.Add(turnNode);
                                cache.AddOrUpdate(turnNode.BoardID, new CacheItem(turnNode.Count), (s, i) => new CacheItem(turnNode.Count));
                                currentCount += innerCount;
                                logger.Log($"ChessStateExplorer - MID: BoardID: {turnNode.BoardID}, Depth: {depth}, From: {turn.ChessBoard.BoardID}, ChildrenCount: {innerCount}, MainCount: {currentCount} - Added to cache", threadId);
                            }
                        }
                    }
                }
            }
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = false;
            // TODO: change the PawnPromotion callback function back to GameManager.HandlePawnPromotion

            logger.Log($"ChessStateExplorer - END: BoardID: {turn.ChessBoard.BoardID}, Depth: {depth}, From: {turn.ChessBoard.BoardID}, Count: {currentCount}", threadId);

            return possibleMoves;
        }

        public List<(string Key, CacheItem AccessCount)> GetTopNCachedItems(int n)
        {
            var topItems = new List<(string Key, CacheItem AccessCount)>();

            foreach (var partition in partitions.Values)
            {
                topItems.AddRange(partition
                    .OrderByDescending(x => x.Value.AccessCount)
                    .Take(n)
                    .Select(x => (x.Key, x.Value)));
            }

            return topItems;
        }

        public void PrintTopCacheItems(int n)
        {
            logger.Log($"Cache Size: {cache.Count}", 0);
            var topItems = cache.OrderByDescending(x => x.Value.AccessCount).Take(n);
            foreach (var item in topItems)
            {
                logger.Log($"BoardID: {item.Key}, AccessCount: {item.Value.AccessCount}, Value: {item.Value.Value}", 0);
            }
        }

        public long CacheSize()
        {
            return cache.Count;
        }
    }
}
