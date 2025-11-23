using System;
using System.Windows;
using OrdinaceApp1.DataAccess;

namespace OrdinaceApp1
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnNacist_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Vytvoříme instanci repository
                var repo = new PacientRepository();

                // 2. Získáme data
                var data = repo.GetPacientiPublic();

                // 3. Naplníme tabulku
                DgPacienti.ItemsSource = data;

                // 4. Info pro uživatele
                TxtStatus.Text = $"Načteno {data.Count} pacientů z Oracle DB.";
            }
            catch (Exception ex)
            {
                // Když se něco pokazí (špatné heslo, VPN nefunguje...)
                MessageBox.Show("Chyba připojení: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                TxtStatus.Text = "Chyba!";
            }
        }
    }
}
