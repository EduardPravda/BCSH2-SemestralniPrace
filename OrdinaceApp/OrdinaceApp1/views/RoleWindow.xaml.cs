using System;
using System.Windows;
using OrdinaceApp1.DataAccess;

namespace OrdinaceApp1.Views
{
    public partial class RoleWindow : Window
    {
        public RoleWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new RoleRepository();
                DgRole.ItemsSource = repo.GetVsechnyRole();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}