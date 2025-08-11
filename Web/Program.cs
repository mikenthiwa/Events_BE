using Application;
using Infrastructure;
using Web.Endpoints;
using Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.AddInfrastructure();
builder.AddApplication();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.RegisterAuthEndpoints();
app.RegisterEventEndpoints();

app.UseHttpsRedirection();

app.UseExceptionHandler();





app.Run();


