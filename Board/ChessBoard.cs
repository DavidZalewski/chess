using Chess.Pieces;
using Chess.Services;
using NUnit.Framework;

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

        public void AddPiece(ChessPiece piece)
        {
            GetSquare(piece.GetCurrentPosition()).Piece = piece;
        }

        public string DisplayBoard()
        {
            string output = "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n";
            int vertIndex = 8;

            for (int f = 0; f < 8; f++)
            {
                output += vertIndex;
                for (int s = 0; s < 8; s++)
                {
                    if (Board[f, s].Piece is not NoPiece) // Check for chess piece
                    {
                        ChessPiece chessPiece = Board[f, s].Piece; // Get the piece directly 
                        Assert.That(chessPiece is not NoPiece);
                        String c = "", p = "", i = "";
                        switch (chessPiece.GetColor())
                        {
                            case ChessPiece.Color.WHITE: c = "W"; break;
                            case ChessPiece.Color.BLACK: c = "B"; break;
                        }

                        switch (chessPiece.GetPiece())
                        {
                            case ChessPiece.Piece.PAWN: p = "P"; break;
                            case ChessPiece.Piece.KNIGHT: p = "K"; break;
                            case ChessPiece.Piece.BISHOP: p = "B"; break;
                            case ChessPiece.Piece.ROOK: p = "R"; break;
                            case ChessPiece.Piece.QUEEN: p = "Q"; break;
                            case ChessPiece.Piece.KING:
                                {
                                    p = "K"; i = ""; break;
                                }
                        }

                        if (!chessPiece.GetPiece().Equals(ChessPiece.Piece.KING))
                        {
                            i = chessPiece.GetId().ToString();
                        }
                        else
                        {
                            i = "0";
                        }

                        output += "|" + c + p + i;
                    }
                    else
                    {
                        output += "|   ";
                    }
                }
                output += "|" + vertIndex + "\n";
                vertIndex--;
            }
            output += "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n";
            return output;
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
