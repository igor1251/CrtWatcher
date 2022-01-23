using ElectrnicDigitalSignatire.Services.Interfaces;

namespace ElectrnicDigitalSignatire.Services.Classes
{
    public class QueryStore : IQueryStore
    {
        string
            _getCertificates = "select * from [certificates] left join [subjects] on [certificates].subjectID = [subjects].id;",
            _getSubjects = "select * from [subjects];",
            _deleteCertificate = "delete from [certificates] where [certificates].id = @ID",
            _deleteCertificateSubject = "delete from [certificates] where [certificates].subjectID = @ID; delete from [subjects] where [subjects].id = @ID;",
            _updateCertificateSubject = "update [subjects]  set subjectPhone = @SubjectPhone, subjectComment = @SubjectComment where [subjects].id = @ID",
            _insertCertificate = "insert into [certificates] (subjectID, certificateHash, algorithm, startDate, endDate) values(@Subject.ID, @CertificateHash, @Algorithm, @StartDate, @EndDate);",
            _insertCertificateSubject = "insert into [subjects] (subjectName, subjectPhone, subjectComment) values(@SubjectName, @SubjectPhone, @SubjectComment);",
            _createTables =
            "CREATE TABLE subjects (" +
            "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
            "subjectName VARCHAR(120) NOT NULL, " +
            "subjectPhone VARCHAR(20), " +
            "subjectComment VARCHAR(200));" +
            "" +
            "CREATE TABLE certificates (" +
            "id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
            "subjectID INTEGER NOT NULL, " +
            "certificateHash VARCHAR(512) NOT NULL, " +
            "algorithm VARCHAR(512) NOT NULL, " +
            "startDate DATETIME NOT NULL, " +
            "endDate DATETIME NOT NULL );";

        public string GetCertificates => _getCertificates;
        public string GetSubjects => _getSubjects;
        public string DeleteCertificate => _deleteCertificate;
        public string DeleteCertificateSubject => _deleteCertificateSubject;
        public string UpdateCertificateSubject => _updateCertificateSubject;
        public string InsertCertificate => _insertCertificate;
        public string InsertCertificateSubject => _insertCertificateSubject;
        public string CreateTables => _createTables;
    }
}
