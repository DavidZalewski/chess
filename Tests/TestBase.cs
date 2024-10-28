using Chess.Globals;
using Chess.Interfaces;
using Tests.Services;

public abstract class TestBase
{
    [SetUp]
    public virtual void Setup()
    {
        IConsole mockConsole = new MockConsoleService();
        StaticLogger.Console = mockConsole;
        StaticLogger.AddTestName();

        var currentTestName = TestContext.CurrentContext.Test.MethodName;
        Console.WriteLine($"--------------------------------------------------{currentTestName} Begin");

    }

    [TearDown]
    public virtual void TearDown()
    {
        StaticLogger.DumpLog();
        var currentTestName = TestContext.CurrentContext.Test.MethodName;
        Console.WriteLine($"=================================================={currentTestName} End");

    }
}