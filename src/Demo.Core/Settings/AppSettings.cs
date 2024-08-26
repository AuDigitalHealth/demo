namespace Demo.Core.Settings
{
    public class AppSettings
    {
        public string ApplicationName { get; set; }

        public string Version { get; set; }

        public string FhirEndpoint { get; set; }

        public string AuthEndpoint { get; set; }

        public string TokenEndpoint { get; set; }

        public string RegisterEndpoint { get; set; }

        public string IntrospectEndpoint { get; set; }
        

        public string SmartEndpoint { get; set; }

        public string FromEmail { get; set; }

        public int RetryCount { get; set; }

        public SmtpServerSettings SmtpServer { get; set; }
    }

    public class SmtpServerSettings
    {
        public string Endpoint { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}