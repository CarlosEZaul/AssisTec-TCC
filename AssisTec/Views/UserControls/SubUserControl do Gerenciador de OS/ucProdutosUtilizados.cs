using System;
using System.Data;
using System.Windows.Forms;
using AssisTec.Models;
using MySql.Data.MySqlClient;


namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucProdutosUtilizados : UserControl
    {
        
        public ucProdutosUtilizados()
        {
            InitializeComponent();
            
            
            
        }
        

        private void txtQntd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}