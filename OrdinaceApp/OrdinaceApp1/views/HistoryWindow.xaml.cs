using System;
using System.Windows;
using OrdinaceApp1.DataAccess;

namespace OrdinaceApp1.Views
{
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();
            NacistLogy();
        }

        private void NacistLogy()
        {
            try
            {
                var repo = new LogRepository();
                var data = repo.GetLogs();
                LogDataGrid.ItemsSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání logů: " + ex.Message);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}