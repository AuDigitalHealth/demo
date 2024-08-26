using System.Text;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;


namespace Demo.FhirWebApi.MediaFormatters
{
    public class JsonFhirInputFormatter : TextInputFormatter
    {
        public const string ApplicationFhirJsonMimeType = "application/fhir+json";

        public JsonFhirInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(ApplicationFhirJsonMimeType));

            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanReadType(Type type) => typeof(Resource).IsAssignableFrom(type);


        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var httpContext = context.HttpContext;

            using var streamReader = new StreamReader(httpContext.Request.Body, encoding);
            string body = await streamReader.ReadToEndAsync();

            try
            {
                FhirJsonParser fhirJsonParser = new FhirJsonParser(ParserSettings.CreateDefault());
                Resource resource = fhirJsonParser.Parse<Resource>(body);

                return await InputFormatterResult.SuccessAsync(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return await InputFormatterResult.FailureAsync();
            }
        }
    }
}
