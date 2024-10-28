using Grains.Models;

namespace Client;

public class ConsoleState
{
    public string PromptText { get; set; } = "Input: ";

    public string Input { get; set; } = string.Empty;

    public IList<Message> Output { get; } = new List<Message>
    {
        null,
        null,
        null,
        null,
        null,
        null
    };
}