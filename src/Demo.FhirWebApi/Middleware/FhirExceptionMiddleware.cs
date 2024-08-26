using System.Net;
using FluentValidation;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc.Formatters;
using Demo.Core.Exceptions;
using Demo.FhirWebApi.MediaFormatters;
using Task = System.Threading.Tasks.Task;

namespace Demo.FhirWebApi.Middleware
{
    public class FhirExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<FhirExceptionMiddleware> _logger;

        public FhirExceptionMiddleware(RequestDelegate requestDelegate, ILogger<FhirExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string eventId = Guid.NewGuid().ToString();
            OperationOutcome outcome;

            _logger.LogError(exception, "Service error ID '{errorId}'", eventId);

            HttpStatusCode statusCode;

            if (exception is DemoValidationException)
            {
                outcome = OperationOutcome.ForException(exception, OperationOutcome.IssueType.Invalid);
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exception is DemoUnauthorizedException)
            {
                outcome = OperationOutcome.ForMessage(null, OperationOutcome.IssueType.Forbidden);
                statusCode = HttpStatusCode.Unauthorized;
            }
            else if (exception is DemoNotFoundException)
            {
                outcome = OperationOutcome.ForException(exception, OperationOutcome.IssueType.NotFound);
                statusCode = HttpStatusCode.NotFound;
            }
            else if (exception is ValidationException) // Fluent Validation exception
            {
                outcome = OperationOutcome.ForException(exception, OperationOutcome.IssueType.Invalid);
                statusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                // Setting as fatal as it's an unknown exception
                outcome = OperationOutcome.ForException(exception, OperationOutcome.IssueType.Unknown,
                    OperationOutcome.IssueSeverity.Fatal);
                statusCode = HttpStatusCode.InternalServerError;
            }

            outcome.Id = eventId;

            _logger.LogDebug("Reponse body '{@eventResponse}'", outcome);

            context.Response.StatusCode = (int)statusCode;

            var formatter = new JsonOutputFormatter();
            var outputFormatterWriteContext = new OutputFormatterWriteContext(context,
                (stream, encoding) => new StreamWriter(stream, encoding),
                typeof(OperationOutcome), outcome);

            await formatter.WriteAsync(outputFormatterWriteContext);
        }
    }
}
