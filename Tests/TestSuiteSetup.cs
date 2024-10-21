using Chess.Globals;
using NUnit.Framework;
using Chess.Board;
using Chess.Pieces;
using Chess.Callbacks;
using Chess.Controller;

namespace Tests
{
    [SetUpFixture]
    public class TestSuiteSetup
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            Console.WriteLine("Global Setup Begin");
            StaticLogger.LoggerConfig.EnableTrace = false;
            StaticLogger.LoggerConfig.EnableMethodDumps = false;
            StaticLogger.LoggerConfig.EnableObjectDumps = false;
            StaticLogger.LoggerConfig.EnableStateChanges = false;
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.LoggerConfig.AddTypeToWhiteList(typeof(SpecialMovesHandlers));
            StaticLogger.LoggerConfig.AddTypeToWhiteList(typeof(GameController));
            StaticLogger.LoggerConfig.AddTypeToWhiteList(typeof(ChessPieceWhitePawn));
            StaticLogger.LoggerConfig.AddTypeToWhiteList(typeof(ChessPieceBlackPawn));
            StaticLogger.LoggerConfig.AddTypeToWhiteList(typeof(LambdaQueue));
            StaticLogger.LoggerConfig.AddTypeToSkipObjectDumps(typeof(Square));
            StaticLogger.LoggerConfig.AddTypeToSkipObjectDumps(typeof(ChessPiece));
            StaticLogger.LoggerConfig.AddTypeToSkipObjectDumps(typeof(ChessBoard));
            StaticLogger.LoggerConfig.AddTypeToSkipForLogging(typeof(Square));
            //StaticLogger.LoggerConfig.AddTypeToSkipForLogging(typeof(ChessBoard));
            //StaticLogger.LoggerConfig.AddTypeToSkipForLogging(typeof(BoardPosition));


            // You can add any other global setup code here
            //StaticLogger.AddTestName(); // Example of setting test name in logs, if needed
            Console.WriteLine("Global Setup Begin");
        }
    }
}
