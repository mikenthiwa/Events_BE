using Application.Common.Interface;
using Application.Features.Events.Command.CreateEvent;
using MediatR;

namespace Application.Events.Command.CreateEvent;

public class CreateEventCommand : IRequest<EventDto>
{
    public string Name { get; set; }
    public string Venue { get; set; }
    public int UserId { get; set; }
}

public class CreateEvent(IEventService eventService) : IRequestHandler<CreateEventCommand, EventDto>
{
    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var eventDetails = new EventDetails
        {
            Name = request.Name,
            Venue = request.Venue,
            UserId = request.UserId
        };
        var eventEntity = await eventService.CreateEventAsync(eventDetails, cancellationToken);
        return eventEntity;
    }
}
