﻿namespace Site
{
    public static class RequestLinks
    {
        private readonly static string IPAddr = "192.168.55.101",
                                       Port = "5000",
                                       Proto = "http";

        public readonly static string UsersResponseLink = Proto + "://" + IPAddr + ":" + Port + "/api/users/",
                                      GetUsersFromDbLink = Proto + "://" + IPAddr + ":" + Port + "/api/users/db",
                                      GetUsersFromSystemStoreLink = Proto + "://" + IPAddr + ":" + Port + "/api/users/system",
                                      CertificatesResponseLink = Proto + "://" + IPAddr + ":" + Port + "/api/certificates/",
                                      GetHostsFromDb = Proto + "://" + IPAddr + ":" + Port + "/api/hosts/db",
                                      GetSettings = Proto + "://" + IPAddr + ":" + Port + "/api/settings/db",
                                      SettingsResponseLink = Proto + "://" + IPAddr + ":" + Port + "/api/settings/",
                                      HostResponseLink = Proto + "://" + IPAddr + ":" + Port + "/api/hosts/";
    }
}
