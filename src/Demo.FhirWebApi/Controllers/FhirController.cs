using Hl7.Fhir.Model;
using Demo.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Demo.Core.Utils;
using Demo.Core.Interfaces;

namespace Demo.FhirWebApi.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("fhir")]
    public class FhirController : ControllerBase
    {
        private readonly IFhirService _fhirService;

        public FhirController(IFhirService fhirService)
        {
            _fhirService = fhirService;
        }

        [HttpGet]
        [Route("metadata")]
        public async Task<ActionResult<CapabilityStatement>> Metadata()
        {
            // No Security
            return await _fhirService.GetCapabilityStatement();
        }

        [HttpGet]
        [Route(".well-known/smart-configuration")]
        public async Task<ActionResult<Configuration>> SmartConfig()
        {
            // No Security
            return await _fhirService.GetConfiguration();
        }

        [HttpGet]
        [Route("{resourceName}")]
        public async Task<ActionResult<Resource>> Search(string resourceName)
        {
            var queryParams = ParamUtils.ParseQueryString(Request.QueryString.Value);
            return await _fhirService.Search(resourceName, queryParams);
        }

        [HttpGet]
        [Route("{resourceName}/{id}")]
        public async Task<ActionResult<Resource>> Read(string resourceName, string id)
        {
            return await _fhirService.Read(resourceName, id);
        }

        [HttpPost]
        [Route("{resourceName}")]
        public async Task<ActionResult<Resource>> Create(string resourceName, [FromBody] Resource bodyResource)
        {
            return await _fhirService.Create(resourceName, bodyResource);
        }

        [HttpPut]
        [Route("{resourceName}/{id}")]
        public async Task<ActionResult<Resource>> Put(string resourceName, string id, [FromBody] Parameters bodyResource)
        {
            return await _fhirService.Put(resourceName, bodyResource, id);
        }

    }
}