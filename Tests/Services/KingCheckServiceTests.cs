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

        [Test(Description = "Test where White Bishop on C1 has Pinned black king on G5 with black knight on F4; Moving Knight to any other position would put King in check from Bishop")]
        public void Test_IsKingInCheck_WhiteBishopPinsBlackKingAndKnight()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new("A1"));
            ChessPiece whiteBishopPiece = new ChessPieceBishop(ChessPiece.Color.WHITE, 2, new("C1"));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new("G5"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F4"));

            List<ChessPiece> chessPieces = new() 
            { 
                whiteBishopPiece, whiteKingPiece, blackKnightPiece, blackKingPiece 
            };

            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            List<BoardPosition> validKnightPositions = new()
            {
                new("D5"), new("E6"), new("D3"), new("E2"), 
                new("G2"), new("H3"), new("H5"), new("G6")
            };
            List<Turn> possibleKnightTurns = new();

            // ideally we shouldn't be testing the knight in this test suite, but since we cant be 100% confident these values
            // are what we expect them to be, might as well make sure this part of the test is not broken
            Assert.Multiple(() =>
            {
                foreach (BoardPosition position in validKnightPositions)
                {
                    Assert.That(blackKnightPiece.IsValidMove(chessBoard, position), Is.True);
                    possibleKnightTurns.Add(new(2, blackKnightPiece, position, chessBoard));
                }
            });

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessBoard, chessPieces);

            Assert.Multiple(() =>
            {
                foreach (Turn turn in possibleKnightTurns)
                {
                    Assert.That(kingCheckService.IsKingInCheck(turn), Is.True, turn.NewPosition.StringValue);
                }
            });
        }

        [Test(Description = "Tests that a white king on E1 cannot move to D1 or D2 as there is a black rook on D8 that would put it in check")]
        public void Test_IsKingInCheck_WouldBeCheckedByBlackRookOnD8()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new("E1"));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new("H8"));
            ChessPiece blackRookPiece = new ChessPieceRook(ChessPiece.Color.BLACK, 1, new("D8"));
            List<ChessPiece> chessPieces = new()
            {
                whiteKingPiece, blackRookPiece, blackKingPiece
            };

            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessBoard, chessPieces);

            Turn moveKingToD1 = new(5, whiteKingPiece, new("D1"), chessBoard);
            Turn moveKingToD2 = new(5, whiteKingPiece, new("D2"), chessBoard);

            Assert.Multiple(() =>
            {
                // technically the move is valid, because IsValidMove does not check if kings are in check
                Assert.That(whiteKingPiece.IsValidMove(chessBoard, new("D1")), Is.True);
                Assert.That(whiteKingPiece.IsValidMove(chessBoard, new("D2")), Is.True);

                // check that this puts the king in check from the rook
                Assert.That(kingCheckService.IsKingInCheck(moveKingToD1), Is.True);
                Assert.That(kingCheckService.IsKingInCheck(moveKingToD2), Is.True);
            });
        }


    }
}
