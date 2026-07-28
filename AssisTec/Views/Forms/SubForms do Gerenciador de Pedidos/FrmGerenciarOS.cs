using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.AtendeClienteService;
using AssisTec.SubForms_do_Gerenciador_de_Pedidos;
using MySql.Data.MySqlClient;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;
using AssisTec.Models;
using AssisTec.Service;
using AssisTec.Views.UserControls.SubUserControl_do_Gerenciador_de_OS;
using Exception = System.Exception;
using Font = System.Drawing.Font;
using Image = iTextSharp.text.Image;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class FrmGerenciarOS : Form
    {
        
        
        private readonly OrdemServicoService _ordemServicoService;
        private int _id;
        private ucDetalhesOS detalhes;
        private ucProdutosUtilizados produtos;
        private ucServicos servicos;
        public FrmGerenciarOS(int id,OrdemServicoService ordemServicoService)
        {
            InitializeComponent();
            _ordemServicoService = ordemServicoService ?? throw new ArgumentNullException(nameof(ordemServicoService));
            _id = id;
            IniciarUserControls();
            ApplyModernDesign();
            ConfigurarBotaoStatus(_ordemServicoService.ObterPorId(id));

        }

        
        private void FrmGerenciarOS_Load(object sender, EventArgs e)
        {
            MostrarTela(detalhes);
        }

        
        #region Desing Moderno
        

        private void ApplyModernDesign()
        {
            try
            {
                this.BackColor = Color.FromArgb(240, 240, 240);
                this.Font = new Font("Segoe UI", 9F);
                panel2.BackColor = Color.FromArgb(32, 45, 64);
                
                DesignComponentes.StyleButton(btnImprimir, Color.FromArgb(0, 120, 215));
                DesignComponentes.StyleButton(btnFechar, Color.FromArgb(209, 17, 65));
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Métodos ou funcoes
        private void IniciarUserControls()
        {
            detalhes = new ucDetalhesOS(_id, _ordemServicoService);
            produtos = new ucProdutosUtilizados(_ordemServicoService, _id);
            servicos = new ucServicos(_ordemServicoService, _id);
            
            detalhes.Dock = DockStyle.Fill;
            produtos.Dock = DockStyle.Fill;
            servicos.Dock = DockStyle.Fill;

            panelConteudo.Controls.Add(detalhes);
            panelConteudo.Controls.Add(produtos);
            panelConteudo.Controls.Add(servicos);
        }

        private void ConfigurarBotaoStatus(OrdemServico ordemServico)
        {
            if (ordemServico != null)
            {
                if (ordemServico.status == "ABERTA")
                {
                    btnStatus.Text = "Cancelar OS";
                }

                if (ordemServico.status == "CANCELADA")
                {
                    btnStatus.Text = "Reabrir OS";
                }
            }
        }
        
        
        private void MostrarTela(UserControl tela)
        {
            if (tela == null) return;

            foreach (Control ctrl in panelConteudo.Controls)
                ctrl.Visible = false;

            tela.Visible = true;
            tela.BringToFront();
            Atualizar();
        }

        private void MudarVisibilidadeBotoes(bool ativo)
        {
            btnSalvar.Visible = ativo;
        }
        public void Atualizar()
        {
            detalhes.CarregarDetalhesOS();
            produtos.AtualizarDados();
            servicos.AtualizarDados();
        }
        

        #endregion

        #region Funções dos componentes
        
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            
        }

        

        private void btnDetalhes_Click(object sender, EventArgs e)
        {
            MudarVisibilidadeBotoes(true);
            MostrarTela(detalhes);
            
        }


        private void btnProdutos_Click(object sender, EventArgs e)
        {
            MudarVisibilidadeBotoes(false);
            produtos.AtualizarDados();
            MostrarTela(produtos);
        }

        private void btnServiços_Click(object sender, EventArgs e)
        {
            MudarVisibilidadeBotoes(false);
            MostrarTela(servicos);
        }

        


        private void btnDesfazer_Click(object sender, EventArgs e)
        {
            Atualizar();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            detalhes.SalvarAlteracoes();
        }
        #endregion


        

        private void btnStatus_Click(object sender, EventArgs e)
        {
            if (_id <= 0)
            {
                MessageBox.Show("Nenhuma Ordem de Serviço selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var ordem = _ordemServicoService.ObterPorId(_id);
                if (ordem == null)
                {
                    MessageBox.Show("Ordem de Serviço não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idTecnico = ordem.id_tecnico.GetValueOrDefault();
                bool isCancelada = ordem.status == "CANCELADA";

                string acao = isCancelada ? "reabrir" : "cancelar";
                
                string mensagemConfirmacao = isCancelada
                    ? $"Deseja realmente reabrir a OS #{_id}?\nOs produtos serão debitados do estoque novamente."
                    : $"Deseja realmente cancelar a OS #{_id}?\nOs produtos utilizados serão devolvidos ao estoque.";

                var confirmacao = MessageBox.Show(
                    mensagemConfirmacao,
                    $"Confirmação de {char.ToUpper(acao[0]) + acao.Substring(1)}",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmacao == DialogResult.No) return;

                bool sucesso = isCancelada
                    ? _ordemServicoService.ReabrirOrdemServico(_id, idTecnico)
                    : _ordemServicoService.CancelarOrdemServico(_id, idTecnico);

                if (sucesso)
                {
                    MessageBox.Show($"Ordem de Serviço {acao}da com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.FindForm()?.Close();
                }
                else
                {
                    MessageBox.Show($"Não foi possível {acao} a Ordem de Serviço.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao processar a solicitação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}