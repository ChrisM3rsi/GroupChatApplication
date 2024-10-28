using System.Collections.Immutable;
using Grains;
using Grains.Models;

namespace Client;

public class ChatService
{
    private readonly IClusterClient _client;
    private readonly Action<Message> _messageHandler;
    private readonly IMessageObserver _observer;
    private IPersonGrain? _personGrain;
    private IGroupChatGrain? _groupChatGrain;

    public ChatService(IClusterClient client, Action<Message> messageHandler)
    {
        _client = client;
        _messageHandler = messageHandler;
        _observer = client.CreateObjectReference<IMessageObserver>(new MessageObserver(messageHandler));
    }

    public async Task HandleInput(string input)
    {
        if (input.StartsWith("/join ") && input.Length > 6)
        {
            _personGrain?.JoinGroup(input[6..], _observer);
            _groupChatGrain = _client.GetGrain<IGroupChatGrain>(input[6..]);
            var chatHistory = await GetChatHistory();
            foreach (var message in chatHistory)
            {
                _messageHandler(message);
            }
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

    private async Task<ImmutableList<Message>> GetChatHistory(int messageCount = 5)
    {
        return await _groupChatGrain?.GetChatHistory(messageCount);
    }
}