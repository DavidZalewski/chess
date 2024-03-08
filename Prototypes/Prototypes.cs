using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chess.Prototypes
{
    namespace ModelA
    {
        // Base class -  Abstract for flexibility
        public abstract class ChessPiece
        {
            // Example property - you can add more like Color, etc.
            public string Name { get; protected set; }
        }

        public class NoPiece : ChessPiece
        {
            private static NoPiece _instance;
            public static NoPiece Instance => _instance ??= new NoPiece();
            private NoPiece() { }  // Private constructor for singleton
        }

        public class Square
        {
            public BoardPosition Position { get; set; }
            public ChessPiece Piece { get; set; } = NoPiece.Instance;
        }

        public class ChessBoard
        {
            public Square[,] Board { get; set; }

            public ChessBoard()
            {
                Board = new Square[8, 8];
                InitializeBoard();
            }

            private void InitializeBoard()
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        Board[row, col] = new Square()
                        {
                            Position = new BoardPosition(row, col),
                            Piece = NoPiece.Instance
                        };
                    }
                }
            }
        }

        // Simple struct for board positions
        public struct BoardPosition
        {
            public int Row { get; }
            public int Column { get; }

            public BoardPosition(int row, int column)
            {
                Row = row;
                Column = column;
            }
        }

        // Unit Test Class
        [TestFixture]
        public class ChessPieceTests
        {
            [Test]
            public void NoPiece_IsSingleton()
            {
                var piece1 = NoPiece.Instance;
                var piece2 = NoPiece.Instance;

                Assert.That(piece1.Equals(piece2));
            }
        }

        [TestFixture]
        public class SquareTests
        {
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
                Assert.That(NoPiece.Instance.Equals(square.Piece));
            }
        }
    }



    namespace ModelB
    {
        public struct BoardPosition
        {
            public int Row { get; }
            public int Column { get; }

            public BoardPosition(int row, int column)
            {
                Row = row;
                Column = column;
            }
        }

        public class Square
        {
            public BoardPosition Position { get; set; }
            public ChessPiece Piece { get; set; }
        }

        public class ChessBoard
        {
            public Square[,] Board { get; set; } = new Square[8,8];
        }

        public class ChessPiece
        {
            // Base properties and methods for chess pieces
        }

        public class NoPiece : ChessPiece
        {
            private static NoPiece _instance;
            public static NoPiece Instance => _instance ??= new NoPiece();
            private NoPiece() { }  // Private constructor for singleton
        }

        [TestFixture]
        public class ChessPieceTests
        {
            [Test]
            public void NoPiece_IsSingleton()
            {
                var piece1 = NoPiece.Instance;
                var piece2 = NoPiece.Instance;

                Assert.That(piece1.Equals(piece2));
            }
        }

        [TestFixture]
        public class SquareTests
        {
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
                Assert.That(NoPiece.Instance.Equals(square.Piece));
            }
        }
    }
}
