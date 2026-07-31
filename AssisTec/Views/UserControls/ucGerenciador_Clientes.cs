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

                DesignComponentes.StyleTextBox(txtBusca);
                DesignComponentes.centralizarPanel(panelBotoes, this.Width);
                DesignComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                

                DesignComponentes.StyleDataGridView(dgvClientes, DataGridViewAutoSizeColumnsMode.Fill);
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

            dgvClientes.Columns[0].HeaderText = "Id";
            dgvClientes.Columns[1].HeaderText = "Nome";
            dgvClientes.Columns[2].HeaderText = "Cpf";
            dgvClientes.Columns[3].HeaderText = "Telefone";
            dgvClientes.Columns[4].HeaderText = "Status";
            dgvClientes.Columns[5].HeaderText = "Data de nasc.";
            dgvClientes.Columns[6].HeaderText = "Cep";
            dgvClientes.Columns[7].HeaderText = "Rua";
            dgvClientes.Columns[8].HeaderText = "Número";
            dgvClientes.Columns[9].HeaderText = "Cidade";
            dgvClientes.Columns[10].HeaderText = "Estado";
            dgvClientes.Columns[11].HeaderText = "Bairro";
            dgvClientes.Columns[12].HeaderText = "Complemento";
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
            btnContato.Enabled = ativo;
            btnImprimirCliente.Enabled = ativo;
            
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
            try
            {
                if (service.ObterPorId(idSelected).Status == "Ativado")
                {
                    DialogResult result = MessageBox.Show("Deseja desativar o cliente ?", "Desativar", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        clienteServiceOs.AlterarStatus(idSelected);
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
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao alterar o status: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            ucHistoricoOS historicoOs = new ucHistoricoOS(idSelected, clienteServiceOs);
            this.Controls.Add(historicoOs);
            historicoOs.BringToFront();
            historicoOs.Left = (this.ClientSize.Width - historicoOs.Width) / 2;
            historicoOs.Top = (this.ClientSize.Height - historicoOs.Height) / 2;
            historicoOs.Show();
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

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                CriarNovoContexto();
                int idCliente = Convert.ToInt32(dgvClientes.CurrentRow.Cells["Id"].Value);
                string nomeCliente = dgvClientes.CurrentRow.Cells["Nome"].Value.ToString();

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                    sfd.FileName = $"Relatorio_Individual_{nomeCliente.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}";
                    sfd.Title = "Salvar Relatório Individual do Cliente";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        clienteServiceOs.GerarRelatorioIndividualClientePdf(idCliente, sfd.FileName);

                        Cursor = Cursors.Default;
                        MessageBox.Show("Relatório individual gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Erro ao gerar o relatório: {exception.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                string filtroNome = txtBusca.Text.Trim();
                bool exibirDesativados = cbDesativado.Checked;

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                    sfd.FileName = $"Relatorio_Geral_Clientes_{DateTime.Now:yyyyMMdd}";
                    sfd.Title = "Salvar Relatório Geral de Clientes";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;

                        
                        clienteServiceOs.GerarRelatorioClientesPdf(filtroNome, exibirDesativados, sfd.FileName);

                        Cursor = Cursors.Default;
                        MessageBox.Show("Relatório geral gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show($"Erro ao gerar o relatório geral: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}