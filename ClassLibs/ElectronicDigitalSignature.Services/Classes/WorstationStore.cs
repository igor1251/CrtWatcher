using ElectronicDigitalSignature.Models.Interfaces;
using ElectronicDigitalSignature.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicDigitalSignature.Services.Classes
{
    public class WorstationStore : IWorkstationStore
    {
        public Task AddWorkstation()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IWorkstation>> GetRegisteredWorkstations()
        {
            throw new NotImplementedException();
        }

        public Task RemoveWorkstation()
        {
            throw new NotImplementedException();
        }
    }
}
