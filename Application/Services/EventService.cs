using Application.Common.Interface;
using Application.Interface;
using Domain.Entities;

namespace Application.Services;

public class EventService(IApplicationDbContext applicationDbContext) : IEventService
{
    public async Task<int> CreateEventAsync(EventDetails eventDetails, CancellationToken cancellationToken)
    {
        var eventEntity = new Event()
        {
            Name = eventDetails.Name,
            Venue = eventDetails.Venue,
            UserId = eventDetails.UserId
        };

        applicationDbContext.Events.Add(eventEntity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return eventEntity.Id;
    }
}
