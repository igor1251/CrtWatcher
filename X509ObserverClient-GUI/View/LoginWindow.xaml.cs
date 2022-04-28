using NetworkOperators.Identity.DataTransferObjects;
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
using System.Windows.Shapes;

namespace X509ObserverClient_GUI.View
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public UserAuthorizationRequest GetUserAuthorizationRequest()
        {
            return new UserAuthorizationRequest() 
            { 
                UserName = loginTextBox.Text, 
                Password = passwordBox.Password 
            };
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(loginTextBox.Text) &&
                !string.IsNullOrEmpty(passwordBox.Password))
            {
                this.DialogResult = true;
            }
            else this.DialogResult = null;
        }
    }
}
