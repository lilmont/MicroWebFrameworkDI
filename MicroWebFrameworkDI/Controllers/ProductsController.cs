using System.Text;
using MicroWebFramework.Entities;

namespace MicroWebFramework.Controllers;
public class ProductsController
{
    private readonly HttpContext _httpContext;

    public ProductsController(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }
    // Products/GetAllProducts
    public void GetAllProducts()
    {
        _httpContext.Response.OutputStream.Write(
                Encoding.UTF8.GetBytes("List of all products."));
    }
    
}
