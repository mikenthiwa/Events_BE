using System.Security.Claims;
using Application.Authentication.Command.Auth;
using Application.Common.Helper;
using Microsoft.AspNetCore.Authorization;
using Application.Common.Model;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Web.Endpoints;

public static class Auth
{
    public static void RegisterAuthEndpoints(this IEndpointRouteBuilder route)
    {
        var auth = route.MapGroup("/auth");
        auth.MapPost("/google", async Task<Results<Ok<Result>, BadRequest>> (ISender sender, AuthWithGoogleCommandRequest command) =>
        {
            var token = await sender.Send(command);
            return TypedResults.Ok(Result.SuccessResponse(StatusCodes.Status200OK, "Login successful", token));
        });

        auth.MapGet("test", (AppUser user) =>
        {
            var userId = user.UserId;
            Console.WriteLine($"User ID: {userId}");
        }).RequireAuthorization();
    }
}
