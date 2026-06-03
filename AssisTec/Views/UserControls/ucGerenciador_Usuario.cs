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
        private readonly UsuarioService service;
        
        public ucGerenciador_Usuario()
        {
            InitializeComponent();
            this.service = new UsuarioService(new UsuarioRepository(new AppDbContext()));
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

                DesingComponentes.StyleTextBox(txtBusca);
                DesingComponentes.centralizarPanel(panelBotoes, this.Width);
                DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));
                DesingComponentes.StyleDataGridView(dgvUsuarios, DataGridViewAutoSizeColumnsMode.Fill);
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
            btnDelete.Enabled = true;
            btnEditar.Enabled = true;
            btnHistorico.Enabled = true;
            btnImprimir.Enabled = true;
        }
        
        public void listGrid()
        {
            try
            {
                dgvUsuarios.DataSource = service.ObterTodos();
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
            dgvUsuarios.Columns[0].Visible = false;
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
            dgvUsuarios.DataSource = service.FiltrarUsuarios(txtBusca.Text, cbInativo.Checked, cbNivel.SelectedIndex);
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
            btnDelete.Enabled = ativo;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Selecione um usuário válido para realizar a exclusão.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult primeiroDialogo = MessageBox.Show("Deseja realmente excluir o usuário selecionado?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (primeiroDialogo == DialogResult.No)
            {
                MessageBox.Show("Operação cancelada");
                return;
            }

            int idLogado = Sessao.usuarioLogado.Id;
            var validacao = service.ValidarAntesDeExcluir(idSelected, idLogado);

            if (!string.IsNullOrEmpty(validacao.mensagem) && !validacao.sucesso)
            {
                MessageBox.Show(validacao.mensagem, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (validacao.sucesso)
            {
                DialogResult segundoDialogo = MessageBox.Show(validacao.mensagem, "Excluir Conta Atual", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (segundoDialogo == DialogResult.No)
                {
                    MessageBox.Show("Operação cancelada");
                    return;
                }
            }

            var resultado = service.ConfirmarExclusao(idSelected);

            if (resultado.sucesso)
            {
                btnDelete.Enabled = false;
                btnEditar.Enabled = false;
                MessageBox.Show(resultado.mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (idSelected == idLogado)
                {
                    Application.Restart();
                }
                else
                {
                    listGrid();
                }
            }
            else
            {
                MessageBox.Show(resultado.mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            service.ExecutarRelatorioGeral(txtBusca.Text, cbInativo.Checked, cbNivel.SelectedIndex);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (idSelected <= 0)
            {
                MessageBox.Show("Selecione um técnico na tabela para gerar o relatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = service.ExecutarRelatorioTecnico(idSelected);
            MessageBox.Show(resultado.mensagem);
        }

        #endregion
    }
}