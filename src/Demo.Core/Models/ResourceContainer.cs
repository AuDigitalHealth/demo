namespace Demo.Core.Models
{
    public class ResourceContainer
    {
        public string FhirId { get; set; }

        public string Identifier { get; set; }

        public string FhirPayload { get; set; }

        public string Hl7Payload { get; set; }

        public string RequestType { get; set; }

        public string Status { get; set; }

        public string SendingHpio { get; set; }

        public string PreferredReceivingHpio { get; set; }

        public string PatientFullName { get; set; }

        public DateTime PatientDob { get; set; }

        public string TestName { get; set; }

        public string TestCode { get; set; }

        public DateTime DateOfReferral { get; set; }

        public string Requester { get; set; }

        public string PatientMobile { get; set; }

        public string PatientEmail { get; set; }

        public string PatientMedicareNumber { get; set; }

        public string PatientIhi { get; set; }
    }
}