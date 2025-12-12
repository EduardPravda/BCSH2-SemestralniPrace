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

            NacistSeznamPacientu();
        }

        private void NacistSeznamPacientu()
        {
            // 1. Ošetření pro neregistrovaného HOSTA (Role 4 nebo 0)
            if (PrihlasenyUzivatel.RoleId == 4 || PrihlasenyUzivatel.IdUzivatel == 0)
            {
                TxtStatus.Text = "Režim Host - Prohlížení ordinačních hodin.";
                MainDataGrid.ItemsSource = null; // Vyčistit tabulku

                var oknoOrdinace = new OrdinaceApp1.Views.OrdinacniDobaWindow(PrihlasenyUzivatel.RoleId);
                oknoOrdinace.Show();

                return;
            }

            // 2. Ošetření pro PACIENTA (Role 3)
            if (PrihlasenyUzivatel.RoleId == 3)
            {
                TxtStatus.Text = "Vítejte v pacientské sekci.";
                MainDataGrid.ItemsSource = null;
                return;
            }

            // 3. Pro LÉKAŘE (2) a ADMINA (1) načteme data
            try
            {
                var repo = new PacientRepository();
                System.Collections.Generic.List<Pacient> data;

                if (PrihlasenyUzivatel.RoleId == 2) // Lékař
                {
                    var lekarRepo = new LekarRepository();
                    int? idLekar = lekarRepo.GetLekarIdByUzivatel(PrihlasenyUzivatel.IdUzivatel);

                    if (idLekar.HasValue)
                    {
                        data = repo.GetPacientiLekare(idLekar.Value);
                        TxtStatus.Text = $"Zobrazeno {data.Count} vašich pacientů.";
                    }
                    else
                    {
                        data = new System.Collections.Generic.List<Pacient>();
                        TxtStatus.Text = "Chyba: Uživatel není propojen s kartou lékaře.";
                    }
                }
                else // Admin (1)
                {
                    data = repo.GetPacientiPublic();
                    TxtStatus.Text = $"Načteno {data.Count} pacientů (Admin pohled).";
                }

                MainDataGrid.ItemsSource = data;
            }
            catch (Exception ex)
            {
                TxtStatus.Text = "Chyba načítání dat: " + ex.Message;
            }
        }

        private void NastavitPrava()
        {
            // 1. Logika pro NE-Administrátora (Lékař i Pacient)
            if (PrihlasenyUzivatel.RoleId != 1)
            {
                if (MenuAdmin != null) MenuAdmin.Visibility = Visibility.Collapsed;
            }

            // 2. Logika čistě pro Pacienta (Role ID 3)
            if (PrihlasenyUzivatel.RoleId == 3)
            {
                // Pacient nesmí vidět seznam ostatních pacientů ani je upravovat
                if (MenuPacientNovy != null) MenuPacientNovy.Visibility = Visibility.Collapsed;
                if (MenuPacientUpravit != null) MenuPacientUpravit.Visibility = Visibility.Collapsed;

                // --- NOVÉ: Skryjeme seznam pacientů (aby neviděl cizí jména) ---
                if (MenuPacientiSeznam != null) MenuPacientiSeznam.Visibility = Visibility.Collapsed;

                // Pacient si sám nepíše neschopenku, recepty, ani nevyšetřuje
                if (MenuNeschopnost != null) MenuNeschopnost.Visibility = Visibility.Collapsed;
                if (MenuPredpisy != null) MenuPredpisy.Visibility = Visibility.Collapsed;
                if (MenuVysetreni != null) MenuVysetreni.Visibility = Visibility.Collapsed;
                if (MenuDoporuceni != null) MenuDoporuceni.Visibility = Visibility.Collapsed;
                if (MenuAnamneza != null) MenuAnamneza.Visibility = Visibility.Collapsed;
                if (MenuAlergie != null) MenuAlergie.Visibility = Visibility.Collapsed;
            }

            if (PrihlasenyUzivatel.RoleId == 4 || PrihlasenyUzivatel.IdUzivatel == 0) // Host
            {
                // Skrýt menu Pacienti
                if (MenuPacienti != null) MenuPacienti.Visibility = Visibility.Collapsed;

                // Skrýt menu Admin
                if (MenuAdmin != null) MenuAdmin.Visibility = Visibility.Collapsed;

                // Skrýt menu Info
                if (MenuInfo != null) MenuInfo.Visibility = Visibility.Collapsed;
            }
        }

        private void MenuOdhlasit_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new OrdinaceApp1.Views.LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void MenuKontakt_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("MUDr. Jan Novák\n" +
                            "Tel: +420 123 456 789\n" +
                            "Email: ordinace@nemocnice.cz\n\n" +
                            "Adresa:\n" +
                            "Nemocniční 1\n" +
                            "500 05 Hradec Králové",
                            "Kontakt na ordinaci", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuUkoncit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuPacientiSeznam_Click(object sender, RoutedEventArgs e)
        {
            NacistSeznamPacientu();
        }

        private void MenuPacientNovy_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.NovyPacientWindow();
            okno.ShowDialog();
            MenuPacientiSeznam_Click(null, null);
        }

        private void MenuSoubory_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.SouboryWindow(PrihlasenyUzivatel);
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
            var okno = new OrdinaceApp1.Views.OrdinacniDobaWindow(PrihlasenyUzivatel.RoleId);
            okno.ShowDialog();
        }
        private void MenuRezervace_Click(object sender, RoutedEventArgs e)
        {
            var okno = new OrdinaceApp1.Views.RezervaceWindow(PrihlasenyUzivatel);
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