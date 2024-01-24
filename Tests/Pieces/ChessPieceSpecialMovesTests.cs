using Chess;
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
        public void Test_IsValidMove_Castling_CannotCastleAtStart()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.False); // King moving to H1 is interpreted as Castle
        }

        [Test]
        public void Test_IsValidMove_Castling_CannotCastle_WhiteKingMoved()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop and knight from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.QUEEN)
            );

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            // in the beginning King's Side Castle should be possible
            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Queen's Side Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.True); // King moving to A8 is interpreted as King's Side Castle

            // King can move to F1
            Assert.That(whiteKing.IsValidMove(chessBoard, new("F1")), Is.True);
            whiteKing.Move(chessBoard, new("F1"));
            whiteKing.SetCurrentPosition(new("F1"));

            // Castle should be impossible
            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Queen's Side Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.False); // King moving to A8 is interpreted as King's Side Castle
            
            // Move King back to E1
            Assert.That(whiteKing.IsValidMove(chessBoard, new("E1")), Is.True);
            whiteKing.Move(chessBoard, new("E1"));

            // Castle should still be impossible to do
            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Queen's Side Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.False); // King moving to A8 is interpreted as King's Side Castle
        }

        [Test]
        public void Test_IsValidMove_Castling_CannotCastle_WhiteKingWasChecked()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));

            // remove bishop and knight from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.QUEEN)
            );

            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            Assert.That(whiteKing is ChessPieceKing, Is.True);
            (whiteKing as ChessPieceKing).Test_SetWasInCheck();

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.False); // King moving to A8 is interpreted as Castle
        }

        [Test]
        public void Test_IsValidMove_Castling_Castle_WhiteKingSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop and knight from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.QUEEN)
            );

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Queens Side Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.True); // King moving to A8 is interpreted as Kings Side Castle
        }

        [Test]
        public void Test_IsValidMove_Castling_Castle_WhiteQueenSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop, knight, and queen from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING)
            );

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.True); // King moving to A1 is interpreted as Queens Side Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.True); // King moving to A8 is interpreted as Kings Side Castle
        }

        [Test]
        public void Test_Move_Castling_Castle_WhiteQueenSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop, knight, and queen from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING)
            );

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            GameController gc = new GameController(chessBoard, chessPieces);

            gc.StartGame();

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.True); // King moving to A1 is interpreted as Queens Side Castle
            whiteKing.Move(chessBoard, new("A1"));

            Assert.That(chessBoard.IsPieceAtPosition(new("C1"), ChessPiece.Color.WHITE, ChessPiece.Piece.KING), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(new("D1"), ChessPiece.Color.WHITE, ChessPiece.Piece.ROOK), Is.True);
        }

        [Test]
        public void Test_Move_Castling_Castle_WhiteKingSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop, knight, and queen from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING)
            );

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            GameController gc = new GameController(chessBoard, chessPieces);

            gc.StartGame();

            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.True); // King moving to H1 is interpreted as King's Side Castle
            whiteKing.Move(chessBoard, new("H1"));

            Assert.That(chessBoard.IsPieceAtPosition(new("G1"), ChessPiece.Color.WHITE, ChessPiece.Piece.KING), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(new("F1"), ChessPiece.Color.WHITE, ChessPiece.Piece.ROOK), Is.True);
        }

        [Test]
        public void Test_Move_Castling_Castle_BlackQueenSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop, knight, and queen from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING)
            );

            ChessPiece blackKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            GameController gc = new GameController(chessBoard, chessPieces);

            gc.StartGame();

            Assert.That(blackKing.IsValidMove(chessBoard, new("A8")), Is.True); // King moving to A1 is interpreted as Queens Side Castle
            blackKing.Move(chessBoard, new("A8"));

            Assert.That(chessBoard.IsPieceAtPosition(new("C8"), ChessPiece.Color.BLACK, ChessPiece.Piece.KING), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(new("D8"), ChessPiece.Color.BLACK, ChessPiece.Piece.ROOK), Is.True);
        }

        [Test]
        public void Test_Move_Castling_Castle_BlackKingSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop, knight, and queen from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING)
            );

            ChessPiece blackKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            GameController gc = new GameController(chessBoard, chessPieces);

            gc.StartGame();

            Assert.That(blackKing.IsValidMove(chessBoard, new("H8")), Is.True); // King moving to H8 is interpreted as King's Side Castle
            blackKing.Move(chessBoard, new("H8"));

            Assert.That(chessBoard.IsPieceAtPosition(new("G8"), ChessPiece.Color.BLACK, ChessPiece.Piece.KING), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(new("F8"), ChessPiece.Color.BLACK, ChessPiece.Piece.ROOK), Is.True);
        }

        [Test]
        public void Test_IsValidMove_Castling_CannotCastle_BlackKingMoved()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop and knight from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.QUEEN)
            );

            ChessPiece blackKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            // in the beginning King's Side Castle should be possible
            Assert.That(blackKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to H1 is interpreted as Queen's Side Castle
            Assert.That(blackKing.IsValidMove(chessBoard, new("H8")), Is.True); // King moving to H8 is interpreted as King's Side Castle

            // King can move to F1
            Assert.That(blackKing.IsValidMove(chessBoard, new("F8")), Is.True);
            blackKing.Move(chessBoard, new("F8"));
            blackKing.SetCurrentPosition(new("F8"));

            // Castle should be impossible
            Assert.That(blackKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to H1 is interpreted as Queen's Side Castle
            Assert.That(blackKing.IsValidMove(chessBoard, new("H8")), Is.False); // King moving to H8 is interpreted as King's Side Castle

            // Move King back to E1
            Assert.That(blackKing.IsValidMove(chessBoard, new("E8")), Is.True);
            blackKing.Move(chessBoard, new("E8"));

            // Castle should still be impossible to do
            Assert.That(blackKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to H1 is interpreted as Queen's Side Castle
            Assert.That(blackKing.IsValidMove(chessBoard, new("H8")), Is.False); // King moving to H8 is interpreted as King's Side Castle
        }

        [Test]
        public void Test_IsValidMove_Castling_CannotCastle_BlackKingWasChecked()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece blackKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));

            // remove bishop and knight from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.QUEEN)
            );

            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            Assert.That(blackKing is ChessPieceKing, Is.True);
            (blackKing as ChessPieceKing).Test_SetWasInCheck();

            // remove all back pieces except for rook and king
            // move the king somewhere
            // move the king back
            // try to castle
            // should fail

            Assert.That(blackKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to H1 is interpreted as Castle
            Assert.That(blackKing.IsValidMove(chessBoard, new("H8")), Is.False); // King moving to H8 is interpreted as Castle
        }

        [Test]
        public void Test_IsValidMove_Castling_Castle_BlackKingSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop and knight from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.QUEEN)
            );

            ChessPiece blackKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            Assert.That(blackKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to H1 is interpreted as Queens Side Castle
            Assert.That(blackKing.IsValidMove(chessBoard, new("H8")), Is.True); // King moving to H8 is interpreted as Kings Side Castle
        }

        [Test]
        public void Test_IsValidMove_Castling_Castle_BlackQueenSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            // remove bishop, knight, and queen from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.PAWN) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING)
            );

            ChessPiece blackKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            Assert.That(blackKing.IsValidMove(chessBoard, new("A8")), Is.True); // King moving to H1 is interpreted as Queens Side Castle
            Assert.That(blackKing.IsValidMove(chessBoard, new("H8")), Is.True); // King moving to H8 is interpreted as Kings Side Castle
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
