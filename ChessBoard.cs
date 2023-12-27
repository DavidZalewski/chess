﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessBoard
    {
        // the last inner array is assumed to be the starting position
        // looking at diagrams where the board is labelled, white is always on the bottom
        // therefore first value in array [0,0] is A8
        // A1 is found at [7,0]
        private int[,] _board = new int[8, 8] 
        {
            { 0 /*A8*/, 0, 0, 0, 0, 0, 0, 0 /*H8*/},
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0 /*A1*/, 0, 0, 0, 0, 0, 0, 0 /*H1*/},
        };

        public ChessBoard() { }

        public int[,] GetBoard() { return _board; }

        // not sure yet if this method needs to exist here, or be moved into another class, but for now keep it here
        public KeyValuePair<int, int> GetBoardPosition(char alpha, int num) {
            int firstIndex = -1;
            switch (alpha)
            {
                case 'A': firstIndex = 7; break;
                case 'B': firstIndex = 6; break;
                case 'C': firstIndex = 5; break;
                case 'D': firstIndex = 4; break;
                case 'E': firstIndex = 3; break;
                case 'F': firstIndex = 2; break;
                case 'G': firstIndex = 1; break;
                case 'H': firstIndex = 0; break;
                default: throw new Exception("Invalid Alpha Position provided");
            }

            if (num >= 1 && num <= 8)
            {
                return new KeyValuePair<int, int>(firstIndex, num - 1);
            }
            else
            {
                throw new Exception("Invalid Number Position provided");
            }
        }
    }
}