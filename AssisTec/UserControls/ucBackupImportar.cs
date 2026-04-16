using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace AssisTec.UserControls
{
    public partial class ucBackupImportar : UserControl
    {
        public ucBackupImportar()
        {
            InitializeComponent();
            DesingModerno();
        }
        conexao con = new conexao();

        private void DesingModerno()
        {
            this.Text = "Backup e Importar";
            this.BackColor = Color.FromArgb(39, 55, 76);

            
        }
        

        private void btnBackup_Click(object sender, EventArgs e)
        {
            con.backupBanco();
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            con.importarBackup();
        }


        private void btnBackup_MouseEnter(object sender, EventArgs e)
        {
            btnBackup.BorderStyle = DashStyle.DashDotDot;
            btnBackup.BorderColor = Color.White;
        }

        
    }
}