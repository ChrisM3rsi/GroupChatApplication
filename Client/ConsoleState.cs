namespace Client;

public class ConsoleState
{
    public string PromptText { get; set; } = "Input: ";

    public string Input { get; set; } = string.Empty;

    public IList<string> Output { get; } = new List<string>
    {
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty
    };
}