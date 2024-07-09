using Chess.Board;
using Chess.Callbacks;
using Chess.Exceptions;
using Chess.GameState;
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
        private int _turnNumber = 0;
        private Action<Turn>? OnTurnHandler;
        private ActionSequence _sequence = new();

        public int TurnNumber { get => _turnNumber; }
        public void SetOnTurnHandler(Action<Turn> action)
        {
            OnTurnHandler = action;
        }

        public GameController(ChessBoard chessBoard)
        {
            _chessBoard = chessBoard;
            _chessPieces = ChessPieceFactory.CreateChessPiecesClassic();
            _kingCheckService = new KingCheckService();
        }

        public GameController(ChessBoard chessBoard, List<ChessPiece> chessPieces)
        {
            _chessBoard = chessBoard;
            _chessPieces = chessPieces;
            _kingCheckService = new KingCheckService();
        }

        public void StartGame()
        {
            _chessBoard.PopulateBoard(_chessPieces);
            _turnNumber = 1;
        }

        // TODO: Encapsulate all string to piece, and piece to string translations in another class
        internal ChessPiece? FindChessPieceFromString(string input)
        {
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

            return chessPiece;
        }

        public bool IsCheck(ChessPiece.Color color)
        {
            return _kingCheckService.IsKingInCheck(color, _chessBoard);
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
            OnTurnHandler?.Invoke(turn);
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

                _chessBoard = gc._chessBoard;
                _chessPieces = gc._chessPieces;
                _turns = gc._turns;
                _turnNumber = gc._turnNumber;
                _kingCheckService = new KingCheckService();

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
            return _chessBoard.DisplayBoard();
        }

        public Turn? GetLastTurn()
        {
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
            _sequence.AddActionInSequence(r);
        }

        public void AddRuleSet(String? r)
        {
            switch(r)
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
            _sequence.PlayActionSequence();
        }

        internal void RuleSetPawnsOnly()
        {
            List<ChessPiece> pieces = ChessPieceFactory.CreateWhitePawns();
            pieces.AddRange(ChessPieceFactory.CreateBlackPawns());
            pieces.Add(new ChessPieceKing(ChessPiece.Color.BLACK, new("E8")));
            pieces.Add(new ChessPieceKing(ChessPiece.Color.WHITE, new("E1")));
            _chessPieces = pieces;
        }

        internal void RuleSetSevenByEight()
        {

        }

        internal void RuleSetKingsForce()
        {

        }

        internal void RuleSetNuclearHorse()
        {
            _chessPieces = ChessPieceFactory.CreateChessPiecesNuclearHorse();
        }

    }
}
