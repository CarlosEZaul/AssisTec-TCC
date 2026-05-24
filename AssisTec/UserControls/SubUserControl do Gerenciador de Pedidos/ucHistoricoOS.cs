using System;
using System.Data;
using System.Windows.Forms;
using AssisTec.Business;
using AssisTec.Data;
using AssisTec.Reports;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls
{
    public partial class ucHistoricoOS : UserControl
    {
        
        Usuario user = new Usuario();
        UsuarioRepository repository = new UsuarioRepository();
        ClienteRepository ClienteRepository = new ClienteRepository();
        UsuarioService service = new UsuarioService();
        UsuarioRelatorio relatorio = new UsuarioRelatorio();
        
        Cliente cliente = new Cliente();
        
        
        public ucHistoricoOS(Pessoa pessoa, int id)
        {
            InitializeComponent();
            DesingModerno();
            
            if (pessoa is Usuario)
            {
                listGridUsuario(id);
            }
            if (pessoa is Cliente)
            {
                listGridCliente(id);
            }
            formatGrid();
           
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
            dgvOS.DataSource = repository.ObterHistoricoOs(id);
        }

        private void listGridCliente(int id)
        {
            dgvOS.DataSource = ClienteRepository.ObterHistoricoOs(id);
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
    }
}