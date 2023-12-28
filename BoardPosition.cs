using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class BoardPosition
    {
        private VERTICAL v_value;
        private HORIZONTAL h_value;
        private string s_value;

        public enum VERTICAL { A = 7, B = 6, C = 5, D = 4, E = 3, F = 2, G = 1, H = 0 }
        public enum HORIZONTAL { ONE = 0, TWO = 1, THREE = 2, FOUR = 3, FIVE = 4, SIX = 5, SEVEN = 6, EIGHT = 7 }

        public BoardPosition(VERTICAL firstIndex, HORIZONTAL secondIndex)
        {
            v_value = firstIndex;
            h_value = secondIndex;

            switch (v_value)
            {
                case VERTICAL.A: s_value = "A"; break;
                case VERTICAL.B: s_value = "B"; break;
                case VERTICAL.C: s_value = "C"; break;
                case VERTICAL.D: s_value = "D"; break;
                case VERTICAL.E: s_value = "E"; break;
                case VERTICAL.F: s_value = "F"; break;
                case VERTICAL.G: s_value = "G"; break;
                case VERTICAL.H: s_value = "H"; break;
            }
            s_value = s_value + ((int)secondIndex + 1).ToString();
        }
        public BoardPosition(VERTICAL firstIndex, HORIZONTAL secondIndex, string stringValue)
        {
            v_value = firstIndex;
            h_value = secondIndex;
            s_value = stringValue;
        }

        public VERTICAL VerticalValue { get { return v_value; } }
        public int VerticalValueAsInt { get { return (int)v_value;} }
        public HORIZONTAL HorizontalValue { get { return h_value;} }
        public int HorizontalValueAsInt { get { return (int)h_value;} }
        public string StringValue { get { return s_value; } }

        public bool EqualTo(BoardPosition other)
        {
            return v_value == other.v_value && h_value == other.h_value;
        }
    }
}
