﻿using Chess;
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
            KingCheckService kingCheckService = new(ChessPieceFactory.CreateChessPieces());

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
            KingCheckService kingCheckService = new(new List<ChessPiece>() { whiteKingPiece, blackKingPiece });

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
            KingCheckService kingCheckService = new(new List<ChessPiece>() { whiteKingPiece, blackKingPiece });

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
            KingCheckService kingCheckService = new(chessPieces);

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
            KingCheckService kingCheckService = new(chessPieces);

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

        [Test(Description = "Test involving 9 pieces [Black Bishop on A5, Black Rook on A1, Black Knight on F3, White Queen on D1, White King on E1, White Bishop on F1, White Pawns on D2, E2, and F2] where knight has put king in check, only a single valid move is possible")]
        public void Test_IsKingInCheck_9PieceWhiteKingInCheck()
        {
            ChessPiece whiteQueenPiece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, new("D1"));
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new("E1"));
            ChessPiece whiteBishopPiece = new ChessPieceBishop(ChessPiece.Color.WHITE, 2, new("F1"));
            ChessPiece whitePawn4Piece = new ChessPieceWhitePawn(4, new("D2"));
            ChessPiece whitePawn5Piece = new ChessPieceWhitePawn(4, new("E2"));
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(4, new("F2"));
            ChessPiece blackBishopPiece = new ChessPieceBishop(ChessPiece.Color.BLACK, 1, new("A5"));
            ChessPiece blackRookPiece = new ChessPieceRook(ChessPiece.Color.BLACK, 1, new("A1"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));

            List<ChessPiece> chessPieces = new()
            {
                whiteQueenPiece, whiteKingPiece, whiteBishopPiece, whitePawn4Piece,
                whitePawn5Piece, whitePawn6Piece, blackBishopPiece, blackRookPiece, blackKnightPiece
            };

            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            List<Turn> possibleMoves = new()
            {
                new(11, whiteQueenPiece, new("A1"), chessBoard), // white queen capture rook
                new(11, whitePawn4Piece, new("D3"), chessBoard), // white pawn 4 move D3
                new(11, whitePawn4Piece, new("D4"), chessBoard), // white pawn 4 move D4
                new(11, whitePawn5Piece, new("E3"), chessBoard), // white pawn 5 move E3
                new(11, whitePawn5Piece, new("E4"), chessBoard), // white pawn 5 move E4
                new(11, whiteBishopPiece, new("H3"), chessBoard), // white bishop move H3
            };

            Turn onlyValidMove = new(11, whitePawn5Piece, new("F3"), chessBoard); // white pawn 5 capture knight on f3

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessPieces);

            Assert.Multiple(() =>
            {
                foreach (Turn turn in possibleMoves)
                {
                    Assert.That(kingCheckService.IsKingInCheck(turn), Is.True, turn.NewPosition.StringValue);
                }

                // have to somehow remove the black knight from the list of pieces, as it was technically captured in this move
                var itemToRemove = chessPieces.Single(piece => piece.GetPiece().Equals(ChessPiece.Piece.KNIGHT));
                chessPieces.Remove(itemToRemove);
                Assert.That(kingCheckService.IsKingInCheck(onlyValidMove), Is.False);
            });
        }

        [Test(Description = "Test involving 8 pieces [Black Bishop on A5, Black Rook on A1, White Queen on D1, White King on E1, White Bishop on F1, White Pawns on D2, E2, and F2] where knight has put king in check, only a single valid move is possible")]
        public void Test_IsKingInCheck_8PieceWhiteKingPinned()
        {
            ChessPiece whiteQueenPiece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, new("D1"));
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new("E1"));
            ChessPiece whiteBishopPiece = new ChessPieceBishop(ChessPiece.Color.WHITE, 2, new("F1"));
            ChessPiece whitePawn4Piece = new ChessPieceWhitePawn(4, new("D2"));
            ChessPiece whitePawn5Piece = new ChessPieceWhitePawn(4, new("E2"));
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(4, new("F2"));
            ChessPiece blackBishopPiece = new ChessPieceBishop(ChessPiece.Color.BLACK, 1, new("A5"));
            ChessPiece blackRookPiece = new ChessPieceRook(ChessPiece.Color.BLACK, 1, new("A1"));

            List<ChessPiece> chessPieces = new()
            {
                whiteQueenPiece, whiteKingPiece, whiteBishopPiece, whitePawn4Piece,
                whitePawn5Piece, whitePawn6Piece, blackBishopPiece, blackRookPiece
            };

            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            List<Turn> possibleMoves = new()
            {
                new(11, whiteQueenPiece, new("C2"), chessBoard), // white queen move C2
                new(11, whitePawn4Piece, new("D3"), chessBoard), // white pawn 4 move D3
                new(11, whitePawn4Piece, new("D4"), chessBoard), // white pawn 4 move D4
            };

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessPieces);

            Assert.Multiple(() =>
            {
                foreach (Turn turn in possibleMoves)
                {
                    Assert.That(kingCheckService.IsKingInCheck(turn), Is.True, turn.NewPosition.StringValue);
                }
            });
        }

        [Test(Description = "Test involving 9 pieces [Black Bishop on A5, Black Rook on A1, Black Knight on F3, White Queen on D1, White King on E1, White Bishop on F1, White Pawns on D2, E2, and F2] where knight has put king in check, only a single valid move is possible")]
        public void Test_IsCheckMate_9PieceWhiteKingInCheck_NotCheckMate()
        {
            ChessPiece whiteQueenPiece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, new("D1"));
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new("E1"));
            ChessPiece whiteBishopPiece = new ChessPieceBishop(ChessPiece.Color.WHITE, 2, new("F1"));
            ChessPiece whitePawn4Piece = new ChessPieceWhitePawn(4, new("D2"));
            ChessPiece whitePawn5Piece = new ChessPieceWhitePawn(5, new("E2"));
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(6, new("F2"));
            ChessPiece blackBishopPiece = new ChessPieceBishop(ChessPiece.Color.BLACK, 1, new("A5"));
            ChessPiece blackRookPiece = new ChessPieceRook(ChessPiece.Color.BLACK, 1, new("A1"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));

            List<ChessPiece> chessPieces = new()
            {
                whiteQueenPiece, whiteKingPiece, whiteBishopPiece, whitePawn4Piece,
                whitePawn5Piece, whitePawn6Piece, blackBishopPiece, blackRookPiece, blackKnightPiece
            };

            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            // last turn made was black knight moving from H4 to F3
            Turn turn = new(12, blackKnightPiece, new("H4"), new("F3"), chessBoard);

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessPieces);

            // White Pawn 6 move to F3 is impossible move (only White Pawn 5 move to F3 is valid)
            // It seems there are problems in the pawn logic, it doesn't see a valid capture 
            // And it seems to think the F Pawn can capture the Knight on F3, which it cant
            Assert.That(kingCheckService.IsCheckMate(turn), Is.False);
        }

        [Test(Description = "Test involving 9 pieces [Black Bishop on A5, Black Bishop on A6, Black Rook on A1, Black Knight on F3, White Queen on D1, White King on E1, White Bishop on F1, White Pawns on D2, and F2] where knight has put king in check, no valid moves are possible")]
        public void Test_IsCheckMate_9PieceWhiteKingInCheck_CheckMate()
        {
            ChessPiece whiteQueenPiece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, new("D1"));
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new("E1"));
            ChessPiece whiteBishopPiece = new ChessPieceBishop(ChessPiece.Color.WHITE, 2, new("F1"));
            ChessPiece whitePawn4Piece = new ChessPieceWhitePawn(4, new("D2"));
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(6, new("F2"));
            ChessPiece blackBishop1Piece = new ChessPieceBishop(ChessPiece.Color.BLACK, 1, new("A5"));
            ChessPiece blackBishop2Piece = new ChessPieceBishop(ChessPiece.Color.BLACK, 2, new("A6"));
            ChessPiece blackRookPiece = new ChessPieceRook(ChessPiece.Color.BLACK, 1, new("A1"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(ChessPiece.Color.BLACK, 1, new("F3"));

            List<ChessPiece> chessPieces = new()
            {
                whiteQueenPiece, whiteKingPiece, whiteBishopPiece, whitePawn4Piece,
                blackBishop2Piece, whitePawn6Piece, blackBishop1Piece, blackRookPiece, blackKnightPiece
            };

            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            // last turn made was black knight moving from H4 to F3
            Turn turn = new(12, blackKnightPiece, new("H4"), new("F3"), chessBoard);

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessPieces);

            Assert.That(kingCheckService.IsCheckMate(turn), Is.True);
        }

        [Test(Description = "Test Check Mate involving 5 pieces")]
        public void Test_IsCheckMate_5PieceCheckMateOnBlack()
        {
            ChessPiece whiteQueenPiece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, new("C7"));
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new("G2"));
            ChessPiece whiteBishopPiece = new ChessPieceBishop(ChessPiece.Color.WHITE, 2, new("F5"));
            ChessPiece whitePawn4Piece = new ChessPieceWhitePawn(4, new("D5"));
            ChessPiece whitePawn3Piece = new ChessPieceWhitePawn(3, new("C4"));
            ChessPiece whitePawn2Piece = new ChessPieceWhitePawn(2, new("B3"));
            ChessPiece whiteRookPiece = new ChessPieceRook(ChessPiece.Color.WHITE, 1, new("A4"));
            ChessPiece whiteKnightPiece = new ChessPieceKnight(ChessPiece.Color.WHITE, 1, new("B5"));

            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new("C5"));

            List<ChessPiece> chessPieces = new()
            {
                whiteQueenPiece, whiteKingPiece, whiteBishopPiece, whitePawn4Piece,
                whitePawn3Piece, whitePawn2Piece, whiteRookPiece, whiteKnightPiece, blackKingPiece
            };

            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            // last turn made was white queen moving from B7 to C7
            Turn turn = new(19, whiteQueenPiece, new("B7"), new("C7"), chessBoard);

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessPieces);

            Assert.That(kingCheckService.IsCheckMate(turn), Is.True);
        }

        [Test(Description = "Test Check Mate is false on a full board")]
        public void Test_IsCheckMate_NoCheckMate_FullBoard()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();
            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            // last turn made was white pawn moving from A2 to A4
            Turn turn = new(1, chessPieces.First(), new("A2"), new("A4"), chessBoard);

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessPieces);

            Assert.That(kingCheckService.IsCheckMate(turn), Is.False);
        }

        [Test(Description = "Test Check Mate on black with 2 pieces")]
        public void Test_IsCheckMate_CheckMateBlack_2PieceMate()
        {
            ChessPiece whiteQueenPiece = new ChessPieceQueen(ChessPiece.Color.WHITE, 1, new("B7"));
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new("D4"));
            ChessPiece whitePawn3Piece = new ChessPieceWhitePawn(3, new("C6"));

            ChessPiece blackRookPiece = new ChessPieceRook(ChessPiece.Color.BLACK, 2, new("C8"));
            ChessPiece blackPawnPiece = new ChessPieceBlackPawn(2, new("C7"));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new("B8"));

            List<ChessPiece> chessPieces = new()
            {
                whiteQueenPiece, whiteKingPiece, whitePawn3Piece,
                blackRookPiece, blackPawnPiece, blackKingPiece
            };

            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            // last turn made was white queen moving from A2 to A4
            Turn turn = new(21, whiteQueenPiece, new("B5"), new("B7"), chessBoard);

            // Construct kingCheckService
            KingCheckService kingCheckService = new(chessPieces);

            Assert.That(kingCheckService.IsCheckMate(turn), Is.True);
        }


    }
}
