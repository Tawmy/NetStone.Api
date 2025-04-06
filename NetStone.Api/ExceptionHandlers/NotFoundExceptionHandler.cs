using Microsoft.AspNetCore.Diagnostics;
using NetStone.Common.Exceptions;

namespace NetStone.Api.ExceptionHandlers;

internal class NotFoundExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException)
        {
            return true;
        }
        
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext { HttpContext = httpContext });
    }
} 