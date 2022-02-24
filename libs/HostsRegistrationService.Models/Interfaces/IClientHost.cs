namespace HostsRegistration.Models.Interfaces
{
    public interface IClientHost
    {
        string HostName { get; set; }
        string IP { get; set; }
        int ConnectionPort { get; set; }
    }
}
