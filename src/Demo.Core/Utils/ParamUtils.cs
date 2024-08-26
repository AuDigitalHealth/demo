using System.Net;
using Demo.Core.Interfaces;

namespace Demo.Core.Utils
{
    public static class ParamUtils
    {
        /// <summary>
        /// Parse the query string.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static IList<QueryParameter> ParseQueryString(string queryString)
        {
            return queryString.Replace("?", string.Empty).Split('&', StringSplitOptions.RemoveEmptyEntries).Select(queryParam =>
            {
                var paramSplit = queryParam.Split('=');
                return new QueryParameter
                {
                    Name = WebUtility.UrlDecode(paramSplit[0]),
                    Value = WebUtility.UrlDecode(paramSplit[1])
                };
            }).ToList();
        }
        
        /// <summary>
        /// Gets a parameter with the name.
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="queryParameters">The query parameters</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when more than one parameter exists with the name.</exception>
        public static QueryParameter GetParameter(string name,  IList<QueryParameter> queryParameters)
        {
            QueryParameter matchedParameter = null;

            foreach (var queryParameter in queryParameters)
            {
                string[] nameParts = queryParameter.Name.Split(":");

                if (name == nameParts.First())
                {
                    if (matchedParameter != null)
                    {
                        throw new ArgumentException($"More than one '{name}' parameter found");
                    }

                    matchedParameter = queryParameter;
                }
            }

            return matchedParameter;
        }

        /// <summary>
        /// Gets a parameter with the name.
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="compareName">Name of compare</param>
        /// <param name="queryParameters">The query parameters</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when more than one parameter exists with the name.</exception>
        public static QueryParameter GetParameter(string name, string compareName, IList<QueryParameter> queryParameters)
        {
            QueryParameter matchedParameter = null;

            foreach (var queryParameter in queryParameters)
            {
                string[] nameParts = queryParameter.Name.Split(":");

                if (name == nameParts.First())
                {
                    if (matchedParameter != null && queryParameter.Value.StartsWith(compareName))
                    {
                        throw new ArgumentException($"More than one '{name}' parameter found");
                    }

                    // Looking for a specific value in the value part
                    if (queryParameter.Value.StartsWith(compareName))
                    {
                        matchedParameter = queryParameter;
                    }
                }
            }

            return matchedParameter;
        }
    }
}
