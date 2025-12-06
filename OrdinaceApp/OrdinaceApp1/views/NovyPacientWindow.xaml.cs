using System;
using System.Windows;
using OrdinaceApp1.DataAccess;

namespace OrdinaceApp1.Views
{
    public partial class NovyPacientWindow : Window
    {
        public NovyPacientWindow()
        {
            InitializeComponent();
            // Nastavíme výchozí datum, aby pole nebylo prázdné
            DpDatumNarozeni.SelectedDate = DateTime.Now.AddYears(-20);
        }

        private void BtnUlozit_Click(object sender, RoutedEventArgs e)
        {
            // --- 1. VALIDACE VSTUPŮ (Splnění bodu 8 - Formuláře) ---
            if (string.IsNullOrWhiteSpace(TxtJmeno.Text) ||
                string.IsNullOrWhiteSpace(TxtPrijmeni.Text) ||
                string.IsNullOrWhiteSpace(TxtMesto.Text))
            {
                MessageBox.Show("Jméno, příjmení a město jsou povinné údaje.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TxtPsc.Text.Length != 5)
            {
                MessageBox.Show("PSČ musí mít přesně 5 znaků.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // --- 2. PŘÍPRAVA DAT ---
                string jmeno = TxtJmeno.Text;
                string prijmeni = TxtPrijmeni.Text;
                DateTime datum = DpDatumNarozeni.SelectedDate ?? DateTime.Now;
                string ulice = TxtUlice.Text;
                string mesto = TxtMesto.Text;
                string psc = TxtPsc.Text;

                // Kontrola, zda je ID uživatele číslo
                if (!int.TryParse(TxtUserId.Text, out int userId))
                {
                    MessageBox.Show("ID Uživatele musí být číslo.");
                    return;
                }

                // --- 3. ULOŽENÍ DO DB PŘES REPOZITÁŘ (Volá SP_Novy_Pacient) ---
                var repo = new PacientRepository();
                repo.PridatPacienta(jmeno, prijmeni, datum, ulice, mesto, psc, userId);

                // Úspěch
                MessageBox.Show("Pacient byl úspěšně uložen!", "Hotovo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Zavře okno
            }
            catch (Exception ex)
            {
                // Pokud databáze vrátí chybu (např. neexistující User ID)
                MessageBox.Show($"Chyba při ukládání: {ex.Message}", "Chyba DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}