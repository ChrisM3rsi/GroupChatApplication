using Grains.Models;

namespace Grains;

public interface IMessageObserver : IGrainObserver
{
    Task OnMessageReceived(Message message);
}