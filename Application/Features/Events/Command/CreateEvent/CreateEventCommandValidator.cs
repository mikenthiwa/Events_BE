using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Interceptors;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Application.Events.Command.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Event name is required");

        RuleFor(x => x.Venue)
            .NotEmpty()
            .WithMessage("Venue is required");

        // RuleFor(x => x.UserId)
        //     .GreaterThan(0)
        //     .WithMessage("User ID must be greater than 0");
    }

    // public IValidationContext? BeforeValidation(EndpointFilterInvocationContext endpointFilterInvocationContext,
    //     IValidationContext validationContext)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public ValidationResult? AfterValidation(EndpointFilterInvocationContext endpointFilterInvocationContext,
    //     IValidationContext validationContext)
    // {
    //     return null;
    // }

}
