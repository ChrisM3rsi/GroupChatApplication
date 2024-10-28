using Grains.Models;
using Microsoft.Extensions.Logging;
using Orleans.Utilities;

namespace Grains.Impl;

public class GroupChatGrain : Grain, IGroupChatGrain
{
    private readonly GroupChatState _groupChat = new ();
    
    private readonly ObserverManager<IMessageObserver> _observers;
    private readonly Dictionary<string, IMessageObserver> _personSubscribersDict;

    public GroupChatGrain(ILogger<GroupChatGrain> logger)
    {
       _observers = new ObserverManager<IMessageObserver>(TimeSpan.FromMinutes(5), logger);
       _personSubscribersDict = new Dictionary<string, IMessageObserver>();
    }
    
    public Task AddPerson(PersonState person , IMessageObserver observer)
    {
       _groupChat.Persons.Add(person);
       _observers.Subscribe(observer, observer);
       _personSubscribersDict[person.Name] = observer;
       return Task.CompletedTask;
    }

    public Task RemovePerson(PersonState person)
    {
       _groupChat.Persons.Remove(person);
       _personSubscribersDict.TryGetValue(person.Name, out var personObserver);

       if (personObserver != null)
       {
          _observers.Unsubscribe(personObserver);
       }
       
       return Task.CompletedTask;
    }

    public Task ReceiveMessage(Message message)
    {
       _groupChat.Messages.Add(message.Text);
       Console.WriteLine(message);
       _observers.Notify(x => x.OnMessageReceived(message));
       Console.WriteLine($"Messages Count: {_groupChat.Messages.Count}"); 
       return Task.CompletedTask;
    }
}