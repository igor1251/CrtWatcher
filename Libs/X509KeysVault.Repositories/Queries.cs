namespace X509KeysVault.Repositories
{
    internal static class Queries
    {
        internal static readonly string GET_SUBJECTS = "SELECT * FROM [Subjects];",
                                      GET_SUBJECT_BY_ID = "SELECT * FROM [Subjects] WHERE ID=@ID;",
                                      GET_DIGITAL_FINGERPRINTS = "SELECT * FROM [DigitalFingerprints];",
                                      GET_DIGITAL_FINGERPRINTS_BY_SUBJECT_ID = "SELECT * FROM [DigitalFingerprints] WHERE SubjectID=@SubjectID;",
                                      GET_DIGITAL_FINGERPRINT_BY_HASH = "SELECT * FROM [DigitalFingerprints] WHERE Hash=@Hash",
                                      ADD_SUBJECT = "INSERT INTO [Subjects] (Name, Phone) VALUES (@Name, @Phone);",
                                      ADD_DIGITAL_FINGERPRINT = "INSERT INTO [DigitalFingerprints] (SubjectID, Hash, Start, End) VALUES (@SubjectID, @Hash, @Start, @End);",
                                      UPDATE_SUBJECT = "UPDATE [Subjects] SET Name=@Name, Phone=@Phone WHERE ID=@ID;",
                                      REMOVE_DIGITAL_FINGERPRINT_BY_ID = "DELETE FROM [DigitalFingerprints] WHERE ID=@ID;",
                                      REMOVE_SUBJECT_BY_ID = "DELETE FROM [Subjects] WHERE ID=@ID;",
                                      REMOVE_DIGITAL_FINGERPRINTS_BY_SUBJECT_ID = "DELETE FROM [DigitalFingerprints] WHERE SubjectID=@ID",
                                      GET_SUBJECT_ID = "SELECT ID FROM [Subjects] WHERE Name=@Name",
            
            
                                      CREATE_TABLES = "CREATE TABLE DigitalFingerprints(" +
                                                      "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                                      "SubjectID INTEGER NOT NULL, " +
                                                      "Hash VARCHAR(512) NOT NULL, " +
                                                      "Start DATETIME NOT NULL, " +
                                                      "End DATETIME NOT NULL);" +
                                                      "" +
                                                      "CREATE TABLE Subjects (" +
                                                      "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                                      "Name VARCHAR(120) NOT NULL, " +
                                                      "Phone VARCHAR(20));";
    }
}
