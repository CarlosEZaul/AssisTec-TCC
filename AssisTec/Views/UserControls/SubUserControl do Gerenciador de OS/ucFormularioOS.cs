using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using AssisTec.Models;

using AssisTec.Repository;
using AssisTec.Service;
using MySql.Data.MySqlClient;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucFormularioOS : UserControl
    {
        
        private readonly OrdemServicoService _ordemServico;
        private readonly ClienteService _clienteService;
        private readonly UsuarioService _usuarioService;
        
        public ucFormularioOS(OrdemServicoService ordemServico, ClienteService clienteService, UsuarioService usuarioService)
        {
            InitializeComponent();
            _ordemServico = ordemServico ?? throw new ArgumentNullException(nameof(ordemServico));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            configurarComponentes();
        }

        private void configurarComponentes()
        {
            List<Usuario> tecnicos = _usuarioService.obterTodosTecnicos();
            cbTecnico.DataSource = null;
            cbTecnico.DisplayMember = "nome";
            cbTecnico.ValueMember = "Id";
            cbTecnico.DataSource = tecnicos;
            cbTecnico.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbTecnico.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbTecnico.DropDownStyle = ComboBoxStyle.DropDown;
            cbTecnico.SelectedIndex = -1;

            List<Cliente> clientes = _clienteService.ObterTodos();
            cbCliente.DataSource = null;
            cbCliente.DisplayMember = "nome";
            cbCliente.ValueMember = "Id";
            cbCliente.DataSource = clientes;
            cbCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbCliente.DropDownStyle = ComboBoxStyle.DropDown;
            cbCliente.SelectedIndex = -1;
        }
        

        private void LimparTxt()
        {
            txtDescricao.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtNdeSerie.Text = "";
            txtAcessorio.Text = "";
            cbEstado.Text = "";
            txtObservacoes.Text = "";
            txtProblemas.Text="";
        }
        
        
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cbCliente.Text) || string.IsNullOrWhiteSpace(cbTecnico.Text) ||
                    string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                    string.IsNullOrWhiteSpace(txtModelo.Text) ||
                    string.IsNullOrWhiteSpace(txtNdeSerie.Text) || string.IsNullOrWhiteSpace(txtAcessorio.Text) ||
                    string.IsNullOrWhiteSpace(cbEstado.Text) ||
                    string.IsNullOrWhiteSpace(txtObservacoes.Text) || string.IsNullOrWhiteSpace(txtProblemas.Text))
                {
                    MessageBox.Show("Preencha todos os campos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                OrdemServico  os = new OrdemServico();
                if (cbCliente.SelectedValue == null)
                {
                    MessageBox.Show("Cliente não selecionado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (cbTecnico.SelectedValue == null)
                {
                    MessageBox.Show("Técnico não selecionado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                os.Cliente.Id = Convert.ToInt32(cbCliente.SelectedValue);
                os.Tecnico.Id = Convert.ToInt32(cbTecnico.SelectedValue);
                os.Equipamento.Descricao = txtDescricao.Text;
                os.Equipamento.Marca = txtMarca.Text;
                os.Equipamento.Modelo = txtMarca.Text;
                os.Equipamento.estado_entrada = cbEstado.Text;
                os.Equipamento.Numero_Serie = txtNdeSerie.Text;
                os.Equipamento.acessorios = txtAcessorio.Text;
                os.problema_relatado = txtProblemas.Text;
                // os();
                // os.atualizarDados(dgvOS);
            }
            catch (Exception ex)
            {
                
            }
            

        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparTxt();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}