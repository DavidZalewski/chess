using Chess.Attributes;
using Chess.Globals;
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
        public Queue<string> Inputs { get; set; } = new Queue<string>();
        public List<string> Outputs { get; } = new List<string>();

        public MockConsoleService() { 
        }

        public MockConsoleService(Queue<string> inputs) 
        {
            Inputs = inputs;
        }

        public virtual string? ReadLine()
        {
            if (Inputs != null)
            {
                if (Inputs.Count == 0)
                {
                    throw new Exception("Mock Console Service has run out of mock inputs to use");
                }
                string readOut = Inputs.Dequeue();
                Console.WriteLine(readOut); // so we can see what the input is supposed to be at this point
                ToDoAttribute.Add("Figure out what we are doing here");
                //StaticLogger.Log(readOut); // TODO: Figure out what we are doing here
                return readOut;
            }

            Exception e = new Exception("Mock Console Service has run out of mock inputs to use");
            //StaticLogger.Log($"{e}", LogLevel.Error);
            throw e;
        }

        public virtual void WriteLine(string? text)
        {
            Outputs.Add(text);
            Console.WriteLine(text);
            //StaticLogger.Log(text);
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
