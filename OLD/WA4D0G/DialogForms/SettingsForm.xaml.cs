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
using WA4D0G.Model.Interfaces;
using WA4D0G.Model.Classes;

namespace WA4D0G.DialogForms
{
    /// <summary>
    /// Interaction logic for SettingsForm.xaml
    /// </summary>
    public partial class SettingsForm : Window
    {
        ISettings settings = new Settings();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private bool IsNumber(string numStr)
        {
            string digits = "0123456789,.";
            foreach (char c in numStr)
            {
                if (digits.IndexOf(c) == -1)
                {
                    return false;
                }
            }
            return true;
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (daysCountTextBox.Text != null && 
                daysCountTextBox.Text != string.Empty && 
                IsNumber(daysCountTextBox.Text))
            {
                settings.WarnDaysCount = uint.Parse(daysCountTextBox.Text);
                DialogResult = true;
            }
            else DialogResult = null;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void localDbRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            settings.PersonalKeyStore = false;
        }

        private void personalKeyStoreRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            settings.PersonalKeyStore = true;
        }

        public ISettings GetCreatedSettings()
        {
            return settings;
        }
    }
}
