namespace DataStructures
{
    public interface IHostsStorageQueries : IBaseStorageQueries
    {
        string GetClientHostsQuery { get; }
        string AddClientHostQuery { get; }
        string UpdateClientHostQuery { get; }
        string DeleteClientHostQuery { get; }
    }
}
