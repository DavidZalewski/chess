using Chess;
using Chess.Board;
using Chess.Pieces;
using Chess.Services;

namespace Tests.Services
{
    public class KingCheckServiceTests
    {
        [Test]
        public void Test_ConstructKingCheckService_Success()
        {
            KingCheckService kingCheckService = new(new ChessBoard(), ChessPieceFactory.CreateChessPieces());

            Assert.That(kingCheckService, Is.Not.Null);
        }

        [Test(Description = "Tests that a white king on E3 cannot move to E4 or D4 when there is a black king on D5")]
        public void Test_IsWhiteKingInCheck_BlackKingChecks()
        {
            // Construct board; set black king on D5 and white king on E3 on chess board object
            ChessBoard chessBoard = new();
            chessBoard.SetBoardValue(new BoardPosition("E3"), 16);
            chessBoard.SetBoardValue(new BoardPosition("D5"), 26);

            // create the chess piece objects with the correct positions as on the chessboard object
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new BoardPosition("E3"));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new BoardPosition("D5"));

            // Construct Turn objects - this doesnt seem right - the board state doesnt match the turn description
            Turn whiteKingTurnD4 = new(9, whiteKingPiece, new BoardPosition("E3"), new BoardPosition("D4"), chessBoard);
            Turn whiteKingTurnE4 = new(9, whiteKingPiece, new BoardPosition("E3"), new BoardPosition("E4"), chessBoard);

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessBoard, new List<ChessPiece>() { whiteKingPiece, blackKingPiece });

            Assert.Multiple(() =>
            {
                Assert.That(kingCheckService.IsKingInCheck(whiteKingTurnD4), Is.True);
                Assert.That(kingCheckService.IsKingInCheck(whiteKingTurnE4), Is.True);
            });
        }

        [Test(Description = "Tests that a black king on D5 cannot move to E4 or D4 when there is a white king on E3")]
        public void Test_IsBlackKingInCheck_WhiteKingChecks()
        {
            // Construct board; set black king on D5 and white king on E3 on chess board object
            ChessBoard chessBoard = new();
            chessBoard.SetBoardValue(new BoardPosition("E3"), 16);
            chessBoard.SetBoardValue(new BoardPosition("D5"), 26);

            // create the chess piece objects with the correct positions as on the chessboard object
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new BoardPosition("E3"));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new BoardPosition("D5"));

            // Construct Turn objects - this doesnt seem right - the board state doesnt match the turn description
            Turn blackKingTurnD4 = new(10, blackKingPiece, new BoardPosition("D5"), new BoardPosition("D4"), chessBoard);
            Turn blackKingTurnE4 = new(10, blackKingPiece, new BoardPosition("D5"), new BoardPosition("E4"), chessBoard);

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessBoard, new List<ChessPiece>() { whiteKingPiece, blackKingPiece });

            Assert.Multiple(() =>
            {
                Assert.That(kingCheckService.IsKingInCheck(blackKingTurnD4), Is.True);
                Assert.That(kingCheckService.IsKingInCheck(blackKingTurnE4), Is.True);
            });
        }

        [Test(Description = "Tests that both white king and black king on their starting squares with no other pieces on board can move until a point where both would be in check from each other")]
        public void Test_IsKingInCheck_UntilKingsWouldCheckEachOther()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.E));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.E));

            ChessBoard chessBoard = new();
            chessBoard.SetBoardValue(new BoardPosition(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.E), 26);
            chessBoard.SetBoardValue(new BoardPosition(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.E), 16);

            BoardPosition e2 = new(BoardPosition.VERTICAL.TWO, BoardPosition.HORIZONTAL.E);
            BoardPosition e3 = new(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.E);
            BoardPosition e4 = new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E);

            BoardPosition e7 = new(BoardPosition.VERTICAL.SEVEN, BoardPosition.HORIZONTAL.E);
            BoardPosition d6 = new(BoardPosition.VERTICAL.SIX, BoardPosition.HORIZONTAL.D);
            BoardPosition d5 = new(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.D);

            Assert.Multiple(() =>
            {
                // White King moves from E1 to E2
                Assert.That(whiteKingPiece.IsValidMove(chessBoard, e2), Is.True);
                whiteKingPiece.Move(chessBoard, e2);
                // Black King moves from E8 to E7
                Assert.That(blackKingPiece.IsValidMove(chessBoard, e7), Is.True);
                blackKingPiece.Move(chessBoard, e7);
                // White King moves from E2 to E3
                Assert.That(whiteKingPiece.IsValidMove(chessBoard, e3), Is.True);
                whiteKingPiece.Move(chessBoard, e3);
                // Black King moves from E7 to D6
                Assert.That(blackKingPiece.IsValidMove(chessBoard, d6), Is.True); // black king moves diagonally in this test case
                blackKingPiece.Move(chessBoard, d6);
                // White King moves from E3 to E4
                Assert.That(whiteKingPiece.IsValidMove(chessBoard, e4), Is.True);
                whiteKingPiece.Move(chessBoard, e4);

                // Black King Cannot move to D4 as it would be in check from the White King at E4
                Assert.That(blackKingPiece.IsValidMove(chessBoard, d5), Is.False);
            });
        }

        // for this test to pass, it requires that the king piece can iterate over all other pieces on the board and check 
        // if those pieces put it in check
        // because the king is a ChessPiece object itself, does it make sense for it to hold a collection of other ChessPiece
        // objects to test for this?
        // or should this be handled in another class, such as the GameController class, which would keep track of pieces on
        // the board and can perform this kind of logic check?
        [Test(Description = "Tests that a white king on E1 cannot move to D1 as there is a black rook on D8 that would put it in check")]
        public void Test_IsKingInCheck_WouldBeCheckedByBlackRookOnD8()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.E));

            ChessBoard chessBoard = new();
            // Black Rook on D8
            chessBoard.SetBoardValue(new BoardPosition(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.D), 24);
            // White King on E1
            chessBoard.SetBoardValue(new BoardPosition(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.E), 16);

            Assert.That(whiteKingPiece.IsValidMove(chessBoard, new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.D)), Is.False);
        }


    }
}
