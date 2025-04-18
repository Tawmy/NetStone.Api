using Microsoft.AspNetCore.Diagnostics;
using NetStone.Common.Exceptions;

namespace NetStone.Api;

internal class ExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ParsingFailedException => StatusCodes.Status503ServiceUnavailable,
            _ => httpContext.Response.StatusCode
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext { HttpContext = httpContext });
    }
}