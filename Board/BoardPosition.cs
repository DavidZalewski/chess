using System.Linq.Expressions;

namespace Chess.Board
{
    public enum RANK { ONE = 7, TWO = 6, THREE = 5, FOUR = 4, FIVE = 3, SIX = 2, SEVEN = 1, EIGHT = 0 }
    public enum FILE { A = 0, B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7 }

    [Serializable]
    public record BoardPosition(RANK Rank, FILE File)
    {
        public string StringValue { get; init; } = $"{File}{8 - (int)Rank}";

        public BoardPosition(string pos) : this(GetRank(pos), GetFile(pos)) {}

        private static RANK GetRank(string pos)
        {
            try
            {
                int num = (int)char.GetNumericValue(pos[1]);
                return GetRank(num);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid argument provided for pos: " + pos, ex);
            }  
        }

        public static RANK GetRank(int num)
        {
            if (num >= 1 && num <= 8)
                return (RANK)(8 - num); // Calculate appropriate RANK value 
            else
                throw new ArgumentException("Invalid rank value. Rank must be between 1 and 8");
        }

        private static FILE GetFile(string pos)
        {
            try
            {
                char alpha = pos[0];
                return GetFile(alpha);
            }   
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid argument provided for pos: " + pos, ex);
            }
        }

        public static FILE GetFile(char alpha)
        {
            return alpha switch
            {
                'A' => FILE.A,
                'B' => FILE.B,
                'C' => FILE.C,
                'D' => FILE.D,
                'E' => FILE.E,
                'F' => FILE.F,
                'G' => FILE.G,
                'H' => FILE.H,
                _ => throw new ArgumentException("Invalid file character provided."),
            };
        }

        public int RankAsInt { get { return (int)Rank; } }
        public int FileAsInt { get { return (int)File; } }
    }
}
