using System;
using System.Data;
using System.Linq;
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
        

        private void ConfigurarComponentes(DataTable dtHistorico)
        {
            if (dtHistorico == null || dtHistorico.Rows.Count == 0)
            {
                dgvOS.DataSource = null;
                cbClientes.DataSource = null;
                cbTécnico.DataSource = null;
                return;
            }

            dgvOS.DataSource = dtHistorico;

            foreach (var coluna in new[] { "ID_CLIENTE", "CLIENTE", "ID_TECNICO", "Técnico Responsável" })
            {
                if (dgvOS.Columns.Contains(coluna))
                    dgvOS.Columns[coluna].Visible = false;
            }

            // Renomeia os headers das colunas combinadas
            if (dgvOS.Columns.Contains("CLIENTE_EXIBICAO"))
                dgvOS.Columns["CLIENTE_EXIBICAO"].HeaderText = "Cliente";

            if (dgvOS.Columns.Contains("TECNICO_EXIBICAO"))
                dgvOS.Columns["TECNICO_EXIBICAO"].HeaderText = "Técnico Responsável";

            // ComboBox usa as colunas cruas (ID + Nome), sem precisar das colunas de exibição
            var listaClientes = dtHistorico.AsEnumerable()
                .Select(row => $"{row.Field<int>("ID_CLIENTE")} - {row.Field<string>("CLIENTE")}")
                .Distinct()
                .OrderBy(texto => texto)
                .ToList();

            cbClientes.DataSource = listaClientes;

            var listaTecnicos = dtHistorico.AsEnumerable()
                .Select(row => $"{row.Field<int>("ID_TECNICO")} - {row.Field<string>("Técnico Responsável")}")
                .Distinct()
                .OrderBy(texto => texto)
                .ToList();

            cbTécnico.DataSource = listaTecnicos;
        }

        private void listGridUsuario(int id)
        {
            DataTable dtHistorico = _usuarioService.obterHistoricoOs(id);
            ConfigurarComponentes(dtHistorico);
            
        }

        private void listGridCliente(int id)
        {
            DataTable dtHistorico = _clienteService.ObterHistoricoOS(id);
            ConfigurarComponentes(dtHistorico);
        } 
        
       
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        
    }
}