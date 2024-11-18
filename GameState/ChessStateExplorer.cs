using Chess.Attributes;
using Chess.Board;
using Chess.Callbacks;
using Chess.Collections;
using Chess.Globals;
using Chess.Pieces;
using Chess.Services;

namespace Chess.GameState
{
    [ToDo("Write your own unit test runner that supports multi process better")]
    internal class ChessStateExplorer
    {
        private static string cacheFilePath = "chess_cache.bin";
        ConcurrentLogger logger = new ConcurrentLogger("ChessStateExplorer_TurnNode");
        private const int REPARTITION_THRESHOLD = 1000; // adjust this value as needed
        private const int PARTITION_SIZE = 8; // adjust this value as needed
        public MultiDimensionalCache<CacheItem> cache = new MultiDimensionalCache<CacheItem>(PARTITION_SIZE);


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

        public List<Turn> GenerateAllPossibleMoves(Turn turn, int depth)
        {
            StaticLogger.Trace();
            int threadId = Thread.CurrentThread.ManagedThreadId;

            logger.Log($"ChessStateExplorer - BEGIN: Generating all possible moves for turn {turn.TurnDescription} at depth {depth}", threadId);
            KingCheckService kingCheckService = new KingCheckService();

            List<Turn> possibleMoves = new();

            if (depth == 0) // base case: reached maximum depth
            {
                logger.Log($"ChessStateExplorer - END: Turn {turn.TurnDescription} maximum depth reached, count: {possibleMoves.Count}", threadId);
                return possibleMoves;
            }

            bool isStaleMate = false;
            if (kingCheckService.IsCheckMate(turn, out isStaleMate))
            {
                logger.Log($"ChessStateExplorer - END: Turn {turn.TurnDescription} at depth {depth} has reached CHECKMATE, count: {possibleMoves.Count} ", threadId);
                return possibleMoves;
            }
            else if (isStaleMate)
            {
                logger.Log($"ChessStateExplorer - END: Turn {turn.TurnDescription} at depth {depth} has reached STALEMATE, count: {possibleMoves.Count} ", threadId);
                return possibleMoves;
            }

            List<ChessPiece> currentSidePieces = turn.ChessPieces.FindAll(piece => !piece.GetColor().Equals((ChessPiece.Color)turn.PlayerTurn));

            // Simulated future turns assume a pawn is always promoted to queen
            // iterate over all board positions
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = true;
            foreach (ChessPiece piece in currentSidePieces)
            {
                List<BoardPosition> positions = piece.GetPossiblePositions(turn.ChessBoard);
                foreach(BoardPosition position in positions)
                {
                    Turn possibleTurn = new(turn.TurnNumber + 1, piece, piece.GetCurrentPosition(), position, turn.ChessBoard);
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
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = false;

            logger.Log($"ChessStateExplorer - END: Turn: {turn.TurnDescription}, Depth: {depth}, MainCount: {possibleMoves.Count},", threadId);

            return possibleMoves;
        }

        public List<TurnNode> GenerateAllPossibleMovesTurnNode(Turn turn, int depth, ref ulong currentCount)
        {
            StaticLogger.Trace();
            int threadId = Thread.CurrentThread.ManagedThreadId;

            logger.Log($"ChessStateExplorer - BEGIN: Generating all possible moves for BoardID {turn.ChessBoard.BoardID} at depth {depth}", threadId);
            KingCheckService kingCheckService = new();

            List<TurnNode> possibleMoves = new();

            if (depth == 0) // base case: reached maximum depth
            {
                logger.Log($"ChessStateExplorer - END: BoardID {turn.ChessBoard.BoardID} maximum depth reached, count: {possibleMoves.Count}", threadId);
                return possibleMoves;
            }

            bool isStaleMate = false;
            if (kingCheckService.IsCheckMate(turn, out isStaleMate))
            {
                ToDoAttribute.Add("Do we need this?");
                turn.IsCheckMate = true;
                logger.Log($"ChessStateExplorer - END: BoardID {turn.ChessBoard.BoardID} at depth {depth} has reached CHECKMATE, count: {possibleMoves.Count} ", threadId);
                return possibleMoves;
            }
            else if (isStaleMate)
            {
                logger.Log($"ChessStateExplorer - END: BoardID {turn.ChessBoard.BoardID} at depth {depth} has reached STALEMATE, count: {possibleMoves.Count} ", threadId);
                return possibleMoves;
            }

            ToDoAttribute.Add("There's no handling of draw by repetition here!");

            List<ChessPiece> currentSidePieces = turn.ChessPieces.FindAll(piece => !piece.GetColor().Equals((ChessPiece.Color)turn.PlayerTurn));

            // Simulated future turns assume a pawn is always promoted to queen
            // iterate over all board positions
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = true;
            foreach (ChessPiece piece in currentSidePieces)
            {
                List<BoardPosition> positions = piece.GetPossiblePositions(turn.ChessBoard);
                foreach (BoardPosition position in positions) 
                {
                    Turn possibleTurn = new(turn.TurnNumber + 1, piece, piece.GetCurrentPosition(), position, turn.ChessBoard);
                    bool isKingInCheck = kingCheckService.IsKingInCheck(possibleTurn);
                    possibleTurn.IsKingInCheck = isKingInCheck;

                    if (possibleTurn.IsValidTurn && !isKingInCheck)
                    {
                        TurnNode turnNode = new(possibleTurn);

                        ++currentCount;
                        //logger.Log($"ChessStateExplorer - MID: Possible Move Found: {possibleTurn.TurnDescription}, Depth: {depth}, From: {turn.TurnDescription}", threadId);

                        //if (cache.TryGetValue(turnNode.BoardID, out CacheItem cacheItem))
                        //{
                        //    cacheItem.InterlockedIncrement();
                        //    ulong innerCount = cacheItem.Value;
                        //    turnNode.Children = new List<TurnNode>();
                        //    turnNode.Count = innerCount;
                        //    possibleMoves.Add(turnNode);
                        //    currentCount += innerCount;
                        //    logger.Log($"ChessStateExplorer - MID: Hitting Cache for BoardID: {turnNode.BoardID}, Depth: {depth}, From: {turn.ChessBoard.BoardID}, ChildrenCount: {innerCount}, MainCount: {currentCount}", threadId);
                        //}
                        //else
                        //{
                            ulong innerCount = 0;
                            turnNode.Children = GenerateAllPossibleMovesTurnNode(possibleTurn, depth - 1, ref innerCount);
                            turnNode.Count = innerCount;
                            possibleMoves.Add(turnNode);
                            //cache.AddOrUpdate(turnNode.BoardID, new CacheItem(turnNode.Count));
                            currentCount += innerCount;
                            logger.Log($"ChessStateExplorer - MID: BoardID: {turnNode.BoardID}, Depth: {depth}, From: {turn.ChessBoard.BoardID}, ChildrenCount: {innerCount}, MainCount: {currentCount} - Added to cache", threadId);
                        //}
                    }
                }
            }

            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = false;

            logger.Log($"ChessStateExplorer - END: BoardID: {turn.ChessBoard.BoardID}, Depth: {depth}, From: {turn.ChessBoard.BoardID}, Count: {currentCount}", threadId);

            return possibleMoves;
        }

        public List<(string Key, CacheItem AccessCount)> GetTopNCachedItems(int n)
        {
            StaticLogger.Trace();
            var topItems = new List<(string Key, CacheItem AccessCount)>();

            foreach (var item in cache._mainCache)
            {
                topItems.Add((item.Key, item.Value));
            }

            return topItems.OrderByDescending(x => x.AccessCount.AccessCount).Take(n).ToList();
        }

        public void PrintTopCacheItems(int n)
        {
            StaticLogger.Trace();
            logger.Log($"Cache Size: {cache._mainCache.Count}", 0);
            var topItems = GetTopNCachedItems(n);
            foreach (var item in topItems)
            {
                logger.Log($"BoardID: {item.Key}, AccessCount: {item.AccessCount.AccessCount}, Value: {item.AccessCount.Value}", 0);
            }
        }

        public long CacheSize()
        {
            StaticLogger.Trace();
            return cache._mainCache.Count;
        }

        public List<TurnNode> GenerateAllPossibleMovesTurnNode_NoRecursion(Turn turn, int depth, ref ulong currentCount)
        {
            StaticLogger.Trace();
            int threadId = Thread.CurrentThread.ManagedThreadId;

            logger.Log($"ChessStateExplorer - BEGIN: Generating all possible moves for BoardID {turn.ChessBoard.BoardID} at depth {depth}", threadId);
            KingCheckService kingCheckService = new();

            List<TurnNode> possibleMoves = new();

            if (depth == 0) // base case: reached maximum depth
            {
                logger.Log($"ChessStateExplorer - END: BoardID {turn.ChessBoard.BoardID} maximum depth reached, count: {possibleMoves.Count}", threadId);
                return possibleMoves;
            }

            bool isStaleMate = false;

            if (kingCheckService.IsCheckMate(turn, out isStaleMate))
            {
                logger.Log($"ChessStateExplorer - END: BoardID {turn.ChessBoard.BoardID} at depth {depth} has reached CHECKMATE, count: {possibleMoves.Count} ", threadId);
                return possibleMoves;
            }
            else if (isStaleMate)
            {
                logger.Log($"ChessStateExplorer - END: BoardID {turn.ChessBoard.BoardID} at depth {depth} has reached STALEMATE, count: {possibleMoves.Count} ", threadId);
                return possibleMoves;
            }

            List<ChessPiece> currentSidePieces = turn.ChessPieces.FindAll(piece => !piece.GetColor().Equals((ChessPiece.Color)turn.PlayerTurn));

            // Simulated future turns assume a pawn is always promoted to queen
            // iterate over all board positions
            SpecialMovesHandlers.ByPassPawnPromotionPromptUser = true;

            var stack = new Stack<(Turn turn, int depth, ulong currentCount)>();
            stack.Push((turn, depth, currentCount));

            while (stack.Count > 0)
            {
                var (currentTurn, currentDepth, currentCurrentCount) = stack.Pop();

                // ... (rest of the method remains the same)

                if (currentDepth > 0)
                {
                    foreach (ChessPiece piece in currentSidePieces)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                BoardPosition pos = new((RANK)i, (FILE)j);
                                Turn possibleTurn = new(currentTurn.TurnNumber + 1, piece, piece.GetCurrentPosition(), pos, currentTurn.ChessBoard);
                                if (possibleTurn.IsValidTurn && !kingCheckService.IsKingInCheck(possibleTurn))
                                {
                                    TurnNode turnNode = new(possibleTurn);
                                    ++currentCurrentCount;
                                    //logger.Log($"ChessStateExplorer - MID: Possible Move Found: {possibleTurn.TurnDescription}, Depth: {currentDepth}, From: {currentTurn.TurnDescription}", threadId);

                                    if (cache.TryGetValue(turnNode.BoardID, out CacheItem cacheItem))
                                    {
                                        cacheItem.InterlockedIncrement();
                                        ulong innerCount = cacheItem.Value;
                                        turnNode.Children = new List<TurnNode>();
                                        turnNode.Count = innerCount;
                                        possibleMoves.Add(turnNode);
                                        currentCurrentCount += innerCount;
                                        logger.Log($"ChessStateExplorer - MID: Hitting Cache for BoardID: {turnNode.BoardID}, Depth: {currentDepth}, From: {currentTurn.ChessBoard.BoardID}, ChildrenCount: {innerCount}, MainCount: {currentCurrentCount}", threadId);
                                    }
                                    else
                                    {
                                        stack.Push((possibleTurn, currentDepth - 1, 0));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return possibleMoves;
        }

        public SortedTupleBag<string, List<ChessPiece>> GetAllAttacks(ChessBoard chessBoard)
        {
            SortedTupleBag<string, List<ChessPiece>> results = new();

            foreach (var piece in chessBoard.GetActivePieces())
            {
                List<ChessPiece> attackedList = piece.GetAttackedPieces(chessBoard);
                if (attackedList.Count != 0)
                    results.Add(piece.GetPieceName(), attackedList);
            }

            return results;
        }

        public void GetAllAttacksForAllPossibleMovesForDepth(Turn turn, int depth)
        {
            Console.WriteLine($"Turn: {turn.TurnNumber}, {turn.TurnDescription}");

            SortedTupleBag<string, List<ChessPiece>> currentTurnResults = new();

            foreach (var piece in turn.ChessBoard.GetActivePieces())
            {
                List<ChessPiece> attackedList = piece.GetAttackedPieces(turn.ChessBoard);
                if (attackedList.Count != 0)
                    currentTurnResults.Add(piece.GetPieceName(), attackedList);
            }

            foreach (var attacked in currentTurnResults)
            {
                string threatsList = "[";
                foreach (ChessPiece threatenedPiece in attacked.Item2)
                {
                    threatsList += threatenedPiece.GetPieceName();
                    threatsList += ",";
                }
                threatsList += "]";
                Console.WriteLine($"Piece: {attacked.Item1}, Threatens: {threatsList}");
            }

            ulong count = 0;
            List<TurnNode> turnNodes = GenerateAllPossibleMovesTurnNode(turn, depth, ref count);

            foreach (var t in turnNodes)
            {
                {
                    Console.WriteLine($"Turn: {t.TurnNumber}, {t.TurnDescription}, From: {turn.TurnDescription}");

                    SortedTupleBag<string, List<ChessPiece>> innerResults = new();
                    ChessBoard cb = new(t.BoardID);

                    foreach (var piece in cb.GetActivePieces())
                    {
                        List<ChessPiece> attackedList = piece.GetAttackedPieces(cb);
                        if (attackedList.Count != 0)
                            innerResults.Add(piece.GetPieceName(), attackedList);
                    }

                    foreach (var attacked in innerResults)
                    {
                        string threatsList = "[";
                        foreach (ChessPiece threatenedPiece in attacked.Item2)
                        {
                            threatsList += threatenedPiece.GetPieceName();
                            threatsList += ",";
                        }
                        threatsList += "]";
                        Console.WriteLine($"Piece: {attacked.Item1}, Threatens: {threatsList}");

                    }
                }

                foreach (var tt in t.Children)
                {
                    Console.WriteLine($"Turn: {tt.TurnNumber}, {tt.TurnDescription}, From: {t.TurnDescription}");

                    SortedTupleBag<string, List<ChessPiece>> innerInnerResults = new();
                    ChessBoard ccbb = new ChessBoard(tt.BoardID);
                    foreach (var piece in ccbb.GetActivePieces())
                    {
                        List<ChessPiece> attackedList = piece.GetAttackedPieces(ccbb);
                        if (attackedList.Count != 0)
                            innerInnerResults.Add(piece.GetPieceName(), attackedList);
                    }

                    foreach (var attacked in innerInnerResults)
                    {
                        string threatsList = "[";
                        foreach (ChessPiece threatenedPiece in attacked.Item2)
                        {
                            threatsList += threatenedPiece.GetPieceName();
                            threatsList += ",";
                        }
                        threatsList += "]";
                        Console.WriteLine($"Piece: {attacked.Item1}, Threatens: {threatsList}");

                    }

                }
            }
        }
    }
}
