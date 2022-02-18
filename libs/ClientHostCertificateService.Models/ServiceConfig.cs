namespace ClientHostCertificateService.Models
{
    public class ServiceConfig
    {
        public ServiceConfig()
        {
            Condition = ServiceCondition.None;
            FrequencyOfVerificateonInHours = 0;
        }

        public ServiceCondition Condition { get; set; }
        public int FrequencyOfVerificateonInHours { get; set; }
    }
}
