using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrdinaceApp.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblInfo;
        private Button btnPacienti;
        private Button btnLekari;

        private void InitializeComponent()
        {
            lblInfo = new Label();
            btnPacienti = new Button();
            btnLekari = new Button();

            SuspendLayout();

            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(20, 20);
            lblInfo.Text = "Načítám data...";

            btnPacienti.Text = "Správa pacientů";
            btnPacienti.Location = new Point(20, 60);
            btnPacienti.Click += (s, e) => MessageBox.Show("Zde bude formulář pro pacienty.");

            btnLekari.Text = "Správa lékařů";
            btnLekari.Location = new Point(160, 60);
            btnLekari.Click += (s, e) => MessageBox.Show("Zde bude formulář pro lékaře.");

            Controls.Add(lblInfo);
            Controls.Add(btnPacienti);
            Controls.Add(btnLekari);

            ClientSize = new Size(350, 130);
            Text = "OrdinaceApp";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
