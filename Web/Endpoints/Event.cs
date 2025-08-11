using Application.Common.Model;
using Application.Events.Command.CreateEvent;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Web.Endpoints;

public static class Event
{
    public static void RegisterEventEndpoints(this IEndpointRouteBuilder route)
    {
        var events = route.MapGroup("/events").AddFluentValidationAutoValidation();
        events.MapPost("/", async  (ISender sender, CreateEventCommand command) =>
        {
            // var eventId = await sender.Send(command);
            // return TypedResults.Created(
            //     $"/events/{eventId}",
            //     Result.SuccessResponse(StatusCodes.Status201Created, "Event created successfully"));
            return TypedResults.Created();
        }).AddFluentValidationAutoValidation();
    }
}
