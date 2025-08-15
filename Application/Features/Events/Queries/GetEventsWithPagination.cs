using System.ComponentModel;
using Application.Common.Interface;
using Application.Features.Events.Command.CreateEvent;
using Application.Model;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Events.Queries;

public class GetEventsRequest: IRequest<PaginatedList<EventDto>>
{
    [FromQuery] public int PageNumber { get; init; } = 1;
    [FromQuery] public int PageSize { get; init; } = 10;
}

public class GetEventsWithPagination(IEventService eventService) : IRequestHandler<GetEventsRequest, PaginatedList<EventDto>>
{
    public Task<PaginatedList<EventDto>> Handle(GetEventsRequest request, CancellationToken cancellationToken)
    {
        return eventService.GetEventsAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
