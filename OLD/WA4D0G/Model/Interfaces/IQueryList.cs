namespace WA4D0G.Model.Interfaces
{
    public interface IQueryList
    {
        string DeleteCertificateQuery { get; }
        string GetCertificateByIDQuery { get; }
        string GetCertificatesQuery { get; }
        string InsertCertificateQuery { get; }
        string UpdateCertificateQuery { get; }
        string GetUnavailableCertificatesQuery { get; }
        string CreateDatabaseQuery { get; }
    }
}
