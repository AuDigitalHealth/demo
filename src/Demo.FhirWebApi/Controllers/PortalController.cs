using Demo.Core.Interfaces;
using Demo.Core.Models;
using Emr.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Demo.FhirWebApi.Controllers
{
    [ApiController]
    [Route("portal")]
    public class PortalController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public PortalController( IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("documents")]
        [SwaggerOperation(OperationId = "Portal_GetDocument")]
        [ProducesResponseType(typeof(DocumentModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<DocumentModel>> GetDocument(string documentName)
        {
            return Ok(await _documentService.GetDocument(documentName));
        }

        [HttpGet("questionnaires")]
        [SwaggerOperation(OperationId = "Portal_GetQuestionnaires")]
        [ProducesResponseType(typeof(IList<QuestionnaireListResponseModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<QuestionnaireListResponseModel>>> GetQuestionnaires()
        {
            return Ok(await _documentService.GetQuestionnaires());
        }
    }
}