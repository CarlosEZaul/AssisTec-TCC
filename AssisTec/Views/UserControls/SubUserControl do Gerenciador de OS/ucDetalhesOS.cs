using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AssisTec.Models;
using AssisTec.Service;
using Exception = System.Exception;
using Font = System.Drawing.Font;
using Image = iTextSharp.text.Image;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucDetalhesOS : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly int _idOS;
        
        public ucDetalhesOS(int IdOS, OrdemServicoService ordemServicoService)
        {
            InitializeComponent();
            _idOS = IdOS;
            _ordemServicoService = ordemServicoService ??  throw new ArgumentNullException(nameof(ordemServicoService));
            ConfigurarComboBox();
            
            CarregarDetalhesOS();
            
        }

        
        

        #region Metodos de Manipulação de Dados

        private void ConfigurarComboBox()
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
        }
        public void CarregarDetalhesOS()
        {
            OrdemServico os = _ordemServicoService.ObterPorId(_idOS);
            try
            {
                txtId.Text = os.id_os.ToString();
                cbCliente.SelectedValue = os.id_cliente;
                cbTecnico.SelectedValue = os.id_tecnico;
                txtEquipamento.Text = os.Equipamento.Descricao;
                txtStatus.Text = os.status;
                txtDataAbertura.Text = os.data_abertura.ToString();
                txtUltimaAtualizacao.Text = os.data_atualizacao.ToString();
                txtValorMaoObra.Text = "R$ "+os.valor_mao_obra.ToString();
                txtValorPecas.Text = "R$ " + os.valor_pecas.ToString();
                txtValorTotal.Text = "R$ " + os.valor_total.ToString();
                txtProblema.Text = os.problema_relatado;
                txtObservacoes.Text = os.observacoes;
                txtDiagnostico.Text = os.diagnostico;

            }
            catch (Exception e)
            {
                throw new ArgumentNullException("Falha ao carregar OS");
            }
        }

        public void SalvarAlteracoes()
        {
            OrdemServico os =  _ordemServicoService.ObterPorId(_idOS);
            try
            {
                os.id_cliente = Convert.ToInt32(cbCliente.SelectedValue);
                os.id_tecnico = Convert.ToInt32(cbTecnico.SelectedValue);
                os.data_atualizacao = DateTime.Now;
                os.problema_relatado = txtProblema.Text;
                os.observacoes = txtObservacoes.Text;
                os.diagnostico = txtDiagnostico.Text;
                if (_ordemServicoService.SalvarAlteracoesOS(os))
                {
                    MessageBox.Show("Alteraçõse feitas com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception e)
            {
                throw new ArgumentNullException("Falha ao salvar altereções");
            }
        }

        

        #endregion
            
       
    }
}