using System.Text;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Task = System.Threading.Tasks.Task;

namespace Demo.FhirWebApi.MediaFormatters
{
    public class JsonOutputFormatter : TextOutputFormatter
    {
        public const string ApplicationFhirJsonMimeType = "application/fhir+json";

        public JsonOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ApplicationFhirJsonMimeType));

            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(Type type) => type == typeof(OperationOutcome) || typeof(Resource).IsAssignableFrom(type);

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            Resource resource = (Resource) context.Object;

            FhirJsonSerializer fhirJsonSerializer = new FhirJsonSerializer(SerializerSettings.CreateDefault());
            string jsonBody = fhirJsonSerializer.SerializeToString(resource);

            await context.HttpContext.Response.WriteAsync(jsonBody);
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            Resource resource = (Resource) context.Object;

            // Location header
            if (!string.IsNullOrWhiteSpace(resource?.Id))
            {
                context.HttpContext.Response.Headers.Add(HeaderNames.Location, resource.ResourceIdentity(string.Empty).OriginalString);
            }

            // Last modified header
            if (resource.Meta?.LastUpdated != null)
            {
                context.HttpContext.Response.Headers.Add(HeaderNames.LastModified, resource.Meta.LastUpdated.Value.UtcDateTime.ToString("r"));
            }
            else
            {
                context.HttpContext.Response.Headers.Add(HeaderNames.LastModified, DateTimeOffset.UtcNow.ToString("r"));
            }

            // ETag header
            if (resource.Meta != null && !string.IsNullOrWhiteSpace(resource.Meta.VersionId))
            {
                context.HttpContext.Response.Headers.Add(HeaderNames.ETag, $"W/\"{resource.Meta.VersionId}\"");
            }

            base.WriteResponseHeaders(context);
        }
    }
}
