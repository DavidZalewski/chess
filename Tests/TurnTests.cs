using Chess;
using Chess.Board;
using Chess.Pieces;
using Chess.Services;

namespace Tests
{
    public class TurnTests
    {

        private List<ChessPiece> GetDifferenceBetweenLists(List<ChessPiece> chessPiecesList1, List<ChessPiece> chessPiecesList2)
        {
            List<ChessPiece> removedChessPieces = chessPiecesList1.Where((ChessPiece cp1) =>
            {
                return !chessPiecesList2.Any((ChessPiece cp2) =>
                {
                    return cp1.GetColor() == cp2.GetColor() &&
                                  cp1.GetId() == cp2.GetId() &&
                                  cp1.GetRealValue() == cp2.GetRealValue() &&
                                  cp1.GetPiece() == cp2.GetPiece();
                });
            }).ToList();
            return removedChessPieces;
        }

        [Test]
        public void Test_ConstructTurn_Success()
        {
            ChessBoard board = new();
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();
            ChessPiece whitePawn = chessPieces.First(pieces => pieces.GetColor() == ChessPiece.Color.WHITE &&
                                                     pieces.GetPiece() == ChessPiece.Piece.PAWN);
            BoardPosition previousPosition = new(whitePawn.GetCurrentPosition());
            BoardPosition newPosition = new(previousPosition.VerticalValue - 2, previousPosition.HorizontalValue);
            board.PopulateBoard(chessPieces);

            Assert.That(whitePawn.IsValidMove(board, newPosition), Is.True);

            Turn turn1 = new(1, whitePawn, previousPosition, newPosition, board, chessPieces);

            Assert.That(turn1, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(turn1.ChessPiece.GetPiece().Equals(whitePawn.GetPiece()) && turn1.ChessPiece.GetId().Equals(whitePawn.GetId()), Is.True);
                Assert.That(turn1.NewPosition, Is.EqualTo(newPosition));
                Assert.That(turn1.TurnNumber, Is.EqualTo(1));
                Assert.That(turn1.PreviousPosition, Is.EqualTo(previousPosition));
                Assert.That(turn1.ChessBoard, Is.Not.Null);
                Assert.That(turn1.ChessBoard.Equals(board), Is.False); // should be a copy, not the same object reference
                Assert.That(turn1.PlayerTurn, Is.EqualTo(Turn.Color.WHITE));
            });
        }

        [Test]
        public void Test_ConstructTurn_Capture_Piece_Success()
        {
            ChessBoard board = new();
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();
            // find white pawn 1
            ChessPiece whitePawn = chessPieces.First(pieces => pieces.GetColor() == ChessPiece.Color.WHITE &&
                                                     pieces.GetPiece() == ChessPiece.Piece.PAWN);

            // set white pawn 1 to A6 as starting position
            board.PopulateBoard(chessPieces);
            whitePawn.Move(board, new("A6"));
            BoardPosition newPosition = new("B7");
            Assert.That(whitePawn.IsValidMove(board, newPosition), Is.True);

            Turn turn = new(1, whitePawn, newPosition, board, chessPieces);

            Assert.That(turn, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(chessPieces.Count, Is.EqualTo(32));
                Assert.That(turn.ChessPieces.Count, Is.EqualTo(31));

                List<ChessPiece> removedChessPieces = GetDifferenceBetweenLists(chessPieces, turn.ChessPieces);

                Assert.That(removedChessPieces.Count == 1);

                ChessPiece removedBlackPawn2Piece = removedChessPieces.First();
                Assert.That(removedBlackPawn2Piece.GetPiece() == ChessPiece.Piece.PAWN &&
                            removedBlackPawn2Piece.GetColor() == ChessPiece.Color.BLACK &&
                            removedBlackPawn2Piece.GetId() == 2);
            });

        }

        [Test]
        public void Test_ConstructTurn_Capture_Piece_EnPassant_Success()
        {
            ChessBoard board = new();
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();
            // find white pawn 1
            ChessPiece whitePawn = chessPieces.First(pieces => pieces.GetColor() == ChessPiece.Color.WHITE &&
                                                     pieces.GetPiece() == ChessPiece.Piece.PAWN);
            // find black pawn 2
            ChessPiece blackPawn2 = chessPieces.First(pieces => pieces.GetColor() == ChessPiece.Color.BLACK &&
                                         pieces.GetPiece() == ChessPiece.Piece.PAWN && pieces.GetId() == 2);
   
            GameController gc = new(board, chessPieces);
            gc.StartGame();

            // set white pawn 1 to A5 and black pawn 2 to B5 to setup en passant
            whitePawn.Move(board, new("A5"));
            blackPawn2.Move(board, new("B5"));
            (blackPawn2 as ChessPiecePawn).SetMovedTwoSquares();

            BoardPosition newPosition = new("B6");
            Assert.That(whitePawn.IsValidMove(board, newPosition), Is.True);

            Turn turn = new(1, whitePawn, newPosition, board, chessPieces);

            Assert.That(turn, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(chessPieces.Count, Is.EqualTo(32));
                Assert.That(turn.ChessPieces.Count, Is.EqualTo(31));

                List<ChessPiece> removedChessPieces = GetDifferenceBetweenLists(chessPieces, turn.ChessPieces);

                Assert.That(removedChessPieces.Count == 1);

                ChessPiece removedBlackPawn2Piece = removedChessPieces.First();
                Assert.That(removedBlackPawn2Piece.GetPiece() == ChessPiece.Piece.PAWN &&
                            removedBlackPawn2Piece.GetColor() == ChessPiece.Color.BLACK &&
                            removedBlackPawn2Piece.GetId() == 2);
            });

        }

    }
}
