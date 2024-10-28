namespace Client;

public class ConsoleService
{
    private readonly ConsoleState _consoleState = new();
    private readonly CancellationTokenSource _cts = new();
    
    public async Task HandleInput(
        Func<string, Task> inputHandler)
    {
        while (!_cts.Token.IsCancellationRequested)
        {
            var inputChar = Console.ReadKey().KeyChar;

            if (inputChar == '\r')
            {
                if (_consoleState.Input == "exit")
                {
                    await _cts.CancelAsync();
                    break;
                }

                await inputHandler(_consoleState.Input);
                _consoleState.Input = string.Empty;
            }
            else
            {
                switch (inputChar)
                {
                    case '\b':

                        if (_consoleState.Input.Length > 0)
                        {
                            _consoleState.Input = _consoleState.Input[..^1];
                        }

                        break;
                    default:
                        _consoleState.Input += inputChar;
                        break;
                }
            }
        }
    }

    public async Task StartConsole(TimeSpan refreshInterval)
    {
        while (!_cts.Token.IsCancellationRequested)
        {
            Console.Clear();
            foreach (var outputLine in _consoleState.Output.TakeLast(5))
            {
                Console.WriteLine(outputLine);
            }

            Console.WriteLine();
            Console.Write(_consoleState.PromptText + _consoleState.Input);

            await Task.Delay(refreshInterval, _cts.Token);
        }
    }

    public void AppendToOutput(string message)
    {
        _consoleState.Output.Add(message);
    }
}