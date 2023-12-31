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

        // the values for VERTICAL enum are flipped - index 0 should be row 8 on chessboard, but that complicates the enum
        // calculations too much, and its an internal implementation issue at this point
        // if there needs to be yet another translation done, can be done in another class
        public enum VERTICAL { ONE = 0, TWO = 1, THREE = 2, FOUR = 3, FIVE = 4, SIX = 5, SEVEN = 6, EIGHT = 7 } 
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
            s_value = s_value + ((int)firstIndex + 1).ToString();
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
