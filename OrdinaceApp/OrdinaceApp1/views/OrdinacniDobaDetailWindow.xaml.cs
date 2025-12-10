using System;
using System.Windows;
using System.Windows.Controls;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class OrdinacniDobaDetailWindow : Window
    {
        public OrdinacniDobaDetailWindow()
        {
            InitializeComponent();
            NacistLekare();
        }

        private void NacistLekare()
        {
            var repo = new LekarRepository();
            CmbLekar.ItemsSource = repo.GetVsechnyLekare();
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (CmbLekar.SelectedValue == null || CmbDen.SelectedItem == null)
            {
                MessageBox.Show("Vyberte lékaře a den.");
                return;
            }

            try
            {
                var denItem = (ComboBoxItem)CmbDen.SelectedItem;

                var o = new OrdinacniDoba
                {
                    IdLekar = (int)CmbLekar.SelectedValue,
                    Den = denItem.Content.ToString(),
                    Zacatek = TxtOd.Text,
                    Konec = TxtDo.Text
                };

                var repo = new OrdinacniDobaRepository();
                repo.PridatDobu(o);

                MessageBox.Show("Uloženo.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba: " + ex.Message);
            }
        }
    }
}