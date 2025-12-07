using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models; 

namespace OrdinaceApp1.Views
{
    public partial class NovyPacientWindow : Window
    {
        public NovyPacientWindow()
        {
            InitializeComponent();

            DpDatumNarozeni.SelectedDate = DateTime.Now.AddYears(-20);

            NacistUzivatele();
        }

        private void NacistUzivatele()
        {
            try
            {
                var repo = new UzivatelRepository();
                CmbUzivatele.ItemsSource = repo.GetVsechnyUzivatele();

                if (CmbUzivatele.Items.Count > 0)
                {
                    CmbUzivatele.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání seznamu uživatelů: " + ex.Message);
            }
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtJmeno.Text) ||
                string.IsNullOrWhiteSpace(TxtPrijmeni.Text) ||
                string.IsNullOrWhiteSpace(TxtMesto.Text) ||
                TxtPsc.Text.Length != 5)
            {
                MessageBox.Show("Vyplňte prosím všechna pole správně (PSČ 5 čísel).", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CmbUzivatele.SelectedValue == null)
            {
                MessageBox.Show("Musíte vybrat uživatele ze seznamu.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int vybraneId = (int)CmbUzivatele.SelectedValue;

            try
            {
                var repo = new PacientRepository();
                repo.PridatPacienta(
                    TxtJmeno.Text,
                    TxtPrijmeni.Text,
                    DpDatumNarozeni.SelectedDate ?? DateTime.Now,
                    TxtUlice.Text,
                    TxtMesto.Text,
                    TxtPsc.Text,
                    vybraneId 
                );

                MessageBox.Show("Pacient byl úspěšně uložen!", "Hotovo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba databáze:\n{ex.Message}\n\n(Pozor: Jeden uživatel může mít jen jednu kartu pacienta!)", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}