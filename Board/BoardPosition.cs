using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Board
{
    [Serializable]
    public class BoardPosition
    {
        private VERTICAL v_value;
        private HORIZONTAL h_value;
        private string s_value = "";

        public enum VERTICAL { ONE = 7, TWO = 6, THREE = 5, FOUR = 4, FIVE = 3, SIX = 2, SEVEN = 1, EIGHT = 0 }
        public enum HORIZONTAL { A = 0, B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7 }

        public BoardPosition(VERTICAL firstIndex, HORIZONTAL secondIndex)
        {
            v_value = firstIndex;
            h_value = secondIndex;

            switch (h_value)
            {
                case HORIZONTAL.A: s_value = "A"; break;
                case HORIZONTAL.B: s_value = "B"; break;
                case HORIZONTAL.C: s_value = "C"; break;
                case HORIZONTAL.D: s_value = "D"; break;
                case HORIZONTAL.E: s_value = "E"; break;
                case HORIZONTAL.F: s_value = "F"; break;
                case HORIZONTAL.G: s_value = "G"; break;
                case HORIZONTAL.H: s_value = "H"; break;
            }
            switch (v_value)
            {
                case VERTICAL.EIGHT: s_value += "8"; break;
                case VERTICAL.SEVEN: s_value += "7"; break;
                case VERTICAL.SIX: s_value += "6"; break;
                case VERTICAL.FIVE: s_value += "5"; break;
                case VERTICAL.FOUR: s_value += "4"; break;
                case VERTICAL.THREE: s_value += "3"; break;
                case VERTICAL.TWO: s_value += "2"; break;
                case VERTICAL.ONE: s_value += "1"; break;
            }
        }
        public BoardPosition(VERTICAL firstIndex, HORIZONTAL secondIndex, string stringValue)
        {
            v_value = firstIndex;
            h_value = secondIndex;
            s_value = stringValue;
        }

        public BoardPosition(BoardPosition other)
        {
            v_value = other.v_value;
            h_value = other.h_value;
            s_value = other.s_value;
        }

        public BoardPosition(string pos)
        {
            if (pos != null && pos.Length == 2)
            {
                char alpha = pos[0];
                try
                {
                    int num = (int)char.GetNumericValue(pos[1]);
                    BoardPosition bp = GetBoardPosition(alpha, num);
                    v_value = bp.v_value;
                    h_value = bp.h_value;
                    s_value = bp.s_value;
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid argument provided for pos: " + pos, ex);
                }
            }
            else
            {
                throw new Exception("Invalid argument provided for pos (value is null or length != 2: " + pos);
            }
        }

        private BoardPosition GetBoardPosition(char alpha, int num)
        {
            int firstIndex = -1;
            int secondIndex = -1;
            switch (alpha)
            {
                case 'A': secondIndex = 0; break;
                case 'B': secondIndex = 1; break;
                case 'C': secondIndex = 2; break;
                case 'D': secondIndex = 3; break;
                case 'E': secondIndex = 4; break;
                case 'F': secondIndex = 5; break;
                case 'G': secondIndex = 6; break;
                case 'H': secondIndex = 7; break;
                default: throw new Exception("Invalid Alpha Position provided");
            }

            switch (num)
            {
                case 1: firstIndex = 7; break;
                case 2: firstIndex = 6; break;
                case 3: firstIndex = 5; break;
                case 4: firstIndex = 4; break;
                case 5: firstIndex = 3; break;
                case 6: firstIndex = 2; break;
                case 7: firstIndex = 1; break;
                case 8: firstIndex = 0; break;
                default: throw new Exception("Invalid Num Position provided");
            }

            return new BoardPosition((VERTICAL)firstIndex, (HORIZONTAL)secondIndex, new string(alpha + num.ToString()));
        }


        public VERTICAL VerticalValue { get { return v_value; } }
        public int VerticalValueAsInt { get { return (int)v_value; } }
        public HORIZONTAL HorizontalValue { get { return h_value; } }
        public int HorizontalValueAsInt { get { return (int)h_value; } }
        public string StringValue { get { return s_value; } }

        public bool EqualTo(BoardPosition other)
        {
            return v_value == other.v_value && h_value == other.h_value;
        }
    }
}
