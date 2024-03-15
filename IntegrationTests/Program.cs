// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

Console.WriteLine("Running Integration Test");
string executableDir = "C:\\Users\\david\\OneDrive\\Documents\\GitHub\\chess\\bin\\Debug\\net7.0\\";
string executablePath = executableDir + "Chess.exe";

Console.WriteLine(executablePath);

Process myProcess = new();
try
{
    myProcess.StartInfo.FileName = executablePath;
    myProcess.StartInfo.UseShellExecute = false; // To redirect input/output
    myProcess.StartInfo.RedirectStandardInput = true;
    myProcess.StartInfo.RedirectStandardOutput = true;
    myProcess.StartInfo.RedirectStandardError = true;
    myProcess.Start();
}
catch (Exception ex)
{
    Console.WriteLine($"There was a problem launching the executable: {ex}");
    return;
}


Random rd = new Random();
string CreateString(int stringLength)
{
    const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
    char[] chars = new char[stringLength];

    for (int i = 0; i < stringLength; i++)
    {
        chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
    }

    return new string(chars);
}

var GenerateRandomInput = () =>
{ 
    return CreateString(rd.Next(100));
};

const int NUM_ITERATIONS = 200;
int iteration = 0;
while (!myProcess.HasExited && iteration < NUM_ITERATIONS) // Keep going until the process ends
{
    string input = GenerateRandomInput(); // Replace with your input generation logic
    myProcess.StandardInput.WriteLine(input);
    //myProcess.StandardOutput.Flush;
    iteration++;
}

Console.WriteLine("Integration Test Complete. Generating Log Report");

async Task MonitorOutput()
{
    StringBuilder outputBuilder = new StringBuilder();
    StringBuilder errorBuilder = new StringBuilder();

    // Using continuous tasks to read asynchronously
    var readOutputTask = Task.Run(() =>
    {
        while (true)
        {
            outputBuilder.Append(myProcess.StandardOutput.ReadLine() + "\n");
        }
    });

    var readErrorTask = Task.Run(() =>
    {
        while (true)
        {
            errorBuilder.Append(myProcess.StandardError.ReadLine() + "\n");
        }
    });

    // Wait for tasks (optionally with timeout) or wait for process to exit
    await Task.WhenAny(readOutputTask, readErrorTask, Task.Delay(5000), Task.FromResult(myProcess.WaitForExitAsync()));

    string output = outputBuilder.ToString();
    string error = errorBuilder.ToString();

    string log = "STANDARD OUTPUT\n" + output + "\n\n\n\n\n\nSTANDARD ERROR\n" + error;

    File.WriteAllText(executableDir + "integration.log", log);

    Console.WriteLine(executableDir + "integration.log created");
    // Rest of your log generation code...
}

// Call the MonitorOutput method 
await MonitorOutput();

try
{
    myProcess.Kill();
    // ... code to start process, send inputs, etc. from above
    myProcess.WaitForExit(); // Wait for the process to finish
}
catch (Exception ex)
{
    // Log crash information (Exception, Stack Trace, Inputs)
    Console.WriteLine(ex);
}
finally
{
    // Handle process resources and close
    myProcess.Close();
}