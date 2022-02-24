namespace HostsRegistration.Services.Interfaces
{
    public interface IQueryStore
    {
        string CreateTablesQuery { get; }
        string GetClientHostsQuery { get; }
        string AddClientHostQuery { get; }
        string UpdateClientHostQuery { get; }
        string DeleteClientHostQuery { get; }
    }
}
