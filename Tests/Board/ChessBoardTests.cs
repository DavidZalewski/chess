using Chess.Board;
using Chess.Pieces;
using Chess.Services;
using static Chess.Pieces.ChessPiece;

namespace Tests.Board
{
    public class ChessBoardTests
    {
        private ChessBoard chessBoard = new();

        [SetUp]
        public void Setup()
        {
            chessBoard = new ChessBoard();
        }

        [Test]
        public void Test_CopyConstructChessBoard_Success()
        {
            ChessBoard board1 = new();
            ChessBoard board2 = new(board1);

            Assert.Multiple(() =>
            {
                Assert.That(board1, Is.Not.Null); // pass
                Assert.That(board2, Is.Not.Null); // pass

                //Assert.That(board1, Is.EqualTo(board2)); // pass???
                Assert.That(board1 != board2); // pass 
                Assert.That(ReferenceEquals(board1, board2), Is.False); // pass
                //Assert.That(board1, Is.Not.EqualTo(board2)); // fail???

                Assert.That(board1.Board, Is.EqualTo(board2.Board));
                Assert.That(Object.ReferenceEquals(board1.Board,board2.Board), Is.False);
            });
        }

        [Test]
        public void Test_IsPieceAtPosition_Success()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.THREE, FILE.F);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 11);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.True);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColor_Success1()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.THREE, FILE.F);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 11);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.WHITE), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK), Is.False);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColor_Success2()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.SIX, FILE.G);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 21);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.WHITE), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK), Is.True);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColorAndPiece_Success1()
        {
            BoardPosition boardPosition = new("G6");
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 21);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.WHITE), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK, ChessPiece.Piece.PAWN), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK, ChessPiece.Piece.ROOK), Is.False);
        }

        [Test]
        public void Test_IsPieceAtPosition_SpecificColorAndPiece_Success2()
        {
            BoardPosition boardPosition = new("G6");
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 24);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.WHITE), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK, ChessPiece.Piece.PAWN), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK, ChessPiece.Piece.ROOK), Is.True);
        }

        [Test]
        public void Test_PopulateBoard_Success()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPieces();
            chessBoard.PopulateBoard(chessPieces);
            Square[,] board = chessBoard.Board;

            Assert.Multiple(() =>
            {
                // iterate through all created chess pieces, and assert that they exist on the board in their correct places
                foreach(ChessPiece piece in chessPieces)
                {
                    BoardPosition boardPosition = piece.GetStartingPosition();
                    Square square = board[boardPosition.RankAsInt, boardPosition.FileAsInt];
                    Assert.That(square, Is.Not.Null);
                    Assert.That(square.Piece.Equals(piece));
                }

                // iterate through ranks 3 through 6 and assert that all pieces are of NoPiece here
                for (int rank = (int)RANK.THREE; rank <= (int)RANK.SIX; rank++)
                {
                    for (int file = (int)FILE.A; file <= (int)FILE.H; file++)
                    {
                        Square square = board[rank, file];
                        Assert.That(square, Is.Not.Null);
                        Assert.That(square.Piece.Equals(NoPiece.Instance));
                    }
                }
            });
        }

        [Test]
        public void Test_RemoveCapturedPieces_Success()
        {
            ChessPiece whiteQueenPiece = new ChessPieceQueen(Color.WHITE, 1, new("D1"));
            ChessPiece whiteKingPiece = new ChessPieceKing(Color.WHITE, new("E1"));
            ChessPiece whiteBishopPiece = new ChessPieceBishop(Color.WHITE, 2, new("F1"));
            ChessPiece whitePawn4Piece = new ChessPieceWhitePawn(4, new("D2"));
            ChessPiece whitePawn5Piece = new ChessPieceWhitePawn(4, new("E2"));
            ChessPiece whitePawn6Piece = new ChessPieceWhitePawn(4, new("F2"));
            ChessPiece blackBishopPiece = new ChessPieceBishop(Color.BLACK, 1, new("A5"));
            ChessPiece blackRookPiece = new ChessPieceRook(Color.BLACK, 1, new("A1"));
            ChessPiece blackKnightPiece = new ChessPieceKnight(Color.BLACK, 1, new("F3"));
            ChessPiece blackPawn1Piece = new ChessPieceBlackPawn(1, new("F5"));

            List<ChessPiece> chessPiecesOnBoard = new()
            {
                whiteQueenPiece, whiteKingPiece, whiteBishopPiece, whitePawn4Piece,
                whitePawn5Piece, whitePawn6Piece, blackBishopPiece, blackRookPiece
            };

            chessBoard.PopulateBoard(chessPiecesOnBoard);

            List<ChessPiece> allChessPieces = new()
            {
                whiteQueenPiece, whiteKingPiece, whiteBishopPiece, whitePawn4Piece,
                whitePawn5Piece, whitePawn6Piece, blackBishopPiece, blackRookPiece, blackKnightPiece, blackPawn1Piece
            };

            List<ChessPiece> prunedPieces = chessBoard.RemovedCapturedPieces(allChessPieces, lcp => true);

            Assert.That(prunedPieces.SequenceEqual(chessPiecesOnBoard), Is.True);
        }

    }
}