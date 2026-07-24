using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using AssisTec.Models;

using AssisTec.Repository;
using AssisTec.Service;
using MySql.Data.MySqlClient;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucFormularioOS : UserControl
    {
        
        private readonly OrdemServicoService _ordemServicoService;
        
        public ucFormularioOS(OrdemServicoService ordemServico)
        {
            InitializeComponent();
            _ordemServicoService = ordemServico ?? throw new ArgumentNullException(nameof(ordemServico));
            configurarComboBox();
        }

        private void configurarComboBox()
        {
            List<Usuario> tecnicos = _ordemServicoService.ObterTecnicosAtivados();
            cbTecnico.DataSource = null;
            cbTecnico.DisplayMember = "nome";
            cbTecnico.ValueMember = "Id";
            cbTecnico.DataSource = tecnicos;
            cbTecnico.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbTecnico.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbTecnico.DropDownStyle = ComboBoxStyle.DropDown;
            cbTecnico.SelectedIndex = -1;

            List<Cliente> clientes = _ordemServicoService.ObterClientes().Where(c=> c.Status == "Ativado").ToList();
            cbCliente.DataSource = null;
            cbCliente.DisplayMember = "nome";
            cbCliente.ValueMember = "Id";
            cbCliente.DataSource = clientes;
            cbCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbCliente.DropDownStyle = ComboBoxStyle.DropDown;
            cbCliente.SelectedIndex = -1;

            cbEstado.Items.Add("Perfeito");
            cbEstado.Items.Add("Marcas de Uso");
            cbEstado.Items.Add("Danificado");
            cbEstado.Items.Add("Incompleto");
        }
        

        private void LimparTxt()
        {
            cbTecnico.SelectedIndex = -1;
            cbCliente.SelectedIndex = -1;
            txtDescricao.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtNdeSerie.Text = "";
            txtAcessorio.Text = "";
            cbEstado.SelectedIndex = -1;
            txtObservacoes.Text = "";
            txtProblemas.Text="";
        }
        
        
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Equipamento equipamento = new Equipamento
                {
                    Descricao = txtDescricao.Text.Trim(),
                    Marca = txtMarca.Text.Trim(),
                    Modelo = txtModelo.Text.Trim(),
                    Numero_Serie = txtNdeSerie.Text.Trim(),
                    estado_entrada = cbEstado.SelectedValue?.ToString() ?? cbEstado.Text,
                    acessorios = txtAcessorio.Text.Trim(),
                    Observacoes = txtObservacoes.Text.Trim()
                };

                OrdemServico os = new OrdemServico
                {
                    id_cliente = cbCliente.SelectedValue != null ? Convert.ToInt32(cbCliente.SelectedValue) : (int?)null,
                    id_tecnico = cbTecnico.SelectedValue != null ? Convert.ToInt32(cbTecnico.SelectedValue) : (int?)null,
                    problema_relatado = txtProblemas.Text.Trim()
                };

                if (_ordemServicoService.SalvarOS(os, equipamento))
                {
                    MessageBox.Show("Ordem de Serviço salva com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar os dados: " , "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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