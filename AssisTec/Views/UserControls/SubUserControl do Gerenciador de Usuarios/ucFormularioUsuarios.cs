using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using AssisTec.Service;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Usuarios
{
    public partial class ucFormularioUsuarios : UserControl
    {
        private int id;
        private int modo;
        private bool okCep;
        private readonly DataGridView dgv;
        private UsuarioService service;
        
        public ucFormularioUsuarios(int _id, int _modo, DataGridView _dgv)
        {
            this.modo = _modo;
            this.dgv = _dgv;
            if (modo != 1)
            {
                this.id = _id;
            }
            InitializeComponent();
            
            CriarNovoContexto();
        }
        
        private void CriarNovoContexto()
        {
            this.service = new UsuarioService(new UsuarioRepository(new AppDbContext()));
        }
        
        private void FormularioUsuarios_Load(object sender, EventArgs e)
        {
            ApplyModernDesign();
            ConfigurarComboBox();
            
            if (modo == 2)
            {
                CarregarDados();
            }

            if (modo == 3)
            {
                cbNivel.SelectedIndex = 0;
                cbNivel.Enabled = false;
                
                cbStatus.SelectedIndex = 0;
                cbStatus.Enabled = false;
            }
        }
        
        #region Design Moderno
        private void ApplyModernDesign()
        {
            try
            {
                this.Text = "Gerenciador de Usuários";
                this.BackColor = Color.FromArgb(39, 55, 76);

                DesingComponentes.StyleTextBox(txtNome);
                DesingComponentes.StyleTextBox(txtSenha);
                DesingComponentes.StyleTextBox(txtRua);
                DesingComponentes.StyleTextBox(txtCidade);
                DesingComponentes.StyleTextBox(txtBairro);
                DesingComponentes.StyleTextBox(txtNumber);
                DesingComponentes.StyleTextBox(txtEstado);
                DesingComponentes.StyleTextBox(txtComp);
                
                DesingComponentes.StyleMaskedTextBox(mtbCPF);
                DesingComponentes.StyleMaskedTextBox(mtbCep);
                DesingComponentes.StyleMaskedTextBox(mtbTel);

                DesingComponentes.StyleButton(btnLimpar, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnFechar, Color.FromArgb(209, 17, 65));
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
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Ativo");
            cbStatus.Items.Add("Desativado");
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            cbNivel.Items.Clear();
            cbNivel.Items.Add("1 - Gerente");
            cbNivel.Items.Add("2 - Atendente");
            cbNivel.Items.Add("3 - Técnico de TI");
            cbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        private void DeleteAll()
        {
            txtNome.Text = string.Empty;
            txtSenha.Text = string.Empty;
            mtbCPF.Text = string.Empty;
            mtbTel.Text = string.Empty;
            mtbCep.Text = string.Empty;
            txtRua.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtCidade.Text = string.Empty;
            txtComp.Text = string.Empty;
            txtNumber.Text = string.Empty;
            txtEstado.Text = string.Empty;
            cbStatus.SelectedIndex = -1;
            cbNivel.SelectedIndex = -1;
            okCep = false;
        }
        
        public void CarregarDados()
        {
            try
            {
                Usuario usuario = service.ObterPorId(id);
                if (usuario == null)
                {
                    MessageBox.Show("Usuário não encontrado.");
                    Fechar();
                    return;
                }

                id = usuario.Id;
                txtNome.Text = usuario.Nome;
                mtbCPF.Text = usuario.Cpf;
                txtSenha.Text = string.Empty; 
                mtbTel.Text = usuario.Telefone;
                int indexNivel = (usuario.Nivel >= 1 && usuario.Nivel <= 3) ? usuario.Nivel - 1 : 1;
                cbNivel.SelectedIndex = indexNivel;
                cbStatus.Text = usuario.Status;
                mtbCep.Text = usuario.Cep;
                okCep = true;
                txtRua.Text = usuario.Rua;
                txtNumber.Text = usuario.Numero;
                txtCidade.Text = usuario.Cidade;
                txtBairro.Text = usuario.Bairro;
                txtEstado.Text = usuario.Estado;
                txtComp.Text = usuario.Complemento;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados do usuário: " + ex.Message);
            }
        }
        
        private Usuario FormUsuario()
        {
            Usuario user = new Usuario();
            user.Id = id;
            user.Nome = txtNome.Text;
            user.Cpf = mtbCPF.Text;
            user.Telefone = mtbTel.Text;
            user.Senha = txtSenha.Text;
            
            if (cbNivel.SelectedItem != null)
            {
                string texto = cbNivel.SelectedItem.ToString();
                user.Nivel = int.Parse(texto.Split('-')[0].Trim());
            }
            
            if (cbStatus.SelectedItem != null)
            {
                user.Status = cbStatus.SelectedItem.ToString();
            }
            
            user.Cep = mtbCep.Text;
            user.Rua = txtRua.Text;
            user.Numero = txtNumber.Text;
            user.Cidade = txtCidade.Text;
            user.Bairro = txtBairro.Text;
            user.Estado = txtEstado.Text;
            user.Complemento = txtComp.Text;

            return user;
        }
        
        private void Fechar()
        {
            this.Dispose();
        }
        #endregion

        #region Eventos dos Componentes
        private async void mtbCep_Leave(object sender, EventArgs e)
        {
            string cepLimpo = mtbCep.Text.Replace("-", "").Replace("_", "").Trim();
            if (!string.IsNullOrWhiteSpace(cepLimpo) && cepLimpo.Length == 8)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    var resultado = await service.ConsultarCepAsync(mtbCep.Text);

                    if (resultado.sucesso)
                    {
                        txtCidade.Text = resultado.cidade;
                        txtRua.Text = resultado.rua;
                        txtBairro.Text = resultado.bairro;
                        txtEstado.Text = resultado.estado;
                        okCep = true;
                    }
                    else
                    {
                        MessageBox.Show("Falha ao localizar CEP ou formato inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        okCep = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao buscar CEP: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    okCep = false;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNome.Text) || !mtbCPF.MaskFull || 
                !mtbTel.MaskFull || !mtbCep.MaskFull || string.IsNullOrEmpty(cbNivel.Text) || string.IsNullOrEmpty(cbStatus.Text))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios corretamente", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((modo == 1 || modo == 3) && string.IsNullOrEmpty(txtSenha.Text))
            {
                MessageBox.Show("A senha é obrigatória para novos usuários", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenha.Focus();
                return;
            }

            if (!okCep)
            {
                MessageBox.Show("CEP inválido ou não verificado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Usuario user = FormUsuario();

                if (modo == 1 || modo == 3)
                {
                    var resultado = service.CadastrarUsuario(user);
                    if (resultado.sucesso)
                    {
                        MessageBox.Show(resultado.messagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DeleteAll();
                        Fechar();
                    }
                    else
                    {
                        MessageBox.Show(resultado.messagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (modo == 2) 
                {
                    var resultado = service.EditarUsuario(user);
                    if (resultado.sucesso)
                    {
                        MessageBox.Show(resultado.mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DeleteAll();
                        Fechar();
                    }
                    else
                    {
                        MessageBox.Show(resultado.mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar a operação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            DeleteAll();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Fechar();
        }
        #endregion
    }
}