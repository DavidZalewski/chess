using Chess;

namespace Tests
{
    public class ChessPieceKingTests
    {
        private readonly BoardPosition e1 = new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.E);
        private readonly BoardPosition e8 = new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.E);

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test_ConstructWhiteKing_Success()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.E));

            Assert.That(whiteKingPiece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(whiteKingPiece.GetCurrentPosition(), Is.EqualTo(e1));
                Assert.That(whiteKingPiece.GetRealValue(), Is.EqualTo(16));
                Assert.That(whiteKingPiece.GetId(), Is.EqualTo(1));
                Assert.That(whiteKingPiece.GetColor(), Is.EqualTo(ChessPiece.Color.WHITE));
                Assert.That(whiteKingPiece.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(whiteKingPiece.GetStartingPosition().EqualTo(whiteKingPiece.GetCurrentPosition()), Is.True);
            });
        }

        [Test]
        public void Test_ConstructBlackKing_Success()
        {
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.E));
            Assert.That(blackKingPiece, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(blackKingPiece.GetCurrentPosition().EqualTo(e8), Is.True);
                Assert.That(blackKingPiece.GetRealValue(), Is.EqualTo(26));
                Assert.That(blackKingPiece.GetId(), Is.EqualTo(1));
                Assert.That(blackKingPiece.GetColor(), Is.EqualTo(ChessPiece.Color.BLACK));
                Assert.That(blackKingPiece.GetPiece(), Is.EqualTo(ChessPiece.Piece.KING));
                Assert.That(blackKingPiece.GetStartingPosition().EqualTo(blackKingPiece.GetCurrentPosition()), Is.True);
            });
        }

        private ChessBoard InitializeFullBoard()
        {
            ChessBoard chessBoard = new ChessBoard();
            int[,] boardValue = new int[8, 8]
            {
                { 24 /*A8*/, 22, 23, 25, 26, 23, 22, 24 /*H8*/},
                { 21, 21, 21, 21, 21, 21, 21, 21 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 11, 11, 11, 11, 11, 11, 11, 11 },
                { 14 /*A1*/, 12, 13, 15, 16, 13, 12, 11 /*H1*/},
            };
            chessBoard.InternalTestOnly_SetBoard(boardValue);

            return chessBoard;
        }

        [Test(Description = "Tests that the white king has no valid moves it can make at the start of the game")]
        public void Test_WhiteKing_IsValidMove_NoValidMovesFromStart()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.E));
            ChessBoard chessBoard = InitializeFullBoard();
            List<BoardPosition> possibleMoves = new();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possibleMoves.Add(new BoardPosition((BoardPosition.VERTICAL)i, (BoardPosition.HORIZONTAL)j));
                }
            }

            Assert.Multiple(() =>
            {
                foreach (BoardPosition possibleMove in possibleMoves)
                {
                    Assert.That(whiteKingPiece.IsValidMove(chessBoard, possibleMove), Is.False, possibleMove.StringValue);
                }
            });
        }

        [Test(Description = "Tests that the black king has no valid moves it can make at the start of the game")]
        public void Test_BlackKing_IsValidMove_NoValidMovesFromStart()
        {
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.E));
            ChessBoard chessBoard = InitializeFullBoard();
            List<BoardPosition> possibleMoves = new();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possibleMoves.Add(new BoardPosition((BoardPosition.VERTICAL)i, (BoardPosition.HORIZONTAL)j));
                }
            }

            Assert.Multiple(() =>
            {
                foreach (BoardPosition possibleMove in possibleMoves)
                {
                    Assert.That(blackKingPiece.IsValidMove(chessBoard, possibleMove), Is.False);
                }
            });
        }

        [Test(Description = "Tests that white or black king on empty board starting on D4 can move to all valid positions")]
        public void Test_King_IsValidMove_ValidMovesFromD4()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D));
            ChessBoard chessBoard = new(); // blank chess board with 2 kings on it
            List<BoardPosition> validMoves = new()
            {
                new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.D), // D3
                new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.C), // C3
                new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.E), // E3
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.C), // C4
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E), // E4
                new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.D), // D5
                new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.C), // C5
                new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.E), // E5
            };

            Assert.Multiple(() =>
            {
                foreach (BoardPosition boardPosition in validMoves)
                {
                    Assert.That(whiteKingPiece.IsValidMove(chessBoard, boardPosition), Is.True);
                    Assert.That(blackKingPiece.IsValidMove(chessBoard, boardPosition), Is.True);
                }
            });
        }

        [Test(Description = "Tests that white or black king on empty board starting on D4 cannot move to these positions")]
        public void Test_King_IsValidMove_InvalidMovesFromD4()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D));
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D));
            ChessBoard chessBoard = new(); // blank chess board with 2 kings on it
            List<BoardPosition> validMoves = new()
            {
                new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.D), // D3
                new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.C), // C3
                new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.E), // E3
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.C), // C4
                new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E), // E4
                new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.D), // D5
                new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.C), // C5
                new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.E), // E5
            };
            List<BoardPosition> possibleMoves = new();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    possibleMoves.Add(new BoardPosition((BoardPosition.VERTICAL)i, (BoardPosition.HORIZONTAL)j));
                }
            }

            Assert.Multiple(() =>
            {
                int assertsInvoked = 0;
                foreach (BoardPosition position in possibleMoves)
                {
                    if (!validMoves.Any(vm => vm.HorizontalValue == position.HorizontalValue && vm.VerticalValue == position.VerticalValue))
                    {
                        Assert.That(whiteKingPiece.IsValidMove(chessBoard, position), Is.False);
                        Assert.That(blackKingPiece.IsValidMove(chessBoard, position), Is.False);
                        assertsInvoked++;
                    }
                }
                Assert.That(assertsInvoked > 0, Is.True);
            });
        }


        [Test(Description = "Tests that a white king on E3 cannot move to E4 or D4 when there is a black king on D5")]
        public void Test_WhiteKing_IsValidMove_NoValidMovesBlackKingChecks()
        {
            ChessPiece whiteKingPiece = new ChessPieceKing(ChessPiece.Color.WHITE, new(BoardPosition.VERTICAL.ONE, BoardPosition.HORIZONTAL.E));

            ChessBoard chessBoard = new(); // blank chess board with 2 kings on it
            // set black king on D5 and white king on E3 on chess board object
            chessBoard.SetBoardValue(new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.D), 26);
            chessBoard.SetBoardValue(new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.E), 16);
            
            // set white king on E3 in its class
            whiteKingPiece.SetCurrentPosition(new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.E));

            Assert.Multiple(() =>
            {
                Assert.That(whiteKingPiece.IsValidMove(chessBoard, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D)), Is.False);
                Assert.That(whiteKingPiece.IsValidMove(chessBoard, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E)), Is.False);
            });
        }

        [Test(Description = "Tests that a black king on D5 cannot move to E4 or D4 when there is a white king on E3")]
        public void Test_BlackKing_IsValidMove_NoValidMovesWhiteKingChecks()
        {
            ChessPiece blackKingPiece = new ChessPieceKing(ChessPiece.Color.BLACK, new(BoardPosition.VERTICAL.EIGHT, BoardPosition.HORIZONTAL.E));
            ChessBoard chessBoard = new(); // blank chess board with 2 kings on it
            // set black king on D5 and white king on E3 on chess board object
            chessBoard.SetBoardValue(new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.D), 26);
            chessBoard.SetBoardValue(new BoardPosition(BoardPosition.VERTICAL.THREE, BoardPosition.HORIZONTAL.E), 16);

            // set black king on D5 in its class
            blackKingPiece.SetCurrentPosition(new BoardPosition(BoardPosition.VERTICAL.FIVE, BoardPosition.HORIZONTAL.D));

            Assert.Multiple(() =>
            {
                Assert.That(blackKingPiece.IsValidMove(chessBoard, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.D)), Is.False);
                Assert.That(blackKingPiece.IsValidMove(chessBoard, new BoardPosition(BoardPosition.VERTICAL.FOUR, BoardPosition.HORIZONTAL.E)), Is.False);
            });
        }

        [Test(Description = "Tests that both white king and black king on their starting squares with no other pieces on board can move until a point where both would be in check from each other")]
        public void Test_Kings_IsValidMove_Valid_UntilKingsWouldCheckEachOther()
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
        public void Test_WhiteKing_IsValidMove_Invalid_WouldBeCheckedByBlackRookOnD8()
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
