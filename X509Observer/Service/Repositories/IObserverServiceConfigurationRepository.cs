using System.Threading.Tasks;
using X509Observer.Service.Entities;

namespace X509Observer.Service.Repositories
{
    public interface IObserverServiceConfigurationRepository
    {
        Task<ObserverServiceConfiguration> LoadObserverServiceConfiguration();
        Task SaveObserverServiceConfiguration(ObserverServiceConfiguration configuration);
    }
}
