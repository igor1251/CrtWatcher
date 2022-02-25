namespace DataStructures
{
    public class UsersStorageQueries : IUsersStorageQueries
    {
        string
            _getUsers = "SELECT * FROM Users",
            _getCertificates = "SELECT ID, CertificateHash, Algorithm, StartDate, EndDate FROM Certificates WHERE UserID=@ID",
            _deleteCertificate = "DELETE FROM Certificates WHERE ID=@ID",
            _deleteUser = "DELETE FROM Users WHERE ID=@ID",
            _updateUsers = "UPDATE Users SET UserName=@UserName, UserPhone=@UserPhone, UserComment=@UserComment WHERE ID=@ID",
            _insertCertificate = "INSERT INTO Certificates (UserID, CertificateHash, Algorithm, StartDate, EndDate) VALUES (@UserID, @CertificateHash, @Algorithm, @StartDate, @EndDate)",
            _insertUser = "INSERT INTO Users (UserName, UserPhone, UserComment) VALUES (@UserName, @UserPhone, @UserComment)",

            _createTables = "CREATE TABLE Certificates (" +
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
                            "UserComment VARCHAR(200));";

        public string GetCertificates => _getCertificates;
        public string GetUsers => _getUsers;
        public string DeleteCertificate => _deleteCertificate;
        public string DeleteUser => _deleteUser;
        public string UpdateUser => _updateUsers;
        public string InsertCertificate => _insertCertificate;
        public string InsertUser => _insertUser;
        public string CreateTables => _createTables;
    }
}
