using Application.Interface;
using Application.Services;
using Domain.Entities;
using MediatR;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.Command.Auth;

public class AuthWithGoogleCommandRequest : IRequest<string>
{
    public required string AccessToken { get; set; }
    public required string IdToken { get; set; }
}

public class AuthWithGoogle(IUserService userService, TokenService tokenService) : IRequestHandler<AuthWithGoogleCommandRequest, string>
{
    
    public async Task<string> Handle(AuthWithGoogleCommandRequest request, CancellationToken cancellationToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
        var user = await userService.GetUserByEmailAsync(payload.Email, cancellationToken);
        user ??= await userService.CreateUserAsync(payload.Email, payload.GivenName, payload.FamilyName, cancellationToken);
        var token = tokenService.GenerateToken(user.Id.ToString(), user.Email);
        return token;
    }
}
