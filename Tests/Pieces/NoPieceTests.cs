using Chess.Board;
using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Pieces
{
    [Category("CORE")]
    internal class NoPieceTests : TestBase
    {
        [Test]
        public void NoPieceIsSingleton()
        {
            var noPiece1 = NoPiece.Instance;
            var noPiece2 = NoPiece.Instance;

            Assert.That(ReferenceEquals(noPiece1, noPiece2), Is.True); // Check if they refer to the same object
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
            Assert.That(NoPiece.Instance == square.Piece);
        }
    }
}
