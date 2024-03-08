using Chess.Board;
using Chess.Pieces;
using Chess.Services;
using static Chess.Pieces.ChessPiece;

namespace Tests.Services
{
    public class ChessPieceFactoryTests
    {
        [Test]
        public void Test_BuildWhitePawns_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateWhitePawns();

            int i = 1;
            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.WHITE));
                    Assert.That(piece.GetPiece(), Is.EqualTo(Piece.PAWN));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.TWO));
                    Assert.That(piece.GetStartingPosition().FileAsInt, Is.EqualTo(i - 1));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetRealValue(), Is.EqualTo(11));
                    Assert.That(piece.GetId(), Is.EqualTo(i));
                });
                i++;
            }
        }

        [Test]
        public void Test_BuildBlackPawns_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateBlackPawns();

            int i = 1;
            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.BLACK));
                    Assert.That(piece.GetPiece(), Is.EqualTo(Piece.PAWN));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.SEVEN));
                    Assert.That(piece.GetStartingPosition().FileAsInt, Is.EqualTo(i - 1));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetRealValue(), Is.EqualTo(21));
                    Assert.That(piece.GetId(), Is.EqualTo(i));
                });
                i++;
            }
        }

        [Test]
        public void Test_BuildWhiteKnights_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateWhiteKnights();

            Assert.That(pieces, Has.Count.EqualTo(2));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.WHITE));
                    Assert.That(piece.GetPiece(), Is.EqualTo(Piece.KNIGHT));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.ONE));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetRealValue(), Is.EqualTo(12));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetStartingPosition().File, Is.EqualTo(FILE.B));
                Assert.That(pieces[0].GetId(), Is.EqualTo(1));
                Assert.That(pieces[1].GetStartingPosition().File, Is.EqualTo(FILE.G));
                Assert.That(pieces[1].GetId(), Is.EqualTo(2));
            });

        }

        [Test]
        public void Test_BuildBlackKnights_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateBlackKnights();

            Assert.That(pieces, Has.Count.EqualTo(2));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.BLACK));
                    Assert.That(piece.GetPiece(), Is.EqualTo(Piece.KNIGHT));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.EIGHT));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetRealValue(), Is.EqualTo(22));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetStartingPosition().File, Is.EqualTo(FILE.B));
                Assert.That(pieces[0].GetId(), Is.EqualTo(1));
                Assert.That(pieces[1].GetStartingPosition().File, Is.EqualTo(FILE.G));
                Assert.That(pieces[1].GetId(), Is.EqualTo(2));
            });

        }

        [Test]
        public void Test_BuildWhiteBishops_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateWhiteBishops();

            Assert.That(pieces, Has.Count.EqualTo(2));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.WHITE));
                    Assert.That(piece.GetPiece(), Is.EqualTo(Piece.BISHOP));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.ONE));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetRealValue(), Is.EqualTo(13));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetStartingPosition().File, Is.EqualTo(FILE.C));
                Assert.That(pieces[0].GetId(), Is.EqualTo(1));
                Assert.That(pieces[1].GetStartingPosition().File, Is.EqualTo(FILE.F));
                Assert.That(pieces[1].GetId(), Is.EqualTo(2));
            });
        }

        [Test]
        public void Test_BuildBlackBishops_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateBlackBishops();

            Assert.That(pieces, Has.Count.EqualTo(2));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.BLACK));
                    Assert.That(piece.GetPiece(), Is.EqualTo(Piece.BISHOP));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.EIGHT));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetRealValue(), Is.EqualTo(23));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetStartingPosition().File, Is.EqualTo(FILE.C));
                Assert.That(pieces[0].GetId(), Is.EqualTo(1));
                Assert.That(pieces[1].GetStartingPosition().File, Is.EqualTo(FILE.F));
                Assert.That(pieces[1].GetId(), Is.EqualTo(2));
            });
        }

        [Test]
        public void Test_BuildWhiteRooks_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateWhiteRooks();

            Assert.That(pieces, Has.Count.EqualTo(2));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.WHITE));
                    Assert.That(piece.GetPiece(), Is.EqualTo(Piece.ROOK));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.ONE));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetRealValue(), Is.EqualTo(14));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetStartingPosition().File, Is.EqualTo(FILE.A));
                Assert.That(pieces[0].GetId(), Is.EqualTo(1));
                Assert.That(pieces[1].GetStartingPosition().File, Is.EqualTo(FILE.H));
                Assert.That(pieces[1].GetId(), Is.EqualTo(2));
            });
        }

        [Test]
        public void Test_BuildBlackRooks_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateBlackRooks();

            Assert.That(pieces, Has.Count.EqualTo(2));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.BLACK));
                    Assert.That(piece.GetPiece(), Is.EqualTo(Piece.ROOK));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.EIGHT));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetRealValue(), Is.EqualTo(24));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetStartingPosition().File, Is.EqualTo(FILE.A));
                Assert.That(pieces[0].GetId(), Is.EqualTo(1));
                Assert.That(pieces[1].GetStartingPosition().File, Is.EqualTo(FILE.H));
                Assert.That(pieces[1].GetId(), Is.EqualTo(2));
            });
        }

        [Test]
        public void Test_BuildWhiteQueenAndKing_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateWhiteQueenAndKing();

            Assert.That(pieces, Has.Count.EqualTo(2));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.WHITE));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.ONE));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetId(), Is.EqualTo(1));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetStartingPosition().File, Is.EqualTo(FILE.D));
                Assert.That(pieces[0].GetPiece(), Is.EqualTo(Piece.QUEEN));
                Assert.That(pieces[0].GetRealValue(), Is.EqualTo(15));
                Assert.That(pieces[1].GetStartingPosition().File, Is.EqualTo(FILE.E));
                Assert.That(pieces[1].GetPiece(), Is.EqualTo(Piece.KING));
                Assert.That(pieces[1].GetRealValue(), Is.EqualTo(16));
            });
        }

        [Test]
        public void Test_BuildBlackQueenAndKing_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateBlackQueenAndKing();

            Assert.That(pieces, Has.Count.EqualTo(2));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.BLACK));
                    Assert.That(piece.GetStartingPosition().Rank, Is.EqualTo(RANK.EIGHT));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                    Assert.That(piece.GetId(), Is.EqualTo(1));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetStartingPosition().File, Is.EqualTo(FILE.D));
                Assert.That(pieces[0].GetPiece(), Is.EqualTo(Piece.QUEEN));
                Assert.That(pieces[0].GetRealValue(), Is.EqualTo(25));
                Assert.That(pieces[1].GetStartingPosition().File, Is.EqualTo(FILE.E));
                Assert.That(pieces[1].GetPiece(), Is.EqualTo(Piece.KING));
                Assert.That(pieces[1].GetRealValue(), Is.EqualTo(26));
            });
        }

        [Test]
        public void Test_BuildWhitePieces_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateWhiteChessPieces();

            Assert.That(pieces, Has.Count.EqualTo(16));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.WHITE));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[0].GetRealValue(), Is.EqualTo(11));
                Assert.That(pieces[1].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[1].GetRealValue(), Is.EqualTo(11));
                Assert.That(pieces[2].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[2].GetRealValue(), Is.EqualTo(11));
                Assert.That(pieces[3].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[3].GetRealValue(), Is.EqualTo(11));
                Assert.That(pieces[4].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[4].GetRealValue(), Is.EqualTo(11));
                Assert.That(pieces[5].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[5].GetRealValue(), Is.EqualTo(11));
                Assert.That(pieces[6].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[6].GetRealValue(), Is.EqualTo(11));
                Assert.That(pieces[7].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[7].GetRealValue(), Is.EqualTo(11));
                Assert.That(pieces[8].GetPiece(), Is.EqualTo(Piece.KNIGHT));
                Assert.That(pieces[8].GetRealValue(), Is.EqualTo(12));
                Assert.That(pieces[9].GetPiece(), Is.EqualTo(Piece.KNIGHT));
                Assert.That(pieces[9].GetRealValue(), Is.EqualTo(12));
                Assert.That(pieces[10].GetPiece(), Is.EqualTo(Piece.BISHOP));
                Assert.That(pieces[10].GetRealValue(), Is.EqualTo(13));
                Assert.That(pieces[11].GetPiece(), Is.EqualTo(Piece.BISHOP));
                Assert.That(pieces[11].GetRealValue(), Is.EqualTo(13));
                Assert.That(pieces[12].GetPiece(), Is.EqualTo(Piece.ROOK));
                Assert.That(pieces[12].GetRealValue(), Is.EqualTo(14));
                Assert.That(pieces[13].GetPiece(), Is.EqualTo(Piece.ROOK));
                Assert.That(pieces[13].GetRealValue(), Is.EqualTo(14));
                Assert.That(pieces[14].GetPiece(), Is.EqualTo(Piece.QUEEN));
                Assert.That(pieces[14].GetRealValue(), Is.EqualTo(15));
                Assert.That(pieces[15].GetPiece(), Is.EqualTo(Piece.KING));
                Assert.That(pieces[15].GetRealValue(), Is.EqualTo(16));
            });
        }

        [Test]
        public void Test_BuildBlackPieces_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateBlackChessPieces();

            Assert.That(pieces, Has.Count.EqualTo(16));

            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(piece.GetColor(), Is.EqualTo(Color.BLACK));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                });
            }

            Assert.Multiple(() =>
            {
                Assert.That(pieces[0].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[0].GetRealValue(), Is.EqualTo(21));
                Assert.That(pieces[1].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[1].GetRealValue(), Is.EqualTo(21));
                Assert.That(pieces[2].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[2].GetRealValue(), Is.EqualTo(21));
                Assert.That(pieces[3].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[3].GetRealValue(), Is.EqualTo(21));
                Assert.That(pieces[4].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[4].GetRealValue(), Is.EqualTo(21));
                Assert.That(pieces[5].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[5].GetRealValue(), Is.EqualTo(21));
                Assert.That(pieces[6].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[6].GetRealValue(), Is.EqualTo(21));
                Assert.That(pieces[7].GetPiece(), Is.EqualTo(Piece.PAWN));
                Assert.That(pieces[7].GetRealValue(), Is.EqualTo(21));
                Assert.That(pieces[8].GetPiece(), Is.EqualTo(Piece.KNIGHT));
                Assert.That(pieces[8].GetRealValue(), Is.EqualTo(22));
                Assert.That(pieces[9].GetPiece(), Is.EqualTo(Piece.KNIGHT));
                Assert.That(pieces[9].GetRealValue(), Is.EqualTo(22));
                Assert.That(pieces[10].GetPiece(), Is.EqualTo(Piece.BISHOP));
                Assert.That(pieces[10].GetRealValue(), Is.EqualTo(23));
                Assert.That(pieces[11].GetPiece(), Is.EqualTo(Piece.BISHOP));
                Assert.That(pieces[11].GetRealValue(), Is.EqualTo(23));
                Assert.That(pieces[12].GetPiece(), Is.EqualTo(Piece.ROOK));
                Assert.That(pieces[12].GetRealValue(), Is.EqualTo(24));
                Assert.That(pieces[13].GetPiece(), Is.EqualTo(Piece.ROOK));
                Assert.That(pieces[13].GetRealValue(), Is.EqualTo(24));
                Assert.That(pieces[14].GetPiece(), Is.EqualTo(Piece.QUEEN));
                Assert.That(pieces[14].GetRealValue(), Is.EqualTo(25));
                Assert.That(pieces[15].GetPiece(), Is.EqualTo(Piece.KING));
                Assert.That(pieces[15].GetRealValue(), Is.EqualTo(26));
            });
        }

        [Test]
        public void Test_BuildPieces_Success()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateChessPieces();

            Assert.That(pieces, Has.Count.EqualTo(32));

            int i = 0;
            foreach (ChessPiece piece in pieces)
            {
                Assert.Multiple(() =>
                {
                    if (i < 16)
                        Assert.That(piece.GetColor(), Is.EqualTo(Color.WHITE));
                    else
                        Assert.That(piece.GetColor(), Is.EqualTo(Color.BLACK));
                    Assert.That(piece.GetCurrentPosition().Rank, Is.EqualTo(piece.GetStartingPosition().Rank));
                    Assert.That(piece.GetCurrentPosition().File, Is.EqualTo(piece.GetStartingPosition().File));
                });
                i++;
            }

        }

        [TestCase(0, typeof(NoPiece))]
        [TestCase(11, typeof(ChessPieceWhitePawn))]
        [TestCase(12, typeof(ChessPieceKnight))]
        [TestCase(13, typeof(ChessPieceBishop))]
        [TestCase(14, typeof(ChessPieceRook))]
        [TestCase(15, typeof(ChessPieceQueen))]
        [TestCase(16, typeof(ChessPieceKing))]
        [TestCase(21, typeof(ChessPieceWhitePawn))]
        [TestCase(22, typeof(ChessPieceKnight))]
        [TestCase(23, typeof(ChessPieceBishop))]
        [TestCase(24, typeof(ChessPieceRook))]
        [TestCase(25, typeof(ChessPieceQueen))]
        [TestCase(26, typeof(ChessPieceKing))]
        public void CreatePieceFromInt_ValidValue_ReturnsCorrectPiece(int value, Type expectedType)
        {
            // Arrange
            var position = new BoardPosition(RANK.ONE, FILE.A); // Example position

            // Act
            var piece = ChessPieceFactory.CreatePieceFromInt(position, value);

            // Assert
            Assert.That(piece.GetType(), Is.EqualTo(expectedType));
            Assert.That(piece.GetColor(), Is.EqualTo((value > 20) ? ChessPiece.Color.BLACK : ChessPiece.Color.WHITE));
            Assert.That(piece.GetPiece(), Is.EqualTo((ChessPiece.Piece)(value % 10))); // Extract specific piece type
            Assert.That(piece.GetCurrentPosition(), Is.EqualTo(position));
            Assert.That(piece.GetRealValue(), Is.EqualTo(value));
        }

        [Test]
        public void CreatePieceFromInt_InvalidValue_ThrowsException()
        {
            // Arrange
            var position = new BoardPosition(RANK.ONE, FILE.A);
            int invalidValue = 99;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ChessPieceFactory.CreatePieceFromInt(position, invalidValue));
        }
    }
}