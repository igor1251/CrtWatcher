using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataStructures;

namespace Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Needed regular expressions

        private readonly string IPv4_ADDR_REGEX = @"(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}",
                                IPv6_ADDR_REGEX = @"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))",
                                PORT_NUMBER_REGEX = @"^((6553[0-5])|(655[0-2][0-9])|(65[0-4][0-9]{2})|(6[0-4][0-9]{3})|([1-5][0-9]{4})|([0-5]{0,5})|([0-9]{1,4}))$",
                                VERIFICATION_FREQUENCY_REGEX = @"^[0-9]+$";

        #endregion

        #region Validation methods

        private bool ValidateMainServerIPField()
        {
            return Regex.IsMatch(mainServerIPTextBox.Text, IPv4_ADDR_REGEX) || Regex.IsMatch(mainServerIPTextBox.Text, IPv6_ADDR_REGEX);
        }

        private bool ValidateMainServerPortField()
        {
            return Regex.IsMatch(mainServerPortTextBox.Text, PORT_NUMBER_REGEX);
        }

        private bool ValidateVerificationFrequencyField()
        {
            return Regex.IsMatch(verificationFrequencyTextBox.Text, VERIFICATION_FREQUENCY_REGEX);
        }

        private bool ValidateFields()
        {
            return ValidateMainServerIPField() && ValidateMainServerPortField() && ValidateVerificationFrequencyField();
        }

        #endregion

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                MessageBox.Show("Заполните поля правильно.");
                return;
            }

            var settings = new Settings();
            settings.MainServerIP = mainServerIPTextBox.Text;
            settings.MainServerPort = int.Parse(mainServerPortTextBox.Text);
            settings.VerificationFrequency = int.Parse(verificationFrequencyTextBox.Text);
            await new SettingsStorage(new DbContext(), new BaseStorageQueries()).UpdateSettings(settings);
            MessageBox.Show("Сохранено!");
        }
    }
}
