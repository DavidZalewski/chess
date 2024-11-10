using Chess.Attributes;
using Chess.Globals;
using Chess.Pieces;
using Chess.Services;
using NUnit.Framework;
using static Chess.Pieces.ChessPiece;

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
        public string BoardID { get; private set; } = string.Empty;

        public ChessBoard()
        {
            StaticLogger.Trace();
            Board = new Square[8, 8];
            InitializeBoard();
            GenerateBoardID();
            StaticLogger.LogObject(this);
        }

        public ChessBoard(ChessBoard? other)
        {
            StaticLogger.Trace();
            if (other == null || other.Board == null)
            {
                StaticLogger.Log("other argument is null for copy constructor - throwing ArgumentNullException", LogLevel.Error);
                throw new ArgumentNullException(nameof(other));
            }

            StaticLogger.LogObject(other, "Dumping other");
            Board = new Square[8, 8]; // Create a new array

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Board[row, col] = new Square(other.Board[row, col]); // Copy each Square
                }
            }
            StaticLogger.LogObject(this, "Copy Construct Complete");
        }

        public ChessBoard(string boardID)
        {
            StaticLogger.Trace();
            Board = new Square[8, 8];
            InitializeBoard();
            int row = 0, file = 0;
            int whiteQueenID = 1;
            int blackQueenID = 1;
            int whiteRookID = 1;
            int blackRookID = 1;
            int whiteBishopID = 1;
            int blackBishopID = 1;
            int whiteKnightID = 1;
            int blackKnightID = 1;
            int whitePawnID = 1;
            int blackPawnID = 1;
            foreach(char c in boardID)
            {
                ChessPiece piece = NoPiece.Instance;
                Square square = Board[row, file];
                switch(c)
                {
                    case '1':
                        {
                            piece = new ChessPieceKing(Color.WHITE, square.Position);

                            break;
                        }
                    case '2':
                        {
                            piece = new ChessPieceKing(Color.BLACK, square.Position);
                            break;
                        }
                    case '3':
                        {
                            piece = new ChessPieceQueen(Color.WHITE, whiteQueenID, square.Position);
                            ++whiteQueenID;
                            break;
                        }
                    case '4':
                        {
                            piece = new ChessPieceQueen(Color.BLACK, blackQueenID, square.Position);
                            ++blackQueenID;
                            break;
                        }
                    case '5':
                        {
                            piece = new ChessPieceRook(Color.WHITE, whiteRookID, square.Position);
                            ++whiteRookID;
                            break;
                        }
                    case '6':
                        {
                            piece = new ChessPieceRook(Color.BLACK, blackRookID, square.Position);
                            ++blackRookID;
                            break;
                        }
                    case '7':
                        {
                            piece = new ChessPieceBishop(Color.WHITE, whiteBishopID, square.Position);
                            ++whiteBishopID;
                            break;
                        }
                    case '8':
                        {
                            piece = new ChessPieceBishop(Color.BLACK, blackBishopID, square.Position);
                            ++blackBishopID;
                            break;
                        }
                    case '9':
                        {
                            piece = new ChessPieceKnight(Color.WHITE, whiteKnightID, square.Position);
                            ++whiteKnightID;
                            break;
                        }
                    case 'A':
                        {
                            piece = new ChessPieceKnight(Color.BLACK, blackKnightID, square.Position);
                            ++blackKnightID;
                            break;
                        }
                    case 'B':
                        {
                            piece = new ChessPieceWhitePawn(whitePawnID, square.Position);
                            ++whitePawnID;
                            break;
                        }
                    case 'C':
                        {
                            piece = new ChessPieceBlackPawn(blackPawnID, square.Position);
                            ++blackPawnID;
                            break;
                        }
                }

                if (piece is not NoPiece)
                    piece.Move(this, piece.GetStartingPosition());

                if (file == 7)
                {
                    file = 0;
                    row++;
                }
                else
                {
                    file++;
                }
            }
        }

        [ToDo("Change this back to private later")]
        public void GenerateBoardID()
        {
            StaticLogger.Trace();
            string id = string.Empty;
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Square square = Board[row, col];
                    if (square.Piece is NoPiece)
                    {
                        id += "0";
                    }
                    else
                    {
                        ChessPiece piece = square.Piece;
                        Piece pieceType = piece.GetPiece();
                        Color pieceColor = piece.GetColor();
                        switch (pieceType)
                        {
                            case Piece.KING:
                                id += pieceColor == Color.WHITE ? "1" : "2";
                                break;
                            case Piece.QUEEN:
                                id += pieceColor == Color.WHITE ? "3" : "4";
                                break;
                            case Piece.ROOK:
                                id += pieceColor == Color.WHITE ? "5" : "6";
                                break;
                            case Piece.BISHOP:
                                id += pieceColor == Color.WHITE ? "7" : "8";
                                break;
                            case Piece.KNIGHT:
                                id += pieceColor == Color.WHITE ? "9" : "A";
                                break;
                            case Piece.PAWN:
                                id += pieceColor == Color.WHITE ? "B" : "C";
                                break;
                        }
                    }
                }
            }
            BoardID = id;
            StaticLogger.Log($"BoardID: {BoardID}", LogLevel.Debug);
        }

        private void InitializeBoard()
        {
            StaticLogger.Trace();
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
            StaticLogger.Trace();
            return Board
                .Cast<Square>()
                .Select(square => square.Piece)
                .Where(piece => piece is not NoPiece)
                .ToList();
        }

        public bool PopulateBoard(List<ChessPiece> chessPieces)
        {
            StaticLogger.Trace();
            foreach (ChessPiece piece in chessPieces)
            {
                piece.Move(this, piece.GetStartingPosition());
            }
            GenerateBoardID();
            return true;
        }

        public bool SetBoardValue(BoardPosition position, int value)
        {
            StaticLogger.Trace();
            ChessPiece piece = ChessPieceFactory.CreatePieceFromInt(position, value);
            Square square = new() { Position = position, Piece = piece };
            SetSquareValue(position, square);
            GenerateBoardID();
            return true;
        }

        public bool IsPieceAtPosition(BoardPosition position)
        {
            StaticLogger.Trace();
            return GetSquare(position).Piece is not NoPiece;
        }

        public bool IsPieceAtPosition(BoardPosition position, ChessPiece.Color color)
        {
            StaticLogger.Trace();
            if (!IsPositionWithinBounds(position))
            {
                return false; // index out of bounds
            }
            ChessPiece piece = GetSquare(position).Piece;
            return (piece is not NoPiece && piece.GetColor() == color);
        }

        public bool IsPieceAtPosition(BoardPosition position, ChessPiece.Color color, ChessPiece.Piece pieceType)
        {
            StaticLogger.Trace();
            ChessPiece piece = GetSquare(position).Piece;
            return (piece is not NoPiece && piece.GetColor() == color && piece.GetPiece() == pieceType);
        }

        public void SetPieceAtPosition(BoardPosition position, ChessPiece piece)
        {
            StaticLogger.Trace();
            GetSquare(position).Piece = piece;
            piece.SetCurrentPosition(position);
            GenerateBoardID();
        }

        public void AddPiece(ChessPiece piece)
        {
            StaticLogger.Trace();
            piece.Move(this, piece.GetStartingPosition());
            GetSquare(piece.GetCurrentPosition()).Piece = piece;
            GenerateBoardID();
        }


        public string DisplayBoard()
        {
            StaticLogger.Trace();
            string output = "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n";
            int arraySize = 8;
            int rankNumber = arraySize;

            for (int f = 0; f < arraySize; f++)
            {
                output += rankNumber;
                for (int s = 0; s < arraySize; s++)
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
                            case ChessPiece.Color.NONE: c = "X"; break;
                        }

                        switch (chessPiece.GetPiece())
                        {
                            case ChessPiece.Piece.PAWN: p = "P"; break;
                            case ChessPiece.Piece.KNIGHT: p = "N"; break;
                            case ChessPiece.Piece.BISHOP: p = "B"; break;
                            case ChessPiece.Piece.ROOK: p = "R"; break;
                            case ChessPiece.Piece.QUEEN: p = "Q"; break;
                            case ChessPiece.Piece.KING:
                                {
                                    p = "K"; i = ""; break;
                                }
                            case ChessPiece.Piece.NO_PIECE:
                                {
                                    p = "X"; i = "X"; break;
                                }
                        }

                        if (!chessPiece.GetPiece().Equals(ChessPiece.Piece.KING))
                        {
                            i = chessPiece.GetId().ToString();
                        }
                        else
                        {
                            i = " ";
                        }

                        output += "|" + c + p + i;
                    }
                    else
                    {
                        output += "|   ";
                    }
                }
                output += "|" + rankNumber + "\n";
                rankNumber--;
            }
            output += "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n";
            return output;
        }

        public void SetSquareValue(BoardPosition position, Square square)
        {
            StaticLogger.Trace();
            StaticLogger.LogMethod(position, square);
            Board[position.RankAsInt, position.FileAsInt] = square;
            StaticLogger.LogObject(square);
            GenerateBoardID();
        }

        public Square GetSquare(BoardPosition position)
        {
            StaticLogger.Trace();
            return Board[position.RankAsInt, position.FileAsInt];
        }

        public bool IsPositionWithinBounds(BoardPosition position)
        {
            StaticLogger.Trace();
            if (position is null) return false;
            return (position.RankAsInt >= 0 && position.RankAsInt <= 7) || (position.FileAsInt >= 0 && position.FileAsInt <= 7);
        }
    }
}
