namespace WA4D0GWebPanel.Models
{
    public class RequestLinks
    {
        private readonly static string IPAddr = "localhost",
                                       Port = "5003";

        public readonly static string SubjectsResponseLink = "https://" + IPAddr + ":" + Port + "/api/subjects/",
                                      GetSubjectsFromDbLink = "https://" + IPAddr + ":" + Port + "/api/subjects/db",
                                      GetSubjectsFromSystemStoreLink = "https://" + IPAddr + ":" + Port + "/api/subjects/system",
                                      CertificatesResponseLink = "https://" + IPAddr + ":" + Port + "/api/certificates/";
    }
}
