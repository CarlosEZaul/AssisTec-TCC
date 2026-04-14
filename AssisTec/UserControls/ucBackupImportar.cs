using System;
using System.Drawing;
using System.Windows.Forms;

namespace AssisTec.UserControls
{
    public partial class ucBackupImportar : UserControl
    {
        public ucBackupImportar()
        {
            InitializeComponent();
            
        }
        conexao con = new conexao();
       
        private void label1_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            con.backupBanco();
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            con.importarBackup();
        }
    }
}