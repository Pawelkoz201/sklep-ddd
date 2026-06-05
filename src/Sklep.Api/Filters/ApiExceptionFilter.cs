using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sklep.Application.Common;
using Sklep.Domain.Common;

namespace Sklep.Api.Filters;

public sealed class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            NotFoundException exception => new NotFoundObjectResult(exception.Message),
            InvalidCredentialsException => new UnauthorizedObjectResult("Invalid credentials."),
            BusinessRuleViolationException exception => new BadRequestObjectResult(exception.Message),
            DomainException exception => new BadRequestObjectResult(exception.Message),
            _ => context.Result
        };

        context.ExceptionHandled = context.Result is not null;
    }
}
