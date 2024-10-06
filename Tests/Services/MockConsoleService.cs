using Chess.Interfaces;
using Chess.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    public class MockConsoleService : IConsole
    {
        public Queue<string> Inputs { get; set; }
        public List<string> Outputs { get; } = new List<string>();

        public MockConsoleService(Queue<string> inputs) 
        {
            Inputs = inputs;
        }

        public string? ReadLine()
        {
            if (Inputs != null)
            {
                if (Inputs.Count == 0)
                {
                    throw new Exception("Mock Console Service has run out of mock inputs to use");
                }
                string readOut = Inputs.Dequeue();
                return readOut;
            }

            throw new Exception("Mock Console Service has run out of mock inputs to use");
        }

        public void WriteLine(string? text)
        {
            Outputs.Add(text);
            Console.WriteLine(text);
        }

        public bool OutputHasExactString(string? text)
        {
            if (Outputs != null)
            {
                return Outputs.Any(o => o.Equals(text));
            }

            throw new Exception("Mock Console Service has null for outputs");
        }

        public bool OutputContainsString(string? text, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
        {
            if (Outputs != null)
            {
                return Outputs.Any(o => o.Contains(text, stringComparison));
            }

            throw new Exception("Mock Console Service has null for outputs");
        }

        public int OutputContainsStringCount(string? text, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
        {
            if (Outputs != null)
            {
                return Outputs.FindAll(o => o.Contains(text, stringComparison)).Count;
            }

            throw new Exception("Mock Console Service has null for outputs");
        }
    }
}
