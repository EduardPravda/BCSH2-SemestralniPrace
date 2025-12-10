using System;
using System.Windows;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;
using OrdinaceApp1.Views;

namespace OrdinaceApp1
{
    public partial class MainWindow : Window
    {
        public Uzivatel PrihlasenyUzivatel { get; private set; }

        public MainWindow(Uzivatel uzivatel)
        {
            InitializeComponent();

            PrihlasenyUzivatel = uzivatel;

            this.Title = $"IS Nemocnice - Přihlášen: {uzivatel.CeleJmeno} (Role ID: {uzivatel.RoleId})";

            NastavitPrava();
        }

        private void NastavitPrava()
        {
            if (PrihlasenyUzivatel.RoleId != 1)
            {
                if (MenuAdmin != null) MenuAdmin.Visibility = Visibility.Collapsed;
            }
        }

        private void MenuOdhlasit_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new OrdinaceApp1.Views.LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void MenuUkoncit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuPacientiSeznam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repo = new PacientRepository();
                var data = repo.GetPacientiPublic();
                MainDataGrid.ItemsSource = data;
                TxtStatus.Text = $"Načteno {data.Count} pacientů.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání: " + ex.Message);
            }
        }

        private void MenuPacientNovy_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.NovyPacientWindow();
            okno.ShowDialog();
            MenuPacientiSeznam_Click(null, null);
        }

        private void MenuSoubory_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.SouboryWindow(PrihlasenyUzivatel.IdUzivatel);
            okno.ShowDialog();
        }

       private void MenuReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repo = new PacientRepository();
                string report = repo.GenerovatReportAlergii();

                if (string.IsNullOrEmpty(report))
                {
                    MessageBox.Show("Report je prázdný.");
                }
                else
                {
                    MessageBox.Show(report, "Report Alergiků (Explicitní kurzor)");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při generování reportu: " + ex.Message);
            }
        }

            private void MenuLog_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.AuditWindow();
            okno.ShowDialog();
        }

        private void MenuSystem_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.SystemWindow();
            okno.ShowDialog();
        }

        private void MenuLeky_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.LekyWindow();
            okno.ShowDialog();
        }
        
        private void MenuSpravaLekaru_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.SpravaLekaruWindow();
            okno.ShowDialog();
        }

        private void MenuVysetreni_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.VysetreniWindow();
            okno.ShowDialog();
        }
        private void MenuSpravaUzivatelu_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.SpravaUzivateluWindow();
            okno.ShowDialog();
        }
        private void MenuPredpisy_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.PredpisyWindow();
            okno.ShowDialog();
        }
        private void MenuNeschopnost_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.NeschopnostWindow();
            okno.ShowDialog();
        }
        private void MenuNemoci_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.NemocWindow();
            okno.ShowDialog();
        }
        private void MenuOrdinace_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.OrdinacniDobaWindow();
            okno.ShowDialog();
        }
        private void MenuRezervace_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.RezervaceWindow();
            okno.ShowDialog();
        }
        private void MenuDoporuceni_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.DoporuceniWindow();
            okno.ShowDialog();
        }
        private void MenuAlergie_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.AlergieWindow();
            okno.ShowDialog();
        }
        private void MenuRole_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.RoleWindow();
            okno.ShowDialog();
        }
        private void MenuAnamneza_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.AnamnezaWindow();
            okno.ShowDialog();
        }
        private void MenuPacientUpravit_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem is Pacient vybranyPacient)
            {
                var okno = new OrdinaceApp1.Views.NovyPacientWindow(vybranyPacient.IdPacient);
                okno.ShowDialog();

                MenuPacientiSeznam_Click(null, null);
            }
            else
            {
                MessageBox.Show("Nejprve označte pacienta v seznamu (klikněte na řádek).");
            }
        }
        /*
        private void MenuLekari_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.LekariWindow();
            okno.ShowDialog();
        }
        */
    }
}