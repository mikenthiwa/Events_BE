using Application.Common.Helper;
using Application.Common.Model;
using Application.Events.Command.CreateEvent;
using Application.Features.Events.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Web.Endpoints;

public static class Event
{
    public static void RegisterEventEndpoints(this IEndpointRouteBuilder route)
    {
        var events = route.MapGroup("/events").AddFluentValidationAutoValidation();
        events.MapPost("/", async  (ISender sender, CreateEventCommand command, AppUser user) =>
            {
                command.UserId = int.TryParse(user.UserId, out var userId)
                    ? userId
                    : throw new UnauthorizedAccessException();
            var eventEntity = await sender.Send(command);
            return TypedResults.Created($"/events/", Result.SuccessResponse(StatusCodes.Status201Created, "Event created successfully", eventEntity));
        })
        .RequireAuthorization()
        .AddFluentValidationAutoValidation();
        
        events.MapGet("/", async (ISender sender, [FromQuery(Name = "pageNumber")] int? pageNumber, [FromQuery(Name = "pageSize")] int? pageSize) =>
        {
            var query = new GetEventsRequest()
            {
                PageNumber = pageNumber ?? 1, // Default to page 1 if not provided
                PageSize = pageSize ?? 10 // Default to page size of 10 if not provided
            };
            var eventList = await sender.Send(query);
            return TypedResults.Ok(Result.SuccessResponse(StatusCodes.Status200OK, "Events retrieved successfully",
                eventList));
        }).RequireAuthorization();
    }
}
