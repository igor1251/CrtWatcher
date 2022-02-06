using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using Unity;
using Unity.Resolution;

using WA4D0G.DialogForms;
using WA4D0G.Model.Interfaces;
using WA4D0G.Model.Classes;
using WA4D0G.ViewModel.Commands;

namespace WA4D0G.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region private fields

        private ICertificate _selectedCertificate;
        private ICertificateFasade _certificateFasade;

        private ICommand _loadCertificatesCommand,
                         _deleteCertificateCommand,
                         _updateCertificateCommand,
                         _saveCertificatesToDatabaseCommand,
                         _editSettingsCommand,
                         _generateUnavailableCertificatesReportCommand;

        private IUnityContainer _unityContainer;
        private ISettingsLoader _settingsExtractor;

        private const string DB_EXTRACTOR_STRING_ID = "SQLiteDbExtractor";
        private const string STORE_EXTRACTOR_STRING_ID = "SystemStoreExtractor";

        #endregion

        #region public fields

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region public properties

        public ObservableCollection<ICertificate> AvailableCertificates { get; set; }
        
        public ICertificate SelectedCertificate
        {
            get { return _selectedCertificate; }
            set
            {
                _selectedCertificate = value;
                OnPropertyChanged("SelectedCertificate");
            }
        }

        public ICommand LoadCertificatesCommand
        {
            get
            {
                if (_loadCertificatesCommand == null)
                {
                    _loadCertificatesCommand = new LoadCertificatesCommand(this);
                }
                return _loadCertificatesCommand;
            }
        }

        public ICommand DeleteCertificateCommand
        {
            get
            {
                if (_deleteCertificateCommand == null)
                {
                    _deleteCertificateCommand = new DeleteCertificateCommand(this);
                }
                return _deleteCertificateCommand;
            }
        }

        public ICommand UpdateCertificateCommand
        {
            get
            {
                if (_updateCertificateCommand == null)
                {
                    _updateCertificateCommand = new UpdateCertificateCommand(this);
                }
                return _updateCertificateCommand;
            }
        }

        public ICommand SaveCertificatesToDatabaseCommand
        {
            get
            {
                if (_saveCertificatesToDatabaseCommand == null)
                {
                    _saveCertificatesToDatabaseCommand = new SaveCertificatesToDatabaseCommand(this);
                }
                return _saveCertificatesToDatabaseCommand;
            }
        }

        public ICommand EditSettingsCommand
        {
            get
            {
                if (_editSettingsCommand == null)
                {
                    _editSettingsCommand = new EditSettingsCommand(this);
                }
                return _editSettingsCommand;
            }
        }

        public ICommand GenerateUnavailableCertificatesReportCommand
        {
            get
            {
                if (_generateUnavailableCertificatesReportCommand == null)
                {
                    _generateUnavailableCertificatesReportCommand = new GenerateUnavailableCertificatesReportCommand(this);
                }
                return _generateUnavailableCertificatesReportCommand;
            }
        }

        #endregion


        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PrepareServices(IUnityContainer container)
        {
            container.RegisterType<ICertificateTool, SQLiteCertificateTool>(TypeLifetime.Singleton);
            container.RegisterType<ICertificateLoader, SQLiteCertificateLoader>(DB_EXTRACTOR_STRING_ID, TypeLifetime.Singleton);
            container.RegisterType<ICertificateLoader, SystemStoreCertificateLoader>(STORE_EXTRACTOR_STRING_ID, TypeLifetime.Singleton);
            container.RegisterType<ISQLiteDbContext, SQLiteDbContext>(TypeLifetime.Singleton);
            container.RegisterType<ISettingsLoader, SettingsLoader>(TypeLifetime.Singleton);
            container.RegisterType<ICertificateFasade, CertificateFasade>(TypeLifetime.Singleton);
            container.RegisterType<IQueryList, SQLiteQueryList>(TypeLifetime.Singleton);
        }

        private void ResolveServices(IUnityContainer container)
        {
            ICertificateLoader _dbExtractor = container.Resolve<ICertificateLoader>(DB_EXTRACTOR_STRING_ID),
                                  _systemStoreExtractor = container.Resolve<ICertificateLoader>(STORE_EXTRACTOR_STRING_ID);

            _certificateFasade = container.Resolve<ICertificateFasade>(new ParameterOverride("DbExtractor", _dbExtractor),
                                                                       new ParameterOverride("SystemStoreExtractor", _systemStoreExtractor));
            
            _settingsExtractor = container.Resolve<ISettingsLoader>();
        }

        public MainViewModel()
        {
            AvailableCertificates = new ObservableCollection<ICertificate>();
            _unityContainer = new UnityContainer();
            PrepareServices(_unityContainer);
            ResolveServices(_unityContainer);
        }

        public async void LoadCertificates()
        {
            foreach (ICertificate item in await _certificateFasade.ExtractCertificatesAsync(await _settingsExtractor.LoadSettingsAsync()))
            {
                AvailableCertificates.Add(item);
            }
        }

        public async void DeleteCertificate()
        {
            if (_selectedCertificate != null)
            {
                await _certificateFasade.DeleteCertificateAsync(SelectedCertificate);
                AvailableCertificates.Remove(SelectedCertificate);
            }
        }

        public async void SaveExtractedCertificatesToDatabase()
        {
            if (await _certificateFasade.SaveCertificateToDatabaseAsync(AvailableCertificates))
            {
                MessageBox.Show("Done.", "Save certificates", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed. See log file for details.", "Save certificates", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void EditSelectedCertificate()
        {
            if (_selectedCertificate != null)
            {
                EditCertificateForm form = new EditCertificateForm(_selectedCertificate);
                if (form.ShowDialog().Value)
                {
                    ICertificate updatedCertificate = form.GetUpdatedCertificate();
                    await _certificateFasade.UpdateCertificateAsync(updatedCertificate);
                    int selectedCertificateIndex = AvailableCertificates.IndexOf(_selectedCertificate);
                    AvailableCertificates.Remove(_selectedCertificate);
                    AvailableCertificates.Insert(selectedCertificateIndex, updatedCertificate);
                }
            }
        }

        public async void EditSettings()
        {
            SettingsForm settingsForm = new SettingsForm();
            if (settingsForm.ShowDialog().Value)
            {
                await _settingsExtractor.SaveSettingsAsync(settingsForm.GetCreatedSettings());
                MessageBox.Show("Saved!", "Message: settings saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public async void GenerateUnavailableCertificatesReport()
        {
            if (await _certificateFasade.GenerateUnavailableCertificatesReportAsync())
            {
                MessageBox.Show("Done.", "Generate report", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed. See log file for details.", "Generate report", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
