using NetworkOperators.Identity.Client;
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
using X509ObserverClient_GUI.View;

namespace X509ObserverClient_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConnectionParameters _connectionParameters = new ConnectionParameters();
        private PassportControl _passportControl;

        //private BitmapImage[] icons = new BitmapImage[2];

        public MainWindow()
        {
            InitializeComponent();
            //icons[0] = new BitmapImage(new Uri("/icons/offline.png", UriKind.Relative));
            //icons[1] = new BitmapImage(new Uri("icons/online.png"));
            //connectionStatusIcon.Source = icons[0];
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _connectionParameters = await ConnectionParametersLoader.ReadServiceParameters();
            if (!string.IsNullOrEmpty(_connectionParameters.ApiKey))
            {

            }
        }

        private void loginMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog().Value)
            {
                var userAuthorizationRequest = loginWindow.GetUserAuthorizationRequest();
                MessageBox.Show("login = " + userAuthorizationRequest.UserName + "\npassword = " + userAuthorizationRequest.Password);
            }
        }

        private void syncMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
