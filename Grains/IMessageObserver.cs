namespace Grains;

public interface IMessageObserver : IGrainObserver
{
    Task OnMessageReceived(string message);
    
}