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
        public MainWindow()
        {
            InitializeComponent();
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
