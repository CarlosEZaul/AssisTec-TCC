using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.DTO;
using AssisTec.Models;
using AssisTec.Service;
using AssisTec.SubForms_do_Gerenciador_de_Pedidos;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque;

namespace AssisTec.UserControls
{
    public partial class ucHome : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly ProdutoService _produtoService;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private readonly ContasPagarService _contasPagarService;
        private readonly ContasReceberService  _contasReceberService;
        CultureInfo culturaBrasil = new CultureInfo("pt-BR");
        private LucroMesDTO _lucroMesDTO = new LucroMesDTO();

        public ucHome(OrdemServicoService ordemServicoService, ProdutoService produtoService, MovimentacaoEstoqueService movimentacaoEstoqueService, ContasPagarService contasPagarService, ContasReceberService contasReceberService)
        {
            InitializeComponent();
           
            _ordemServicoService = ordemServicoService ??  throw new ArgumentNullException(nameof(ordemServicoService));
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _movimentacaoEstoqueService =  movimentacaoEstoqueService ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            _contasPagarService = contasPagarService ?? throw new ArgumentNullException(nameof(contasPagarService));
            _contasReceberService = contasReceberService ?? throw new ArgumentNullException(nameof(contasReceberService));
            ConfigurarData();
            DesingModerno();
            AtualizarGrid();
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
            DesignComponentes.ArredondarPainel(panelBotoes,15,Color.FromArgb(50, 50, 50), 1 );
            DesignComponentes.AdicionarImagemNoBotao(btnOs, Properties.Resources.ordemServico);
            DesignComponentes.AdicionarImagemNoBotao(btnCliente, Properties.Resources.AdicionarEntidade);
            DesignComponentes.AdicionarImagemNoBotao(btnEntradaEstoque, Properties.Resources.EntradaEstoque);
            DesignComponentes.AdicionarImagemNoBotao(btnSaidaEstoque, Properties.Resources.SaidaEstoque);
            DesignComponentes.centralizarControl(tlpBotoes, panelBotoes.Width, panelBotoes.Top);
            


        }
        
        

        #endregion
        
        private void ConfigurarSubComponente(UserControl uc)
        {
            uc.Disposed += (s, e) => AtualizarGrid();
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Location = new Point((this.Width - uc.Width) / 2, (this.Height - uc.Height) / 2);
        }

        private void ConfigurarData()
        {
            string dia = DateTime.Now.Day.ToString();
            string mes = DateTime.Now.ToString("MMMM",  culturaBrasil);
            string ano = DateTime.Now.Year.ToString();
            string diaDaSemana = DateTime.Now.ToString("dddd", culturaBrasil).ToUpper();
            lblNome.Text = $"Bem-vindo de volta, {Sessao.usuarioLogado.Nome}";
            lblData.Text = $"{diaDaSemana}, {dia} De {mes} De {ano}";
        }

        private void ConfigurarCards()
        {
            lblOrdemServico.Text = _ordemServicoService.ObterQntOsAbertas().ToString();
            
            var (totalRecebido, totalPago, totalPagar, lucroLiquido) = _lucroMesDTO.ObterLucroDoMes(DateTime.Now.Month, DateTime.Now.Year);
            
            lblFaturamento.Text = lucroLiquido.ToString("C", culturaBrasil);
            lblContaPagar.Text = totalPagar.ToString("C", culturaBrasil);

            var abaixoMinimo = _produtoService.obterTotais().abaixoMinimo;
            lblMinimo.Text = abaixoMinimo.ToString();
        }

        private void AtualizarGrid()
        {
            ConfigurarCards();
            dgvOS.DataSource = _ordemServicoService.OrdensRecentes();
            dataGridView1.DataSource = _produtoService.ProdutosAbaixoMinimo();
        }
        
        private bool ValidarAcessoEstoque()
        {
            if (Sessao.usuarioLogado != null && Sessao.usuarioLogado.Nivel == 3)
            {
                MessageBox.Show("Acesso Negado! Técnicos não possuem permissão para gerenciar o módulo de Estoque.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }


        private void btnOs_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucFormularioOS(_ordemServicoService));
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucFormulario_Clientes(1,null));
        }

        private void btnEntradaEstoque_Click(object sender, EventArgs e)
        {
            if (!ValidarAcessoEstoque())
            {
                return;
            }
            ConfigurarSubComponente(new ucRegistrarEntrada( _produtoService, _movimentacaoEstoqueService, _contasPagarService));
            
        }

        private void btnSaidaEstoque_Click(object sender, EventArgs e)
        {
            if (!ValidarAcessoEstoque())
            {
                return;
            }
            ConfigurarSubComponente(new ucRegistrarSaida( _produtoService, _movimentacaoEstoqueService, _contasReceberService));
            
        }
    }
}