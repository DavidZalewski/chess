using Chess.Callbacks;
using Chess.Controller;
using Chess.GameState;
using Chess.Interfaces;
using Chess.Pieces;
using System.ComponentModel.Design;

namespace Chess
{
    internal class GameManager
    {
        private IConsole _console;
        private GameController _gameController;
        private ChessStateExplorer _explorer = new ChessStateExplorer();
        private bool _AIMode = false;
        private string _AICommand = "";

        public GameManager(IConsole console, GameController gameController)
        {
            _console = console;
            _gameController = gameController;
            SpecialMovesHandlers.PawnPromotionPromptUser = HandlePawnPromotion;
        }

        public void Start()
        {
            _console.WriteLine("Chess Application Starting");

            StartTutorial();

            _console.WriteLine("Select RuleSet:");
            _console.WriteLine("*Classic* -- The game we all know and love");
            _console.WriteLine("*PawnsOnly* -- No knights, bishops, rooks, or queens. Just a king and his pawns");
            _console.WriteLine("*NuclearHorse* -- The game where the most powerful piece is also the most destructive, can you survive the environment?");
            _console.WriteLine("*SevenByEight* -- The board is changed to be a 7 x 8 grid instead");
            _console.WriteLine("*KingsForce* -- A pawn connected to a king can capture in all directions; the king can disable a square");
            string? input = _console.ReadLine();

            _console.WriteLine($"*Adding RuleSet: {input}");
            _gameController.AddRuleSet(input);

            _console.WriteLine("Applying Rule Sets");
            _gameController.ApplyRuleSet();

            _console.WriteLine("Use AI for Black? (y, n)?");
            input = _console.ReadLine();

            if (input?.ToLower() == "y")
            {
                _console.WriteLine("Enabling AI...");
                _AIMode = true;
            }

            _gameController.SetOnTurnHandler((Turn turn) =>
            {
                if (_AIMode)
                {
                    ulong _unused = 0;
                    List<TurnNode> turns = _explorer.GenerateAllPossibleMovesTurnNode(turn, 3, ref _unused);

                    //List<TurnNode> turnsLeadingToCheckMateForAI = turns.OrderByDescending(tn1 =>
                    //{
                    //    foreach (TurnNode tn2 in tn1.Children)
                    //    {
                    //        foreach (TurnNode tn3 in tn2.Children)
                    //        {
                    //            if (tn3.IsCheckMate && tn3.Side() == 0)
                    //            {
                    //                return true;
                    //            }
                    //        }
                    //        if (tn2.IsCheckMate && tn2.Side() == 0)
                    //        {
                    //            return true;
                    //        }
                    //    }
                    //    return tn1.IsCheckMate && tn1.Side() == 0;
                    //}).ToList();

                    // Sort by least number of moves for opponent (Children.Count), then by most number of moves for current player (turns.Count - tn.Children.Count)
                    List<TurnNode> goodTurns = turns
                    .OrderByDescending(tn1 => {
                        foreach (TurnNode tn2 in tn1.Children)
                        {
                            foreach (TurnNode tn3 in tn2.Children)
                            {
                                if (tn3.IsCheckMate && tn3.Side() == 1)
                                {
                                    return true;
                                }
                            }
                            if (tn2.IsCheckMate && tn2.Side() == 1)
                            {
                                return true;
                            }
                        }
                        return tn1.IsCheckMate && tn1.Side() == 1;
                    })
                    .ThenBy(tn1 => {
                        foreach (TurnNode tn2 in tn1.Children)
                        {
                            foreach (TurnNode tn3 in tn2.Children)
                            {
                                if (tn3.IsKingInCheck && tn3.Side() == 1)
                                {
                                    return true;
                                }
                            }
                            if (tn2.IsKingInCheck && tn2.Side() == 1)
                            {
                                return true;
                            }
                        }
                        return tn1.IsKingInCheck && tn1.Side() == 1;
                    })
                    .ThenBy(tn1 => {
                        foreach (TurnNode tn2 in tn1.Children)
                        {
                            foreach (TurnNode tn3 in tn2.Children)
                            {
                                if (tn3.TurnDescription.Contains("capture") && tn3.Side() == 0)
                                {
                                    return true;
                                }
                            }
                            if (tn2.TurnDescription.Contains("capture") && tn2.Side() == 0)
                            {
                                return true;
                            }
                        }
                        return tn1.TurnDescription.Contains("capture") && tn1.Side() == 0;
                    })
                                //.ThenBy(tn => tn.Children.Count())
                                //.ThenByDescending(tn => turns.Count - tn.Children.Count)
                    .ToList();

                    _console.WriteLine($"The possible number of moves at this current move: {turn.TurnDescription} are: {turns.Count}");
                    Random r = new Random();
                    TurnNode bestTurnToMake = goodTurns.First(); // Select the first turn after sorting
                    TurnNode randomTurnToMake = goodTurns[r.Next(goodTurns.Count)]; // A random turn

                    _console.WriteLine($"*************Best turn to make here is: {bestTurnToMake.Command}");
                    _console.WriteLine($"*************Random turn to make here is: {randomTurnToMake.Command}");
                    _console.WriteLine($"Deciding whether to pick the best move or a random move...");

                    //_AICommand = randomTurnToMake.Command;
                    if (r.NextDouble() > 0.98710717)
                    {
                        _console.WriteLine("I choose random!");
                        _AICommand = randomTurnToMake.Command;
                    }
                    else
                    {
                        _console.WriteLine("I choose best move!");
                        _AICommand = bestTurnToMake.Command;
                    }
                }
            });
            PlayGame();
        }

        private void StartTutorial()
        {
            _console.WriteLine("Would you like to play the tutorial? (y/n):");
            string? input = "";

            while (input != null)
            {
                input = _console.ReadLine();

                if (input != null && input.ToLower().Equals("y"))
                {
                    _console.WriteLine("Starting tutorial");
                    _console.WriteLine("Chess is a game of planning, tactics, and strategy.");
                    _console.WriteLine("Let's jump right in by starting an example game. You can exit the tutorial anytime by typing exit");
                    _gameController.StartGame();
                    _console.WriteLine(_gameController.DisplayBoard());
                    _console.WriteLine("The game starts with White moving first, to move a piece, type in the name of the piece, followed by the position on the board.");

                    TutorialLoop();
                }
                else
                    break;
            }
        }

        private void TutorialLoop()
        {
            string? input = "";

            while (input != null)
            {
                _console.WriteLine("Start the game by moving WP4 and moving it to D4 by typing in the following: WP4 D4");
                input = _console.ReadLine();
                if (input != null && input.ToUpper().Equals("WP4 D4"))
                {
                    Turn? turn = _gameController.GetTurnFromCommand(input);
                    if (turn != null)
                    {
                        _gameController.ApplyTurnToGameState(turn);
                        _console.WriteLine(turn.TurnDescription);
                        _console.WriteLine(_gameController.DisplayBoard());

                        _console.WriteLine("You can see that WP4 has moved from D2 to D4");
                    }
                    else
                    {
                        _console.WriteLine("Something unexpected has happened in the tutorial.");
                        break;
                    }
                }
                else
                {
                    _console.WriteLine("Wrong command typed in.");
                }
            }
        }

        private void PlayGame()
        {
            _gameController.StartGame();

            string? input = "";

            while (input != "quit")
            {
                _console.WriteLine(_gameController.DisplayBoard());

                if (_gameController.TurnNumber % 2 == 0)
                {
                    _console.WriteLine("Turn " + _gameController.TurnNumber + " - Black to move. Please enter a command (piece name + position : ie. 'BP4 D5')");
                    if (_gameController.GetLastTurn() != null && _gameController.IsCheck(ChessPiece.Color.BLACK))
                        _console.WriteLine("Black King is currently in check");
                }
                else
                {
                    _console.WriteLine("Turn " + _gameController.TurnNumber + " - White to move. Please enter a command (piece name + position : ie. 'WK1 C3')");
                    if (_gameController.GetLastTurn() != null && _gameController.IsCheck(ChessPiece.Color.WHITE))
                        _console.WriteLine("White King is currently in check");
                }

                try
                {
                    // THIS IS A PROGRAMMING MACRO FLAG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    if (_AIMode && _gameController.TurnNumber % 2 == 0)
                    //if (_AIMode)
                    {
                        input = _AICommand;
                    }
                    else
                    {
                        input = _console.ReadLine();
                    }
                    (string? command, string? argument) = ParseInput(input);

                    if (command == null || argument == null)
                    {
                        _console.WriteLine("Invalid command. Please enter a command.");
                        continue;
                    }

                    switch (command)
                    {
                        case "save":
                            _gameController.SaveGameState(argument);
                            continue;
                        case "load":
                            _gameController.LoadGameState(argument);
                            continue;
                    };

                    // TODO: Move this logic into GetTurnFromCommand?
                    if (!command.StartsWith('w') && !command.StartsWith('b'))
                    {
                        _console.WriteLine("Invalid Color Specified. Use B or W when starting command.");
                        continue;
                    }
                    if ((command.StartsWith('w') && _gameController.TurnNumber % 2 == 0) || (command.StartsWith('b') && _gameController.TurnNumber % 2 != 0))
                    {
                        _console.WriteLine("Wrong Color Selected. It is not that sides turn yet. Please Try Again");
                        continue;
                    }

                    // Todo: resolve duplicate input parsing logic here and above
                    Turn? turn = _gameController.GetTurnFromCommand(input);
                    if (turn == null)
                    {
                        _console.WriteLine("Invalid Move. Please Try Again.");
                        continue;
                    }

                    if (_gameController.IsCheckMate(turn))
                    {
                        if (turn.PlayerTurn.Equals(Turn.Color.WHITE))
                            _console.WriteLine("White wins against Black by CheckMate!");
                        else
                            _console.WriteLine("Black wins against White by CheckMate!");
                        _console.WriteLine("Game Over.");
                        _gameController.ApplyTurnToGameState(turn);
                        _console.WriteLine(_gameController.DisplayBoard());
                        break; // break out of game loop
                    }
                    else if (_gameController.IsCheck(turn))
                    {
                        _console.WriteLine("Invalid Move. King would be in Check. Please Try Again");
                        continue;
                    }
                    else
                    {
                        _gameController.ApplyTurnToGameState(turn);

                        _console.WriteLine("Applying NuclearHorse After Turn Effects");
                        List<ChessPiece>? nuclearHorsePieces = _gameController?.GetLastTurn()?
                            .ChessPieces?.FindAll(cp => cp is NuclearHorsePiece).ToList();
                        List<ChessPiece>? nuclearBishopPieces = _gameController?.GetLastTurn()?
                            .ChessPieces?.FindAll(cp => cp is NuclearBishopPiece).ToList();

                        // first do nuclear horses
                        foreach (NuclearHorsePiece nuclearHorsePiece in nuclearHorsePieces)
                        {
                            _ = nuclearHorsePiece.ImplementMove(_gameController?.GetLastTurn()?.ChessBoard, turn.NewPosition);
                        }
                        // then do bishop breaking the inaccessible squares
                        foreach (NuclearBishopPiece nuclearBishopPiece in nuclearBishopPieces)
                        {
                            _ = nuclearBishopPiece.ImplementMove(_gameController?.GetLastTurn()?.ChessBoard, turn.NewPosition);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _console.WriteLine("Exception encountered.");
                    _console.WriteLine(ex.ToString());
                    break;
                }
            } // end while

            _console.WriteLine("GameManager: Ending Game");
        }

        private string HandlePawnPromotion()
        {
            _console.WriteLine("Pawn Promoted. Choose type: (Q - Queen, R - Rook, K - Knight, B - Bishop)");
            string? choice;
            while (true)
            {
                choice = _console.ReadLine()?.ToLower();

                if (choice != "q" && choice != "r" && choice != "k" && choice != "b")
                    _console.WriteLine("Invalid choice. Please try again.");
                else
                    break;
            }

            return choice;
        }

        private static (string? command, string? argument) ParseInput(string? input)
        {
            if (String.IsNullOrWhiteSpace(input)) return (null, null);

            string[] parts = input.ToLower().Split(' ');
            return (parts[0], parts.Length > 1 ? parts[1] : null);
        }
    }
}