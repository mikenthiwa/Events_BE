using Application.Common.Interface;
using Application.Common.Model;
using Application.Interface;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'TodoDb' not found.");
        var jwtConfiguration = new JwtConfiguration(builder.Configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddDbContext<ApplicationDbContext>((options) =>
        {
            options.UseNpgsql(connectionString);
        });
        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        // builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        builder.Services.AddAuthentication(jwtConfiguration =>
            {
                jwtConfiguration.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                jwtConfiguration.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
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
                x.Events = new JwtBearerEvents
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
                };
            });

    }
}
