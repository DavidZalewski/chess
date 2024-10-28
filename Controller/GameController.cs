using Chess.Attributes;
using Chess.Board;
using Chess.Callbacks;
using Chess.Exceptions;
using Chess.GameState;
using Chess.Globals;
using Chess.Pieces;
using Chess.Services;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chess.Controller
{
    [Serializable]
    public class GameController
    {
        private ChessBoard _chessBoard;
        private KingCheckService _kingCheckService;
        private List<ChessPiece> _chessPieces = new();
        private List<Turn> _turns = new();
        private int _turnNumber = 1;
        private Action<Turn>? OnTurnHandler;
        private ActionSequence _sequence = new();

        public int TurnNumber { get => _turnNumber; set => _turnNumber = value; }
        public void SetOnTurnHandler(Action<Turn> action)
        {
            StaticLogger.Trace();
            OnTurnHandler = action;
        }

        public GameController(ChessBoard chessBoard)
        {
            StaticLogger.Trace();
            _chessBoard = chessBoard;
            _chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
            _kingCheckService = new KingCheckService();
        }

        public GameController(ChessBoard chessBoard, List<ChessPiece> chessPieces)
        {
            StaticLogger.Trace();
            _chessBoard = chessBoard;
            _chessPieces = chessPieces;
            _kingCheckService = new KingCheckService();
        }

        public void StartGame()
        {
            StaticLogger.Trace();
            _chessBoard.PopulateBoard(_chessPieces);
            SpecialMovesHandlers.promotionTracker = new PromotionTracker();
            //_turnNumber = 1;
        }

        // TODO: Encapsulate all string to piece, and piece to string translations in another class
        internal ChessPiece? FindChessPieceFromString(string input)
        {
            StaticLogger.Trace();
            ChessPiece.Color color;
            ChessPiece.Piece piece;
            int id = 0;

            char[] chars = input.ToUpper().ToCharArray();

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

            ChessPiece? chessPiece = _chessPieces.FirstOrDefault(p => p.GetColor().Equals(color) && p.GetPiece().Equals(piece) && p.GetId().Equals(id), null);
            StaticLogger.LogObject(chessPiece);
            return chessPiece;
        }

        public bool IsCheck(ChessPiece.Color color)
        {
            StaticLogger.Trace();
            return _kingCheckService.IsKingInCheck(color, _chessBoard);
        }

        public bool IsCheck(Turn turn)
        {
            StaticLogger.Trace();
            return _kingCheckService.IsKingInCheck(turn);
        }

        public bool IsCheckMate(Turn turn)
        {
            StaticLogger.Trace();
            return _kingCheckService.IsCheckMate(turn);
        }

        public Turn? GetTurnFromCommand(string input)
        {
            StaticLogger.Trace();
            string[] inputs = input.Split(' ');
            if (inputs.Length != 2) return null;
            try
            {
                ChessPiece? chessPiece = FindChessPieceFromString(inputs[0]);
                if (chessPiece == null) return null;

                // see if we can find a chess piece from the second argument. If we can, we assume the player is trying to capture this piece
                ChessPiece? opponentPiece = FindChessPieceFromString(inputs[1]);
                BoardPosition destinationPosition = null;
                if (opponentPiece == null)
                {
                    destinationPosition = new(inputs[1]);
                }
                else
                {
                    destinationPosition = opponentPiece.GetCurrentPosition();
                }

                Turn turn = new(_turnNumber, chessPiece, destinationPosition, _chessBoard);
                if (!turn.IsValidTurn) return null;
                turn.Command = input;
                return turn;
            }
            catch (Exception e)
            {
                StaticLogger.Log($"Exception: {e.Message}", LogLevel.Error, LogCategory.General);
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void ApplyTurnToGameState(Turn turn)
        {
            StaticLogger.Trace();
            StaticLogger.LogMethod(turn);
            _chessBoard = turn.ChessBoard;
            _chessPieces = turn.ChessPieces;
            _turns.Add(turn);
            _turnNumber++;
            StaticLogger.Log($"Applying Turn To Game State (TurnNumber: {_turnNumber})", LogLevel.Debug);
            OnTurnHandler?.Invoke(turn);
            LambdaQueue.Drain(this); // Used for EnPassant to expire the window where this move can still be made
        }

        public ChessBoard GetChessBoard() 
        {
            StaticLogger.Trace();
            return _chessBoard; 
        }

        public bool SaveGameState(string saveFileName)
        {
            StaticLogger.Trace();
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
                StaticLogger.Log($"Failed to save game state due to exception: {e.Message}", LogLevel.Error, LogCategory.General);
                Console.WriteLine("Failed to save game state due to exception: " + e.Message);
                return false;
            }
            finally
            {
                if (stream != null) { stream.Close(); }
            }
        }

        [TestNeeded]
        public bool LoadGameState(string saveFileName)
        {
            StaticLogger.Trace();
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

                _chessBoard = gc._chessBoard;
                _chessPieces = gc._chessPieces;
                _turns = gc._turns;
                _turnNumber = gc._turnNumber;
                _kingCheckService = new KingCheckService();

                ChessPiece.SetCastleCallbackFunction(SpecialMovesHandlers.DoCastleMove);
                ChessPiece.SetIsEnPassantCallbackFunction(SpecialMovesHandlers.IsEnPassantMove);

                StaticLogger.Log($"Successfully loaded {saveFileName}");
                Console.WriteLine("Successfully loaded " + saveFileName);
            }
            catch (Exception e)
            {
                StaticLogger.Log($"Failed to load game state due to exception: {e.Message}");
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
            StaticLogger.Trace();
            return _chessBoard.DisplayBoard();
        }

        public Turn? GetLastTurn()
        {
            StaticLogger.Trace();
            if (_turns.Count == 0)
                return null;
            else
                return _turns[^1];
        }


        /*
         * 
         * 
         * 
         * RULESETS
         * 
         * 
         * 
         */

        public void AddRuleSet(Action r)
        {
            StaticLogger.Trace();
            _sequence.AddActionInSequence(r);
        }

        public void AddRuleSet(String? r)
        {
            StaticLogger.Trace();
            switch (r)
            {
                case "PawnsOnly":
                    {
                        AddRuleSet(RuleSetPawnsOnly); break;
                    }
                case "SevenByEight":
                    {
                        AddRuleSet(RuleSetSevenByEight); break;
                    }
                case "KingsForce":
                    {
                        AddRuleSet(RuleSetKingsForce); break;
                    }
                case "NuclearHorse":
                    {
                        AddRuleSet(RuleSetNuclearHorse); break;
                    }
                case "Classic": return;
                default: return;
            }
        }

        public void ApplyRuleSet()
        {
            StaticLogger.Trace();
            _sequence.PlayActionSequence();
        }

        internal void RuleSetPawnsOnly()
        {
            StaticLogger.Trace();
            List<ChessPiece> pieces = ChessPieceFactory.CreateWhitePawns();
            pieces.AddRange(ChessPieceFactory.CreateBlackPawns());
            pieces.Add(new ChessPieceKing(ChessPiece.Color.BLACK, new("E8")));
            pieces.Add(new ChessPieceKing(ChessPiece.Color.WHITE, new("E1")));
            _chessPieces = pieces;
        }

        internal void RuleSetSevenByEight()
        {
            StaticLogger.Trace();
        }

        internal void RuleSetKingsForce()
        {
            StaticLogger.Trace();
        }

        [TestNeeded]
        internal void RuleSetNuclearHorse()
        {
            StaticLogger.Trace();
            _chessPieces = ChessPieceFactory.CreateChessPiecesNuclearHorse();
        }

        [TestNeeded]
        internal bool ContainsRuleSet(string v)
        {
            StaticLogger.Trace();
            return v.ToLowerInvariant() switch
            {
                "nuclearhorse" => _sequence.IsActionInSequence(RuleSetNuclearHorse),
                "kingsforce" => _sequence.IsActionInSequence(RuleSetKingsForce),
                "sevenbyeight" => _sequence.IsActionInSequence(RuleSetSevenByEight),
                "pawnsonly" => _sequence.IsActionInSequence(RuleSetPawnsOnly),
                _ => false,
            };
        }

        [TestNeeded]
        internal void NuclearHorseEndTurnHandler()
        {
            StaticLogger.Trace();
            List<ChessPiece> nuclearBishops = _chessPieces.FindAll(p => p is NuclearBishopPiece).ToList();
            // yes this duplicates the move, but its the simplest way of ensuring disabled blocks are not in the bishops path
            foreach (NuclearBishopPiece nuclearBishop in nuclearBishops)
                nuclearBishop.Move(_chessBoard, nuclearBishop.GetCurrentPosition());
        }
    }
}
