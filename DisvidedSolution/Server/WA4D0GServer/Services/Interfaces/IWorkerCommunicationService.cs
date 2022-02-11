using ElectronicDigitalSignatire.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WA4D0GServer.Services.Interfaces
{
    public interface IWorkerCommunicationService
    {
        Task<IEnumerable<ICertificateSubject>> FetchAllCertificateSubjects(string uri);
        Task<ICertificateSubject> FetchCertificateSubject(string uri);
    }
}
