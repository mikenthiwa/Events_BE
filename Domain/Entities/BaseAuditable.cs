namespace Domain.Entities;

public class BaseAuditable
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
