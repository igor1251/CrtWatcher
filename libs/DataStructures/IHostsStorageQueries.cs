namespace DataStructures
{
    public interface IHostsStorageQueries
    {
        string CreateTablesQuery { get; }
        string GetClientHostsQuery { get; }
        string AddClientHostQuery { get; }
        string UpdateClientHostQuery { get; }
        string DeleteClientHostQuery { get; }
    }
}
