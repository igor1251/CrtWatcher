﻿namespace Site
{
    public class RequestLinks
    {
        private readonly static string IPAddr = "crt-server",
                                       Port = "80",
                                       Proto = "https";

        public readonly static string UsersResponseLink = Proto + "://" + IPAddr + ":" + Port + "/api/users/",
                                      GetUsersFromDbLink = Proto + "://" + IPAddr + ":" + Port + "/api/users/db",
                                      GetUsersFromSystemStoreLink = Proto + "://" + IPAddr + ":" + Port + "/api/users/system",
                                      CertificatesResponseLink = Proto + "://" + IPAddr + ":" + Port + "/api/certificates/",
                                      GetHostsFromDb = Proto + "://" + IPAddr + ":" + Port + "/api/hosts/db",
                                      GetSettings = Proto + "://" + IPAddr + ":" + Port + "/api/settings/db",
                                      SettingsResponseLink = Proto + "://" + IPAddr + ":" + Port + "/api/settings/";
    }
}
