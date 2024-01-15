namespace MicroWebFramework.Contracts;

public interface INotificationService
{
    public void Send(string destination, string message);
}
