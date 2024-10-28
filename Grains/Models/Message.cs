namespace Grains.Models;

[GenerateSerializer]
public class Message
{
    [Id(0)]
    public DateTime Timestamp { get; set; }
    
    [Id(1)]
    public string Text { get; set; } = string.Empty;

    [Id(2)] 
    public string Sender { get; set; } = string.Empty;

    [Id(3)] 
    public string ChannelName { get; set; } = string.Empty;
}