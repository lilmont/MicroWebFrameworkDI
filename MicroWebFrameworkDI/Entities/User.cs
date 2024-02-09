namespace MicroWebFramework.Entities;
public class User
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
}
