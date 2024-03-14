using Chess.Board;
using Chess.Controller;
using Chess.GameState;
using Chess.Pieces;
using Chess.Services;

namespace Tests
{
    public class GameControllerTests
    {
        [Test]
        public void Test_ConstructGameController_Success()
        {
            ChessBoard chessBoard = new ChessBoard();
            GameController gameController = new GameController(chessBoard);

            Assert.That(gameController, Is.Not.Null);
        }

        [Test]
        public void Test_GameController_InterpretCommand1_Success()
        {

        }

        [Test]
        public void Test_GameController_FindChessPieceFromString_WhiteKnight1_Success()
        {
            String piece = "WK1";
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteKnight1 = chessPieces.First(pieces => {
                return pieces.GetPiece().Equals(ChessPiece.Piece.KNIGHT) &&
                       pieces.GetColor().Equals(ChessPiece.Color.WHITE) &&
                       pieces.GetId().Equals(1);
            });

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            ChessPiece chessPiece = gameController.FindChessPieceFromString(piece);

            Assert.That(chessPiece, Is.Not.Null);
            Assert.That(chessPiece.Equals(whiteKnight1), Is.True);
        }

        [Test]
        public void Test_GameController_FindChessPieceFromString_BlackBishop2_Success()
        {
            String piece = "BB2";
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece blackBishop2 = chessPieces.First(pieces => {
                return pieces.GetPiece().Equals(ChessPiece.Piece.BISHOP) &&
                       pieces.GetColor().Equals(ChessPiece.Color.BLACK) &&
                       pieces.GetId().Equals(2);
            });

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            ChessPiece chessPiece = gameController.FindChessPieceFromString(piece);

            Assert.That(chessPiece, Is.Not.Null);
            Assert.That(chessPiece.Equals(blackBishop2), Is.True);
        }

        [Test]
        public void Test_GameController_FindChessPieceFromString_BlackPawn7_Success()
        {
            String piece = "BP7";
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece blackPawn7 = chessPieces.First(pieces => {
                return pieces.GetPiece().Equals(ChessPiece.Piece.PAWN) &&
                       pieces.GetColor().Equals(ChessPiece.Color.BLACK) &&
                       pieces.GetId().Equals(7);
            });

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            ChessPiece chessPiece = gameController.FindChessPieceFromString(piece);

            Assert.That(chessPiece, Is.Not.Null);
            Assert.That(chessPiece.Equals(blackPawn7), Is.True);
        }

        [Test]
        public void Test_GameController_FindChessPieceFromString_WhiteRook2_Success()
        {
            String piece = "WR2";
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece blackPawn7 = chessPieces.First(pieces => {
                return pieces.GetPiece().Equals(ChessPiece.Piece.ROOK) &&
                       pieces.GetColor().Equals(ChessPiece.Color.WHITE) &&
                       pieces.GetId().Equals(2);
            });

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            ChessPiece chessPiece = gameController.FindChessPieceFromString(piece);

            Assert.That(chessPiece, Is.Not.Null);
            Assert.That(chessPiece.Equals(blackPawn7), Is.True);
        }

        [Test]
        public void Test_GameController_FindChessPieceFromString_WhiteQueen_Success()
        {
            String piece = "WQ1";
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece whiteQueen1 = chessPieces.First(pieces => {
                return pieces.GetPiece().Equals(ChessPiece.Piece.QUEEN) &&
                       pieces.GetColor().Equals(ChessPiece.Color.WHITE) &&
                       pieces.GetId().Equals(1);
            });

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            ChessPiece chessPiece = gameController.FindChessPieceFromString(piece);

            Assert.That(chessPiece, Is.Not.Null);
            Assert.That(chessPiece.Equals(whiteQueen1), Is.True);
        }

        [Test]
        public void Test_GameController_FindChessPieceFromString_BlackKing_Success()
        {
            String piece = "BK";
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessPiece blackKing = chessPieces.First(pieces => {
                return pieces.GetPiece().Equals(ChessPiece.Piece.KING) &&
                       pieces.GetColor().Equals(ChessPiece.Color.BLACK) &&
                       pieces.GetId().Equals(1);
            });

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);
            ChessPiece chessPiece = gameController.FindChessPieceFromString(piece);

            Assert.That(chessPiece, Is.Not.Null);
            Assert.That(chessPiece.Equals(blackKing), Is.True);
        }

        [TestCase("A")]
        [TestCase("A_")]
        [TestCase("88")]
        [TestCase("PO")]
        [TestCase("983")]
        [TestCase("A@4")]
        [TestCase("")]
        [TestCase("sriojgwi 4whiu hwu4h woweng")]
        public void Test_GameController_FindChessPieceFromString_InvalidInput_ReturnsNull(String input)
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);

            Assert.That(gameController.FindChessPieceFromString(input), Is.Null);
        }

        [TestCase("WK1 C3")]
        [TestCase("WK1 A3")]
        [TestCase("WK2 F3")]
        [TestCase("WK2 H3")]
        [TestCase("WP1 A3")]
        [TestCase("WP1 A4")]
        [TestCase("WP2 B3")]
        [TestCase("WP2 B4")]
        [TestCase("WP3 C3")]
        [TestCase("WP3 C4")]
        [TestCase("WP4 D3")]
        [TestCase("WP4 D4")]
        [TestCase("WP5 E3")]
        [TestCase("WP5 E4")]
        [TestCase("WP6 F3")]
        [TestCase("WP6 F4")]
        [TestCase("WP7 G3")]
        [TestCase("WP7 G4")]
        [TestCase("WP8 H3")]
        [TestCase("WP8 H4")]
        public void Test_GameController_GetMoveFromCommand_WhiteTurn1(String input)
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();
            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);
            GameController gameController = new(chessBoard);

            Turn turn = gameController.GetTurnFromCommand(input);

            Assert.That(turn, Is.Not.Null);

            String[] inputs = input.Split(new char[] { ' ' });
            BoardPosition boardPosition = new(inputs[1]);

            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition == turn.NewPosition, Is.True);

            ChessPiece? piece = gameController.FindChessPieceFromString(inputs[0]);
            Assert.That(piece, Is.Not.Null);
            Assert.That(turn.ChessPiece.Equals(piece), Is.False); // the copies of this piece have different positions
            gameController.ApplyTurnToGameState(turn); // update the game state
            piece = gameController.FindChessPieceFromString(inputs[0]);
            Assert.That(turn.ChessPiece.Equals(piece), Is.True); // the piece should be updated now with the turn board state
        }

        [TestCase("BK1 A6")]
        [TestCase("BK1 C6")]
        [TestCase("BK2 F6")]
        [TestCase("BK2 H6")]
        [TestCase("BP1 A6")]
        [TestCase("BP1 A5")]
        [TestCase("BP2 B6")]
        [TestCase("BP2 B5")]
        [TestCase("BP3 C6")]
        [TestCase("BP3 C5")]
        [TestCase("BP4 D6")]
        [TestCase("BP4 D5")]
        [TestCase("BP5 E6")]
        [TestCase("BP5 E5")]
        [TestCase("BP6 F6")]
        [TestCase("BP6 F5")]
        [TestCase("BP7 G6")]
        [TestCase("BP7 G5")]
        [TestCase("BP8 H6")]
        [TestCase("BP8 H5")]
        public void Test_GameController_GetMoveFromCommand_BlackTurn2(String input)
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();
            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);
            GameController gameController = new(chessBoard);

            Turn turn = gameController.GetTurnFromCommand(input);

            Assert.That(turn, Is.Not.Null);

            String[] inputs = input.Split(new char[] { ' ' });
            BoardPosition boardPosition = new(inputs[1]);

            Assert.That(boardPosition, Is.Not.Null);
            Assert.That(boardPosition == turn.NewPosition, Is.True);

            ChessPiece? piece = gameController.FindChessPieceFromString(inputs[0]);

            Assert.That(piece, Is.Not.Null);
            Assert.That(turn.ChessPiece.Equals(piece), Is.False); // the copies of this piece have different positions
            gameController.ApplyTurnToGameState(turn); // update the game state
            piece = gameController.FindChessPieceFromString(inputs[0]);
            Assert.That(turn.ChessPiece.Equals(piece), Is.True); // the piece should be updated now with the turn board state
        }
    }
}
