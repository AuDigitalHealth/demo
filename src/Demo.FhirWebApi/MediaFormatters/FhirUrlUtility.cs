using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Demo.FhirWebApi.MediaFormatters
{
    public static class FhirUrlUtility
    {
        public static Uri GetResourceBase(HttpRequest httpRequest)
        {
            Uri requestUrl = new (UriHelper.GetDisplayUrl(httpRequest));

            var scheme = requestUrl.Scheme;
            var host = requestUrl.Host;
            var port = requestUrl.IsDefaultPort ? string.Empty : $":{requestUrl.Port}";
            var pathBase = httpRequest.Path.Value.TrimEnd('/');

            return new Uri($"{scheme}://{host}{port}{pathBase}/");
        }
    }
}
