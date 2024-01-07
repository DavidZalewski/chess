using Chess.Board;
using Chess.Pieces;
using Chess.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Pieces
{
    public class ChessPieceSpecialMovesTests
    {

        [Test]
        public void Test_Castling_CannotCastleAtStart()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessPiece whiteRook1 = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.ROOK) && 
                                                              piece.GetId().Equals(1));
            ChessPiece whiteRook2 = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.ROOK) &&
                                                              piece.GetId().Equals(2));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to A8 is interpreted as Castle
        }

        [Test]
        public void Test_Castling_CannotCastle_KingMoved()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            // remove all back pieces except for rook and king
            // move the king somewhere
            // move the king back
            // try to castle
            // should fail

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to A8 is interpreted as Castle
            Assert.Fail("Not implemented!");

        }

        [Test]
        public void Test_Castling_CannotCastle_KingWasChecked()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            // remove all back pieces except for rook and king
            // move the king somewhere
            // move the king back
            // try to castle
            // should fail

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to A8 is interpreted as Castle
            Assert.Fail("Not implemented!");

        }

        [Test]
        public void Test_Castling_Castle_KingSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            // remove all back pieces except for rook and king
            // move the king somewhere
            // move the king back
            // try to castle
            // should fail

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to A8 is interpreted as Castle
            Assert.Fail("Not implemented!");

        }

        [Test]
        public void Test_Castling_Castle_QueenSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            // remove all back pieces except for rook and king
            // move the king somewhere
            // move the king back
            // try to castle
            // should fail

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to A8 is interpreted as Castle
            Assert.Fail("Not implemented!");

        }

        [Test]
        public void Test_Pawn_EnPassant()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            // remove all back pieces except for rook and king
            // move the king somewhere
            // move the king back
            // try to castle
            // should fail

            Assert.Fail("Not implemented!");
        }
    }
}
