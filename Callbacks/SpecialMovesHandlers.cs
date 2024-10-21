using Chess.Attributes;
using Chess.Board;
using Chess.GameState;
using Chess.Globals;
using Chess.Pieces;
using NUnit.Framework;

namespace Chess.Callbacks
{
    [Serializable]
    internal class SpecialMovesHandlers
    {
        public static Action<string>? GetActionFromResult;
        public static Func<string>? PawnPromotionPromptUser;
        public static bool ByPassPawnPromotionPromptUser { get; set; } = false;
        public static PromotionTracker promotionTracker = new PromotionTracker();

        public static bool DoCastleMove(ChessBoard cb, BoardPosition bp, ChessPiece king)
        {
            StaticLogger.Trace();
            string action = "";
            int hv = bp.FileAsInt;
            int vv = bp.RankAsInt;

            ChessPiece? rook = cb.GetActivePieces().Find(p => p.GetCurrentPosition() == bp);

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
                    action = "Castle (Queen Side)";
                    kh -= 2;
                    rh += 3;
                }
                else if (d == -3)
                {
                    // king side castle
                    // k goes +2 squares
                    // r goes -2 squares
                    action = "Castle (King Side)";
                    kh += 2;
                    rh -= 2;
                }
                else
                {
                    cb.GenerateBoardID();
                    throw new Exception("Unexpected Horizontal Distance Found when castling. Are you sure this is a valid castle? BoardID: " + cb.BoardID);
                }

                BoardPosition kingLastPosition = king.GetCurrentPosition();
                BoardPosition rookLastPosition = rook.GetCurrentPosition();

                // set board manually
                cb.SetBoardValue(kingLastPosition, 0);
                cb.SetBoardValue(rookLastPosition, 0);

                cb.SetPieceAtPosition(new(v, kh), king);
                cb.SetPieceAtPosition(new(v, rh), rook);
            }

            GetActionFromResult?.Invoke(action);

            return true;
        }

        public static bool IsEnPassantMove(ChessBoard chessBoard, BoardPosition boardPosition, ChessPiece pawnAttemptingEnPassant)
        {
            StaticLogger.Trace();
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
            BoardPosition bpl = pawnPos.Left();
            BoardPosition bpr = pawnPos.Right();
            List<BoardPosition> boardPositionsToCheck = new();

            if (bpl != null)
                boardPositionsToCheck.Add(bpl);
            if (bpr != null)
                boardPositionsToCheck.Add(bpr);

            foreach (BoardPosition bpToCheck in boardPositionsToCheck)
            {
                // Is there an opponent piece at this position?
                if (chessBoard.IsPieceAtPosition(bpToCheck, opponentColor))
                {
                    ChessPiece? opponentPiece = chessBoard.GetActivePieces().Find((ChessPiece cp) => cp.GetCurrentPosition() == bpToCheck);
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

        [TestNeeded]
        internal static void PawnPromotion(ChessBoard board, BoardPosition position, ChessPiece piece)
        {
            StaticLogger.Trace();
            string choice = "Q";
            if (PawnPromotionPromptUser != null && !ByPassPawnPromotionPromptUser)
                choice = PawnPromotionPromptUser.Invoke();

            Func<string, ChessPiece> switchReturnPiece = (string chosenPiece) =>
            {
                int newPieceID;
                // This fixes the issue with KingCheckService incrementing this counter with potential future moves
                if (ByPassPawnPromotionPromptUser)
                {
                    Console.WriteLine($"PawnPromotion: ByPassPawnPromotionPromptUser set - use 0 for newPieceID");
                    newPieceID = 0;
                }
                else
                {
                    newPieceID = promotionTracker.GetNextID(piece.GetColor(), chosenPiece.ToUpper());
                }
                // TODO: CheckMate Service when creating potential future moves, inadvertently creates pawn promotions, incrementing this counter unexpectedly
                Console.WriteLine($"PawnPromotion: newPieceID = {newPieceID} , choice: {choice}");
                return chosenPiece switch
                {
                    "q" => new ChessPieceQueen(piece.GetColor(), newPieceID, position),
                    "r" => new ChessPieceRook(piece.GetColor(), newPieceID, position),
                    "k" => new ChessPieceKnight(piece.GetColor(), newPieceID, position),
                    "b" => new ChessPieceBishop(piece.GetColor(), newPieceID, position),
                    _ => new ChessPieceQueen(piece.GetColor(), newPieceID, position),
                };
            };

            ChessPiece newPiece = switchReturnPiece(choice);
            board.AddPiece(newPiece);
        }
    }
}
