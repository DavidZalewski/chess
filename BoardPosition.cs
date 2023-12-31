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
