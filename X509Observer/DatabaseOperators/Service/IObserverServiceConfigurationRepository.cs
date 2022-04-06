using System.Threading.Tasks;
using X509Observer.Primitives.Service;

namespace X509Observer.DatabaseOperators.Service
{
    public interface IObserverServiceConfigurationRepository
    {
        Task<ObserverServiceConfiguration> LoadObserverServiceConfiguration();
        Task SaveObserverServiceConfiguration(ObserverServiceConfiguration configuration);
    }
}
