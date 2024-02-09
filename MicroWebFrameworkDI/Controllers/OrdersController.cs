using System.Text;
using System.Text.Json;
using MicroWebFramework.Contracts;
using MicroWebFramework.Entities;

namespace MicroWebFramework.Controllers;
public class OrdersController
{
    private readonly HttpContext _httpContext;
    private readonly INotificationService _notificationService;

    public OrdersController(HttpContext httpContext,
        INotificationService notificationService)
    {
        _httpContext = httpContext;
        _notificationService = notificationService;
    }

    List<Order> orders = new List<Order>()
    {
        new Order() { Id = 1, Title = "Order1" },
        new Order() { Id = 2, Title = "Order2" },
        new Order() { Id = 3, Title = "Order3" },
        new Order() { Id = 4, Title = "Order4" },
        new Order() { Id = 5, Title = "Order5" },
    };

    // Orders/GetAllOrders
    public void GetAllOrders()
    {
        string ordersJson = JsonSerializer.Serialize(orders, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        _httpContext.Response.OutputStream.Write(
                Encoding.UTF8.GetBytes(ordersJson));
    }
}
