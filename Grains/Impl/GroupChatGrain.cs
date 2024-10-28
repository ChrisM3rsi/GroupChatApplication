using System.Collections.Immutable;
using Grains.Models;
using Microsoft.Extensions.Logging;
using Orleans.Utilities;

namespace Grains.Impl;

public class GroupChatGrain : Grain<GroupChatState>, IGroupChatGrain
{
    private readonly ObserverManager<IMessageObserver> _observerManager;

    public GroupChatGrain(ILogger<GroupChatGrain> logger)
    {
       _observerManager = new ObserverManager<IMessageObserver>(TimeSpan.FromMinutes(5), logger);
    }
    
    public async Task AddPerson(PersonState person, IMessageObserver observer)
    {
       State.Persons.Add(person);
       State.Observers[person.Name] = observer;
       var personGrain = GrainFactory.GetGrain<IPersonGrain>(person.Name);
       _observerManager.Subscribe(personGrain, observer);
       await WriteStateAsync();
    }

    public async Task RemovePerson(PersonState person)
    {
       State.Persons.Remove(person);
       State.Observers.Remove(person.Name);
       var personGrain = GrainFactory.GetGrain<IPersonGrain>(person.Name);
       _observerManager.Unsubscribe(personGrain);
       await WriteStateAsync();
    }

    public async Task ReceiveMessage(Message message)
    {
       State.Messages.Add(message);
       await _observerManager.Notify(x => x.OnMessageReceived(message));
       await WriteStateAsync();
    }

    public Task<ImmutableList<Message>> GetChatHistory(int lastMessageCount)
    {
       var messages = State.Messages
          .TakeLast(lastMessageCount)
          .ToImmutableList();
       
       return Task.FromResult(messages);
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
       Console.WriteLine($"{IdentityString} is activated");

       foreach (var person in State.Persons)
       {
          var personGrain = GrainFactory.GetGrain<IPersonGrain>(person.Name);
          _observerManager.Subscribe(personGrain, State.Observers[person.Name]);
       }
        
       return base.OnActivateAsync(cancellationToken);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
       Console.WriteLine($"{IdentityString} is deactivated reason: {reason}");
        
       return base.OnDeactivateAsync(reason, cancellationToken);
    }
}