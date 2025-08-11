using System.Reflection;
using Application.Common.Helper;
using Application.Common.Interface;
using Application.Common.Model;
using Application.Interface;
using Application.Services;
using Application.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IHostApplicationBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        builder.Services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });
        builder.Services.AddValidatorsFromAssembly(assembly);
        builder.Services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IEventService, EventService>();
        builder.Services.AddTransient<JwtConfiguration>();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddTransient<AppUser>();
        builder.Services.AddHttpContextAccessor();
    }
}
