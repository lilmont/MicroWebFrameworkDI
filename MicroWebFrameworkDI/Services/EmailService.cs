using MicroWebFramework.Contracts;

namespace MicroWebFramework.Services;

public class EmailService : INotificationService
{
    public void Send(string destination, string message)
    {
        $"Sending email to email address: {destination}".Dump();
    }
}
