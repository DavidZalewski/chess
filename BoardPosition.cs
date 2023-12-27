using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class BoardPosition
    {
        private int _firstIndex;
        private int _secondIndex;

        public BoardPosition(int firstIndex, int secondIndex)
        {
            _firstIndex = firstIndex;
            _secondIndex = secondIndex;
        }

        public int FirstIndex { get { return _firstIndex;} }

        public int SecondIndex { get { return _secondIndex;} }
    }
}
