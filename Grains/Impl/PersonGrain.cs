using Grains.Models;

namespace Grains.Impl;

public class PersonGrain : Grain, IPersonGrain //TODO: check if Person should inherit from IMessageObserver
{
    private readonly PersonState _person = new ();
    

    public async Task JoinGroup(string groupChatName, IMessageObserver observer)
    {
        var groupChatGrain = GrainFactory.GetGrain<IGroupChatGrain>(groupChatName);
        await groupChatGrain.AddPerson(_person, observer); // TODO: possible refactor to _person.Name
        _person.GroupName = groupChatName;
    }

    public Task LeaveGroup()
    {
        if (_person.GroupName == null)
        {
            return Task.CompletedTask;
        }

        var groupChatGrain = GrainFactory.GetGrain<IGroupChatGrain>(_person.GroupName);
        groupChatGrain.RemovePerson(_person); // TODO: possible refactor to _person.Name
        _person.GroupName = default;
        return Task.CompletedTask;
    }

    public Task SendMessage(string message)
    {
        if (_person.GroupName == null)
        {
            return Task.CompletedTask;
        }

        _person.MessagesSent++;
        var groupChatGrain = GrainFactory.GetGrain<IGroupChatGrain>(_person.GroupName);
        groupChatGrain.ReceiveMessage(message);
        return Task.CompletedTask;
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _person.Name = IdentityString ;
        return base.OnActivateAsync(cancellationToken);
    }
}