using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using OrdinaceApp1.DataAccess;
using OrdinaceApp1.Models;

namespace OrdinaceApp1.Views
{
    public partial class SouboryWindow : Window
    {
        private Uzivatel _prihlasenyUzivatel;

        public SouboryWindow(Uzivatel u)
        {
            InitializeComponent();
            _prihlasenyUzivatel = u;
            NacistData();
        }

        private void NacistData()
        {
            var repo = new SouborRepository();

            // Pokud je Admin (1), vidí vše. Ostatní vidí jen své soubory.
            if (_prihlasenyUzivatel.RoleId == 1)
            {
                DgSoubory.ItemsSource = repo.GetSeznamSouboru();
            }
            else
            {
                DgSoubory.ItemsSource = repo.GetSouboryUzivatele(_prihlasenyUzivatel.IdUzivatel);
            }
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                byte[] data = File.ReadAllBytes(dlg.FileName);
                string nazev = System.IO.Path.GetFileName(dlg.FileName);
                string pripona = System.IO.Path.GetExtension(dlg.FileName);

                var repo = new SouborRepository();
                repo.NahratSoubor(nazev, "application/file", pripona, data, "Popis", _prihlasenyUzivatel.IdUzivatel);

                NacistData(); 
            }
        }
        private void BtnStahnout_Click(object sender, RoutedEventArgs e)
        {
            if (DgSoubory.SelectedItem is Soubor vybranyZeSeznamu)
            {
                try
                {
                    var repo = new SouborRepository();
                    var souborSDaty = repo.GetSouborData(vybranyZeSeznamu.IdSoubor);

                    if (souborSDaty == null || souborSDaty.Data == null)
                    {
                        MessageBox.Show("Soubor neobsahuje žádná data nebo se nepodařilo načíst.");
                        return;
                    }

                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.FileName = souborSDaty.Nazev; 
                    saveDialog.Filter = "Všechny soubory (*.*)|*.*";

                    if (saveDialog.ShowDialog() == true)
                    {
                        File.WriteAllBytes(saveDialog.FileName, souborSDaty.Data);
                        MessageBox.Show("Soubor byl úspěšně stažen.", "Hotovo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při stahování: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Nejprve vyberte soubor ze seznamu.");
            }
        }
        private void BtnUpravit_Click(object sender, RoutedEventArgs e)
        {
            if (DgSoubory.SelectedItem is Soubor vybrany)
            {
                var okno = new SouborEditWindow(vybrany);
                okno.ShowDialog();
                NacistData(); 
            }
            else
            {
                MessageBox.Show("Vyberte soubor k úpravě.");
            }
        }

        private void BtnSmazat_Click(object sender, RoutedEventArgs e)
        {
            if (DgSoubory.SelectedItem is Soubor vybrany)
            {
                if (MessageBox.Show($"Opravdu smazat soubor '{vybrany.Nazev}'?", "Varování", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var repo = new SouborRepository();
                        repo.SmazatSoubor(vybrany.IdSoubor);
                        NacistData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba při mazání: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vyberte soubor ke smazání.");
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}