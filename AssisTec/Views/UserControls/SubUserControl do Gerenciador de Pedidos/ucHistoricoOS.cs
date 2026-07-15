using System;
using System.Data;
using System.Windows.Forms;

using AssisTec.Models;

using AssisTec.Repository;
using AssisTec.Service;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls
{
    
    
    public partial class ucHistoricoOS : UserControl
    {
        private readonly UsuarioService _usuarioService;
        private readonly ClienteService _clienteService;
        
        public ucHistoricoOS(int id, UsuarioService  usuarioService)
        {
            InitializeComponent();
            DesingModerno();
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            listGridUsuario(id);
           
        }

        public ucHistoricoOS(int id, ClienteService clienteService)
        {
            InitializeComponent();
            DesingModerno();    
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            listGridCliente(id);
        }

        #region DesingModerno

        private void DesingModerno()
        {
            DesingComponentes.StyleDataGridView(dgvOS, DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
        }
        

        #endregion

        private void listGridUsuario(int id)
        {
            dgvOS.DataSource = _usuarioService.obterHistoricoOs(id);
        }

        private void listGridCliente(int id)
        {
            dgvOS.DataSource = _clienteService.ObterHistoricoOS(id);
        } 
        
       
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        
        private void formatGrid()
        {
            if (dgvOS.Columns.Count <= 0) return;
            // Headers
            dgvOS.Columns[0].HeaderText = "ID_OS";
            dgvOS.Columns[1].HeaderText = "Cliente";
            dgvOS.Columns[2].HeaderText = "Técnico Responsável";
            
            dgvOS.Columns[3].HeaderText = "Equipamento";
            dgvOS.Columns[4].HeaderText = "Status";
            dgvOS.Columns[5].HeaderText = "Data de abertura";
            dgvOS.Columns[6].HeaderText = "Última Atualização";
            dgvOS.Columns[7].HeaderText = "Data de Encerramento";
            dgvOS.Columns[8].HeaderText = "Valor Mão de Obra";
            dgvOS.Columns[9].HeaderText = "Valor p/ peça";
            dgvOS.Columns[10].HeaderText = "Valor total";
            dgvOS.Columns[11].HeaderText = "Problema relatado";
            dgvOS.Columns[11].HeaderText = "Diagnóstico";
            dgvOS.Columns[11].HeaderText = "Observações";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}