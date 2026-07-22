using System;
using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;
using AssisTec.Models;
using Exception = System.Exception;
using Font = System.Drawing.Font;
using Image = iTextSharp.text.Image;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucDetalhesPedidos : UserControl
    {
        
        private int id;
        private string uf;
        private OrdemServico _ordemServico;
        
        public ucDetalhesPedidos(OrdemServico ordemServico)
        {
            InitializeComponent();
            _ordemServico = ordemServico;
        }

        private void ucDetalhesPedidos_Load(object sender, EventArgs e)
        {
            txtId.Text = _ordemServico.id_os.ToString();
            //CarregarDadosPedido();
        }
        

        #region Metodos de Manipulação de Dados

        

        

        #endregion
            
       
    }
}