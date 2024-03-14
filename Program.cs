using Chess.Board;
using Chess.Controller;
using Chess.GameState;
using Chess.Pieces;
using Chess.Services;
using System.Collections.Immutable;
using System.Linq;

namespace Chess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Chess Application Starting");

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);

            Console.WriteLine("Would you like to play the tutorial? (y/n):");
            String? input = "";

            while (input != null)
            {
                input = Console.ReadLine();

                if (input != null && input.ToLower().Equals("y"))
                {
                    Console.WriteLine("Starting tutorial");
                    Console.WriteLine("Chess is a game of planning, tactics, and strategy.");
                    Console.WriteLine("Let's jump right in by starting an example game. You can exit the tutorial anytime by typing exit");
                    gameController.StartGame();
                    Console.WriteLine(gameController.DisplayBoard());
                    Console.WriteLine("The game starts with White moving first, to move a piece, type in the name of the piece, followed by the position on the board.");

                    while (input != null)
                    {
                        Console.WriteLine("Start the game by moving WP4 and moving it to D4 by typing in the following: WP4 D4");
                        input = Console.ReadLine();
                        if (input.ToUpper().Equals("WP4 D4"))
                        {
                            Turn? turn = gameController.GetTurnFromCommand(input);
                            if (turn != null)
                            {
                                gameController.ApplyTurnToGameState(turn);
                                Console.WriteLine(turn.TurnDescription);
                                Console.WriteLine(gameController.DisplayBoard());

                                Console.WriteLine("You can see that WP4 has moved from D2 to D4");
                            }
                            else
                            {
                                Console.WriteLine("Something unexpected has happened in the tutorial.");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong command typed in.");
                        }
                    }
                }
                else
                    break;
            }

            Console.WriteLine("Select init board state (all, pawns)?");
            input = Console.ReadLine();

            if (input == "pawns")
            {
                Console.WriteLine("Init Pawns Only");
                List<ChessPiece> pieces = ChessPieceFactory.CreateWhitePawns();
                pieces.AddRange(ChessPieceFactory.CreateBlackPawns());
                pieces.Add(new ChessPieceKing(ChessPiece.Color.BLACK, new("E8")));
                pieces.Add(new ChessPieceKing(ChessPiece.Color.WHITE, new("E1")));
                gameController = new GameController(chessBoard, pieces);
            }

            gameController.StartGame();



            while (input != "quit")
            {
                Console.WriteLine(gameController.DisplayBoard());

                if (gameController.TurnNumber % 2 == 0)
                {
                    Console.WriteLine("Turn " + gameController.TurnNumber + " - Black to move. Please enter a command (piece name + position : ie. 'BP4 D5')");
                    if (gameController.GetLastTurn() != null && gameController.IsCheck(ChessPiece.Color.BLACK))
                    {
                        Console.WriteLine("Black King is currently in check");
                    }
                }
                else
                {
                    Console.WriteLine("Turn " + gameController.TurnNumber + " - White to move. Please enter a command (piece name + position : ie. 'WK1 C3')");
                    if (gameController.GetLastTurn() != null && gameController.IsCheck(ChessPiece.Color.WHITE))
                    {
                        Console.WriteLine("White King is currently in check");
                    }
                }

                try
                {
                    input = Console.ReadLine();
                    if (input != null && input.Length > 0)
                    {
                        if (input.ToLower().Contains("save") || input.ToLower().Contains("load"))
                        {
                            string[] inputs = input.Split(' ');
                            if (inputs.Length == 2)
                            {
                                string fileName = inputs[1];

                                if (input.ToLower().Contains("save"))
                                    gameController.SaveGameState(fileName);
                                else if (input.ToLower().Contains("load"))
                                    gameController.LoadGameState(fileName);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Invalid Command for Save. ie: Save g1");
                            }

                        }
                        else
                        {
                            char C = input.ToCharArray()[0];

                            if (C != 'B' && C != 'W')
                            {
                                Console.WriteLine("Invalid Color Specified. Use B or W when starting command");
                                continue;
                            }
                            else
                            {
                                if ((C == 'W' && gameController.TurnNumber % 2 != 0) || (C == 'B' && gameController.TurnNumber % 2 == 0))
                                {
                                }
                                else
                                {
                                    Console.WriteLine("Wrong Color Selected. It is not that sides turn yet. Please Try Again");
                                    continue;
                                }
                            }
                        }

                        Turn? turn = gameController.GetTurnFromCommand(input);

                        if (turn != null)
                        {
                            if (gameController.IsCheckMate(turn))
                            {
                                if (turn.PlayerTurn.Equals(Turn.Color.WHITE))
                                {
                                    Console.WriteLine("White wins against Black by CheckMate!");
                                }
                                else
                                {
                                    Console.WriteLine("Black wins against White by CheckMate!");
                                }
                                Console.WriteLine("Game Over.");
                                gameController.ApplyTurnToGameState(turn);
                                Console.WriteLine(gameController.DisplayBoard());
                                break;
                            }
                            if (gameController.IsCheck(turn))
                            {
                                Console.WriteLine("Invalid Move. King would be in Check. Please Try Again");
                                continue;
                            }
                            else
                            {
                                gameController.ApplyTurnToGameState(turn);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Move. Please Try Again.");
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception encountered.");
                    Console.WriteLine(ex.ToString());
                }

            } // end while

            Console.WriteLine("Program Terminated Successfully");
        }
    }
}
