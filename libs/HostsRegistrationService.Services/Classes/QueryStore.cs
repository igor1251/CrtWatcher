using HostsRegistrationService.Services.Interfaces;

namespace HostsRegistrationService.Services.Classes
{
    public class QueryStore : IQueryStore
    {
        private readonly static string _createTablesQuery = "CREATE TABLE RegisteredHosts (ID INTEGER PRIMARY KEY AUTOINCREMENT, HostName VARCHAR(50) NOT NULL, IP VARCHAR(30) NOT NULL, ConnectionPort INTEGER NOT NULL);",
            _getClientsHostQuery = "SELECT * FROM [RegisteredHosts]",
            _addClientHostQuery = "INSERT INTO [RegisteredHosts] (HostName, IP, ConnectionPort) VALUES (@HostName, @IP, @ConnectionPort)",
            _updateClientHostQuery = "UPDATE [RegisteredHosts] SET ConnectionPort = @ConnectionPort",
            _deleteClientHostQuery = "DELETE FROM [RegisteredHosts] WHERE IP = @IP";
        public string CreateTablesQuery => _createTablesQuery;

        public string GetClientHostsQuery => _getClientsHostQuery;

        public string AddClientHostQuery => _addClientHostQuery;

        public string UpdateClientHostQuery => _updateClientHostQuery;

        public string DeleteClientHostQuery => _deleteClientHostQuery;
    }
}
