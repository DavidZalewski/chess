using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    internal class ChessDotComReplayMockConsoleService : MockConsoleService
    {
        private string _fileName;
        private string _expectedOutcome;
        private bool _showOutputOfWinningGames = false;
        private static int _totalFiles = 0;
        private static int _successfulValidationFiles = 0;
        private static int _failedValidationFiles = 0;


        public ChessDotComReplayMockConsoleService(Queue<string> inputs, string file, string expectedOutcome, bool showOutputOfWinningGames) : base(inputs)
        {
            _fileName = file;
            _expectedOutcome = expectedOutcome;
            _showOutputOfWinningGames = showOutputOfWinningGames;
        }
        public override string? ReadLine()
        {
            if (Inputs != null)
            {
                if (Inputs.Count == 0)
                {
                    throw new Exception("Mock Console Service has run out of mock inputs to use");
                }
                string readOut = Inputs.Dequeue();
                Outputs.Add(readOut); // so we can see what the input is supposed to be at this point
                return readOut;
            }

            Exception e = new Exception("Mock Console Service has run out of mock inputs to use");
            throw e;
        }

        public override void WriteLine(string? text)
        {
            Outputs.Add(text);
        }

        public bool VerifyReplayAndLogIfFailed()
        {
            _totalFiles += 1;
            // FAILED TEST CASE MATCH
            if (!OutputContainsString(_expectedOutcome))
            {
                //Outputs.Add($"The game outcome did not match the expected checkmate. (Expected Outcome: {_expectedOutcome}, File: {_fileName})");
                foreach(var output in Outputs)
                {
                    Console.WriteLine(output);
                }
                _failedValidationFiles += 1;
                return false;
            }
            else // SUCCESSFUL VALIDATION OF MATCH
            {
                Console.WriteLine($"Successfully verified replay {_fileName}");
                //Outputs.Add($"The game outcome did not match the expected checkmate. (Expected Outcome: {_expectedOutcome}, File: {_fileName})");
                if (_showOutputOfWinningGames)
                    foreach (var output in Outputs)
                        Console.WriteLine(output);
                _successfulValidationFiles += 1;
                return true;
            }  
        }

        public static void OutputTotals()
        {
            Console.WriteLine($"Successfully verified {_successfulValidationFiles} out of {_totalFiles} replays");
            Console.WriteLine($"{_failedValidationFiles} replays failed to verify.");
        }
    }
}
