using Chess.Board;
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
            ChessPiece.SetCastleCallbackFunction(this.CastleCallBackFunction);
            ChessPiece.SetIsEnPassantCallbackFunction(this.IsEnPassantCallBackFunction);
            _turnNumber = 1;
        }

        public bool CastleCallBackFunction(ChessBoard cb, BoardPosition bp, ChessPiece king)
        {
            int hv = bp.FileAsInt;
            int vv = bp.RankAsInt;

            ChessPiece? rook = _chessPieces.Find(p =>
            {
                BoardPosition cbp = p.GetCurrentPosition();
                return cbp.FileAsInt == hv && cbp.RankAsInt == vv;
            });

            Assert.That(rook, Is.Not.Null);

            if (rook != null)
            {
                Assert.That(rook.GetPiece(), Is.EqualTo(ChessPiece.Piece.ROOK));

                // is king left of rook, or right of rook?
                int d = king.GetCurrentPosition().FileAsInt - rook.GetCurrentPosition().FileAsInt;
                FILE kh = king.GetCurrentPosition().File;
                FILE rh = rook.GetCurrentPosition().File;
                RANK v = king.GetCurrentPosition().Rank;
                // k=4, r=0 4-0=4, k=4, r2=8, k-r2=4-7 = -3
                if (d == 4)
                {
                    // queen side castle
                    // k goes -2 squares
                    // r goes +3 squares
                    kh -= 2;
                    rh += 3;
                }
                else if (d == -3)
                {
                    // king side castle
                    // k goes +2 squares
                    // r goes -2 squares
                    kh += 2;
                    rh -= 2;
                }
                else
                {
                    throw new Exception("Unexpected Horizontal Distance Found when castling. Are you sure this is a valid castle?");
                }

                BoardPosition kingLastPosition = king.GetCurrentPosition();
                BoardPosition rookLastPosition = rook.GetCurrentPosition();

                // set board manually
                cb.SetBoardValue(kingLastPosition, 0);
                cb.SetBoardValue(rookLastPosition, 0);

                king.SetCurrentPosition(new(v, kh));
                rook.SetCurrentPosition(new(v, rh));

                cb.SetBoardValue(king.GetCurrentPosition(), king.GetRealValue());
                cb.SetBoardValue(rook.GetCurrentPosition(), rook.GetRealValue());
            }

            return true;
        }

        public bool IsEnPassantCallBackFunction(ChessBoard chessBoard, BoardPosition boardPosition, ChessPiece pawnAttemptingEnPassant)
        {
            RANK enPassantRow;
            ChessPiece.Color opponentColor;
            int enPassantOffSet = 0;

            if (pawnAttemptingEnPassant.GetColor().Equals(ChessPiece.Color.WHITE))
            {
                enPassantRow = RANK.FIVE;
                opponentColor = ChessPiece.Color.BLACK;
                enPassantOffSet = -1;
            }
            else
            {
                enPassantRow = RANK.FOUR;
                opponentColor = ChessPiece.Color.WHITE;
                enPassantOffSet = +1;
            }

            BoardPosition pawnPos = pawnAttemptingEnPassant.GetCurrentPosition();

            // Is the pawn in the correct Row to do this? En Passant is only possible if a pawn is on a specific row on the board
            if (pawnPos.Rank != enPassantRow)
                return false;

            // Are there opponent pieces to its immediate left or right?
            
            // TODO: Provide better constructors for these kinds of operations
            BoardPosition bpl = new(pawnPos.Rank, (FILE) pawnPos.FileAsInt - 1);
            BoardPosition bpr = new(pawnPos.Rank, (FILE) pawnPos.FileAsInt + 1);

            foreach(BoardPosition bpToCheck in new List<BoardPosition>() { bpl, bpr })
            {
                // Is there an opponent piece at this position?
                if (chessBoard.IsPieceAtPosition(bpToCheck, opponentColor))
                {
                    ChessPiece? opponentPiece = _chessPieces.Find((ChessPiece cp) => cp.GetCurrentPosition().EqualTo(bpToCheck));
                    Assert.That(opponentPiece, Is.Not.Null, "This assertion failed. If ChessBoard.IsPieceAtPosition returns true, the piece must exist in the collection");

                    // Is the opponent piece a pawn?
                    if (opponentPiece is ChessPiecePawn)
                    {
                        // Did that pawn move 2 squares?
                        if ((opponentPiece as ChessPiecePawn).MovedTwoSquares)
                        {
                            // is the position a capture position?
                            // Get the opponent pawn position, and get the position that is 1 square behind it
                            BoardPosition oppPos = opponentPiece.GetCurrentPosition();
                            // this operation needs to support both + 1 (for black) and -1 (for white)
                            BoardPosition enPassantCapturePos = new((RANK) oppPos.RankAsInt + enPassantOffSet, oppPos.File);
                            if (enPassantCapturePos.EqualTo(boardPosition))
                            {
                                (opponentPiece as ChessPiecePawn).IsEnPassantTarget = true;
                                return true; // This is a valid En Passant capture move
                            }
                        }
                    }
                }

            }

            return false;
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

                Turn turn = new(_turnNumber, chessPiece, new(inputs[1]), _chessBoard, _chessPieces);
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

                ChessPiece.SetCastleCallbackFunction(this.CastleCallBackFunction);
                ChessPiece.SetIsEnPassantCallbackFunction(this.IsEnPassantCallBackFunction);

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

        public string DisplayBoard(ChessBoard chessBoard)
        {
            int[,] boardData = chessBoard.GetBoard();
            String output = "*|*A*|*B*|*C*|*D*|*E*|*F*|*G*|*H*|*\n";
            int vertIndex = 8;

            for (int f = 0; f < 8; f++)
            {
                output += vertIndex;
                for (int s = 0; s < 8; s++)
                {
                    BoardPosition boardPosition = new((RANK)f, (FILE)s);
                    if (chessBoard.IsPieceAtPosition(boardPosition))
                    {
                        ChessPiece chessPiece = _chessPieces.First(p => p.GetCurrentPosition().EqualTo(boardPosition));
                        if (chessPiece == null)
                        {
                            throw new Exception("Unexpected! Fix this");
                        }
                        else
                        {
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

        public Turn? GetLastTurn()
        {
            if (_turns.Count == 0)
                return null;
            else
                return _turns[^1];
        }
    }
}
