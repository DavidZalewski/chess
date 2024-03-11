using Chess.Pieces;
using Chess.Services;

namespace Chess.Board
{
    [Serializable]
    public class ChessBoard
    {
        // the last inner array is assumed to be the starting position
        // looking at diagrams where the board is labelled, white is always on the bottom
        // therefore first value in array [0,0] is A8
        // A1 is found at [7,0]

        // This code is kept for historical reasons, so that at a glance we can see how this board is abstracted in its raw presentation
        // All future code beyond this point simply encapsulates this internal representation in an intuitive way
        //private int[,] _board = new int[8, 8]
        //{
        //    { 0 /*A8*/, 0, 0, 0, 0, 0, 0, 0 /*H8*/},
        //    { 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0 /*A1*/, 0, 0, 0, 0, 0, 0, 0 /*H1*/},
        //};

        public Square[,] Board { get; set; }

        public ChessBoard() 
        {
            Board = new Square[8, 8];
            InitializeBoard();
        }

        public ChessBoard(ChessBoard? other)
        {
            if (other == null || other.Board == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Board = new Square[8, 8]; // Create a new array

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Board[row, col] = new Square(other.Board[row, col]); // Copy each Square
                }
            }
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Board[row, col] = new Square()
                    {
                        Position = new BoardPosition((RANK)row, (FILE)col),
                        Piece = NoPiece.Instance
                    };
                }
            }
        }

        // TODO: Create Test for this
        public List<ChessPiece> GetActivePieces()
        {
            return Board
                .Cast<Square>()
                .Select(square => square.Piece)
                .Where(piece => piece is not NoPiece)
                .ToArray()
                .ToList();
        }

        public bool PopulateBoard(List<ChessPiece> chessPieces)
        {
            foreach (ChessPiece piece in chessPieces)
            {
                piece.Move(this, piece.GetStartingPosition());
            }

            return true;
        }

        public bool SetBoardValue(BoardPosition position, int value)
        {
            // Quick placeholder logic during the change
            ChessPiece piece = ChessPieceFactory.CreatePieceFromInt(position, value);
            Square square = new() { Position = position, Piece = piece };
            SetSquareValue(position, square);
            return true;
        }

        public bool IsPieceAtPosition(BoardPosition position)
        {
            return GetSquare(position).Piece is not NoPiece;
        }

        public bool IsPieceAtPosition(BoardPosition position, ChessPiece.Color color)
        {
            if ((position.RankAsInt < 0 || position.RankAsInt > 7) ||
                (position.FileAsInt < 0 || position.FileAsInt > 7))
            {
                return false; // index out of bounds
            }
            ChessPiece piece = GetSquare(position).Piece;
            return (piece is not NoPiece && piece.GetColor() == color);
        }

        public bool IsPieceAtPosition(BoardPosition position, ChessPiece.Color color, ChessPiece.Piece pieceType)
        {
            ChessPiece piece = GetSquare(position).Piece;
            return (piece is not NoPiece && piece.GetColor() == color && piece.GetPiece() == pieceType);
        }

        public void SetPieceAtPosition(BoardPosition position, ChessPiece piece)
        {
            GetSquare(position).Piece = piece;
            piece.SetCurrentPosition(position);
        }

        private void SetSquareValue(BoardPosition position, Square square)
        {
            Board[position.RankAsInt, position.FileAsInt] = square;
        }

        private Square GetSquare(BoardPosition position)
        {
            return Board[position.RankAsInt, position.FileAsInt];
        }
    }
}
