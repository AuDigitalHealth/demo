using Hl7.Fhir.Model;
using Microsoft.Extensions.Options;
using Demo.Core.Settings;
using Demo.Core.Interfaces;
using Demo.Core.Exceptions;
using Hl7.Fhir.Serialization;
using Demo.Core.Models;
using Demo.Core.Utils;
using Hl7.Fhir.Rest;
using Patient = Hl7.Fhir.Model.Patient;
using Questionnaire = Hl7.Fhir.Model.Questionnaire;
using QuestionnaireResponse = Hl7.Fhir.Model.QuestionnaireResponse;

namespace Demo.Core.Services
{
    public class FhirService : IFhirService
    {
        private readonly AppSettings _appSettings;

        public const string ModiProfileUrl = "http://ns.electronichealth.net.au/fhir/StructureDefinition/dh-servicerequest-modi-1";

        private const string LastUpdatedParameter = "_lastUpdated";
        private const string IdentifierParameter = "identifier";
        private const string PerformerParameter = "performer";
        private const string PageParameter = "_page";
        private const string CountParameter = "_count";

        public FhirService( IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        //////////////////////
        // Functions
        // 0) CapabilityStatement
        // 1) CIS Upload ServiceRequest - POST
        // 2) CIS Cancels ServiceRequest (Status) - PATCH
        // 3) RIS Search ServiceRequest - GET (Search)
        // 4) RIS Gets ServiceRequest - GET
        // 5) RIS Update ServiceRequest (Status) - PATCH
        //////////////////////

        public async Task<CapabilityStatement> GetCapabilityStatement()
        {
            var capabilityStatement = new CapabilityStatement
            {
                Name = "EMR Agent FHIR API"
            };

            capabilityStatement.Rest = new List<CapabilityStatement.RestComponent>
            {
                new CapabilityStatement.RestComponent
                {
                    Mode = CapabilityStatement.RestfulCapabilityMode.Server,
                    Resource = new List<CapabilityStatement.ResourceComponent>
                    {
                        new CapabilityStatement.ResourceComponent
                        {
                            Profile = ModiProfileUrl,
                            Type = nameof(ResourceType.ServiceRequest),
                            Versioning = CapabilityStatement.ResourceVersionPolicy.Versioned,
                            ReadHistory = false,
                            Interaction = new List<CapabilityStatement.ResourceInteractionComponent>
                            {
                                new CapabilityStatement.ResourceInteractionComponent
                                {
                                    Code = CapabilityStatement.TypeRestfulInteraction.Read
                                },
                                new CapabilityStatement.ResourceInteractionComponent
                                {
                                    Code = CapabilityStatement.TypeRestfulInteraction.Create
                                },
                                new CapabilityStatement.ResourceInteractionComponent
                                {
                                    Code = CapabilityStatement.TypeRestfulInteraction.Update
                                },
                                new CapabilityStatement.ResourceInteractionComponent
                                {
                                    Code = CapabilityStatement.TypeRestfulInteraction.Patch
                                },
                                new CapabilityStatement.ResourceInteractionComponent
                                {
                                    Code = CapabilityStatement.TypeRestfulInteraction.SearchType
                                }
                            },
                            SearchParam = new List<CapabilityStatement.SearchParamComponent>
                            {
                                new CapabilityStatement.SearchParamComponent
                                {
                                    Name = "performer",
                                    Type = SearchParamType.String
                                },
                                new CapabilityStatement.SearchParamComponent
                                {
                                    Name = "identifier",
                                    Type = SearchParamType.String
                                },
                                new CapabilityStatement.SearchParamComponent
                                {
                                    Name = "_lastUpdated",
                                    Type = SearchParamType.Date
                                }
                            }
                        }
                    }
                }
            };

            return capabilityStatement;
        }

        public async Task<Configuration> GetConfiguration()
        {
            var configuration = new Configuration
            {
                AuthorizationEndpoint = _appSettings.AuthEndpoint,
                TokenEndpoint = _appSettings.TokenEndpoint,
                TokenEndpointAuthMethodsSupported = new[]
                {
                    "authorize-post",
                    //"client_secret_basic",
                    //"private_key_jwt"
                },
                GrantTypesSupported = new[]
                {
                    //"authorization_code",
                    "client_credentials"
                },
                RegistrationEndpoint = _appSettings.RegisterEndpoint,
                ScopesSupported = new[]
                {
                    "openid", "fhirUser", "launch", "launch/patient", "patient/*.rs", "user/Practitioner.rs", "online_access", "launch/questionnaire", "questionnaire/*.rs"
                },
                ResponseTypesSupported = new[] { "code" },
                //ManagementEndpoint = "https://ehr.example.com/user/manage",
                IntrospectionEndpoint = _appSettings.IntrospectEndpoint,
                //RevocationEndpoint = "https://ehr.example.com/user/revoke",
                CodeChallengeMethodsSupported = new[] { "S256" },
                Capabilities = new[]
                {
                    "launch-ehr",
                    "permission-patient",
                    "permission-v2",
                    "client-public",
                    "client-confidential-symmetric",
                    "context-ehr-patient",
                    "sso-openid-connect"
                }
            };

            return configuration;
        }


        // READ Patient Resource
        public async Task<Resource> Read(string resourceName, string id)
        {
            switch (resourceName)
            {
                // HL7 Resources
                case nameof(Practitioner):
                    return new Practitioner
                    {
                        Id = "12345",
                        Name = new List<HumanName>
                        {
                            new HumanName
                            {
                                Family = "Smith",
                                Given = new List<string> { "Bob" },
                                Use = HumanName.NameUse.Official
                            }
                        }
                    };
                case nameof(Encounter):
                    return new Encounter
                    {
                        Id = "12345",
                        Status = Encounter.EncounterStatus.Arrived,
                        Participant = new List<Encounter.ParticipantComponent>
                        {
                            new Encounter.ParticipantComponent
                            {
                                Period = new Period
                                {
                                    Start = "10 Jan 2024",
                                    End = "10 Jan 2024",
                                }
                            }
                        }
                       
                    };

                case nameof(Patient):

                    return new Patient
                    {
                        Id = "ce553796c155438a93335db183b4f060",
                        Name = new List<HumanName>
                        {
                            new HumanName
                            {
                                Family = "Doe",
                                Given = new List<string> { "Jane" },
                                Use = HumanName.NameUse.Official
                            }
                        },
                        Gender = AdministrativeGender.Female,
                        BirthDate = "1967/04/10",
                        Extension = new List<Extension>
                        {
                            new Extension
                            {
                                Url = "http://hl7.org.au/fhir/StructureDefinition/indigenous-status",
                                Value = new Coding("https://healthterminologies.gov.au/fhir/ValueSet/australian-indigenous-status-1", "1", "Aboriginal but not Torres Strait Islander origin")
                                }
                            },
                        Identifier = new List<Identifier>
                        {
                            new Identifier
                            {
                                Type = new CodeableConcept
                                {
                                    Coding = new List<Coding>
                                    {
                                        new Coding("http://terminology.hl7.org/CodeSystem/v2-0203", "NI", "National unique individual identifier")
                                    },
                                    
                                    Text = "IHI"
                                },
                                System = "http://ns.electronichealth.net.au/id/hi/ihi/1.0",
                                Value = "8003608833357361"
                            }
                        },
                        Address = new List<Address>
                        {
                            new Address
                            {
                                Use = Address.AddressUse.Home,
                                Line = new List<string>
                                {
                                    "4 Brisbane Street"
                                },
                                City = "Brisbane",
                                State = "QLD",
                                PostalCode = "4000",
                                Country = "Australia"
                            }
                        }
                        
                    };

                case nameof(Questionnaire):
                    return new Questionnaire();
                case nameof(QuestionnaireResponse):
                    return new QuestionnaireResponse();

                default:
                    throw new DemoValidationException($"Unknown resource type {resourceName}");
            }
        }

        // CREATE QuestionnaireResponse Resource
        public async Task<Resource> Create(string resourceName, Resource resource)
        {
            // Check match
            if (resourceName != resource.TypeName)
            {
                throw new DemoValidationException(
                    $"Resource name {resourceName} and resource type {resource.TypeName} do not match.");
            }

            switch (resourceName)
            {
                // HL7 Resources
                case nameof(QuestionnaireResponse):

                    var questionnaireResponse = (QuestionnaireResponse) resource;

                    // Save to Memory?

                    //ResourceIdentity resourceIdentity = new ResourceIdentity(questionnaireResponse.Subject.Reference);
                    //Data.Entities.Patient patient = await _emrDbContext.Patients.FirstAsync(p => p.ExternalId == resourceIdentity.Id);

                    //var fhirJsonSerializer = new FhirJsonSerializer();
                    //string resourceJson = await fhirJsonSerializer.SerializeToStringAsync(resource);

                    //_emrDbContext.QuestionnaireResponses.Add(new Data.Entities.QuestionnaireResponse
                    //{
                    //    ExternalId = IdUtils.GenerateExternalId("GUID"),
                    //    QuestionnaireResponseData = resourceJson,
                    //    Patient = patient,
                    //    LastUpdated = DateTime.UtcNow
                    //});

                    //await _emrDbContext.SaveChangesAsync();

                    return resource;

                default:
                    throw new DemoValidationException($"Unknown resource type {resourceName}");
            }
        }

        // NOT USED
        public async Task<Resource> Put(string resourceName, Resource resource, string id)
        {
            // Check match
            if (resourceName != resource.TypeName)
            {
                throw new DemoValidationException(
                    $"Resource name {resourceName} and resource type {resource.TypeName} do not match.");
            }

            switch (resourceName)
            {
                // HL7 Resources
                case nameof(QuestionnaireResponse):
                    
                    var questionnaireResponse = (QuestionnaireResponse) resource;

                    // Save to Memory

                    //ResourceIdentity resourceIdentity = new ResourceIdentity(questionnaireResponse.Subject.Reference);
                    //Data.Entities.Patient patient = await _emrDbContext.Patients.FirstAsync(p => p.ExternalId == resourceIdentity.Id);

                    //var fhirJsonSerializer = new FhirJsonSerializer();
                    //string resourceJson = await fhirJsonSerializer.SerializeToStringAsync(resource);

                    //var questionnaireResponseEntity = _emrDbContext.QuestionnaireResponses
                    //    .Include(q => q.Patient)
                    //    .Include(q => q.QuestionnaireResponseData)
                    //    .FirstOrDefault(q => q.ExternalId == id);
                    
                    //if(questionnaireResponseEntity == null)
                    //    throw new DemoValidationException($"Unknown id resource type {resourceName}");
                    
                    //questionnaireResponseEntity.QuestionnaireResponseData = resourceJson;
                    //questionnaireResponseEntity.Patient = patient;
                    //questionnaireResponseEntity.LastUpdated = DateTime.UtcNow;
                    
                    //await _emrDbContext.SaveChangesAsync();

                    return resource;

                default:
                    throw new DemoValidationException($"Unknown body resource type {resourceName}");
            }
        }

        // NOT USED
        public async Task<Bundle> Search(string resourceName, IList<QueryParameter> queryParameters)
        {
            switch (resourceName)
            {
                case nameof(ServiceRequest):
                {
                    return new Bundle();
                }
                case nameof(Questionnaire):
                {
                    return new Bundle();
                }
                case nameof(QuestionnaireResponse):
                {
                    return new Bundle();
                }
                // Other Resources requested
                case nameof(MedicationStatement):
                case nameof(Condition):
                case nameof(Immunization):
                case nameof(CarePlan):
                case nameof(Observation):
                case nameof(Procedure):
                case nameof(AllergyIntolerance):
                case nameof(MedicationRequest):
                case nameof(Consent):
                case nameof(DiagnosticReport):
                case nameof(ClinicalImpression):
                case nameof(Device):
                case nameof(DeviceUseStatement):
                    {
                    return new Bundle();
                }

                default:
                    return new Bundle();
                    //throw new DemoValidationException($"Unknown resource type {resourceName}");
            }
        }

        // NOT USED
        public async Task<ResourceContainer> Search(string dummy, string resourceName, IList<QueryParameter> queryParameters)
        {
            switch (resourceName)
            {
                case nameof(ServiceRequest):
                {
                    return new ResourceContainer(); // await SearchServiceRequestPortal(queryParameters);
                }
                default:
                    throw new DemoValidationException($"Unknown resource type {resourceName}");
            }
        }


    }
}