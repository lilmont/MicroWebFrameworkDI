using MicroWebFramework.Contracts;

namespace MicroWebFramework.Services;

public class SMSService : INotificationService
{
    public void Send(string destination, string message)
    {
        $"Sending sms to phone number: {destination}".Dump();
    }
}
