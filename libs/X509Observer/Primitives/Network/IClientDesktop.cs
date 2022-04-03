using System;

namespace X509Observer.Primitives.Network
{
    public interface IClientDesktop
    {
        string IP { get; set; }
        string Name { get; set; }
        string Comment { get; set; }
        DateTime LastResponseTime { get; set; }
    }
}
