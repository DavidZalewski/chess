using Chess.Board;
using Chess.Pieces;
using Chess.Services;

namespace Chess
{
    // TODO: load g1
    // keep playing until you get checkmate
    // save game before checkmate move
    /// <summary>
    ///  debug why game doesnt think its checkmate
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Chess Application Starting");

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);

            gameController.StartGame();

            String? input = "";

            while (input != "quit" || input != "over")
            {
                Console.WriteLine(gameController.DisplayBoard());
                
                if (gameController.TurnNumber % 2 == 0)
                {
                    Console.WriteLine("Turn " + gameController.TurnNumber + " - Black to move. Please enter a command (piece name + position : ie. 'BP4 D5')");
                    if (gameController.GetLastTurn() != null && gameController.IsKingInCheck(ChessPiece.Color.BLACK))
                    {
                        Console.WriteLine("Black King is currently in check");
                    } 
                }
                else
                {
                    Console.WriteLine("Turn " + gameController.TurnNumber + " - White to move. Please enter a command (piece name + position : ie. 'WK1 C3')");
                    if (gameController.GetLastTurn() != null && gameController.IsKingInCheck(ChessPiece.Color.WHITE))
                    {
                        Console.WriteLine("Black King is currently in check");
                    }
                }         

                try
                {
                    input = Console.ReadLine();
                    if (input != null && input.Length > 0)
                    {
                        if (input.ToLower().Contains("save"))
                        {
                            string[] inputs = input.Split(' ');
                            if (inputs.Length == 2)
                            {
                                string fileName = inputs[1];
                                gameController.SaveGameState(fileName);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Invalid Command for Save. ie: Save g1");
                            }

                        }
                        else if (input.ToLower().Contains("load"))
                        {
                            string[] inputs = input.Split(' ');
                            if (inputs.Length == 2)
                            {
                                string fileName = inputs[1];
                                gameController.LoadGameState(fileName);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Invalid Command for Load. ie: Save g1");
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
                                input = "over";
                                continue;
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
                
            }
        }
    }
}
