namespace DataStructures
{
    public interface IUsersStorageQueries
    {
        string DeleteCertificate { get; }
        string DeleteUser { get; }
        string UpdateUser { get; }
        string InsertCertificate { get; }
        string InsertUser { get; }
        string GetCertificates { get; }
        string GetUsers { get; }
        string CreateTables { get; }
    }
}
