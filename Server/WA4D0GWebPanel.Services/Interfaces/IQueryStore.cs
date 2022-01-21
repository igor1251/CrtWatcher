namespace WA4D0GWebPanel.Services.Interfaces
{
    public interface IQueryStore
    {
        string DeleteCertificate { get; }
        string DeleteCertificateSubject { get; }
        string UpdateCertificateSubject { get; }
        string InsertCertificate { get; }
        string InsertCertificateSubject { get; }
        string GetCertificates { get; }
        string GetSubjects { get; }
        string CreateTables { get; }
    }
}
