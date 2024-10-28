using Grains.Models;

namespace Grains.Impl;

public class PersonGrain : Grain<PersonState>, IPersonGrain //TODO: check if Person should inherit from IMessageObserver
{
    public async Task JoinGroup(string groupChatName, IMessageObserver observer)
    {
        var groupChatGrain = GrainFactory.GetGrain<IGroupChatGrain>(groupChatName);
        await groupChatGrain.AddPerson(State, observer); // TODO: possible refactor to _person.Name
        State.GroupName = groupChatName;
        await WriteStateAsync();
    }

    public async Task LeaveGroup()
    {
        if (State.GroupName == null)
        {
            return;
        }

        var groupChatGrain = GrainFactory.GetGrain<IGroupChatGrain>(State.GroupName);
        await groupChatGrain.RemovePerson(State); // TODO: possible refactor to _person.Name
        State.GroupName = default;
        await WriteStateAsync();
    }

    public async Task SendMessage(string messageText)
    {
        if (State.GroupName == null)
        {
            return;
        }

        var groupChatGrain = GrainFactory.GetGrain<IGroupChatGrain>(State.GroupName);
        State.MessagesSent++;
        await groupChatGrain.ReceiveMessage(new Message
        {
            Timestamp = DateTime.UtcNow,
            Text = messageText,
            Sender = State.Name,
            ChannelName = State.GroupName
        });
        await WriteStateAsync();
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        State.Name = IdentityString;
        Console.WriteLine($"{IdentityString} is activated");
        return base.OnActivateAsync(cancellationToken);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        Console.WriteLine($"{IdentityString} is deactivated reason: {reason}");
        
        return base.OnDeactivateAsync(reason, cancellationToken);
    }
}