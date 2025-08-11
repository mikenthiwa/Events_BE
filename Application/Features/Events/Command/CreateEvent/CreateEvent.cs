using Application.Common.Interface;
using MediatR;

namespace Application.Events.Command.CreateEvent;

public class CreateEventCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Venue { get; set; }
    public int UserId { get; set; }
}

public class CreateEvent(IEventService eventService) : IRequestHandler<CreateEventCommand, int>
{
    public async Task<int> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var eventDetails = new EventDetails
        {
            Name = request.Name,
            Venue = request.Venue,
            UserId = request.UserId
        };
        var eventId = await eventService.CreateEventAsync(eventDetails, cancellationToken);
        return eventId;
    }
}
