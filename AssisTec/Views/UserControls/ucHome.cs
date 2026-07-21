using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.DTO;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.UserControls
{
    public partial class ucHome : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly ProdutoService _produtoService;
        CultureInfo culturaBrasil = new CultureInfo("pt-BR");
        private LucroMesDTO _lucroMesDTO = new LucroMesDTO();

        public ucHome(OrdemServicoService  ordemServicoService, ProdutoService produtoService)
        {
            InitializeComponent();
           
            _ordemServicoService = ordemServicoService ??  throw new ArgumentNullException(nameof(ordemServicoService));
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            ConfigurarComponentes();
            DesingModerno();
            listGrid();
        }

        #region DesingModerno

        private void DesingModerno()
        {
            
            
            //Ordens de servico
            DesignComponentes.ApplyLabelStyles(lblOsRecentes);
            DesignComponentes.StyleDataGridView(dgvOS);
            DesignComponentes.ArredondarPainel(panel6, 15, Color.FromArgb(50, 50, 50), 1);
            DesignComponentes.RecortarCantosDataGridView(dgvOS, 15);
            DesignComponentes.centralizarWidthControl(dgvOS, panel6.Width);
            

            //Estoque
            DesignComponentes.StyleDataGridView(dataGridView1);
            DesignComponentes.RecortarCantosDataGridView(dataGridView1,15);
            DesignComponentes.ArredondarPainel(panel7, 15, Color.FromArgb(50, 50, 50), 1);
            DesignComponentes.centralizarWidthControl(dataGridView1, panel7.Width);
            DesignComponentes.AdicionarImagemNaLabel(lblEstoque, Properties.Resources.abaixo_minimo);
            DesignComponentes.centralizarWidthControl(lblEstoque, panel7.Width);
            
            //Botoes
            DesignComponentes.AdicionarImagemNoBotao(btnOs, Properties.Resources.ordemServico);
            DesignComponentes.AdicionarImagemNoBotao(btnCliente, Properties.Resources.AdicionarEntidade);
            DesignComponentes.AdicionarImagemNoBotao(btnEntradaEstoque, Properties.Resources.EntradaEstoque);
            DesignComponentes.AdicionarImagemNoBotao(btnSaidaEstoque, Properties.Resources.SaidaEstoque);
            DesignComponentes.centralizarControl(tlpBotoes, panelBotoes.Width, panelBotoes.Top);
            


        }
        
        

        #endregion

        private void ConfigurarComponentes()
        {
            string dia = DateTime.Now.Day.ToString();
            string mes = DateTime.Now.ToString("MMMM",  culturaBrasil);
            string ano = DateTime.Now.Year.ToString();
            string diaDaSemana = DateTime.Now.ToString("dddd", culturaBrasil).ToUpper();
            lblNome.Text = $"Bem-vindo de volta, {Sessao.usuarioLogado.Nome}";
            lblData.Text = $"{diaDaSemana}, {dia} De {mes} De {ano}";

            lblOrdemServico.Text = _ordemServicoService.obterOsAbertas().ToString();
            
            var (totalRecebido, totalPago, totalPagar, lucroLiquido) = _lucroMesDTO.ObterLucroDoMes(DateTime.Now.Month, DateTime.Now.Year);
            
            lblFaturamento.Text = lucroLiquido.ToString("C", culturaBrasil);
            lblContaPagar.Text = totalPagar.ToString("C", culturaBrasil);

            var abaixoMinimo = _produtoService.obterTotais().abaixoMinimo;
            lblMinimo.Text = abaixoMinimo.ToString();
        }

        private void listGrid()
        {
            dgvOS.DataSource = _ordemServicoService.OrdensRecentes();
            dataGridView1.DataSource = _produtoService.ProdutosAbaixoMinimo();
        }


        private void btnOs_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}