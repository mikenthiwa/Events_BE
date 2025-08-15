using Application.Features.Events.Command.CreateEvent;
using Application.Model;
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
    public Task<EventDto> CreateEventAsync(EventDetails eventDetails, CancellationToken cancellationToken);
    public Task<PaginatedList<EventDto>> GetEventsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
