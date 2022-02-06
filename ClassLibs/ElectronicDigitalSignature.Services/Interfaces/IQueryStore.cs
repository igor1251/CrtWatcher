namespace ElectronicDigitalSignatire.Services.Interfaces
{
    public interface IQueryStore
    {
        string DeleteCertificate { get; }
        string DeleteSubject { get; }
        string UpdateSubject { get; }
        string InsertCertificate { get; }
        string InsertSubject { get; }
        string GetCertificates { get; }
        string GetSubjects { get; }
        string CreateTables { get; }
    }
}
