using X509Observer.Primitives.Network;

namespace X509Observer.Primitives.Service
{
    public interface IObserverServiceConfiguration
    {
        ObserverServiceCondition Condition { get; set; }
        ConnectionInfo ConnectionInfo { get; set; }
    }
}
