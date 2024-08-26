using Hl7.Fhir.Model;

namespace Demo.Core.Utils
{
    public abstract class ParamType
    {
        public string Name { get; set; }

        public ModelInfo.SearchParamDefinition SearchParamDefinition { get; set; }
    }
}