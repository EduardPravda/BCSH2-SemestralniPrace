using System;
using System.Windows;
using OrdinaceApp1.DataAccess; 

namespace OrdinaceApp1.Views
{
    public partial class AuditWindow : Window
    {
        public AuditWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new LogRepository();
                var data = repo.GetLogs();

                DgAudit.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nepodařilo se načíst historii:\n" + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}