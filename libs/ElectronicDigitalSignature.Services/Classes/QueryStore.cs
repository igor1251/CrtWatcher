using ElectronicDigitalSignatire.Services.Interfaces;

namespace ElectronicDigitalSignatire.Services.Classes
{
    public class QueryStore : IQueryStore
    {
        string
            _getSubjects = "SELECT * FROM Subjects",
            _getCertificates = "SELECT ID, CertificateHash, Algorithm, StartDate, EndDate FROM Certificates WHERE SubjectID=@ID",
            _deleteCertificate = "DELETE FROM Certificates WHERE ID=@ID",
            _deleteSubject = "DELETE FROM Subjects WHERE ID=@ID",
            _updateSubject = "UPDATE Subjects SET SubjectName=@SubjectName, SubjectPhone=@SubjectPhone, SubjectComment=@SubjectComment WHERE ID=@ID",
            _insertCertificate = "INSERT INTO Certificates (SubjectID, CertificateHash, Algorithm, StartDate, EndDate) VALUES (@SubjectID, @CertificateHash, @Algorithm, @StartDate, @EndDate)",
            _insertSubject = "INSERT INTO Subjects (SubjectName, SubjectPhone, SubjectComment) VALUES (@SubjectName, @SubjectPhone, @SubjectComment)",

            _createTables = "CREATE TABLE Certificates (" +
                            "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                            "SubjectID INTEGER NOT NULL, " +
                            "CertificateHash VARCHAR(512) NOT NULL, " +
                            "Algorithm VARCHAR(512) NOT NULL, " +
                            "StartDate DATETIME NOT NULL, " +
                            "EndDate DATETIME NOT NULL);" +
                            "" +
                            "CREATE TABLE Subjects (" +
                            "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                            "SubjectName VARCHAR(120) NOT NULL, " +
                            "SubjectPhone VARCHAR(20), " +
                            "SubjectComment VARCHAR(200));";

        public string GetCertificates => _getCertificates;
        public string GetSubjects => _getSubjects;
        public string DeleteCertificate => _deleteCertificate;
        public string DeleteSubject => _deleteSubject;
        public string UpdateSubject => _updateSubject;
        public string InsertCertificate => _insertCertificate;
        public string InsertSubject => _insertSubject;
        public string CreateTables => _createTables;
    }
}
