namespace DataStructures
{
    public class UsersStorageQueries : BaseStorageQueries, IUsersStorageQueries
    {
        string
            _getUsers = "SELECT * FROM Users",
            _getCertificates = "SELECT ID, CertificateHash, Algorithm, StartDate, EndDate FROM Certificates WHERE UserID=@ID",
            _deleteCertificate = "DELETE FROM Certificates WHERE ID=@ID",
            _deleteUser = "DELETE FROM Users WHERE ID=@ID",
            _updateUsers = "UPDATE Users SET UserName=@UserName, UserPhone=@UserPhone, UserComment=@UserComment WHERE ID=@ID",
            _insertCertificate = "INSERT INTO Certificates (UserID, CertificateHash, Algorithm, StartDate, EndDate) VALUES (@UserID, @CertificateHash, @Algorithm, @StartDate, @EndDate)",
            _insertUser = "INSERT INTO Users (UserName, UserPhone, UserComment) VALUES (@UserName, @UserPhone, @UserComment)";

        public string GetCertificates => _getCertificates;
        public string GetUsers => _getUsers;
        public string DeleteCertificate => _deleteCertificate;
        public string DeleteUser => _deleteUser;
        public string UpdateUser => _updateUsers;
        public string InsertCertificate => _insertCertificate;
        public string InsertUser => _insertUser;
    }
}
