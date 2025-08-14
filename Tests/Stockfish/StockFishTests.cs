using Chess.Board;
using Chess.Controller;
using Chess.GameState;
using Chess.Globals;
using Chess.Interfaces;
using Chess.Pieces;
using Chess.Services;
using System.Diagnostics;
using Tests.Services;

namespace Tests.StockFish
{
    [Category("CORE")]
    //[Parallelizable(ParallelScope.All)] // YOU CANNOT RUN THESE ASYNC ANYMORE
    public class StockFishTests : TestBase
    {
        [Test]
        public void LaunchSubProcess_Success()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.FileName = "C:\\Users\\david\\OneDrive\\Documents\\GitHub\\chess\\Stockfish\\stockfish\\stockfish-windows-x86-64-avx2.exe";
            Process? stockfish = Process.Start(processStartInfo);

            Assert.That(stockfish, Is.Not.Null);

            StreamWriter stockFishInput = stockfish?.StandardInput;
            StreamReader stockFishOutput = stockfish?.StandardOutput;

            // NOT USED
            Queue<string> mockConsoleInputs = new();
            mockConsoleInputs.Enqueue("n"); // no to tutorial
            mockConsoleInputs.Enqueue("Classic"); // init game with all pieces
            mockConsoleInputs.Enqueue("n"); // no to ai
            mockConsoleInputs.Enqueue("WP4 D4");
            // NOT USED


            StaticLogger.Trace();
            ChessBoard chessBoard = new();
            GameController gameController = new GameController(chessBoard);
            IConsole consoleService = new MockConsoleService(mockConsoleInputs);
            GameManager game = new(consoleService, gameController);
            game.GameController.StartGame();
            


            using (stockFishInput)
            {
                stockFishInput.WriteLine("uci");
                string? o;
                while ((o = stockFishOutput.ReadLine()) != null)
                {
                    if (o == "uciok")
                    {
                        Console.WriteLine("uciok message found");
                        
                        break;
                    }
                }

                // simulating move
                ChessPiece openingPiece = chessBoard.GetSquare(new("E2")).Piece;
                openingPiece.Move(chessBoard, new("E4"));
                //

                // convert to uci notation
                string ucimove = openingPiece.GetStartingPosition().StringValue.ToLower() + openingPiece.GetCurrentPosition().StringValue.ToLower();
                Console.WriteLine($"converted ucimove: {ucimove}");

                stockFishInput.WriteLine($"position startpos moves {ucimove}");
                stockFishInput.WriteLine("go depth 10");

                //Thread.Sleep(1000);
                // synchronization needed to ensure subprocess and game are in sync

                // NOTE: Dont need to validate stockfish moves with our game engine

                while ((o = stockFishOutput.ReadLine()) != null)
                {
                    if (o.Contains("bestmove"))
                    {
                        Console.WriteLine("bestmove message found");
                        string bm = o.Split(' ')[1].ToUpper();
                        string pm = o.Split(" ")[3].ToUpper();

                        Console.WriteLine("Extracted BestMove: " + bm);

                        string bm1 = bm.Substring(0, 2);
                        string bm2 = bm.Substring(2, 2);

                        Console.WriteLine("Extracted Parts: " + bm1 + " " + bm2);

                        BoardPosition bp = new(bm1);
                        Assert.That(bp, Is.Not.Null);
                        Square sq = chessBoard.GetSquare(bp);
                        Assert.That(sq, Is.Not.Null);
                        ChessPiece piece = sq.Piece;

                        Assert.That(piece, Is.Not.Null);

                        Console.WriteLine("Located Piece: " + piece.GetPieceName());

                        piece.Move(chessBoard, new(bm2));

                        Console.WriteLine(chessBoard.DisplayBoard());
                        consoleService.ToDetailedString();
                        consoleService.WriteLine("quit");
                        stockFishInput.WriteLine("quit");
                        break;
                    }
                }
            }
        }
    }
}
