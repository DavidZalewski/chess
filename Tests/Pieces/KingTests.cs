﻿using Chess.Board;
using Chess.Pieces;
using Chess.Services;
using static Chess.Pieces.ChessPiece;

namespace Tests.Pieces
{
    [Category("CORE")]
    public class KingTests : TestBase
    {
        private readonly BoardPosition e1 = new(RANK.ONE, FILE.E);
        private readonly BoardPosition e8 = new(RANK.EIGHT, FILE.E);

        [Test]
        public void ConstructWhiteKing_Success()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(RANK.ONE, FILE.E));

            Assert.That(whiteKingPiece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(whiteKingPiece.GetCurrentPosition(), Is.EqualTo(e1));
                Assert.That(whiteKingPiece.GetRealValue(), Is.EqualTo(16));
                Assert.That(whiteKingPiece.GetId(), Is.EqualTo(1));
                Assert.That(whiteKingPiece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(whiteKingPiece.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(whiteKingPiece.GetStartingPosition() == whiteKingPiece.GetCurrentPosition(), Is.True);
            });
        }

        [Test]
        public void CloneWhiteKing_Success()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(RANK.ONE, FILE.E));
            ChessPiece clone = whiteKingPiece.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, whiteKingPiece), Is.False);
                Assert.That(clone.GetCurrentPosition(), Is.EqualTo(e1));
                Assert.That(clone.GetRealValue(), Is.EqualTo(16));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(clone.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(clone.GetStartingPosition() == whiteKingPiece.GetCurrentPosition(), Is.True);
            });
        }

        [Test]
        public void ConstructBlackKing_Success()
        {
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(RANK.EIGHT, FILE.E));
            Assert.That(blackKingPiece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(blackKingPiece.GetCurrentPosition() == e8, Is.True);
                Assert.That(blackKingPiece.GetRealValue(), Is.EqualTo(26));
                Assert.That(blackKingPiece.GetId(), Is.EqualTo(1));
                Assert.That(blackKingPiece.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(blackKingPiece.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(blackKingPiece.GetStartingPosition() == blackKingPiece.GetCurrentPosition(), Is.True);
            });
        }

        [Test]
        public void CloneBlackKing_Success()
        {
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(RANK.EIGHT, FILE.E));
            ChessPiece clone = blackKingPiece.Clone();
            
            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.Null);
                Assert.That(ReferenceEquals(clone, blackKingPiece), Is.False);
                Assert.That(clone.GetCurrentPosition() == e8, Is.True);
                Assert.That(clone.GetRealValue(), Is.EqualTo(26));
                Assert.That(clone.GetId(), Is.EqualTo(1));
                Assert.That(clone.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(clone.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(clone.GetStartingPosition() == blackKingPiece.GetCurrentPosition(), Is.True);
            });
        }

        [Test(Description = "Tests that the white king has no valid moves it can make at the start of the game")]
        public void WhiteKing_IsValidMove_NoValidMovesFromStart()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
            ChessPiece whiteKingPiece = chessPieces.Find((ChessPiece cp) => cp.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                                            cp.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);
            List<BoardPosition> possibleMoves = new();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possibleMoves.Add(new BoardPosition((RANK)i, (FILE)j));
                }
            }

            Assert.Multiple(() =>
            {
                Assert.That(whiteKingPiece is not null);
                foreach (BoardPosition possibleMove in possibleMoves)
                {
                    Assert.That(whiteKingPiece.IsValidMove(chessBoard, possibleMove), Is.False, possibleMove.StringValue);
                }
            });
        }

        [Test(Description = "Tests that the black king has no valid moves it can make at the start of the game")]
        public void BlackKing_IsValidMove_NoValidMovesFromStart()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
            ChessPiece blackKingPiece = chessPieces.Find((ChessPiece cp) => cp.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                                            cp.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);
            List<BoardPosition> possibleMoves = new();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possibleMoves.Add(new BoardPosition((RANK)i, (FILE)j));
                }
            }

            Assert.Multiple(() =>
            {
                Assert.That(blackKingPiece is not null);
                foreach (BoardPosition possibleMove in possibleMoves)
                {
                    Assert.That(blackKingPiece.IsValidMove(chessBoard, possibleMove), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white or black king on empty board starting on D4 can move to all valid positions")]
        public void King_IsValidMove_ValidMovesFromD4()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(RANK.FOUR, FILE.D));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(RANK.FOUR, FILE.D));
            ChessBoard chessBoard = new(); // blank chess board with 2 kings on it
            List<BoardPosition> validMoves = new()
            {
                new BoardPosition(RANK.THREE, FILE.D), // D3
                new BoardPosition(RANK.THREE, FILE.C), // C3
                new BoardPosition(RANK.THREE, FILE.E), // E3
                new BoardPosition(RANK.FOUR, FILE.C), // C4
                new BoardPosition(RANK.FOUR, FILE.E), // E4
                new BoardPosition(RANK.FIVE, FILE.D), // D5
                new BoardPosition(RANK.FIVE, FILE.C), // C5
                new BoardPosition(RANK.FIVE, FILE.E), // E5
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in validMoves)
                {
                    Assert.That(whiteKingPiece.IsValidMove(chessBoard, boardPosition), Is.True);
                    Assert.That(blackKingPiece.IsValidMove(chessBoard, boardPosition), Is.True);
                }
            });
        }

        [Test(Description = "Tests that white or black king on empty board starting on D4 cannot move to these positions")]
        public void King_IsValidMove_InvalidMovesFromD4()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(RANK.FOUR, FILE.D));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(RANK.FOUR, FILE.D));
            ChessBoard chessBoard = new();
            List<BoardPosition> validMoves = new()
            {
                new BoardPosition(RANK.THREE, FILE.D), // D3
                new BoardPosition(RANK.THREE, FILE.C), // C3
                new BoardPosition(RANK.THREE, FILE.E), // E3
                new BoardPosition(RANK.FOUR, FILE.C), // C4
                new BoardPosition(RANK.FOUR, FILE.E), // E4
                new BoardPosition(RANK.FIVE, FILE.D), // D5
                new BoardPosition(RANK.FIVE, FILE.C), // C5
                new BoardPosition(RANK.FIVE, FILE.E), // E5
            };
            List<BoardPosition> possibleMoves = new();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possibleMoves.Add(new BoardPosition((RANK)i, (FILE)j));
                }
            }

            Assert.Multiple(() =>
            {
                int assertsInvoked = 0;
                foreach (BoardPosition position in possibleMoves)
                {
                    if (!validMoves.Any(vm => vm == position))
                    {
                        Assert.That(whiteKingPiece.IsValidMove(chessBoard, position), Is.False, $"White King on D4 should not be able to move to {position.StringValue}");
                        Assert.That(blackKingPiece.IsValidMove(chessBoard, position), Is.False, $"Black King on D4 should not be able to move to {position.StringValue}");
                        assertsInvoked++;
                    }
                }
                Assert.That(assertsInvoked, Is.GreaterThan(0));
            });
        }

        [TestCase("B2")]
        [TestCase("B3")]
        [TestCase("B4")]
        [TestCase("B5")]
        [TestCase("B6")]
        [TestCase("B7")]
        [TestCase("G2")]
        [TestCase("G3")]
        [TestCase("G4")]
        [TestCase("G5")]
        [TestCase("G6")]
        [TestCase("G7")]
        public void King_GetValidSquares_AlwaysReturns8_FromThesePositions(string startPos)
        {
            BoardPosition startingPosition = new(startPos);
            ChessPiece piece = new ChessPieceKing(ChessPiece.Color.WHITE, startingPosition);
            ChessBoard chessBoard = new();
            chessBoard.AddPiece(piece);

            List<BoardPosition> results = piece.GetPossiblePositions(chessBoard);

            Assert.That(results.Count, Is.EqualTo(8));
        }

        [TestCase("A1")]
        [TestCase("A8")]
        [TestCase("H1")]
        [TestCase("H8")]
        public void King_GetValidSquares_AlwaysReturns3_FromThesePositions(string startPos)
        {
            BoardPosition startingPosition = new(startPos);
            ChessPiece piece = new ChessPieceKing(ChessPiece.Color.WHITE, startingPosition);
            ChessBoard chessBoard = new();
            chessBoard.AddPiece(piece);

            List<BoardPosition> results = piece.GetPossiblePositions(chessBoard);

            Assert.That(results.Count, Is.EqualTo(3));
        }

        [Test]
        public void King_GetAttackedPieces_Returns_Fork()
        {
            ChessPiece blackKing = new ChessPieceKing(Color.BLACK, new("D5"));
            ChessPiece whiteRook1 = new ChessPieceRook(Color.WHITE, 1, new("C3"));
            ChessPiece whiteRook2 = new ChessPieceRook(Color.WHITE, 2, new("E3"));
            ChessPiece whiteKnight = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new("D3"));
            ChessBoard chessBoard = new();
            chessBoard.AddPiece(blackKing);
            chessBoard.AddPiece(whiteRook1);
            chessBoard.AddPiece(whiteRook2);
            chessBoard.AddPiece(whiteKnight);

            blackKing.Move(chessBoard, new("D4"));

            List<ChessPiece> kingAttacking = blackKing.GetAttackedPieces(chessBoard);
            List<ChessPiece> whiteAttacking = whiteRook1.GetAttackedPieces(chessBoard);
            whiteAttacking.AddRange(whiteRook2.GetAttackedPieces(chessBoard));
            whiteAttacking.AddRange(whiteKnight.GetAttackedPieces(chessBoard));

            Assert.Multiple(() =>
            {
                Assert.That(kingAttacking.Count == 3, "Expected Black King to fork all White Pieces");
                Assert.That(whiteAttacking.Count == 0, "None of White Pieces can attack this King");
                Assert.That(kingAttacking[2] is ChessPieceKnight, "Expected the Knight to be 3rd element");
                Assert.That(kingAttacking[1] is ChessPieceRook, "Expected the Rook to be 2nd element");
                Assert.That(kingAttacking[0] is ChessPieceRook, "Expected the Rook to be 1st element");
            });
        }


    }
}
