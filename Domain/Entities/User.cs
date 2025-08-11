namespace Domain.Entities;

public class User : BaseAuditable
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsAdmin { get; set; } = false;
}
