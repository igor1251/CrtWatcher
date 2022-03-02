using DataStructures;
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

namespace ObserverConfigurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string IPv4_ADDR_REGEX = @"(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}";
        private readonly string IPv6_ADDR_REGEX = @"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";
        private readonly string PORT_NUMBER_REGEX = @"^((6553[0-5])|(655[0-2][0-9])|(65[0-4][0-9]{2})|(6[0-4][0-9]{3})|([1-5][0-9]{4})|([0-5]{0,5})|([0-9]{1,4}))$";

        Settings newSettings;

        public MainWindow()
        {
            InitializeComponent();
            newSettings = new Settings();
        }

        private async void SaveInformation()
        {
            newSettings.MainServerIP = serverIPTextBox.Text;
            newSettings.MainServerPort = int.Parse(connectionPortTextBox.Text);
            var settingsStorage = new SettingsStorage();
            await settingsStorage.SaveSettingsToFile(newSettings);
            MessageBox.Show("Saved!");
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(connectionPortTextBox.Text) ||
                string.IsNullOrEmpty(serverIPTextBox.Text))
            {
                MessageBox.Show("Fill in all the fields!");
                return;
            }

            if (!Regex.IsMatch(serverIPTextBox.Text, IPv4_ADDR_REGEX) ||
                    !Regex.IsMatch(serverIPTextBox.Text, IPv6_ADDR_REGEX) ||
                    serverIPTextBox.Text != "localhost")
            {
                MessageBox.Show("Invalid IP!");
                return;
            }

            if (!Regex.IsMatch(connectionPortTextBox.Text, PORT_NUMBER_REGEX))
            {
                MessageBox.Show("Invalid port!");
                return;
            }

            SaveInformation();
        }
    }
}
