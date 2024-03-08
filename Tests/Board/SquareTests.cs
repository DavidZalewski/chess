using Chess.Board;
using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Board
{
    internal class SquareTests
    {
        [Test]
        public void SquareIsInitializedCorrectly()
        {
            var square = new Square()
            {
                Position = new BoardPosition(RANK.FOUR, FILE.F),
                Piece = NoPiece.Instance
            };

            Assert.Equals(RANK.FOUR, square.Position.Rank);
            Assert.Equals(FILE.F, square.Position.File);
            Assert.Equals(NoPiece.Instance, square.Piece);
        }
    }
}
