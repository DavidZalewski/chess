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

        public ChessDotComReplayMockConsoleService(Queue<string> inputs, string file, string expectedOutcome) : base(inputs)
        {
            _fileName = file;
            _expectedOutcome = expectedOutcome;
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
            if (!OutputContainsString(_expectedOutcome))
            {
                //Outputs.Add($"The game outcome did not match the expected checkmate. (Expected Outcome: {_expectedOutcome}, File: {_fileName})");
                foreach(var output in Outputs)
                {
                    Console.WriteLine(output);
                }
                return false;
            }
            else
            {
                Console.WriteLine($"Successfully verified replay {_fileName}");
            }
            return true;
        }
    }
}
