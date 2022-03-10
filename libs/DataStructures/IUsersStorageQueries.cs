namespace DataStructures
{
    public interface IUsersStorageQueries : IBaseStorageQueries
    {
        string DeleteCertificate { get; }
        string DeleteUser { get; }
        string UpdateUser { get; }
        string InsertCertificate { get; }
        string InsertUser { get; }
        string GetCertificates { get; }
        string GetUsers { get; }
    }
}
