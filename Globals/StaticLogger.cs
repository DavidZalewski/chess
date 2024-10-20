using Chess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Globals
{
    // TODO: Have this class hold a static collection of Loggers. Then Instantiate a logger for each class in the project.
    // Each sub logger will have the class name and method information auto generated. That way when I call StaticLogger.Log("Some Message")
    // It will appear as "ClassName.MethodName(): Some Message"
    public class StaticLogger
    {
        static public IConsole Console { get; set; }
        static public string TestName { get; set; } = ""; 

        static public void Log(string message)
        {
            Console.WriteLine($"{TestName}: {message}");
        }
    }
}
