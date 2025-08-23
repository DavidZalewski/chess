using Chess.Board;
using Chess.Controller;
using Chess.Pieces;
using Chess.Services;

namespace Tests.Pieces
{
    [Category("CORE")]
    [Parallelizable(ParallelScope.All)]
    public class ChessPieceSpecialMovesTests : TestBase
    {

        [Test]
        public void IsValidMove_Castling_CannotCastleAtStart()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));
            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.False); // King moving to H1 is interpreted as Castle
        }

        [Test]
        public void IsValidMove_Castling_CannotCastle_WhiteKingMoved()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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
            // It's good. The King is on E square, the Queen is on D square in ChessPieceFactory methods
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
        public void IsValidMove_Castling_CannotCastle_WhiteKingWasChecked()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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
            (whiteKing as ChessPieceKing).SetWasInCheck();

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.False); // King moving to A8 is interpreted as Castle
        }

        [Test]
        public void IsValidMove_Castling_WhiteCannotCastle_CastleSquaresChecked()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

            ChessPiece whiteKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));

            ChessPiece whiteQueen = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.QUEEN));

            ChessPiece blackQueen = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.QUEEN));

            // remove bishop and knight from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.QUEEN)
            );

            Assert.That(chessPieces.Remove(whiteQueen), Is.True);

            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            blackQueen.Move(chessBoard, new("B6")); // black queen threatens king side and queen side castle squares

            Assert.That(whiteKing is ChessPieceKing, Is.True);
            Assert.That(blackQueen is ChessPieceQueen, Is.True);

            Assert.That(whiteKing.IsValidMove(chessBoard, new("A1")), Is.False); // King moving to A1 is interpreted as Castle
            Assert.That(whiteKing.IsValidMove(chessBoard, new("H1")), Is.False); // King moving to H1 is interpreted as Castle
        }

        [Test]
        public void IsValidMove_Castling_BlackCannotCastle_CastleSquaresChecked()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

            ChessPiece blackKing = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.KING));

            ChessPiece whiteQueen = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.WHITE) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.QUEEN));

            ChessPiece blackQueen = chessPieces.First(piece => piece.GetColor().Equals(ChessPiece.Color.BLACK) &&
                                                              piece.GetPiece().Equals(ChessPiece.Piece.QUEEN));

            // remove bishop and knight from the board
            chessPieces = chessPieces.FindAll(p => p.GetPiece().Equals(ChessPiece.Piece.ROOK) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.KING) ||
                                                   p.GetPiece().Equals(ChessPiece.Piece.QUEEN)
            );

            Assert.That(chessPieces.Remove(blackQueen), Is.True);

            ChessBoard chessBoard = new();

            chessBoard.PopulateBoard(chessPieces);

            whiteQueen.Move(chessBoard, new("G3")); // white queen threatens king side and queen side castle squares

            Assert.That(blackKing is ChessPieceKing, Is.True);
            Assert.That(whiteQueen is ChessPieceQueen, Is.True);

            Assert.That(blackKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to A8 is interpreted as Castle
            Assert.That(blackKing.IsValidMove(chessBoard, new("H8")), Is.False); // King moving to H8 is interpreted as Castle
        }



        [Test]
        public void IsValidMove_Castling_Castle_WhiteKingSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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
        public void IsValidMove_Castling_Castle_WhiteQueenSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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
        public void Move_Castling_Castle_WhiteQueenSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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

            Square sq = chessBoard.GetSquare(new("D1"));
            ChessPiece rook = sq.Piece;

            Assert.Multiple(() =>
            {
                Assert.That(chessBoard.IsPieceAtPosition(new("C1"), ChessPiece.Color.WHITE, ChessPiece.Piece.KING), Is.True);
                Assert.That(chessBoard.IsPieceAtPosition(new("D1"), ChessPiece.Color.WHITE, ChessPiece.Piece.ROOK), Is.True);
                Assert.That(rook.GetId(), Is.EqualTo(1), "Expected WR1 to stay as WR1 and not be renamed");
                Assert.That(rook.GetCurrentPosition(), Is.EqualTo(new BoardPosition("D1")), "Expected rook current position to Equal D1");
            });
        }

        [Test]
        public void Move_Castling_Castle_WhiteKingSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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

            Square sq = chessBoard.GetSquare(new("F1"));
            ChessPiece rook = sq.Piece;

            Assert.Multiple(() =>
            {
                Assert.That(chessBoard.IsPieceAtPosition(new("G1"), ChessPiece.Color.WHITE, ChessPiece.Piece.KING), Is.True);
                Assert.That(chessBoard.IsPieceAtPosition(new("F1"), ChessPiece.Color.WHITE, ChessPiece.Piece.ROOK), Is.True);
                Assert.That(rook.GetId(), Is.EqualTo(2), "Expected WR2 to stay as WR2 and not be renamed");
                Assert.That(rook.GetCurrentPosition(), Is.EqualTo(new BoardPosition("F1")), "Expected rook current position to Equal F1");
            });

            

        }

        [Test]
        public void Move_Castling_Castle_BlackQueenSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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

            Square sq = chessBoard.GetSquare(new("D8"));
            ChessPiece rook = sq.Piece;

            Assert.Multiple(() =>
            {
                Assert.That(chessBoard.IsPieceAtPosition(new("C8"), ChessPiece.Color.BLACK, ChessPiece.Piece.KING), Is.True);
                Assert.That(chessBoard.IsPieceAtPosition(new("D8"), ChessPiece.Color.BLACK, ChessPiece.Piece.ROOK), Is.True);
                Assert.That(rook.GetId(), Is.EqualTo(1), "Expected BR1 to stay as BR1 and not be renamed");
                Assert.That(rook.GetCurrentPosition(), Is.EqualTo(new BoardPosition("D8")), "Expected rook current position to Equal D8");
            });
        }

        [Test]
        public void Move_Castling_Castle_BlackKingSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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

            Square sq = chessBoard.GetSquare(new("F8"));
            ChessPiece rook = sq.Piece;

            Assert.Multiple(() =>
            {
                Assert.That(chessBoard.IsPieceAtPosition(new("G8"), ChessPiece.Color.BLACK, ChessPiece.Piece.KING), Is.True);
                Assert.That(chessBoard.IsPieceAtPosition(new("F8"), ChessPiece.Color.BLACK, ChessPiece.Piece.ROOK), Is.True);
                Assert.That(rook.GetId(), Is.EqualTo(2), "Expected BR2 to stay as BR2 and not be renamed");
                Assert.That(rook.GetCurrentPosition(), Is.EqualTo(new BoardPosition("F8")), "Expected rook current position to Equal F8");

            });
        }

        [Test]
        public void IsValidMove_Castling_CannotCastle_BlackKingMoved()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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
        public void IsValidMove_Castling_CannotCastle_BlackKingWasChecked()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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
            (blackKing as ChessPieceKing).SetWasInCheck();

            // remove all back pieces except for rook and king
            // move the king somewhere
            // move the king back
            // try to castle
            // should fail

            Assert.That(blackKing.IsValidMove(chessBoard, new("A8")), Is.False); // King moving to H1 is interpreted as Castle
            Assert.That(blackKing.IsValidMove(chessBoard, new("H8")), Is.False); // King moving to H8 is interpreted as Castle
        }

        [Test]
        public void IsValidMove_Castling_Castle_BlackKingSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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
        public void IsValidMove_Castling_Castle_BlackQueenSide()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();

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

        [Test(Description = "Tests that the White Pawn on row 5 can capture a Black Pawn via enpassant on squares left and right of the white pawn square")]
        [TestCase("B5", "A5", "C5", "A6", "C6")]
        [TestCase("C5", "B5", "D5", "B6", "D6")]
        [TestCase("D5", "C5", "E5", "C6", "E6")]
        [TestCase("E5", "F5", "D5", "F6", "D6")]
        [TestCase("F5", "E5", "G5", "E6", "G6")]
        [TestCase("G5", "F5", "H5", "F6", "H6")]
        public void WhitePawn_IsValidMove_EnPassant(String whitePawnPos, String blackPawnPosLeft, String blackPawnPosRight, String enPassantPosLeft, String enPassantPosRight)
        {
            // Create White Pawn at {{ whitePawnPos }}
            // ID argument can be mocked as it has no affect on logic here
            ChessPiece whitePawnPiece = new ChessPieceWhitePawn(1, new(whitePawnPos));
            ChessPiece blackPawnPieceLeft = new ChessPieceBlackPawn(1, new(blackPawnPosLeft));
            ChessPiece blackPawnPieceRight = new ChessPieceBlackPawn(2, new(blackPawnPosRight));

            // Need to set some internal state values to perform this test
            (blackPawnPieceLeft as ChessPiecePawn).SetMovedTwoSquares();
            (blackPawnPieceRight as ChessPiecePawn).SetMovedTwoSquares();

            ChessBoard board = new();
            List<ChessPiece> chessPieces = new() { whitePawnPiece, blackPawnPieceLeft, blackPawnPieceRight };

            GameController gameController = new(board, chessPieces);
            gameController.StartGame();

            Assert.Multiple(() =>
            {
                Assert.That(whitePawnPiece.IsValidMove(board, new(enPassantPosLeft)), Is.True);
                Assert.That((blackPawnPieceLeft as ChessPiecePawn).IsEnPassantTarget, Is.True);
                Assert.That(whitePawnPiece.IsValidMove(board, new(enPassantPosRight)), Is.True);
                Assert.That((blackPawnPieceRight as ChessPiecePawn).IsEnPassantTarget, Is.True);
            });
        }

        [Test(Description = "Tests that the Black Pawn on row 4 can capture a White Pawn via enpassant on squares left and right of the black pawn square")]
        [TestCase("B4", "A4", "C4", "A3", "C3")]
        [TestCase("C4", "B4", "D4", "B3", "D3")]
        [TestCase("D4", "C4", "E4", "C3", "E3")]
        [TestCase("E4", "F4", "D4", "F3", "D3")]
        [TestCase("F4", "E4", "G4", "E3", "G3")]
        [TestCase("G4", "F4", "H4", "F3", "H3")]
        public void BlackPawn_IsValidMove_EnPassant(String blackPawnPos, String whitePawnPosLeft, String whitePawnPosRight, String enPassantPosLeft, String enPassantPosRight)
        {
            // Create Black Pawn at {{ blackPawnPos }}
            // ID argument can be mocked as it has no affect on logic here
            ChessPiece blackPawnPiece = new ChessPieceBlackPawn(1, new(blackPawnPos));
            ChessPiece whitePawnPieceLeft = new ChessPieceWhitePawn(1, new(whitePawnPosLeft));
            ChessPiece whitePawnPieceRight = new ChessPieceWhitePawn(2, new(whitePawnPosRight));

            // Need to set some internal state values to perform this test
            (whitePawnPieceLeft as ChessPiecePawn).SetMovedTwoSquares();
            (whitePawnPieceRight as ChessPiecePawn).SetMovedTwoSquares();

            ChessBoard board = new();
            List<ChessPiece> chessPieces = new() { blackPawnPiece, whitePawnPieceLeft, whitePawnPieceRight };

            GameController gameController = new(board, chessPieces);
            gameController.StartGame();

            Assert.Multiple(() =>
            {
                Assert.That(blackPawnPiece.IsValidMove(board, new(enPassantPosLeft)), Is.True);
                Assert.That((whitePawnPieceLeft as ChessPiecePawn).IsEnPassantTarget, Is.True);
                Assert.That(blackPawnPiece.IsValidMove(board, new(enPassantPosRight)), Is.True);
                Assert.That((whitePawnPieceRight as ChessPiecePawn).IsEnPassantTarget, Is.True);
            });
        }


    }
}
