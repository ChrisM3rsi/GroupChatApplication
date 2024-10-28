namespace Grains;

public interface IPersonGrain : IGrainWithStringKey
{
    Task JoinGroup(string groupChatName, IMessageObserver observer);

    Task LeaveGroup();

    Task SendMessage(string messageText);
}