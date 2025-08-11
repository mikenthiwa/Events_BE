namespace Domain.Entities;

public class Event : BaseAuditable
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Venue { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
