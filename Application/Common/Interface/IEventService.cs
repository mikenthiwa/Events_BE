using Domain.Entities;

namespace Application.Common.Interface;

public class EventDetails
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Venue { get; set; }
    public int UserId { get; set; }
}

public interface IEventService
{
    public Task<int> CreateEventAsync(EventDetails eventDetails, CancellationToken cancellationToken);
}
