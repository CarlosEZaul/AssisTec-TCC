using System;
using System.Windows.Forms;
using System.Drawing;
using AssisTec.Models;
using AssisTec.Repository;
using AssisTec.Reports;
using AssisTec.Service;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes;

namespace AssisTec.UserControls
{
    public partial class ucGerenciador_Clientes : UserControl
    {
        private int idSelected;
        private readonly ClienteService service;
        private readonly IClienteRepository repository;
        //private readonly ClienteRelatorio relatorio;
 
        public ucGerenciador_Clientes()
        {
            InitializeComponent();
            
            var context = new AppDbContext();
            this.repository = new ClienteRepository(context);
            this.service = new ClienteService(this.repository);
            //this.relatorio = new ClienteRelatorio();

            ApplyModernDesign();
        }

        private void ucGerenciadorClientes_Load(object sender, EventArgs e)
        {
            btnNew.Focus();
            ListGrid(); 
        }
        
        #region Design Moderno
        private void ApplyModernDesign()
        {
            try
            {
                this.Text = "Gerenciador de Clientes";
                this.BackColor = Color.FromArgb(39, 55, 76);

                DesingComponentes.StyleTextBox(txtBusca);
                DesingComponentes.centralizarPanel(panelBotoes, this.Width);
                DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));

                DesingComponentes.StyleDataGridView(dgvClientes, DataGridViewAutoSizeColumnsMode.Fill);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message);
            }
        }
        #endregion

        #region Métodos de Interface
        private void FormartGrid()
        { 
            if (dgvClientes.Columns.Count <= 0) return;

            dgvClientes.Columns[0].HeaderText = "ID";
            dgvClientes.Columns[0].Visible = false;
            dgvClientes.Columns[1].HeaderText = "NOME";
            dgvClientes.Columns[2].HeaderText = "CPF";
            dgvClientes.Columns[3].HeaderText = "TELEFONE";
            dgvClientes.Columns[4].HeaderText = "DATA DE NASC.";
            dgvClientes.Columns[5].HeaderText = "CEP";
            dgvClientes.Columns[6].HeaderText = "RUA";
            dgvClientes.Columns[7].HeaderText = "NÚMERO";
            dgvClientes.Columns[8].HeaderText = "CIDADE";
            dgvClientes.Columns[9].HeaderText = "BAIRRO";
            dgvClientes.Columns[10].HeaderText = "ESTADO";
            dgvClientes.Columns[11].HeaderText = "COMPLEMENTO";
        }

        private void EnableBtn()
        {
            btnEditar.Enabled = true;
            btnDelete.Enabled = true;
            btnImprimir.Enabled = true;
            btnOS.Enabled = true;
        }

        private void DisableBtn()
        {
            btnEditar.Enabled = false;
            btnDelete.Enabled = false;
            btnImprimir.Enabled = false;
            btnOS.Enabled = false;
        }
        
        public void ListGrid()
        {
            try
            {
                dgvClientes.DataSource = service.ObterTodos();
                FormartGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Filtro()
        {
            try
            {
                dgvClientes.DataSource = service.FiltrarClientes(txtBusca.Text);
                FormartGrid();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro na busca: " + ex.Message);
            }
        }
        #endregion

        #region Eventos dos Componentes
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Selecione um cliente válido para realizar a exclusão.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Deseja realmente deletar o cliente selecionado?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                var (podeExcluir, mensagem) = service.ValidarExclusao(idSelected);

                if (!podeExcluir)
                {
                    MessageBox.Show(mensagem, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (service.DeletarCliente(idSelected))
                    {
                        MessageBox.Show("Cliente removido com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ListGrid();
                        DisableBtn();
                    }
                    else
                    {
                        MessageBox.Show("Falha ao tentar excluir o cliente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            ListGrid();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvClientes.Rows.Count > 0)
            {
                try
                {
                    EnableBtn();
                    idSelected = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            Filtro();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            var ucFormularioClientes = new ucFormulario_Clientes(0, 1, dgvClientes);
            
            this.Controls.Add(ucFormularioClientes);
            ucFormularioClientes.BringToFront();
            ucFormularioClientes.Left = (this.ClientSize.Width - ucFormularioClientes.Width) / 2;
            ucFormularioClientes.Top = (this.ClientSize.Height - ucFormularioClientes.Height) / 2;
            ucFormularioClientes.Show();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Selecione um cliente na tabela para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ucFormularioClientes = new ucFormulario_Clientes(idSelected, 2, dgvClientes);
            
            this.Controls.Add(ucFormularioClientes);
            ucFormularioClientes.BringToFront();
            ucFormularioClientes.Left = (this.ClientSize.Width - ucFormularioClientes.Width) / 2;
            ucFormularioClientes.Top = (this.ClientSize.Height - ucFormularioClientes.Height) / 2;
            ucFormularioClientes.Show();
        }

        private void btnOS_Click(object sender, EventArgs e)
        {
            // if (idSelected <= 0) return;
            //
            // Pessoa pessoa = new Cliente();
            // var ucHistorico = new ucHistoricoOS(pessoa, idSelected);
            // ucHistorico.Left = (this.ClientSize.Width - ucHistorico.Width) / 2;
            // ucHistorico.Top = (this.ClientSize.Height - ucHistorico.Height) / 2;
            // this.Controls.Add(ucHistorico);
            //
            // ucHistorico.BringToFront();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0) return;
            //relatorio.ImprimirCliente(idSelected);
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            //relatorio.gerarRelatorioTodosClientes();
        }

        private void btnContato_Click(object sender, EventArgs e)
        {
        }
        #endregion
    }
}