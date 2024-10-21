using Chess.Globals;
using NUnit.Framework;
using Chess.Board;
using Chess.Pieces;

namespace Chess.Tests
{
    [SetUpFixture]
    public class TestSuiteSetup
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            // TODO: Move this into a single place where it initializes these settings for all tests
            StaticLogger.LoggerConfig.MinimumLogLevel = LogLevel.Debug;
            StaticLogger.LoggerConfig.AddTypeToSkipObjectDumps(typeof(Square));
            StaticLogger.LoggerConfig.AddTypeToSkipObjectDumps(typeof(ChessPiece));
            StaticLogger.LoggerConfig.AddTypeToSkipObjectDumps(typeof(ChessBoard));
            StaticLogger.LoggerConfig.AddTypeToSkipForLogging(typeof(Square));
            //StaticLogger.LoggerConfig.AddTypeToSkipForLogging(typeof(ChessBoard));
            //StaticLogger.LoggerConfig.AddTypeToSkipForLogging(typeof(BoardPosition));


            // You can add any other global setup code here
            //StaticLogger.AddTestName(); // Example of setting test name in logs, if needed
        }
    }
}
