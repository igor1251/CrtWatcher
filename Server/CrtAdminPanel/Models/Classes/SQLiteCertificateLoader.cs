using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CrtAdminPanel.Models.Interfaces;
using WA4D0G.MaintenanceTools;
using Dapper;

namespace CrtAdminPanel.Models.Classes
{
    public class SQLiteCertificateLoader : ICertificateLoader
    {
        #region private fields

        private ISQLiteDbContext _dbContext;
        private IQueryList _queryList;
        private ISettingsLoader _settingsExtractor;

        #endregion


        public SQLiteCertificateLoader(ISQLiteDbContext DbContext,
                                          IQueryList QueryList, 
                                          ISettingsLoader settingsExtractor)
        {
            _dbContext = DbContext;
            _queryList = QueryList;
            _settingsExtractor = settingsExtractor;
        }

        private Task<bool> IsDatabaseFileExistsAsync()
        {
            /*
            var taskCompletionSource = new TaskCompletionSource<bool>();
            taskCompletionSource.SetResult(File.Exists(_dbContext.DatabaseFile));
            return taskCompletionSource.Task;
            */
            return Task.FromResult<bool>(File.Exists(_dbContext.DatabaseFile));
        }

        public async Task<List<Certificate>> ExtractCertificatesListAsync()
        {
            if (!await IsDatabaseFileExistsAsync())
            {
                await Logger.WriteAsync("Error: Can't load certificates. Database file not found.");
                return new List<Certificate>();
            }

            try
            {
                return (await _dbContext.Connection.QueryAsync<Certificate>(_queryList.GetCertificatesQuery)).ToList();
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync("Error: Can't load certificates from database. " + ex.Message);
                return new List<Certificate>();
            }
        }

        public async Task<List<Certificate>> ExtractUnavailableCertificatesListAsync()
        {
            await Logger.WriteAsync("Trying to select unavailable certificates from database....");

            if (!await IsDatabaseFileExistsAsync())
            {
                await Logger.WriteAsync("Error: Database file not found.");
                return new List<Certificate>();
            }

            try
            {
                ISettings settings = await _settingsExtractor.LoadSettingsAsync(); ;
                return (await _dbContext.Connection.QueryAsync<Certificate>(_queryList.GetUnavailableCertificatesQuery, settings)).ToList();
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync("Error: Can't get unavailable certificates. " + ex.Message);
                return new List<Certificate>();
            }
        }
    }
}
