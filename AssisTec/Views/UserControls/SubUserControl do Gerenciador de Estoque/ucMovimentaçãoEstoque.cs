using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucMovimentaçãoEstoque : UserControl
    {
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private readonly ProdutoService _produtoService;
        public ucMovimentaçãoEstoque(MovimentacaoEstoqueService  movimentacaoEstoqueService, ProdutoService produtoService)
        {
            InitializeComponent();
            _movimentacaoEstoqueService = movimentacaoEstoqueService ??  throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            DesingModerno();
            ConfigurarComponentes();
            AtualizarGrid();
        }

        #region Funções

        private void DesingModerno()
        {
            DesignComponentes.centralizarPanel(panelBotoes, this.Width);
            DesignComponentes.StyleDataGridView(dgvMovimentacao,DataGridViewAutoSizeColumnsMode.Fill);
            DesignComponentes.StyleButton(btnFechar, Color.Red);
        }

        private void ConfigurarComponentes()
        {
            var produtos = _produtoService.obterDescricaoProdutos() as List<object>;

            if (produtos == null)
            {
                var listaGenerica = _produtoService.obterDescricaoProdutos() as System.Collections.IEnumerable;
                if (listaGenerica != null)
                {
                    produtos = new List<object>();
                    foreach (var item in listaGenerica)
                    {
                        produtos.Add(item);
                    }
                }
            }

            if (produtos != null)
            {
                var listaExibicao = new List<object>();
        
                listaExibicao.Add(new { Produto = "Todos" });
        
                listaExibicao.AddRange(produtos);

                cbProduto.DisplayMember = "Produto";
                cbProduto.DataSource = listaExibicao;
            }
            
            cbProduto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbProduto.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbProduto.DropDownStyle = ComboBoxStyle.DropDown;

            cbTipoMovimentação.Items.Add("Todos");
            cbTipoMovimentação.Items.Add("ENTRADA");
            cbTipoMovimentação.Items.Add("SAIDA");
            cbTipoMovimentação.SelectedIndex = 0;
        }

        private void AtualizarGrid()
        {
            dgvMovimentacao.DataSource = _movimentacaoEstoqueService.ListarMovimentacaoEstoque();
            FormatGrid();
        }

        private void FormatGrid()
        {
            
            if (dgvMovimentacao.Columns.Count <= 0) return;

            dgvMovimentacao.Columns[0].HeaderText = "ID_Movimentação";
            dgvMovimentacao.Columns[1].HeaderText = "ID_Produto";
            dgvMovimentacao.Columns[2].HeaderText = "Data";
            dgvMovimentacao.Columns[3].HeaderText = "Quantidade";
            dgvMovimentacao.Columns[4].HeaderText = "Valor";
            dgvMovimentacao.Columns[5].HeaderText = "Descrição";
            dgvMovimentacao.Columns[6].HeaderText = "Tipo da Movimentação";
        }

        private void AplicarFiltro()
        {
            try
            {
                DateTime? dataInicio = null;
                DateTime? dataFim = null;

                if (DateTime.TryParse(mtbDataInicio.Text, out DateTime dtIni))
                {
                    dataInicio = dtIni;
                }

                if (DateTime.TryParse(mtbDataFim.Text, out DateTime dtFim))
                {
                    dataFim = dtFim;
                }

                string produtoSelecionado = cbProduto.Text;
                string tipoMovimentacao = cbTipoMovimentação.Text;

                dgvMovimentacao.DataSource = _movimentacaoEstoqueService.Filtrar(dataInicio, dataFim, produtoSelecionado, tipoMovimentacao);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar o filtro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        
        

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            cbProduto.SelectedIndex = 0;
            mtbDataInicio.Text = string.Empty;
            mtbDataFim.Text = string.Empty;
            AplicarFiltro();
            AtualizarGrid();
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                
                DateTime? dataInicio = null;
                DateTime? dataFim = null;

                if (DateTime.TryParse(mtbDataInicio.Text, out DateTime dtIni))
                {
                    dataInicio = dtIni;
                }

                if (DateTime.TryParse(mtbDataFim.Text, out DateTime dtFim))
                {
                    dataFim = dtFim;
                }

                string produtoSelecionado = cbProduto.Text;
                string tipoMovimentacao = cbTipoMovimentação.Text;

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    saveFileDialog.FileName = "Relatorio_Movimentacoes_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";
                    saveFileDialog.Title = "Relatorio Movimentacoes";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _movimentacaoEstoqueService.GerarRelatorioPdf(dataInicio, dataFim, produtoSelecionado, tipoMovimentacao,  saveFileDialog.FileName);
                        MessageBox.Show("Relatório gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
                
            }
            catch (Exception exception)
            {
                MessageBox.Show("Falha ao gerar relatório: " + exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}