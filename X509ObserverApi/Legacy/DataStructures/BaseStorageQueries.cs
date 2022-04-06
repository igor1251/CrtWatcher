namespace DataStructures
{
    public class BaseStorageQueries : IBaseStorageQueries
    {
        private readonly string _createTables = "CREATE TABLE Certificates (" +
                                                "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                                "UserID INTEGER NOT NULL, " +
                                                "CertificateHash VARCHAR(512) NOT NULL, " +
                                                "Algorithm VARCHAR(512) NOT NULL, " +
                                                "StartDate DATETIME NOT NULL, " +
                                                "EndDate DATETIME NOT NULL);" +
                                                "" +
                                                "CREATE TABLE Users (" +
                                                "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                                "UserName VARCHAR(120) NOT NULL, " +
                                                "UserPhone VARCHAR(20), " +
                                                "UserComment VARCHAR(200));" +
                                                "" +
                                                "CREATE TABLE RegisteredHosts (" +
                                                "HostName VARCHAR(50) NOT NULL, " +
                                                "IP VARCHAR(30) NOT NULL, " +
                                                "ConnectionPort INTEGER NOT NULL);" +
                                                "" +
                                                "CREATE TABLE Settings (" +
                                                "VerificationFrequency INTEGER NOT NULL, " +
                                                "MainServerPort INTEGER NOT NULL, " +
                                                "MainServerIP VARCHAR(50) NOT NULL);";


        public string CreateTables => _createTables;
    }
}
