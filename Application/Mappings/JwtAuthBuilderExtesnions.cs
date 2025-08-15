using Application.Common.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application.Mappings;

public static class JwtAuthBuilderExtensions
{
    public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection service, IConfiguration configuration)
    {
        var jwtConfiguration = new JwtConfiguration(configuration);
        service.AddAuthorization();

        return service.AddAuthentication(jwtConf =>
        {
            jwtConf.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            jwtConf.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                IssuerSigningKey =
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtConfiguration.Secret)),
            };
            options.Events = new JwtBearerEvents
            {
                    
                OnMessageReceived = context =>
                {
                    // If you want to handle the token in a custom way, you can do it here
                    // For example, you can read the token from a custom header
                    string authorization = context.Request.Headers["Authorization"];
                    if (string.IsNullOrEmpty(authorization))
                    {
                        context.NoResult();
                    }
                    else
                    {
                        context.Token = authorization.Replace("Bearer ", string.Empty);
                    }

                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    if (context.AuthenticateFailure is SecurityTokenExpiredException)
                    {
                        throw new UnauthorizedAccessException("Token has expired. Please log in again.");
                    }
                    return Task.CompletedTask;
                },
            };
        });
    }
}
