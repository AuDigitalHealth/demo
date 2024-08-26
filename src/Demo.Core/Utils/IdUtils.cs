using Hl7.Fhir.Model;

namespace Demo.Core.Utils
{
    public static class IdUtils
    {
        private const string GuidIdType = "GUID";
        public const string Separator = "-";
        
        public static string GenerateExternalId(string pcaResourceType)
        {
            string guid = Guid.NewGuid().ToString();
            string prefix;

            // Special cases - leave as GUIDs
            if (pcaResourceType == GuidIdType || pcaResourceType == ResourceType.Bundle.ToString())
            {
                return guid;
            }

            switch (pcaResourceType)
            {
                case "NoPrefix":
                    prefix = "";
                    break;
                default:
                    prefix = "";
                    break;
            }

            // Add prefix and remove '-' in guid
            return (prefix + guid.Replace(Separator, ""));
        }
    }
}