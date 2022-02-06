using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WA4D0G.MaintenanceTools;
using WA4D0G.Model.Interfaces;

namespace WA4D0G.Model.Classes
{
    public class CertificateFasade : ICertificateFasade
    {
        ICertificateLoader _dbLoader, 
                              _systemStoreLoader;
        ICertificateTool _certificateTool;

        public CertificateFasade(ICertificateLoader DbExtractor,
                                 ICertificateLoader SystemStoreExtractor,
                                 ICertificateTool Operator)
        {
            _dbLoader = DbExtractor;
            _systemStoreLoader = SystemStoreExtractor;
            _certificateTool = Operator;
        }

        private async Task<List<Certificate>> ExtractCertificatesFromDatabase()
        {
            return await _dbLoader.ExtractCertificatesListAsync();
        }

        private async Task<List<Certificate>> ExtractCertificatesFromSystemStore()
        {
            return await _systemStoreLoader.ExtractCertificatesListAsync();
        }

        public async Task<List<Certificate>> ExtractCertificatesAsync(ISettings settings) 
        {
            List<Certificate> availableCertificates;
            await Logger.WriteAsync("Extracting certificates....");
            await Logger.WriteAsync("Use system key store : " + settings.PersonalKeyStore.ToString());
            if (settings.PersonalKeyStore)
            {
                availableCertificates = await ExtractCertificatesFromSystemStore();
            }
            else
            {
                availableCertificates = await ExtractCertificatesFromDatabase();
                if (availableCertificates.Count == 0)
                {
                    await Logger.WriteAsync("Database key store is empty. Using personal key store....");
                    availableCertificates = await ExtractCertificatesFromSystemStore();
                }
            }

            if (availableCertificates.Count == 0) await Logger.WriteAsync("Both key stores are empty.");
            else await Logger.WriteAsync("Certificates have been extracted successfully.");
            return availableCertificates;
        }

        public async Task UpdateCertificateAsync(ICertificate certificate)
        {
            await _certificateTool.UpdateCertificateInDatabaseAsync(certificate);
        }

        public async Task DeleteCertificateAsync(ICertificate certificate)
        {
            await _certificateTool.DeleteCertificateFromDatabaseAsync(certificate);
        }

        public async Task<bool> GenerateUnavailableCertificatesReportAsync()
        {
            await Logger.WriteAsync("Generating unavailable certificates report, using database key store....");
            return await ExcelGenerator.GenerateReportAsync(await _dbLoader.ExtractUnavailableCertificatesListAsync());
            
            /*
            await Logger.WriteAsync("Generating unavailable certificates report, using system key store....");
            return await ExcelGenerator.GenerateReportAsync(await _systemStoreExtractor.ExtractUnavailableCertificatesListAsync());
            */
        }

        public async Task<bool> SaveCertificateToDatabaseAsync(ICertificate certificate)
        {
            return await _certificateTool.SaveCertificateToDatabaseAsync(certificate);
        }

        public async Task<bool> SaveCertificateToDatabaseAsync(ObservableCollection<ICertificate> certificates)
        {
            return await _certificateTool.SaveCertificateToDatabaseAsync(certificates);
        }
    }
}
