using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.AtendeClienteService;
using AssisTec.Business;
using AssisTec.Data;
using AssisTec.Reports;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes;
using MySql.Data.MySqlClient;
using Exception = System.Exception;

namespace AssisTec.UserControls
{
    public partial class ucGerenciador_Clientes : UserControl
    {
        conexao con = new conexao();
        Cliente cliente = new Cliente();
        ClienteService service = new ClienteService();
        ClienteRository repository = new ClienteRository();
        ClienteRelatorio  relatorio = new ClienteRelatorio();
        string sql;
        MySqlCommand cmd;
        private int modo;
        private int id;
        private string uf;
        private bool okCep;
 
        public ucGerenciador_Clientes()
        {
            InitializeComponent();
            
            ApplyModernDesign();
        }

        private void ucGerenciadorClientes_Load(object sender, EventArgs e)
        {
            btnNew.Focus();
            listGrid(); 
        }
        
        #region Design Moderno

        private void ApplyModernDesign()
        {
            try
            {
                this.Text = "Gerenciador de Clientes";
                this.BackColor = Color.FromArgb(39, 55, 76);

                // Labels
                //DesingComponentes.ApplyLabelStyles(this);

                // TextBox
                DesingComponentes.StyleTextBox(txtBusca);
                
                // Botões
                DesingComponentes.centralizarPanel(panelBotoes, this.Width);
                DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));

                // DataGridView
                DesingComponentes.StyleDataGridView(dgvClientes, DataGridViewAutoSizeColumnsMode.Fill);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message);
            }
        }

        #endregion

        #region Métodos de Manipulação de Dados

        
        
        private void formartGrid()
        { 
            if (dgvClientes.Columns.Count > 0)
            {
                dgvClientes.Columns[0].HeaderText = "ID";
                dgvClientes.Columns[0].Visible = false;
                dgvClientes.Columns[1].HeaderText = "NOME";
                dgvClientes.Columns[2].HeaderText = "CPF";
                dgvClientes.Columns[3].HeaderText = "TELEFONE";
                dgvClientes.Columns[4].HeaderText = "DATA DE NASC.";
                dgvClientes.Columns[5].HeaderText = "CEP";
                dgvClientes.Columns[6].HeaderText = "RUA";
                dgvClientes.Columns[7].HeaderText = "NUMERO";
                dgvClientes.Columns[8].HeaderText = "CIDADE";
                dgvClientes.Columns[9].HeaderText = "BAIRRO";
                dgvClientes.Columns[10].HeaderText = "ESTADO";
                dgvClientes.Columns[11].HeaderText = "COMPLEMENTO";
            }
            
            
        }

        private void enableBtn()
        {
            btnEditar.Enabled = true;
            btnDelete.Enabled = true;
            btnImprimir.Enabled = true;
            btnOS.Enabled = true;
        }

        private void disableBtn()
        {
            btnEditar.Enabled = false;
            btnDelete.Enabled = false;
            btnImprimir.Enabled = false;
            btnOS.Enabled = false;
        }
        
        private void listGrid()
        {
            try
            {
                dgvClientes.DataSource = repository.ObterTodosClientes();
                formartGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Deletar cliente?", "excluir",  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                var (podeExcluir, mensagem) = service.ValidarExclusao(id);

                if (!podeExcluir)
                {
                    MessageBox.Show("Nao e possivel exluir com OS em andamento");
                }
                else
                {
                    if (service.DeletarCliente(id))
                    {
                        dgvClientes.DataSource = repository.ObterTodosClientes();
                        disableBtn();
                    }
                }
            }
            
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvClientes.Rows.Count > 0)
            {
                try
                {
                    enableBtn();
                    btnEditar.Enabled = true;
                    id = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells[0].Value);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dgvClientes.DataSource = repository.buscarClientes(txtBusca.Text);
                formartGrid();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Erro na busca: " + ex.Message);
            }
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            ucFormulario_Clientes ucFormularioClientes = new ucFormulario_Clientes(id, 1, dgvClientes);
            
            this.Controls.Add(ucFormularioClientes);
            ucFormularioClientes.BringToFront();
            ucFormularioClientes.Left = (this.ClientSize.Width - ucFormularioClientes.Width)/2;
            ucFormularioClientes.Top = (this.ClientSize.Height - ucFormularioClientes.Height)/2;
            ucFormularioClientes.Show();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ucFormulario_Clientes ucFormularioClientes = new ucFormulario_Clientes(id, 2, dgvClientes);
            
            this.Controls.Add(ucFormularioClientes);
            ucFormularioClientes.BringToFront();
            ucFormularioClientes.Left = (this.ClientSize.Width - ucFormularioClientes.Width)/2;
            ucFormularioClientes.Top = (this.ClientSize.Height - ucFormularioClientes.Height)/2;
            ucFormularioClientes.Show();
        }

        private void btnOS_Click(object sender, EventArgs e)
        {
            Pessoa pessoa = new Cliente();
            ucHistoricoOS ucHistorico = new ucHistoricoOS(pessoa,id);
            ucHistorico.Left = (this.ClientSize.Width - ucHistorico.Width)/2;
            ucHistorico.Top = (this.ClientSize.Height - ucHistorico.Height)/2;
            this.Controls.Add(ucHistorico);
            
            ucHistorico.BringToFront();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            relatorio.ImprimirCliente(id);
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            
            relatorio.gerarRelatorioTodosClientes();
        }

        private async void btnContato_Click(object sender, EventArgs e)
        {
            cliente.WhatsAppWeb("18996785479", "teste");
        }
    }
        #endregion
}
