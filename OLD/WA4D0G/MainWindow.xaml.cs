using System.Windows;

namespace WA4D0G
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void authorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Insegrim", 
                            "Information: Author", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information);
        }

        private void quitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
