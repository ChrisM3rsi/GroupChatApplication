namespace Grains.Impl;

public class MessageObserver : IMessageObserver
{
    public Task OnMessageReceived(string message)
    {
        Console.WriteLine($"Message Received: {message}");
        return Task.CompletedTask;
    }
}