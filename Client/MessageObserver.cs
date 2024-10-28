using Grains;
using Grains.Models;

namespace Client;

public class MessageObserver : IMessageObserver
{
    private readonly Action<Message> _messageHandler;

    public MessageObserver(Action<Message> messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public Task OnMessageReceived(Message message)
    {
        _messageHandler(message);
        return Task.CompletedTask;
    }
}