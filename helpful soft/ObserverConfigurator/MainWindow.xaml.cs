using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(connectionPortTextBox.Text) ||
                string.IsNullOrEmpty(serverIPTextBox.Text))
            {
                MessageBox.Show("Fill in all the fields!");
            }
            else
            {
                var settingsStorage = new SettingsStorage();
                await settingsStorage.SaveSettingsToFile(new Settings
                {
                    MainServerIP = serverIPTextBox.Text,
                    MainServerPort = int.Parse(connectionPortTextBox.Text),
                    VerificationFrequency = 10
                });
                MessageBox.Show("Saved!");
            }
        }
    }
}
