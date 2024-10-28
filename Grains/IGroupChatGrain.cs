using System.Collections.Immutable;
using Grains.Models;

namespace Grains;

public interface IGroupChatGrain : IGrainWithStringKey
{
    Task AddPerson(PersonState person, IMessageObserver observer);
    
    Task RemovePerson(PersonState person);
    
    Task ReceiveMessage(Message message);
    
    Task<ImmutableList<Message>> GetChatHistory(int lastMessageCount);
}