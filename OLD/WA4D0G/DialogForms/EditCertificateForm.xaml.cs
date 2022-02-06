using System.Windows;
using WA4D0G.Model.Interfaces;

namespace WA4D0G.DialogForms
{
    /// <summary>
    /// Логика взаимодействия для EditCertificateForm.xaml
    /// </summary>
    public partial class EditCertificateForm : Window
    {
        ICertificate updatedCertificate;

        public EditCertificateForm(ICertificate certificate)
        {
            InitializeComponent();

            subjectTextBox.Text = certificate.HolderFIO;
            phoneTextBox.Text = certificate.HolderPhone;
            startDateTextBox.Text = certificate.CertStartDateTime.ToString();
            endDateTextBox.Text = certificate.CertEndDateTime.ToString();

            updatedCertificate = certificate;
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (phoneTextBox.Text != null && phoneTextBox.Text != string.Empty)
            {
                updatedCertificate.HolderPhone = phoneTextBox.Text;
                DialogResult = true;
            }
            else DialogResult = null;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public ICertificate GetUpdatedCertificate()
        {
            return updatedCertificate;
        }
    }
}
