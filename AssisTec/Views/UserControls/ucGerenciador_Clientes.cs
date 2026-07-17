using System;
using System.Windows.Forms;
using System.Drawing;
using AssisTec.Models;
using AssisTec.Repository;

using AssisTec.Service;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes;

namespace AssisTec.UserControls
{
    public partial class ucGerenciador_Clientes : UserControl
    {
        private int idSelected;
        private ClienteService service;
        private ClienteService clienteServiceOs;
 
        public ucGerenciador_Clientes()
        {
            InitializeComponent();
            CriarNovoContexto();
            btnNew.Focus();
            ListGrid(); 
            
            ApplyModernDesign();
        }

        private void CriarNovoContexto()
        {
            this.service = new ClienteService(new ClienteRepository(new AppDbContext()));
            this.clienteServiceOs = new ClienteService(new ClienteRepository(new AppDbContext()), new OrdemServicoRepository(new AppDbContext()));
        }

        private void ucGerenciadorClientes_Load(object sender, EventArgs e)
        {
            
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
            dgvClientes.Columns[4].HeaderText = "STATUS";
            dgvClientes.Columns[5].HeaderText = "DATA DE NASC.";
            dgvClientes.Columns[6].HeaderText = "CEP";
            dgvClientes.Columns[7].HeaderText = "RUA";
            dgvClientes.Columns[8].HeaderText = "NÚMERO";
            dgvClientes.Columns[9].HeaderText = "CIDADE";
            dgvClientes.Columns[10].HeaderText = "ESTADO";
            dgvClientes.Columns[11].HeaderText = "BAIRRO";
            dgvClientes.Columns[12].HeaderText = "COMPLEMENTO";
        }
        
        public void ListGrid()
        {
            try
            {
                CriarNovoContexto();
                dgvClientes.DataSource = null;
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
                CriarNovoContexto();
                dgvClientes.DataSource = null;
                dgvClientes.DataSource = service.ObterComFiltros(txtBusca.Text, cbDesativado.Checked);
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
            btnStatus.Enabled = ativo;
            btnAtualizar.Enabled = ativo;
            txtBusca.Enabled = ativo;
            dgvClientes.Enabled = ativo;
            btnOS.Enabled = ativo;
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

        private void btnStatus_Click(object sender, EventArgs e)
        {
            if (service.ObterPorId(idSelected).Status == "Ativado")
            {
                DialogResult result = MessageBox.Show("Deseja desativar o cliente ?", "Desativar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    service.AlterarStatus(idSelected);
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Deseja ativar o cliente ?", "Ativar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    service.AlterarStatus(idSelected);
                }
            }
            ListGrid();
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
            ucHistoricoOS historicoOs = new ucHistoricoOS(idSelected, clienteServiceOs);
            this.Controls.Add(historicoOs);
            historicoOs.BringToFront();
            historicoOs.Left = (this.ClientSize.Width - historicoOs.Width) / 2;
            historicoOs.Top = (this.ClientSize.Height - historicoOs.Height) / 2;
            historicoOs.Show();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
        }

        private async void btnContato_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Por favor, selecione um cliente válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnContato.Enabled = false;

            try
            {
                Cliente cliente = service.ObterPorId(idSelected);

                if (cliente == null)
                {
                    MessageBox.Show("Cliente não encontrado no sistema.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(cliente.Telefone))
                {
                    MessageBox.Show("Este cliente não possui um telefone cadastrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool sucesso = await ContatoWhatsApp.EntrarContato(cliente.Telefone);

                if (sucesso)
                {
                    MessageBox.Show("Contato iniciado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Falha ao iniciar contato. Verifique a conexão.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro inesperado: {ex.Message}", "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnContato.Enabled = true;
            }
        }
        #endregion

        private void cbDesativado_CheckedChanged(object sender, EventArgs e)
        {
            Filtro();
        }
    }
}