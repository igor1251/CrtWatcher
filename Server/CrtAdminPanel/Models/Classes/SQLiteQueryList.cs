using CrtAdminPanel.Models.Interfaces;

namespace CrtAdminPanel.Models.Classes
{
    public class SQLiteQueryList : IQueryList
    {
        public readonly string        _deleteCertificateQuery =          @"DELETE FROM Certificates 
                                                                           WHERE Certificates.ID=@ID;",

                                      _getCertificateByIDQuery =         @"SELECT *
                                                                           FROM Certificates 
                                                                           WHERE Certificates.ID=@ID;",

                                      _getCertificatesQuery =            @"SELECT* FROM Certificates;",

                                      _insertCertificateQuery =          @"INSERT OR IGNORE INTO Certificates (HolderFIO, HolderPhone, CertStartDateTime, CertEndDateTime) 
                                                                           VALUES (@HolderFIO, @HolderPhone, @CertStartDateTime, @CertEndDateTime);",

                                      _updateCertificateQuery =          @"UPDATE Certificates 
                                                                           SET HolderPhone=@HolderPhone 
                                                                           WHERE ID=@ID",

                                      _getUnavailableCertificatesQuery = @"SELECT * 
                                                                           FROM Certificates 
                                                                           WHERE julianday(CertEndDateTime) - julianday('now') <= @WarnDaysCount",

                                      _createDatabaseQuery =             @"CREATE TABLE Certificates(
                                                                           ID                  INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                           HolderFIO           VARCHAR(120) NOT NULL,
                                                                           HolderPhone         VARCHAR(10) NOT NULL,
                                                                           CertStartDateTime   TEXT NOT NULL,
                                                                           CertEndDateTime     TEXT NOT NULL)";

        string IQueryList.DeleteCertificateQuery
        {
            get => _deleteCertificateQuery;
        }

        string IQueryList.GetCertificateByIDQuery 
        {
            get => _getCertificateByIDQuery;
        }
        string IQueryList.GetCertificatesQuery
        {
            get => _getCertificatesQuery;
        }

        string IQueryList.InsertCertificateQuery
        {
            get => _insertCertificateQuery;
        }

        string IQueryList.UpdateCertificateQuery
        {
            get => _updateCertificateQuery;
        }

        string IQueryList.GetUnavailableCertificatesQuery
        {
            get => _getUnavailableCertificatesQuery;
        }

        string IQueryList.CreateDatabaseQuery
        {
            get => _createDatabaseQuery;
        }
    }
}
