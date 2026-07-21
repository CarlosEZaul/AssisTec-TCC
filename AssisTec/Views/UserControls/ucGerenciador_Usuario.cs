using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using AssisTec.Service;
using AssisTec.Models;
using AssisTec.Repository;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Usuarios;

namespace AssisTec.UserControls
{
    public partial class ucGerenciador_Usuario : UserControl
    {
        private int idSelected;
        private UsuarioService service;
        private UsuarioService serviceOs;
        public ucGerenciador_Usuario()
        {
            InitializeComponent();
            CriarNovoContexto();
        }

        private void CriarNovoContexto()
        {
            this.service = new UsuarioService(new UsuarioRepository(new AppDbContext()));
            this.serviceOs =  new UsuarioService(new UsuarioRepository(new AppDbContext()), new OrdemServicoRepository(new AppDbContext()));
        }

        private void ucGerenciador_Usuario_Load(object sender, EventArgs e)
        {
            ConfigurarComboBox();
            ApplyModernDesign();
            listGrid();
            formartGrid();
        }

        #region Design Moderno
        private void ApplyModernDesign()
        {
            try
            {
                this.Text = "Gerenciador de Usuários";
                this.BackColor = Color.FromArgb(39, 55, 76);

                DesignComponentes.StyleTextBox(txtBusca);
                DesignComponentes.centralizarPanel(panelBotoes, this.Width);
                DesignComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                DesignComponentes.StyleButton(btnStatus, Color.FromArgb(0, 120, 215));
                DesignComponentes.StyleDataGridView(dgvUsuarios, DataGridViewAutoSizeColumnsMode.Fill);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message);
            }
        }
        #endregion

        #region Metodos de Interface
        private void ConfigurarComboBox()
        {
            cbNivel.Items.Clear();
            var lista = new List<dynamic>()
            {
                new { Id = 0, Nome = "Todos" },
                new { Id = 1, Nome = "1- Gerente" },
                new { Id = 2, Nome = "2- Atendente" },
                new { Id = 3, Nome = "3- Técnico" }
            };
            cbNivel.DataSource = lista;
            cbNivel.DisplayMember = "Nome";
            cbNivel.ValueMember = "Id";
            cbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        private void enableBtn()
        {
            btnNew.Enabled = true;
            btnStatus.Enabled = true;
            btnEditar.Enabled = true;
            btnHistorico.Enabled = true;
            btnImprimir.Enabled = true;
        }
        
        public void listGrid()
        {
            try
            {
                CriarNovoContexto();
                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = service.ObterTodos();
                formartGrid();
                cbInativo.Checked = false;
                cbNivel.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void formartGrid()
        {
            if (dgvUsuarios.Columns.Count <= 0) return;
            
            dgvUsuarios.Columns[0].HeaderText = "ID";
            dgvUsuarios.Columns[1].HeaderText = "Nome";
            dgvUsuarios.Columns[2].HeaderText = "CPF";
            dgvUsuarios.Columns[3].HeaderText = "Senha";
            dgvUsuarios.Columns[3].Visible = false;
            dgvUsuarios.Columns[4].HeaderText = "Telefone";
            dgvUsuarios.Columns[5].HeaderText = "Nível";
            dgvUsuarios.Columns[6].HeaderText = "Status";
            dgvUsuarios.Columns[7].HeaderText = "CEP";
            dgvUsuarios.Columns[8].HeaderText = "Rua";
            dgvUsuarios.Columns[9].HeaderText = "Número";
            dgvUsuarios.Columns[10].HeaderText = "Cidade";
            dgvUsuarios.Columns[11].HeaderText = "Bairro";
            dgvUsuarios.Columns[12].HeaderText = "Estado";
            dgvUsuarios.Columns[13].HeaderText = "Complemento";
        }

        private void Filtro()
        {
            CriarNovoContexto();
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = service.FiltrarUsuarios(txtBusca.Text, cbInativo.Checked, cbNivel.SelectedIndex);
            formartGrid();
        }
        
        private void AbrirFormularioUsuario(int modoOperacao)
        {
            ControleEstadoComponentes(false);

            ucFormularioUsuarios ucFormularioUsuarios = new ucFormularioUsuarios(idSelected, modoOperacao, dgvUsuarios);
            
            ucFormularioUsuarios.Disposed += (sender, e) =>
            {
                ControleEstadoComponentes(true);
                listGrid();
            };
            
            this.Controls.Add(ucFormularioUsuarios);
            ucFormularioUsuarios.BringToFront();
            ucFormularioUsuarios.Left = (this.ClientSize.Width - ucFormularioUsuarios.Width) / 2;
            ucFormularioUsuarios.Top = (this.ClientSize.Height - ucFormularioUsuarios.Height) / 2;
            ucFormularioUsuarios.Show();
        }

        private void ControleEstadoComponentes(bool ativo)
        {
            btnNew.Enabled = ativo;
            btnEditar.Enabled = ativo;
            btnStatus.Enabled = ativo;
            btnAtualizar.Enabled = ativo;
            txtBusca.Enabled = ativo;
            dgvUsuarios.Enabled = ativo;
        }
        #endregion

        #region Eventos dos Componentes
        private void btnNew_Click(object sender, EventArgs e)
        {
            AbrirFormularioUsuario(1);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Selecione um usuário na tabela para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AbrirFormularioUsuario(2);
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsuarios.Rows.Count > 0)
            {
                try
                {
                    enableBtn();
                    idSelected = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Selecione um usuário válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var usuario = service.ObterPorId(idSelected);
            
            string mensagem = "";

            if (usuario.Status == "Ativo")
            {
                mensagem = "Deseja desativar o usuário?";
            }
            else
            {
                mensagem = "Deseja ativar o usuário?";
            }

            DialogResult primeiroDialogo = MessageBox.Show(mensagem, "Alterar Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (primeiroDialogo == DialogResult.No)
            {
                MessageBox.Show("Operação cancelada");
                return;
            }

            int idLogado = Sessao.usuarioLogado.Id;
            var validacao = service.ValidarAntesDeDeativar(idSelected, idLogado);

            if (!string.IsNullOrEmpty(validacao.mensagem) && !validacao.sucesso)
            {
                MessageBox.Show(validacao.mensagem, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (validacao.sucesso)
            {
                DialogResult segundoDialogo = MessageBox.Show(validacao.mensagem, "Desativar Conta Atual", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (segundoDialogo == DialogResult.No)
                {
                    MessageBox.Show("Operação cancelada");
                    return;
                }
            }

            var resultado = service.AlterarStatus(idSelected);

            if (resultado)
            {
                MessageBox.Show("Status alterado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (idSelected == idLogado)
                {
                    Application.Restart();
                }
                else
                {
                    idSelected = 0;
                    btnStatus.Enabled = false;
                    btnEditar.Enabled = false;
                    
                    if (dgvUsuarios != null && dgvUsuarios.Rows.Count > 0)
                    {
                        dgvUsuarios.ClearSelection();
                    }

                    listGrid();
                }
            }
            else
            {
                MessageBox.Show("Erro ao alterar status", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            Filtro();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Filtro();
        }

        private void cbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filtro();
        }

        private void cbNivel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filtro();
        }

        private void btnHistorico_Click(object sender, EventArgs e)
        {
            ucHistoricoOS historicoOs = new ucHistoricoOS(idSelected, serviceOs);
            this.Controls.Add(historicoOs);
            historicoOs.BringToFront();
            historicoOs.Left = (this.ClientSize.Width - historicoOs.Width) / 2;
            historicoOs.Top = (this.ClientSize.Height - historicoOs.Height) / 2;
            historicoOs.Show();
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                string nome = txtBusca.Text.Trim();
                bool apenasInativos = cbInativo.Checked;
        
                int nivel = 0;
                if (cbNivel.SelectedValue != null && int.TryParse(cbNivel.SelectedValue.ToString(), out int valNivel))
                {
                    nivel = valNivel;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Arquivos PDF (*.pdf)|*.pdf";
                    saveFileDialog.FileName = "Relatorio_Usuarios_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";
                    saveFileDialog.Title = "Salvar Relatório de Usuários";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        serviceOs.GerarRelatorioUsuariosPdf(nome, apenasInativos, nivel, saveFileDialog.FileName);
                        MessageBox.Show("Relatório de usuários gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar o relatório: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Selecione um técnico na tabela para gerar o relatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Arquivos PDF (*.pdf)|*.pdf";
                    saveFileDialog.FileName = $"Relatorio_Produtividade_Tecnico_{idSelected}_{DateTime.Now:yyyyMMdd}.pdf";
                    saveFileDialog.Title = "Salvar Relatório de Produtividade";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        serviceOs.GerarRelatorioIndividualPdf(idSelected, saveFileDialog.FileName);
                        MessageBox.Show("Relatório de produtividade gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar o relatório: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

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
                Usuario usuario = service.ObterPorId(idSelected);
                

                if (usuario == null)
                {
                    MessageBox.Show("Cliente não encontrado no sistema.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(usuario.Telefone))
                {
                    MessageBox.Show("Este cliente não possui um telefone cadastrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool sucesso = await ContatoWhatsApp.EntrarContato(usuario.Telefone);

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
    }
}