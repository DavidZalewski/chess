namespace Chess.Board
{
    public enum RANK { ONE = 7, TWO = 6, THREE = 5, FOUR = 4, FIVE = 3, SIX = 2, SEVEN = 1, EIGHT = 0 }
    public enum FILE { A = 0, B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7 }

    [Serializable]
    public record BoardPosition(RANK Rank, FILE File)
    {
        public string StringValue { get; init; } = $"{File}{8 - (int)Rank}";

        public BoardPosition(string pos) : this(GetRank(pos), GetFile(pos)) { }

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

        public BoardPosition Left()
        {
            if (File == FILE.A) return null; // If the file is already at the leftmost edge, return null
            return new BoardPosition(Rank, (FILE)(FileAsInt - 1)); // Subtract 1 from the file
        }

        public BoardPosition Right()
        {
            if (File == FILE.H) return null; // If the file is already at the rightmost edge, return null
            return new BoardPosition(Rank, (FILE)(FileAsInt + 1)); // Add 1 to the file
        }

        public BoardPosition Up()
        {
            if (Rank == RANK.ONE) return null; // If the rank is already at the topmost edge, return null
            return new BoardPosition(Rank - 1, File); // Subtract 1 from the rank
        }

        public BoardPosition Down()
        {
            if (Rank == RANK.EIGHT) return null; // If the rank is already at the bottommost edge, return null
            return new BoardPosition(Rank + 1, File); // Add 1 to the rank
        }

        public BoardPosition Offset(int rankOffset, int fileOffset)
        {
            int newRank = RankAsInt - rankOffset; // need to invert this to make it intuitive
            int newFile = FileAsInt + fileOffset;

            if (newRank < 0 || newRank > 8 || newFile < 0 || newFile > 8) return null; // If the new rank or file is out of bounds, return null

            return new BoardPosition((RANK)newRank, (FILE)newFile); // Create a new BoardPosition with the new rank and file
        }

        public bool IsDiagonal(BoardPosition other)
        {
            int rankDiff = Math.Abs((int)Rank - (int)other.Rank);
            int fileDiff = Math.Abs((int)File - (int)other.File);

            return rankDiff == fileDiff; // If the absolute difference in rank and file is equal, then the positions are diagonal to each other
        }

        public bool IsOnSameFile(BoardPosition other)
        {
            return File == other.File; // If the files are equal, then the positions are on the same file
        }

        public bool IsOnSameRank(BoardPosition other)
        {
            return Rank == other.Rank; // If the ranks are equal, then the positions are on the same rank
        }
    }
}
