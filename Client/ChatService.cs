using Grains;
using Grains.Models;

namespace Client;

public class ChatService
{
    private readonly IClusterClient _client;
    private readonly IMessageObserver _observer;
    private IPersonGrain? _personGrain;

    public ChatService(IClusterClient client, Action<Message> messageHandler)
    {
        _client = client;
        _observer = client.CreateObjectReference<IMessageObserver>(new MessageObserver(messageHandler));
    }

    public void HandleInput(string input)
    {
        if (input.StartsWith("/join ") && input.Length > 6)
        {
            _personGrain?.JoinGroup(input[6..], _observer);
        }        
        else if (input.StartsWith("/name ") && input.Length > 6)
        {
            _personGrain = _client.GetGrain<IPersonGrain>(input[6..]);
        }
        else
        {
            _personGrain?.SendMessage(input);
        }
    }
}