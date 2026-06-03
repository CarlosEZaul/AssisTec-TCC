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
        private ClienteService service;
 
        public ucGerenciador_Clientes()
        {
            InitializeComponent();
            this.service = new ClienteService(new ClienteRepository(new AppDbContext()));
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

        
        
        public void ListGrid()
        {
            try
            {
                dgvClientes.DataSource = service.ObterTodos();
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
                using (var context = new AppDbContext())
                {
                    var repository = new ClienteRepository(context);
                    var service = new ClienteService(repository);
                    
                    dgvClientes.DataSource = service.FiltrarClientes(txtBusca.Text);
                }
                FormartGrid();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro na busca: " + ex.Message);
            }
        }

        private void AbrirFormularioCliente(int modoOperacao)
        {
            ControleEstadoComponentes(false);

            ucFormulario_Clientes ucFormularioClientes = new ucFormulario_Clientes(idSelected, modoOperacao, dgvClientes);
            
            ucFormularioClientes.Disposed += (sender, e) =>
            {
                ControleEstadoComponentes(true);
                ListGrid();
            };
            
            this.Controls.Add(ucFormularioClientes);
            ucFormularioClientes.BringToFront();
            ucFormularioClientes.Left = (this.ClientSize.Width - ucFormularioClientes.Width) / 2;
            ucFormularioClientes.Top = (this.ClientSize.Height - ucFormularioClientes.Height) / 2;
            ucFormularioClientes.Show();
        }

        private void ControleEstadoComponentes(bool ativo)
        {
            btnNew.Enabled = ativo;
            btnEditar.Enabled = ativo;
            btnDelete.Enabled = ativo;
            btnAtualizar.Enabled = ativo;
            txtBusca.Enabled = ativo;
            dgvClientes.Enabled = ativo;
        }
        
        #endregion

        #region Eventos dos Componentes
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            AbrirFormularioCliente(1);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Selecione um cliente na tabela para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AbrirFormularioCliente(2);
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvClientes.Rows.Count > 0)
            {
                try
                {
                    ControleEstadoComponentes(true);
                    idSelected = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Selecione um cliente válido para realizar a exclusão.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult primeiroDialogo = MessageBox.Show("Deseja realmente deletar o cliente selecionado?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (primeiroDialogo == DialogResult.No)
            {
                MessageBox.Show("Operação cancelada");
                return;
            }

            using (var context = new AppDbContext())
            {
                var repository = new ClienteRepository(context);
                var service = new ClienteService(repository);

                var (podeExcluir, mensagem) = service.ValidarExclusao(idSelected);

                if (!podeExcluir)
                {
                    MessageBox.Show(mensagem, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (service.DeletarCliente(idSelected))
                {
                    ControleEstadoComponentes(false);
                    MessageBox.Show("Cliente removido com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListGrid();
                }
                else
                {
                    MessageBox.Show("Falha ao tentar excluir o cliente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            ListGrid();
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            Filtro();
        }

        private void btnOS_Click(object sender, EventArgs e)
        {
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
        }

        private void btnContato_Click(object sender, EventArgs e)
        {
        }
        
        #endregion
    }
}