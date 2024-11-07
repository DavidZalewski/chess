using Chess.Attributes;
using Chess.Globals;
using Chess.Pieces;

namespace Chess.GameState
{
    public class PromotionTracker
    {
        private Dictionary<string, int> promotionCounters;

        public PromotionTracker()
        {
            StaticLogger.Trace();
            promotionCounters = new Dictionary<string, int>
            {
                { "WQ", 2 }, { "BQ", 2 }, // Queens start at 2 for both White and Black
                { "WR", 3 }, { "BR", 3 }, // Rooks start at 3
                { "WK", 3 }, { "BK", 3 }, // Knights start at 3
                { "WB", 3 }, { "BB", 3 }  // Bishops start at 3
            };
        }

        // Get and increment the next ID for a promoted piece
        public int GetNextID(ChessPiece.Color color, string pieceType)
        {
            StaticLogger.Trace();
            string key = $"{(color == ChessPiece.Color.WHITE ? "W" : "B")}{pieceType}";
            int id = promotionCounters[key];
            promotionCounters[key]++;
            return id;
        }
    }
}
