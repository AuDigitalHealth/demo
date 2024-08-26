using Hl7.Fhir.Model;
using Demo.Core.Models;

namespace Demo.Core.Interfaces
{
    public interface IFhirService
    {
        Task<CapabilityStatement> GetCapabilityStatement();

        Task<Configuration> GetConfiguration();

        Task<Bundle> Search(string resourceName, IList<QueryParameter> queryParameters);

        Task<ResourceContainer> Search(string dummy, string resourceName, IList<QueryParameter> queryParameters);

        Task<Resource> Read(string resourceName, string id);

        Task<Resource> Create(string resourceName, Resource resource);

        Task<Resource> Put(string resourceName, Resource resource, string id);
    }

    public class QueryParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class IncludeParameter
    {
        public string ResourceName { get; set; }

        public string PropertyToInclude { get; set; }
    }
}