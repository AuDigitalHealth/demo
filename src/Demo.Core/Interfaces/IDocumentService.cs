using Demo.Core.Models;
using Emr.Core.Models;

namespace Demo.Core.Interfaces;

public interface IDocumentService
{
    Task<DocumentModel> GetDocument(string documentName);

    Task<IList<QuestionnaireListResponseModel>> GetQuestionnaires();
}