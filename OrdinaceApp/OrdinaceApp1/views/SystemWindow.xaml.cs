using System;
using System.Windows;
using System.Windows.Data;
using OrdinaceApp1.DataAccess;

namespace OrdinaceApp1.Views
{
    public partial class SystemWindow : Window
    {
        public SystemWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new SystemRepository();
                var data = repo.GetDatabaseObjects();

                ListCollectionView view = new ListCollectionView(data);
                view.GroupDescriptions.Add(new PropertyGroupDescription("Typ"));

                DgObjects.ItemsSource = view;
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