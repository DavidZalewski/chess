using Chess.Board;
using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Pieces
{
    internal class NoPieceTests
    {
        [Test]
        public void NoPieceIsSingleton()
        {
            var noPiece1 = NoPiece.Instance;
            var noPiece2 = NoPiece.Instance;

            Assert.Equals(noPiece1, noPiece2); // Check if they refer to the same object
        }

        [Test]
        public void Square_CanBeConstructed()
        {
            var square = new Square();
            Assert.That(() => square is not null);
        }

        [Test]
        public void Square_HasCorrectDefaultPiece()
        {
            var square = new Square();
            Assert.Equals(NoPiece.Instance, square.Piece);
        }
    }
}
