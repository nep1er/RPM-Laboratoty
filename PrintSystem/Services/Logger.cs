using PrintSystem.Services.Mediator;

namespace PrintSystem.Services;

public class Logger : Colleague
{
    private readonly List<string> _logs = new();
    private readonly bool _consoleOutput;

    public Logger(bool consoleOutput = true)
    {
        _consoleOutput = consoleOutput;
    }
    public void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        var logEntry = $"[{timestamp}] {message}";

        _logs.Add(logEntry);
        if (_consoleOutput)
            Console.WriteLine(logEntry);
    }
    public IReadOnlyList<string> GetLogs() => _logs.AsReadOnly();
    public void Clear() => _logs.Clear();
}