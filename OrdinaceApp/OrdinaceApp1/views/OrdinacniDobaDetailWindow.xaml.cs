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
            try
            {
                // 1. Validace vstupů
                if (CmbLekar.SelectedValue == null || CmbDen.SelectedValue == null) // Předpokládám CmbDen pro dny
                {
                    MessageBox.Show("Vyberte lékaře a den.");
                    return;
                }

                // 2. Převod textu (např. "08:00") na DateTime
                // DateTime.Parse automaticky přidá k času dnešní datum. 
                // To je pro Oracle v pořádku (datum tam být musí, my ho budeme ignorovat).
                if (!DateTime.TryParse(TxtOd.Text, out DateTime casOd) ||
                    !DateTime.TryParse(TxtDo.Text, out DateTime casDo))
                {
                    MessageBox.Show("Zadejte čas ve správném formátu (např. 08:00).");
                    return;
                }

                // 3. Vytvoření objektu
                var ordinacniDoba = new OrdinacniDoba
                {
                    IdLekar = (int)CmbLekar.SelectedValue,
                    Den = CmbDen.Text, // Nebo SelectedValue, podle toho jak máte dny
                    Zacatek = casOd,
                    Konec = casDo
                };

                // 4. Uložení
                var repo = new OrdinacniDobaRepository();
                repo.PridatDobu(ordinacniDoba);

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