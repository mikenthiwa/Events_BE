using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Results;
using SharpGrip.FluentValidation.AutoValidation.Shared.Extensions;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Application.Validation;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{

    public IResult CreateResult(EndpointFilterInvocationContext context, FluentValidation.Results.ValidationResult validationResult)
    {
        var validationProblemErrors = validationResult.ToValidationProblemErrors();
        throw new ValidationException("Validation failed", validationProblemErrors.SelectMany(e => e.Value.Select(value => new ValidationFailure(e.Key, value))));
        // return Results.ValidationProblem(validationProblemErrors);
    }
    
}
