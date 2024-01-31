using MicroWebFramework.Contracts;
using MicroWebFramework.Entities;
using MicroWebFramework.Services;
using System.Text;

namespace MicroWebFramework.Controllers;

public class UsersController
{
    private readonly HttpContext _httpContext;
    private readonly IEnumerable<INotificationService> _notificationService;

    public UsersController(HttpContext httpContext,
        IEnumerable<INotificationService> notificationService)
    {
        _httpContext = httpContext;
        _notificationService = notificationService;
    }

    List<User> users = new()
    {
        new User() { Id = 1, Name = "User1", PhoneNumber = "123", Email="user1@example.com" },
        new User() { Id = 2, Name = "User2", PhoneNumber = "456", Email="user2@example.com" },
        new User() { Id = 3, Name = "User3", PhoneNumber = "789", Email="user3@example.com" },
        new User() { Id = 4, Name = "User4", PhoneNumber = "159", Email="user4@example.com" },
        new User() { Id = 5, Name = "User5", PhoneNumber = "753", Email="user5@example.com" },
    };

    // Users/GetUserById/{id}
    public void GetUserById(int id)
    {
        if (!users.Any(p => p.Id == id))
        {
            _httpContext.Response.OutputStream.Write(
                Encoding.UTF8.GetBytes($"No user was found with id: {id}!!"));
            return;
        }
        _httpContext.Response.OutputStream.Write(
                Encoding.UTF8.GetBytes(
                    users.SingleOrDefault(p => p.Id == id).Name));
        return;
    }

    // Users/CreateUser/{id}
    public void CreateUser(int userId)
    {
        var user = users.SingleOrDefault(p => p.Id == userId);
        if (user != null)
        {
            if (!string.IsNullOrEmpty(user.Email))
            {
                _notificationService.SingleOrDefault(p=>p is EmailService).
                    Send(user.Email, $"User {user.Name} is created.");
            }
            else
                _notificationService.SingleOrDefault(p => p is SMSService).
                    Send(user.PhoneNumber, $"User {user.Name} is created.");
            _httpContext.Response.OutputStream.Write(
               Encoding.UTF8.GetBytes($"Notification was sent to {user.Name}"));
        }
            
    }
}
