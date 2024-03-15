using Chess.Callbacks;
using Chess.Controller;
using Chess.GameState;
using Chess.Interfaces;
using Chess.Pieces;

namespace Chess
{
    internal class GameManager
    {
        private IConsole _console;
        private GameController _gameController;

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

            _console.WriteLine("Select init board state (all, pawns)?");
            string? input = _console.ReadLine();

            if (input == "pawns")
            {
                _console.WriteLine("Init Pawns Only");
                _gameController.InitPawnsOnly();
            }

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
                    input = _console.ReadLine();
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
                        _gameController.ApplyTurnToGameState(turn);
                }
                catch (Exception ex)
                {
                    _console.WriteLine("Exception encountered.");
                    _console.WriteLine(ex.ToString());
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