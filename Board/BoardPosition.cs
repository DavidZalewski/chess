namespace Chess.Board
{
    public enum RANK { ONE = 7, TWO = 6, THREE = 5, FOUR = 4, FIVE = 3, SIX = 2, SEVEN = 1, EIGHT = 0 }
    public enum FILE { A = 0, B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7 }

    [Serializable]
    public sealed class BoardPosition
    {
        public RANK Rank { get; }
        public FILE File { get; }
        public string StringValue { get; }

        public BoardPosition(RANK rank, FILE file)
        {
            Rank = rank;
            File = file;
            StringValue = $"{file}{8 - (int)rank}";
        }

        public BoardPosition(string pos)
        {
            if (pos != null && pos.Length == 2)
            {
                char alpha = pos[0];
                try
                {
                    int num = (int)char.GetNumericValue(pos[1]);
                    Rank = GetRank(num);
                    File = GetFile(alpha);
                    StringValue = $"{File}{8 - (int)Rank}";
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Invalid argument provided for pos: " + pos, ex);
                }
            }
            else
            {
                throw new ArgumentException("Invalid argument provided for pos (value is null or length != 2: " + pos);
            }
        }

        public BoardPosition(BoardPosition other)
        {
            Rank = other.Rank;
            File = other.File;
            StringValue = other.StringValue;
        }

        public static RANK GetRank(int num)
        {
            if (num >= 1 && num <= 8)
            {
                return (RANK)(8 - num); // Calculate appropriate RANK value 
            }
            else
            {
                throw new ArgumentException("Invalid rank value. Rank must be between 1 and 8");
            }
        }

        public static FILE GetFile(char alpha)
        {
            switch (alpha)
            {
                case 'A': return FILE.A;
                case 'B': return FILE.B;
                case 'C': return FILE.C;
                case 'D': return FILE.D;
                case 'E': return FILE.E;
                case 'F': return FILE.F;
                case 'G': return FILE.G;
                case 'H': return FILE.H;
                default: throw new ArgumentException("Invalid file character provided.");
            }
        }

        public static bool operator ==(BoardPosition a, BoardPosition b)
        {
            // Check for nulls
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }

            return a.Equals(b);
        }

        public static bool operator !=(BoardPosition a, BoardPosition b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is BoardPosition other)
            {
                return Rank == other.Rank && File == other.File;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Rank, File);
        }

        public int RankAsInt { get { return (int)Rank; } }
        public int FileAsInt { get { return (int)File; } }

        public bool EqualTo(BoardPosition other)
        {
            return Rank == other.Rank && File == other.File;
        }
    }
}
