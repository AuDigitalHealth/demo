using System.Text.RegularExpressions;
using Demo.Core.Interfaces;
using Hl7.Fhir.Model;

namespace Demo.Core.Utils
{
    public enum StringModifiers
    {
        Exact,
        Contains
    }
    
    public enum ParamModifiers
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Equal,
        NotEqual,
        StartsAfter,
        EndsBefore,
        Approximately
    }
    
    public enum TokenModifiers
    {
        Text,
        Not,
        Above,
        Below,
        In,
        NotIn,
        OfType
    }

    public class StringParamType : ParamType
    {
        public StringModifiers? Modifier { get; set; }

        public string Value { get; set; }
    } 
    
    public class TypeParamType : ParamType
    {
        public IList<ResourceType> Resources { get; set; }
    }
    
    public class TokenValueModel
    {
        public string Code { get; set; }

        public string System { get; set; }
    }
    
    public class DateParamType : ParamType
    {
        public DateParamValueModel DateValue { get; set; }
    }

    public class DateParamValueModel
    {
        public ParamModifiers? Modifier { get; set; }

        public DateTime Value { get; set; }
    }
    
    public class NumberParamType : ParamType
    {
        public NumberValueModel Value { get; set; }
    }

    public class NumberValueModel
    {
        public ParamModifiers? Modifier { get; set; }

        public decimal Value { get; set; }

        public int GetInteger()
        {
            return (int)Value;
        }
    }
    
    public class TokenParamType : ParamType
    {
        public TokenModifiers? Modifier { get; set; }

        public TokenValueModel TokenValue { get; set; }

        // List of 'OR' values
        /*public IList<TokenValueModel> TokenValues { get; set; }*/
    }
    
    public class ParamBuilder
    {
        /// <summary>
        /// Creates a string parameter.
        /// </summary>
        /// <param name="queryParameter">queryParameter</param>
        /// <param name="resourceType">resourceType</param>
        /// <returns></returns>
        public static StringParamType BuildStringParam(QueryParameter queryParameter, ResourceType? resourceType = null)
        {
            if (string.IsNullOrWhiteSpace(queryParameter.Name))
            {
                throw new ArgumentException("Name is null or empty");
            }

            string[] nameParts = queryParameter.Name.Split(":");

            string name;
            StringModifiers? modifier = null;
            if (nameParts.Length == 1)
            {
                name = nameParts.First();
            }
            else if (nameParts.Length == 2)
            {
                name = nameParts.First();

                // Determine the modifier
                switch (nameParts.Last().ToLower())
                {
                    case "exact":
                        modifier = StringModifiers.Exact;
                        break;
                    case "contains":
                        modifier = StringModifiers.Contains;
                        break;
                    default:
                        throw new ArgumentException($"Invalid string modifier '{nameParts.Last()}'");
                }
            }
            else
            {
                throw new ArgumentException($"Error processing parameter '{queryParameter.Name}'");
            }

            ModelInfo.SearchParamDefinition searchParamDefinition = null;
            if (resourceType.HasValue)
            {
                searchParamDefinition = GetParamDefinition(name, resourceType.Value);
            }

            StringParamType stringParamType = new StringParamType
            {
                Value = queryParameter.Value,
                Name = name,
                Modifier = modifier,
                SearchParamDefinition = searchParamDefinition
            };

            return stringParamType;
        }

        /// <summary>
        /// Creates a '_type' parameter.
        /// </summary>
        /// <param name="typeParamValue">typeParamValue</param>
        /// <returns></returns>
        public static TypeParamType BuildTypeParam(string typeParamValue)
        {
            IList<TokenValueModel> tokenValueModels = BuildTokenParamValues(typeParamValue);
            if (!tokenValueModels.Any())
            {
                throw new ArgumentException("No resource types specified");
            }

            IList<ResourceType> resourceTypes = new List<ResourceType>();
            foreach (TokenValueModel tokenValueModel in tokenValueModels)
            {
                if (Enum.TryParse(tokenValueModel.Code, false, out ResourceType resourceType))
                {
                    resourceTypes.Add(resourceType);
                }
                else
                {
                    throw new ArgumentException($"Invalid resource type '{tokenValueModel}'");
                }
            }

            return new TypeParamType
            {
                Resources = resourceTypes,
                Name = "_type"
            };
        }

        /// <summary>
        /// Creates a date parameter.
        /// </summary>
        /// <param name="queryParameter">queryParameter</param>
        /// <param name="resourceType">resourceType</param>
        /// <returns></returns>
        public static DateParamType BuildDateParam(QueryParameter queryParameter, ResourceType? resourceType = null)
        {
            if (string.IsNullOrWhiteSpace(queryParameter.Name))
            {
                throw new ArgumentException("Name is null or empty");
            }

            ModelInfo.SearchParamDefinition searchParamDefinition = null;
            if (resourceType.HasValue)
            {
                searchParamDefinition = GetParamDefinition(queryParameter.Name, resourceType.Value);
            }

            DateParamValueModel dateParamValue = BuildDateParamValue(queryParameter.Value);

            return new DateParamType
            {
                Name = queryParameter.Name,
                DateValue = dateParamValue,
                SearchParamDefinition = searchParamDefinition
            };
        }

        /// <summary>
        /// Creates the value part of the date parameter.
        /// </summary>
        /// <param name="dateValue">dateValue</param>
        /// <returns></returns>
        public static DateParamValueModel BuildDateParamValue(string dateValue)
        {
            var dateParam = new DateParamValueModel();

            var match = Regex.Match(dateValue, "^([a-z][a-z])?(\\d.+)$");
            if (match.Success)
            {
                if (match.Groups[1].Success)
                {
                    dateParam.Modifier = GetModifier(match.Groups[1].Value);
                }

                bool isValidDate = DateTime.TryParse(match.Groups[2].Value, out var parsedDate);
                if (isValidDate)
                {
                    dateParam.Value = parsedDate;
                }
                else
                {
                    throw new ArgumentException($"Invalid date time format '{match.Groups[2].Value}'");
                }
            }

            return dateParam;
        }

        /// <summary>
        /// Creates a number parameter.
        /// </summary>
        /// <param name="queryParameter">The query parameter.</param>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// Name is null or empty
        /// or
        /// Invalid number value '{queryParameter.Value}'
        /// or
        /// Unable to parse number value '{queryParameter.Value}'
        /// </exception>
        public static NumberParamType BuildNumberParam(QueryParameter queryParameter, ResourceType? resourceType = null)
        {
            if (string.IsNullOrWhiteSpace(queryParameter.Name))
            {
                throw new ArgumentException("Name is null or empty");
            }

            string[] valueParts = queryParameter.Value.Split(":");

            string valuePart;
            ParamModifiers? paramModifier = null;
            if (valueParts.Length == 1)
            {
                valuePart = valueParts.First();
            }
            else if (valueParts.Length == 2)
            {
                valuePart = valueParts.Last();
                paramModifier = GetModifier(valueParts.First());
            }
            else
            {
                throw new ArgumentException($"Invalid number value '{queryParameter.Value}'");
            }

            if (!decimal.TryParse(valuePart, out decimal result))
            {
                throw new ArgumentException($"Unable to parse number value '{queryParameter.Value}'");
            }

            var numberParam = new NumberParamType
            {
                Name = queryParameter.Name,
                Value = new NumberValueModel
                {
                    Value = result,
                    Modifier = paramModifier
                }
            };

            return numberParam;
        }

        /// <summary>
        /// Creates a token parameter.
        /// </summary>
        /// <param name="queryParameter">queryParameter</param>
        /// <param name="resourceType">resourceType</param>
        /// <returns></returns>
        public static TokenParamType BuildTokenParam(QueryParameter queryParameter, ResourceType? resourceType = null)
        {
            if (string.IsNullOrWhiteSpace(queryParameter.Name))
            {
                throw new ArgumentException("Name is null or empty");
            }

            string[] nameParts = queryParameter.Name.Split(":");

            string name;
            TokenModifiers? tokenModifier = null;
            if (nameParts.Length == 1)
            {
                // No modifier
                name = nameParts[0];
            }
            else if (nameParts.Length == 2)
            {
                // Modifier present
                name = nameParts[0].ToLower();

                // Determine the modifier
                switch (nameParts[1])
                {
                    case "not":
                        tokenModifier = TokenModifiers.Not;
                        break;
                    case "text":
                        tokenModifier = TokenModifiers.Text;
                        break;
                    case "above":
                        tokenModifier = TokenModifiers.Above;
                        break;
                    case "below":
                        tokenModifier = TokenModifiers.Below;
                        break;
                    case "in":
                        tokenModifier = TokenModifiers.In;
                        break;
                    case "not-in":
                        tokenModifier = TokenModifiers.NotIn;
                        break;
                    case "of-type":
                        tokenModifier = TokenModifiers.OfType;
                        break;
                    default:
                        throw new ArgumentException($"Invalid token modifier '{nameParts[1]}'");
                }
            }
            else
            {
                throw new ArgumentException($"Name '{queryParameter.Name}' is invalid");
            }

            ModelInfo.SearchParamDefinition searchParamDefinition = null;
            if (resourceType.HasValue)
            {
                searchParamDefinition = GetParamDefinition(name, resourceType.Value);
            }

            return new TokenParamType
            {
                TokenValue = BuildTokenParamValue(queryParameter.Value),
                Name = name,
                Modifier = tokenModifier,
                SearchParamDefinition = searchParamDefinition
            };
        }

        public static IList<TokenValueModel> BuildTokenParamValues(string tokenValue)
        {
            string[] tokenValueParts = tokenValue.Split(",");

            IList<TokenValueModel> tokenValueModels = new List<TokenValueModel>();
            foreach (string tokenValuePart in tokenValueParts)
            {
                tokenValueModels.Add(BuildTokenParamValue(tokenValuePart.Trim()));
            }

            return tokenValueModels;
        }

        /// <summary>
        /// Creates the value part of a token parameter.
        /// </summary>
        /// <param name="tokenValue">tokenValue</param>
        /// <returns></returns>
        public static TokenValueModel BuildTokenParamValue(string tokenValue)
        {
            string tokenRegex = @"^(?:([^\|]+)?\|)?([^\|]+)?$";

            Match match = Regex.Match(tokenValue, tokenRegex);
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid token value format '{tokenValue}'");
            }

            string code = null;
            if (match.Groups[2].Value != string.Empty)
            {
                code = match.Groups[2].Value;
            }

            string system = null;
            if (match.Groups[1].Value != string.Empty)
            {
                system = match.Groups[1].Value;
            }

            return new TokenValueModel
            {
                Code = code,
                System = system
            };
        }

        /// <summary>
        /// Gets the FHIR parameter definition.
        /// </summary>
        /// <param name="paramName">paramName</param>
        /// <param name="resourceType">resourceType</param>
        /// <exception cref="ArgumentException">Thrown when no matching parameter definition found.</exception>
        /// <returns></returns>
        private static ModelInfo.SearchParamDefinition GetParamDefinition(string paramName, ResourceType resourceType)
        {
            ModelInfo.SearchParamDefinition searchParamDefinition = ModelInfo.SearchParameters.SingleOrDefault(
                f => f.Resource == resourceType.ToString() && f.Name == paramName);

            if (searchParamDefinition != null)
            {
                return searchParamDefinition;
            }

            throw new ArgumentException($"No matching search parameter '{paramName}' defined for resource '{resourceType}'");
        }

        /// <summary>
        /// Get the modifier.
        /// </summary>
        /// <param name="modifier">modifier</param>
        /// <returns></returns>
        private static ParamModifiers GetModifier(string modifier)
        {
            switch (modifier)
            {
                case "eq":
                    return ParamModifiers.Equal;
                case "ne":
                    return ParamModifiers.NotEqual;
                case "gt":
                    return ParamModifiers.GreaterThan;
                case "lt":
                    return ParamModifiers.LessThan;
                case "ge":
                    return ParamModifiers.GreaterThanOrEqual;
                case "le":
                    return ParamModifiers.LessThanOrEqual;
                case "sa":
                    return ParamModifiers.StartsAfter;
                case "eb":
                    return ParamModifiers.EndsBefore;
                case "ap":
                    return ParamModifiers.Approximately;
                default:
                    throw new ArgumentException($"Invalid modifier '{modifier}'");
            }
        }
    }
}