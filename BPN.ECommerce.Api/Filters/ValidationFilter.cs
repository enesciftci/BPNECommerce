using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BPN.ECommerce.Api.Filters;

public sealed class ValidationFilter<T> : IEndpointFilter
{
    private readonly ILogger<ValidationFilter<T>> logger;

    public ValidationFilter(ILogger<ValidationFilter<T>> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argToValidate = (T?)context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T));
        if (argToValidate is null)
        {
            logger.LogCritical("ValidationFilter: Type {Type} not found in arguments. TraceId: {TraceId}", typeof(T), context.HttpContext.TraceIdentifier);
            return Results.UnprocessableEntity();
        }

        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
        var validationResult = await validator.ValidateAsync(argToValidate!);

        if (!validationResult.IsValid)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            
            return new ValidationProblemDetails()
            {
                Status = (int)HttpStatusCode.UnprocessableEntity,
                Errors = validationResult.ToDictionary(),
                Title = "Validation Failed"
            };
        }

        return await next(context);
    }
}
