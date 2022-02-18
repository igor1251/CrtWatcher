namespace WA4D0GWebPanel.Models
{
    public class RequestLinks
    {
        private readonly static string IPAddr = "localhost",
                                       Port = "5001";

        public readonly static string UsersResponseLink = "https://" + IPAddr + ":" + Port + "/api/users/",
                                      GetUsersFromDbLink = "https://" + IPAddr + ":" + Port + "/api/users/db",
                                      GetUsersFromSystemStoreLink = "https://" + IPAddr + ":" + Port + "/api/users/system",
                                      CertificatesResponseLink = "https://" + IPAddr + ":" + Port + "/api/certificates/";
    }
}
