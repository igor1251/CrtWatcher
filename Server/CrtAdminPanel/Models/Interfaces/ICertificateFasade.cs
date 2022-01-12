﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CrtAdminPanel.Models.Classes;

namespace CrtAdminPanel.Models.Interfaces
{
    public interface ICertificateFasade
    {
        Task<List<Certificate>> ExtractCertificatesAsync(ISettings settings);
        Task UpdateCertificateAsync(ICertificate certificate);
        Task DeleteCertificateAsync(ICertificate certificate);
        Task<bool> SaveCertificateToDatabaseAsync(ICertificate certificate);
        Task<bool> SaveCertificateToDatabaseAsync(ObservableCollection<ICertificate> certificates);
        Task<bool> GenerateUnavailableCertificatesReportAsync();
    }
}
