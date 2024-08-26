export const environment = {
	production: true,
	// FIXED to this URL
	//fhirApiEndpoint: '#{FHIR_ENDPOINT}',
	//version: '#{Octopus.Release.Number}',
	//dateCreated: '#{Octopus.Deployment.Created | Format "dd-MMM-yyyy HH:mm"}',
	fhirApiEndpoint: 'DO NOT USE',
	version: '1.0.0',
	dateCreated: new Date(),
	//demoApiEndpoint: 'https://sandbox.digitalhealth.gov.au/DemoFhirWebApi/v2/fhir',
	demoApiEndpoint: 'https://bne-drp-iis.digitalhealth.gov.au/DemoFhirWebApi/v2',
};
