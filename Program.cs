using Chess.Board;
using Chess.Services;

namespace Chess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Chess Application Starting");

            ChessBoard chessBoard = new();
            GameController gameController = new(chessBoard);

            gameController.StartGame();

            String input = "";

            while (input != "quit" || input != "over")
            {
                Console.WriteLine(gameController.DisplayBoard());
                
                if (gameController.TurnNumber % 2 == 0)
                {
                    Console.WriteLine("Turn " + gameController.TurnNumber + " - Black to move. Please enter a command (piece name + position : ie. 'BP4 D5')");
                }
                else
                {
                    Console.WriteLine("Turn " + gameController.TurnNumber + " - White to move. Please enter a command (piece name + position : ie. 'WK1 C3')");
                }

                

                try
                {
                    input = Console.ReadLine();

                    if (input.Length > 0)
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
                    

                    Turn turn = gameController.GetTurnFromCommand(input);

                    if (turn != null)
                    {
                        if (turn.ChessPiece.IsValidMove(gameController.GetChessBoard(), turn.NewPosition))
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
                                turn.ChessPiece.SetCurrentPosition(turn.NewPosition);
                                gameController.UpdateBoard(turn.ChessBoard);
                                gameController.IncrementTurn();                 
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Move. Please Try Again.");
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Command. Please Try Again.");
                        continue;
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
