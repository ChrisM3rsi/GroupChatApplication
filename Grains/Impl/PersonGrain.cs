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

    public async Task LeaveGroup()
    {
        if (_person.GroupName == null)
        {
            return;
        }

        var groupChatGrain = GrainFactory.GetGrain<IGroupChatGrain>(_person.GroupName);
        await groupChatGrain.RemovePerson(_person); // TODO: possible refactor to _person.Name
        _person.GroupName = default;
    }

    public async Task SendMessage(string messageText)
    {
        if (_person.GroupName == null)
        {
            return;
        }

        _person.MessagesSent++;
        var groupChatGrain = GrainFactory.GetGrain<IGroupChatGrain>(_person.GroupName);
        await groupChatGrain.ReceiveMessage(new Message
        {
            Timestamp = DateTime.UtcNow,
            Text = messageText,
            Sender = _person.Name,
            ChannelName = _person.GroupName
        });
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _person.Name = IdentityString ;
        return base.OnActivateAsync(cancellationToken);
    }
}