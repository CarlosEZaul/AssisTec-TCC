using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AssisTec.Business;
using AssisTec.Data;
using AssisTec.Reports;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Usuarios;

namespace AssisTec.UserControls
{
    public partial class ucGerenciador_Usuario : UserControl
    {
        
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int modo;
        private int id;
        private string uf;
        private bool okCep;
        private Usuario user = new Usuario();
        UsuarioRepository repository = new UsuarioRepository();
        UsuarioService service = new UsuarioService();
        UsuarioRelatorio  relatorio = new UsuarioRelatorio();
        
        
        
        public ucGerenciador_Usuario()
        {
            InitializeComponent();
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

                 // TextBox
                 DesingComponentes.StyleTextBox(txtBusca);
                 

                 // Botões
                 DesingComponentes.centralizarPanel(panelBotoes, this.Width);
                 DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                 DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));

                 // DataGridView
                 DesingComponentes.StyleDataGridView(dgvUsuarios, DataGridViewAutoSizeColumnsMode.Fill);

             }
             catch (Exception ex)
             {
                 MessageBox.Show("Erro ao aplicar design: " + ex.Message);
             }
         }

        

        #endregion
        

        #region Metodos ou funcoes

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
                dgvUsuarios.DataSource = repository.ObterTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void formartGrid()
        {
            if (dgvUsuarios.Columns.Count <= 0) return;
            // Headers
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
            dgvUsuarios.DataSource =
                repository.ObterComFiltros(txtBusca.Text, cbInativo.Checked, cbNivel.SelectedIndex);
           
        }
        
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            ucFormularioUsuarios ucFormularioUsuarios = new ucFormularioUsuarios(id, modo = 1, dgvUsuarios);
            this.Controls.Add(ucFormularioUsuarios);
            ucFormularioUsuarios.BringToFront();
            ucFormularioUsuarios.Left = (this.ClientSize.Width - ucFormularioUsuarios.Width)/2;
            ucFormularioUsuarios.Top = (this.ClientSize.Height - ucFormularioUsuarios.Height)/2;
            ucFormularioUsuarios.Show();
        }
        
        
        //
        #endregion

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ucFormularioUsuarios ucFormularioUsuarios = new ucFormularioUsuarios(id, modo = 2, dgvUsuarios);
            this.Controls.Add(ucFormularioUsuarios);
            ucFormularioUsuarios.BringToFront();
            ucFormularioUsuarios.Left = (this.ClientSize.Width - ucFormularioUsuarios.Width)/2;
            ucFormularioUsuarios.Top = (this.ClientSize.Height - ucFormularioUsuarios.Height)/2;
            ucFormularioUsuarios.Show(); 
        }
        

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsuarios.Rows.Count > 0)
            {
                try
                {
                    enableBtn();
                    
                    

                    id = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells[0].Value);

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult =
                MessageBox.Show("Deseja excluir o usuário? ", "Excluir", MessageBoxButtons.YesNo);

            var (podeExcluir, mensagem) = service.ValidarExclusao(id);

            
            
            if (dialogResult == DialogResult.Yes)
            {
                if (!podeExcluir)
                {
                    MessageBox.Show(mensagem, "Não pode excluir", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (Sessao.usuarioLogado.id == id)
                    {
                        dialogResult = MessageBox.Show("Deseja excluir a conta do sistema?", "Excluir", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (service.DeletarUsuario(id))
                            {
                                btnDelete.Enabled = false;
                                btnEditar.Enabled = false;
                                MessageBox.Show("Usuário deletado com sucesso", "Excluir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Application.Restart();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Operação cancelada");
                            btnDelete.Enabled = false;
                            btnEditar.Enabled = false;
                        }
                    
                    }
                    else
                    {
                        if (service.DeletarUsuario(id))
                        {
                            btnDelete.Enabled = false;
                            btnEditar.Enabled = false;
                            MessageBox.Show("Usuário deletado com sucesso", "Excluir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            listGrid();
                        }
                    }
                }
                
                
            }
            else
            {
                MessageBox.Show("Operação cancelada");
                btnDelete.Enabled = false;
                btnEditar.Enabled = false;
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
            Pessoa pessoa = new Usuario();
            ucHistoricoOS ucHistorico = new ucHistoricoOS(pessoa,id);
            ucHistorico.Left = (this.ClientSize.Width - ucHistorico.Width)/2;
            ucHistorico.Top = (this.ClientSize.Height - ucHistorico.Height)/2;
            this.Controls.Add(ucHistorico);
            
            ucHistorico.BringToFront();
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            
            relatorio.GerarRelatorioGeral(txtBusca.Text, cbInativo.Checked, cbNivel.SelectedIndex);
        }

        

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            var (sucesso, mensagem, caminho) = relatorio.GerarRelatorioTecnico(id);
            if (sucesso)
            {
                MessageBox.Show(mensagem);
            }
            else
            {
                MessageBox.Show(mensagem);
            }
            
           
        }

       
    }
}
    
