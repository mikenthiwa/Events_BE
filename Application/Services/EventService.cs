using Application.Common.Interface;
using Application.Features.Events.Command.CreateEvent;
using AutoMapper.QueryableExtensions;

using Application.Mappings;
using Application.Model;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class EventService(IApplicationDbContext applicationDbContext, IMapper _mapper) : IEventService
{
    public async Task<EventDto> CreateEventAsync(EventDetails eventDetails, CancellationToken cancellationToken)
    {
        var eventEntity = new Event()
        {
            Name = eventDetails.Name,
            Venue = eventDetails.Venue,
            UserId = eventDetails.UserId
        };

        applicationDbContext.Events.Add(eventEntity);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<EventDto>(eventEntity);
    }

    public async Task<PaginatedList<EventDto>> GetEventsAsync(int pageNumber, int pageSize,
        CancellationToken cancellationToken)
    {
        return await applicationDbContext.Events
            .OrderBy(e => e.Id)
            .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(pageNumber, pageSize, cancellationToken);
    }
}
