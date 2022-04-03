namespace X509Observer.Primitives.Network
{
    public interface IConnectionInfo
    {
        string ServerIP { get; set; }
        string ServerPort { get; set; }
        string Protocol { get; set; }
    }
}
