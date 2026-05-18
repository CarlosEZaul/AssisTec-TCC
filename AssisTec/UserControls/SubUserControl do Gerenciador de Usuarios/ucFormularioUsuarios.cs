using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mysqlx.Session;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Usuarios
{
    public partial class ucFormularioUsuarios : UserControl
    {
        
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int id;
        private string uf;
        private bool okCep;
        private int modo;
        private DataGridView dgv;
        private Usuario usuario;
        
        public ucFormularioUsuarios(int _id, int _modo,  DataGridView _dgv)
        {
            id = _id;
            modo = _modo;
            dgv = _dgv;
            InitializeComponent();
        }
        
        private void FormularioUsuarios_Load(object sender, EventArgs e)
        {
            ApplyModernDesign();
            ConfigurarComboBox();
            if (modo ==2)
            {
                carregarDados();
            }
            
        }
        
        # region Desing Moderno
        private void ApplyModernDesign()
        {
            try
            {
                this.Text = "Gerenciador de Usuários";
                this.BackColor = Color.FromArgb(39, 55, 76);

                // Labels
                // DesingComponentes.ApplyLabelStyles(this);

                // TextBox
                DesingComponentes.StyleTextBox(txtNome);
                DesingComponentes.StyleTextBox(txtSenha);
                DesingComponentes.StyleTextBox(txtRua);
                DesingComponentes.StyleTextBox(txtCidade);
                DesingComponentes.StyleTextBox(txtBairro);
                DesingComponentes.StyleTextBox(txtRua);
                DesingComponentes.StyleTextBox(txtNumber);
                DesingComponentes.StyleTextBox(txtEstado);
                DesingComponentes.StyleTextBox(txtRua);
                
                

                // MaskedTextBox
                DesingComponentes.StyleMaskedTextBox(mtbCPF);
                DesingComponentes.StyleMaskedTextBox(mtbCep);

                // Botões
                DesingComponentes.StyleButton(btnLimpar, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnFechar, Color.FromArgb(209, 17, 65));
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message);
            }
        }
        #endregion
        
        # region métodos ou funcoes
        private void ConfigurarComboBox()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Ativo");
            cbStatus.Items.Add("Desativado");
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            cbNivel.Items.Clear();
            
            cbNivel.Items.Add("1 - Gerente");
            cbNivel.Items.Add("2- Atendente");
            cbNivel.Items.Add("3 - Técnico de TI");
            
            cbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        private void deleteAll()
        {
            txtNome.Text = "";
            txtSenha.Text = "";
            mtbCPF.Text = "";
            mtbTel.Text = "";
            txtSenha.Text = "";
            mtbCep.Text = "";
            txtRua.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            txtComp.Text = "";
            txtNumber.Text = "";
            txtEstado.Text = "";
            cbStatus.SelectedText = null;
            cbNivel.SelectedText = null;
            
        }
        
        public void carregarDados()
        {
            int indexNivel;
            try
            {
                Usuario usuario = new Usuario();
                usuario = usuario.carregarDados(id);
                id = usuario.id;
                txtNome.Text = usuario.nome;
                mtbCPF.Text = usuario.cpf;
                txtSenha.Text = "********";
                mtbTel.Text = usuario.telefone;

                if (usuario.Nivel == 1)
                {
                    indexNivel = 0;
                }
                else if (usuario.Nivel == 2)
                {
                    indexNivel = 1;
                }
                else
                {
                    indexNivel = 2;
                }

                cbNivel.SelectedIndex = indexNivel;
                cbStatus.Text = usuario.Status;
                mtbCep.Text = usuario.cep;
                txtRua.Text = usuario.rua;
                txtNumber.Text = usuario.numero.ToString();
                txtCidade.Text = usuario.cidade;
                txtBairro.Text = usuario.bairro;
                txtEstado.Text = usuario.estado;
                txtComp.Text = usuario.complemento;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados do usuário: " + ex.Message);
            }

            con.CloseConnection();
            
            
        }
        
        Usuario formUsuario()
        {
            Usuario user = new Usuario();
            user.id = id;
            user.nome = txtNome.Text;
            user.cpf = mtbCPF.Text;
            user.telefone = mtbTel.Text;
            user.Senha = txtSenha.Text;
            
            if (cbNivel.SelectedItem != null)
            {
                string texto = cbNivel.SelectedItem.ToString();
                user.Nivel = int.Parse(texto.Split('-')[0].Trim());
            }
            
            if (cbStatus.SelectedItem != null)
            {
                string texto = cbStatus.SelectedItem.ToString();
                user.Status = texto;
            }

            user.Senha = txtSenha.Text;
            
            
            user.cep = mtbCep.Text;
            user.rua = txtRua.Text;
            user.numero = Convert.ToInt32(txtNumber.Text);
            user.cidade = txtCidade.Text;
            user.bairro = txtBairro.Text;
            user.estado = txtEstado.Text;
            user.complemento = txtComp.Text;

            return user;
        }
        

        
        private void fechar()
        {
            this.Dispose();
        }
        
        async Task BuscarCep(string cep)
        {
            try
            {
                
                Cursor = Cursors.WaitCursor;
                
                BuscaCEP buscaCep = new BuscaCEP();
                buscaCep.cep = cep;
                buscaCep.Consultar();
                
                txtCidade.Text = buscaCep.cidade;
                txtRua.Text = buscaCep.rua;
                txtBairro.Text = buscaCep.bairro;
                txtEstado.Text = buscaCep.estado;
                
                okCep = true;
                
                if (buscaCep.cidade == null && buscaCep.rua == null && buscaCep.bairro == null)
                {
                    MessageBox.Show("Falha ao localizar CEP!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    okCep = false;
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("CEP inválido!" + ex, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                okCep = false;
            }
            finally
            {
                
                Cursor = Cursors.Default;
            }
        }
        
        #endregion

        

        #region botões ou componentes

        private void mtbCep_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(mtbCep.Text) && mtbCep.Text.Replace("-", "").Length == 8)
            {
                BuscarCep(mtbCep.Text);
            }
        }
        
        

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if(string.IsNullOrEmpty(txtNome.Text)|| string.IsNullOrEmpty(txtSenha.Text)|| !mtbCPF.MaskFull|| !mtbTel.MaskFull ||
               string.IsNullOrEmpty(cbNivel.Text)|| string.IsNullOrEmpty(cbStatus.Text))
            {
                MessageBox.Show("Preencha todos os campos corretamente", "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtNome.Focus();
                return;
            }

            if (okCep == false)
            {
                MessageBox.Show("CEP inváldo", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                
                Usuario user = formUsuario();
                if (modo == 1)
                {

                    if (user.novoUsuario(user))
                    {
                        deleteAll();
                    }
                    dgv.DataSource = user.atualizarDados();
                    
                }

                if (modo == 2) 
                {

                    if (user.editarUsuario(user))
                    {
                        deleteAll();
                        fechar();
                    }
                    dgv.DataSource = user.atualizarDados();
                    
                }

                if (modo == 3)
                {
                    if (user.novoUsuario(user))
                    {
                        fechar();
                    }
                }
    
            
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerenciar usuários", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }
        

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            deleteAll();
        }

        #endregion


        private void btnFechar_Click(object sender, EventArgs e)
        {
            fechar();
        }
    }
}