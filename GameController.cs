using Chess.Board;
using Chess.Callbacks;
using Chess.Exceptions;
using Chess.Pieces;
using Chess.Services;
using NUnit.Framework;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chess
{
    [Serializable]
    public class GameController
    {
        private ChessBoard _chessBoard;
        private KingCheckService _kingCheckService;
        private List<ChessPiece> _chessPieces = new();
        private List<Turn> _turns = new();
        private int _turnNumber = 0;

        public int TurnNumber { get => _turnNumber; }

        public GameController(ChessBoard chessBoard)
        {
            this._chessBoard = chessBoard;
            _chessPieces = ChessPieceFactory.CreateChessPieces();
            this._kingCheckService = new KingCheckService();
        }

        public GameController(ChessBoard chessBoard, List<ChessPiece> chessPieces)
        {
            this._chessBoard = chessBoard;
            this._chessPieces = chessPieces;
            this._kingCheckService = new KingCheckService();
        }

        public void StartGame()
        {
            _chessBoard.PopulateBoard(_chessPieces);
            // TODO:
            // Why is GameController even registering callbacks?
            // The Turn object is what handles the state of the game
            // Does this actually need to exist?
            ChessPiece.SetCastleCallbackFunction(SpecialMovesHandlers.DoCastleMove);
            ChessPiece.SetIsEnPassantCallbackFunction(SpecialMovesHandlers.IsEnPassantMove);
            _turnNumber = 1;
        }



        public void ParseMove(String consoleInput)
        {

        }

        internal ChessPiece? FindChessPieceFromString(string input)
        {
            ChessPiece.Color color;
            ChessPiece.Piece piece;
            int id = 0;

            char[] chars = input.ToCharArray();

            if (chars.Length != 2 && chars.Length != 3)
                return null;

            switch (chars[0])
            {
                case 'W':
                    {
                        color = ChessPiece.Color.WHITE;
                        break;
                    }
                case 'B':
                    {
                        color = ChessPiece.Color.BLACK;
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }

            switch (chars[1])
            {
                case 'P':
                    {
                        piece = ChessPiece.Piece.PAWN;
                        break;
                    }
                case 'K':
                    {
                        if (chars.Length == 3)
                            piece = ChessPiece.Piece.KNIGHT;
                        else if (chars.Length == 2)
                        {
                            piece = ChessPiece.Piece.KING;
                            id = 1;
                        }
                        else
                            return null;
                        break;
                    }
                case 'B':
                    {
                        piece = ChessPiece.Piece.BISHOP;
                        break;
                    }
                case 'R':
                    {
                        piece = ChessPiece.Piece.ROOK;
                        break;
                    }
                case 'Q':
                    {
                        piece = ChessPiece.Piece.QUEEN;
                        break;
                    }
                default: return null;
            }

            if (chars.Length == 3)
                if (!int.TryParse(chars[2].ToString(), out id))
                    return null;

            ChessPiece chessPiece = _chessPieces.First(p => p.GetColor().Equals(color) && p.GetPiece().Equals(piece) && p.GetId().Equals(id));

            return chessPiece;
        }

        // TODO: Refactor this. Why do we have duplicate InCheck methods, one implemented here, other using service?
        public bool IsKingInCheck(ChessPiece.Color color)
        {
            ChessPiece chessPieceKing = _chessPieces.First(p => p.GetPiece().Equals(ChessPiece.Piece.KING) && p.GetColor().Equals(color));
            if (chessPieceKing == null)
                return false;
            if (chessPieceKing.GetColor().Equals(ChessPiece.Color.WHITE))
            {
                bool IsInCheck = _chessPieces.Any(p => p.GetColor().Equals(ChessPiece.Color.BLACK) && p.IsValidMove(_chessBoard, chessPieceKing.GetCurrentPosition()));
                return IsInCheck;
            }
            else
            {
                bool IsInCheck = _chessPieces.Any(p => p.GetColor().Equals(ChessPiece.Color.WHITE) && p.IsValidMove(_chessBoard, chessPieceKing.GetCurrentPosition()));
                return IsInCheck;
            }
        }

        public bool IsCheck(Turn turn)
        {
            return _kingCheckService.IsKingInCheck(turn);
        }

        public bool IsCheckMate(Turn turn)
        {
            return _kingCheckService.IsCheckMate(turn);
        }

        public Turn? GetTurnFromCommand(string input)
        {
            String[] inputs = input.Split(' ');
            if (inputs.Length != 2) return null;
            try
            {
                ChessPiece? chessPiece = FindChessPieceFromString(inputs[0]);

                if (chessPiece == null) return null;

                Turn turn = new(_turnNumber, chessPiece, new(inputs[1]), _chessBoard);
                return turn;
            }
            catch (InvalidMoveException e)
            {
                Console.WriteLine("Invalid Move. Please Try Again.");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void ApplyTurnToGameState(Turn turn)
        {
            _chessBoard = turn.ChessBoard;
            _chessPieces = turn.ChessPieces;
            _turns.Add(turn);
            _turnNumber++;
        }

        public ChessBoard GetChessBoard() { return _chessBoard; }

        public bool SaveGameState(string saveFileName)
        {
            string dir = Directory.GetCurrentDirectory();

            string saveFile = Path.Combine(dir, saveFileName);
            Stream? stream = null;
            try
            {
                // Opens a file and serializes the object into it in binary format.
                stream = File.Open(saveFile, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();

#pragma warning disable SYSLIB0011 // Type or member is obsolete
                formatter.Serialize(stream, this);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                //string jsonGameController = Newtonsoft.Json.JsonConvert.SerializeObject(this);
                Console.WriteLine(saveFileName + " saved successfully");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to save game state due to exception: " + e.Message);
                return false;
            }
            finally
            {
                if (stream != null) { stream.Close(); }
            }
        }

        public bool LoadGameState(string saveFileName)
        {
            Stream? stream = null;
            try
            {
                string dir = Directory.GetCurrentDirectory();

                string saveFile = Path.Combine(dir, saveFileName);

                string jsonGameController = File.ReadAllText(saveFile);

                // Opens file "data.xml" and deserializes the object from it.
                stream = File.Open(saveFileName, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();

#pragma warning disable SYSLIB0011 // Type or member is obsolete
                GameController gc = (GameController)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                this._chessBoard = gc._chessBoard;
                this._chessPieces = gc._chessPieces;
                this._turns = gc._turns;
                this._turnNumber = gc._turnNumber;
                this._kingCheckService = new KingCheckService();

                ChessPiece.SetCastleCallbackFunction(SpecialMovesHandlers.DoCastleMove);
                ChessPiece.SetIsEnPassantCallbackFunction(SpecialMovesHandlers.IsEnPassantMove);

                Console.WriteLine("Successfully loaded " + saveFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load game state due to exception: " + e.Message);
                return false;
            }
            finally
            {
                if (stream != null) { stream.Close(); }
            }
            return true;
        }

        public string DisplayBoard()
        {
            return DisplayBoard(_chessBoard);
        }

        // TODO: Move this method to ChessBoard
        public string DisplayBoard(ChessBoard chessBoard)
        {
            Square[,] boardData = chessBoard.Board; // Use the Square array directly
            string output = "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n";
            int vertIndex = 8;

            for (int f = 0; f < 8; f++)
            {
                output += vertIndex;
                for (int s = 0; s < 8; s++)
                {
                    if (boardData[f, s].Piece is not NoPiece) // Check for chess piece
                    {
                        ChessPiece chessPiece = boardData[f, s].Piece; // Get the piece directly 
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
        //public string DisplayBoard(ChessBoard chessBoard)
        //{
        //    int[,] boardData = chessBoard.Board;
        //    String output = "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n";
        //    int vertIndex = 8;

        //    for (int f = 0; f < 8; f++)
        //    {
        //        output += vertIndex;
        //        for (int s = 0; s < 8; s++)
        //        {
        //            BoardPosition boardPosition = new((RANK)f, (FILE)s);
        //            if (chessBoard.IsPieceAtPosition(boardPosition))
        //            {
        //                ChessPiece chessPiece = _chessPieces.First(p => p.GetCurrentPosition() == boardPosition);
        //                if (chessPiece == null)
        //                {
        //                    throw new Exception("Unexpected! Fix this");
        //                }
        //                else
        //                {
        //                    String c = "", p = "", i = "";
        //                    switch (chessPiece.GetColor())
        //                    {
        //                        case ChessPiece.Color.WHITE: c = "W"; break;
        //                        case ChessPiece.Color.BLACK: c = "B"; break;
        //                    }

        //                    switch (chessPiece.GetPiece())
        //                    {
        //                        case ChessPiece.Piece.PAWN: p = "P"; break;
        //                        case ChessPiece.Piece.KNIGHT: p = "K"; break;
        //                        case ChessPiece.Piece.BISHOP: p = "B"; break;
        //                        case ChessPiece.Piece.ROOK: p = "R"; break;
        //                        case ChessPiece.Piece.QUEEN: p = "Q"; break;
        //                        case ChessPiece.Piece.KING:
        //                            {
        //                                p = "K"; i = ""; break;
        //                            }
        //                    }

        //                    if (!chessPiece.GetPiece().Equals(ChessPiece.Piece.KING))
        //                    {
        //                        i = chessPiece.GetId().ToString();
        //                    }
        //                    else
        //                    {
        //                        i = "0";
        //                    }

        //                    output += "|" + c + p + i;
        //                }
        //            }
        //            else
        //            {
        //                output += "|   ";
        //            }
        //        }
        //        output += "|" + vertIndex + "\n";
        //        vertIndex--;
        //    }
        //    output += "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n";
        //    return output;
        //}

        public Turn? GetLastTurn()
        {
            if (_turns.Count == 0)
                return null;
            else
                return _turns[^1];
        }
    }
}
