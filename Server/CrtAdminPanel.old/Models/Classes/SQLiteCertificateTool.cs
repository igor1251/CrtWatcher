using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using WA4D0G.MaintenanceTools;
using CrtAdminPanel.Models.Interfaces;
using Dapper;

namespace CrtAdminPanel.Models.Classes
{
    public class SQLiteCertificateTool : ICertificateTool
    {
        #region private fields

        private ISQLiteDbContext _dbContext;
        private IQueryList _queryList;

        #endregion

        public SQLiteCertificateTool(ISQLiteDbContext DbContext, 
                                         IQueryList QueryList)
        {
            _dbContext = DbContext;
            _queryList = QueryList;
        }

        private Task<bool> IsDatabaseFileExistsAsync()
        {
            return Task.FromResult<bool>(File.Exists(_dbContext.DatabaseFile));
        }

        private async Task<bool> ExecuteSimpleDbQueryAsync(string query, string waitingMessage, string succeedMessage, string failedMessage)
        {
            await Logger.WriteAsync(waitingMessage);
            try
            {
                await _dbContext.Connection.ExecuteAsync(query);
                await Logger.WriteAsync(succeedMessage);
                return true;
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync(failedMessage + " " + ex.Message);
                return false;
            }
        }

        private async Task<bool> ExecuteCombinedDbQueryAsync(string query, 
                                                             ICertificate certificate, 
                                                             string waitingMessage, 
                                                             string succeedMessage, 
                                                             string failedMessage, 
                                                             bool createDbIfNotExist)
        {
            await Logger.WriteAsync(waitingMessage);


            if (!await IsDatabaseFileExistsAsync())
            {
                await Logger.WriteAsync("Error: Database file not found.");
                if (createDbIfNotExist)
                {
                    await Logger.WriteAsync("Creating....");
                    await CreateDatabase();
                    await Logger.WriteAsync("Database created.");
                }
                else return false;
            }

            if (_dbContext.Connection.State == System.Data.ConnectionState.Closed)
            {
                await Logger.WriteAsync("Connection closed. Opening....");
                await _dbContext.Connection.OpenAsync();
                await Logger.WriteAsync("Connection opened.");
            }

            
            try
            {
                await _dbContext.Connection.ExecuteAsync(query, certificate);
                await Logger.WriteAsync(succeedMessage);
                return true;
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync(failedMessage + " " + ex.Message);
                return false;
            }
            finally
            {
                await Logger.WriteAsync("Closing connection....");
                _dbContext.Connection.Close();
                await Logger.WriteAsync("Connection closed.");
            }
        }

        private async Task<bool> ExecuteCombinedDbQueryAsync(string query,
                                                             ObservableCollection<ICertificate> certificates,
                                                             string waitingMessage,
                                                             string succeedMessage,
                                                             string failedMessage,
                                                             bool createDbIfNotExist)
        {
            await Logger.WriteAsync(waitingMessage);
            if (!await IsDatabaseFileExistsAsync())
            {
                await Logger.WriteAsync("Error: Database file not found.");
                if (createDbIfNotExist)
                {
                    await Logger.WriteAsync("Creating....");
                    await CreateDatabase();
                    await Logger.WriteAsync("Database created.");
                }
                else return false;
            }

            if (_dbContext.Connection.State == System.Data.ConnectionState.Closed)
            {
                await Logger.WriteAsync("Connection closed. Opening....");
                await _dbContext.Connection.OpenAsync();
                await Logger.WriteAsync("Connection opened.");
            }

            try
            {
                foreach (ICertificate certificate in certificates)
                {
                    await _dbContext.Connection.ExecuteAsync(query, certificate);
                }
                await Logger.WriteAsync(succeedMessage);
                return true;
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync(failedMessage + " " + ex.Message);
                return false;
            }
            finally
            {
                await Logger.WriteAsync("Closing connection....");
                _dbContext.Connection.Close();
                await Logger.WriteAsync("Connection closed.");
            }
        }

        public async Task DeleteCertificateFromDatabaseAsync(ICertificate certificate)
        {
            await ExecuteCombinedDbQueryAsync(_queryList.DeleteCertificateQuery,
                                              certificate,
                                              "Deliting selected certificate from database....",
                                              "Successfully deleted.",
                                              "Error: Can't delete certificate.",
                                              false);
        }

        public async Task<bool> SaveCertificateToDatabaseAsync(ObservableCollection<ICertificate> certificates)
        {
            return await ExecuteCombinedDbQueryAsync(_queryList.InsertCertificateQuery,
                                                     certificates,
                                                     "Trying to saving changes....",
                                                     "Succesfully saved.",
                                                     "Error: Can't save certificates.",
                                                     true);
        }

        public async Task<bool> SaveCertificateToDatabaseAsync(ICertificate certificate)
        {
            return await ExecuteCombinedDbQueryAsync(_queryList.InsertCertificateQuery,
                                                     certificate,
                                                     "Trying to saving changes....",
                                                     "Succesfully saved.",
                                                     "Error: Can't save certificate.",
                                                     true);
        }

        public async Task UpdateCertificateInDatabaseAsync(ICertificate certificate)
        {
            await ExecuteCombinedDbQueryAsync(_queryList.UpdateCertificateQuery,
                                              certificate,
                                              "Trying to applying changes....",
                                              "Succesfully updated.",
                                              "Error: Can't update certificate.",
                                              false);
        }

        private async Task CreateDatabase()
        {
            await ExecuteSimpleDbQueryAsync(_queryList.CreateDatabaseQuery,
                                            "Creating new database file....",
                                            "Succesfully created.",
                                            "Error: Can't create database.");
        }

        public async Task<Certificate> GetCertificateByIDAsync(uint id)
        {
            try
            {
                return await _dbContext.Connection.QueryFirstOrDefaultAsync<Certificate>(_queryList.GetCertificateByIDQuery, new { id });
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync("Failed to search certificate" + " " + ex.Message);
                return new Certificate();
            }
        }
    }
}
