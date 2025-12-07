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
using OrdinaceApp1.DataAccess;

namespace OrdinaceApp1.Views
{
    public partial class LekariWindow : Window
    {
        public LekariWindow()
        {
            InitializeComponent();
            NacistData();
        }

        private void NacistData()
        {
            try
            {
                var repo = new LekarRepository();
                LstLekari.ItemsSource = repo.GetHierarchielekaru();
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