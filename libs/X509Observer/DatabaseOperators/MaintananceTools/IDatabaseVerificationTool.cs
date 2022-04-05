using System.Threading.Tasks;

namespace X509Observer.DatabaseOperators.MaintananceTools
{
    public interface IDatabaseVerificationTool
    {
        bool CheckDatabaseFileExists();
        Task CreateDatabaseAsync();
    }
}
