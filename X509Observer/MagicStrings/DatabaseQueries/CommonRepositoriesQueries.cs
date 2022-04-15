﻿namespace X509Observer.MagicStrings.DatabaseQueries
{
    public static class CommonRepositoriesQueries
    {
        public static readonly string CREATE_DATABASE = "CREATE TABLE DigitalFingerprints (" +
                                                        "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                                        "SubjectID INTEGER NOT NULL, " +
                                                        "Hash VARCHAR(512) NOT NULL, " +
                                                        "Start DATETIME NOT NULL, " +
                                                        "End DATETIME NOT NULL);" +
                                                        "" +
                                                        "CREATE TABLE Subjects (" +
                                                        "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                                        "Name VARCHAR(120) NOT NULL, " +
                                                        "Phone VARCHAR(20));" +
                                                        "" +
                                                        "CREATE TABLE ApiUsers (" +
                                                        "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                                        "UserName VARCHAR(120) NOT NULL, " +
                                                        "PasswordHash VARCHAR(256) NOT NULL," + 
                                                        "Role VARCHAR(50));" + 
                                                        "" + 
                                                        "CREATE TABLE ApiKeys (" +
                                                        "ApiUserID INTEGER NOT NULL," + 
                                                        "Value VARCHAR(256) NOT NULL," +
                                                        "ExpirationTime DATETIME NOT NULL);";
    }
}
