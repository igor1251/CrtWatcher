namespace DataStructures
{
    public class HostsStorageQueries : BaseStorageQueries, IHostsStorageQueries
    {
        private readonly static string _getClientsHostQuery = "SELECT * FROM [RegisteredHosts]",
                                       _addClientHostQuery = "INSERT INTO [RegisteredHosts] (HostName, IP, ConnectionPort) VALUES (@HostName, @IP, @ConnectionPort)",
                                       _updateClientHostQuery = "UPDATE [RegisteredHosts] SET ConnectionPort = @ConnectionPort",
                                       _deleteClientHostQuery = "DELETE FROM [RegisteredHosts] WHERE IP = @IP";

        public string GetClientHostsQuery => _getClientsHostQuery;

        public string AddClientHostQuery => _addClientHostQuery;

        public string UpdateClientHostQuery => _updateClientHostQuery;

        public string DeleteClientHostQuery => _deleteClientHostQuery;
    }
}
