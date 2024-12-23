using Chess;
using Chess.Board;
using Chess.Pieces;
using Chess.Services;
using System.Collections.Generic;
using static Chess.Pieces.ChessPiece;

namespace Tests.Board
{
    [Category("CORE")]
    public class ChessBoardTests : TestBase
    {
        private ChessBoard chessBoard = new();

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            chessBoard = new ChessBoard();
        }

        [Test]
        public void CopyConstructChessBoard_Success()
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
        public void ChessBoardCopyConstructor_NullArgument_ThrowsArgumentNullException()
        {
            Assert.That(() => new ChessBoard((ChessBoard)null), Throws.ArgumentNullException);
        }

        [Test]
        public void ChessBoardConstructorBoardID_Constructs_Proper_Board_State()
        {
            string BoardID = "0A00200060000CCC00080A00C0C00000000C000B9B0B00B0B7B00B7050000010";
            ChessBoard board = new(BoardID);
            Console.WriteLine(board.DisplayBoard());
        }

        [Test]
        public void IsPieceAtPosition_Success()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.THREE, FILE.F);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 11);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.True);
        }

        [Test]
        public void IsPieceAtPosition_SpecificColor_Success1()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.THREE, FILE.F);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 11);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.WHITE), Is.True);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK), Is.False);
        }

        [Test]
        public void IsPieceAtPosition_SpecificColor_Success2()
        {
            BoardPosition boardPosition = new BoardPosition(RANK.SIX, FILE.G);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition), Is.False);
            chessBoard.SetBoardValue(boardPosition, 21);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.WHITE), Is.False);
            Assert.That(chessBoard.IsPieceAtPosition(boardPosition, Color.BLACK), Is.True);
        }

        [Test]
        public void IsPieceAtPosition_SpecificColorAndPiece_Success1()
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
        public void IsPieceAtPosition_SpecificColorAndPiece_Success2()
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
        public void PopulateBoard_Success()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
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
        public void SetPieceAtPosition_PieceExistsInCorrectSquare()
        {
            // Arrange
            BoardPosition a1 = new("A1");
            BoardPosition d4 = new("D4");
            BoardPosition h8 = new("H8");
            var positions = new[] { a1, d4, h8 };
            var pieces = new ChessPiece[] { new ChessPieceRook(Color.WHITE, 1, a1),
                              new ChessPieceQueen(Color.BLACK, 1, d4),
                              new ChessPieceBishop(Color.BLACK, 2, h8) };

            for (int i = 0; i < positions.Length; i++)
            {
                // Act
                chessBoard.SetPieceAtPosition(positions[i], pieces[i]);
                var square = chessBoard.Board[positions[i].RankAsInt, positions[i].FileAsInt];

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(square.Piece, Is.EqualTo(pieces[i]), $"Piece not set correctly at {positions[i]}");
                    Assert.That(square.Piece.GetCurrentPosition(), Is.EqualTo(positions[i]), $"Piece position not updated correctly at {positions[i]}");
                });
            }
        }

        [Test]
        public void GetActivePieces_NewBoard_Returns32Pieces()
        {
            // Arrange
            ChessBoard board = new();
            board.PopulateBoard(ChessPieceFactory.CreateChessPiecesClassic());

            // Act
            List<ChessPiece> activePieces = board.GetActivePieces();

            // Assert
            Assert.That(activePieces.Count == 32);
        }

        [Test]
        public void GetActivePieces_RemovedPieces_ReturnsCorrectCount()
        {
            // Arrange
            ChessBoard board = new ChessBoard();
            board.PopulateBoard(ChessPieceFactory.CreateChessPiecesClassic());

            // Act
            board.SetPieceAtPosition(new ("A1"), NoPiece.Instance);
            board.SetPieceAtPosition(new ("B1"), NoPiece.Instance);
            board.SetPieceAtPosition(new ("C1"), NoPiece.Instance);

            List<ChessPiece> activePieces = board.GetActivePieces();

            // Assert
            Assert.That(activePieces.Count == 29);
        }

        [Test]
        public void DisplayBoard_Success()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
            ChessBoard chessBoard = new();
            chessBoard.PopulateBoard(chessPieces);

            String boardAsConsoleOutput = chessBoard.DisplayBoard();

            Assert.That(boardAsConsoleOutput, Is.Not.Null);

            Console.WriteLine(boardAsConsoleOutput);
        }

        [Test]
        public void BoardID_InitialValue_EmptyString()
        {
            Assert.That(chessBoard.BoardID.Equals("0000000000000000000000000000000000000000000000000000000000000000"));
        }

        [Test]
        public void BoardID_AfterPopulateBoard_NotEmptyString()
        {
            List<ChessPiece> chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
            chessBoard.PopulateBoard(chessPieces);
            Console.WriteLine($"initialBoardID: {chessBoard.BoardID}");
            Assert.That(chessBoard.BoardID, Is.Not.Empty);
            Assert.That(chessBoard.BoardID.Length, Is.EqualTo(64));
        }

        [Test]
        public void BoardID_AfterSetPieceAtPosition_UpdatesBoardID()
        {
            BoardPosition position = new BoardPosition(RANK.ONE, FILE.A);
            ChessPiece piece = new ChessPieceKing(Color.WHITE, position);
            chessBoard.SetPieceAtPosition(position, piece);
            string initialBoardID = chessBoard.BoardID;

            Console.WriteLine($"initialBoardID: {initialBoardID}");

            position = new BoardPosition(RANK.ONE, FILE.B);
            piece = new ChessPieceQueen(Color.WHITE, 1, position);
            chessBoard.SetPieceAtPosition(position, piece);
            string updatedBoardID = chessBoard.BoardID;

            Console.WriteLine($"updatedBoardID: {updatedBoardID}");

            Assert.That(initialBoardID, Is.Not.EqualTo(updatedBoardID));   
        }

        [Test]
        public void BoardID_DefaultBoard_ReturnsCorrectID()
        {
            // Arrange
            ChessBoard chessBoard = new ChessBoard();

            // Act
            string boardID = chessBoard.BoardID;

            // Assert
            Assert.That(boardID.Length, Is.EqualTo(64));
            Assert.That(boardID, Is.EqualTo(new string('0', 64)));
        }
    }
}