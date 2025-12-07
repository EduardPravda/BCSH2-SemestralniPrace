using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using OrdinaceApp1.DataAccess;

namespace OrdinaceApp1.Views
{
    public partial class SouboryWindow : Window
    {
        private int _userId;

        public SouboryWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            NacistData();
        }

        private void NacistData()
        {
            var repo = new SouborRepository();
            DgSoubory.ItemsSource = repo.GetSeznamSouboru();
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
                repo.NahratSoubor(nazev, "application/file", pripona, data, "Popis", _userId);

                NacistData(); 
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}