namespace Site
{
    public class RequestLinks
    {
        private readonly static string IPAddr = "localhost",
                                       Port = "5000";

        public readonly static string UsersResponseLink = "https://" + IPAddr + ":" + Port + "/api/users/",
                                      GetUsersFromDbLink = "https://" + IPAddr + ":" + Port + "/api/users/db",
                                      GetUsersFromSystemStoreLink = "https://" + IPAddr + ":" + Port + "/api/users/system",
                                      CertificatesResponseLink = "https://" + IPAddr + ":" + Port + "/api/certificates/",
                                      GetHostsFromDb = "https://" + IPAddr + ":" + Port + "/api/hosts/db",
                                      GetSettings = "https://" + IPAddr + ":" + Port + "/api/settings/db",
                                      SettingsResponseLink = "https://" + IPAddr + ":" + Port + "/api/settings/";
    }
}
