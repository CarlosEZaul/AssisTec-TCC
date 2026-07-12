using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque;

namespace AssisTec.UserControls
{
    public partial class ucGerenciadorEstoque : UserControl
    {
        private readonly List<Label> _listaLabelsTotais;
        private readonly ProdutoService _service;
        private readonly ContasPagarService _contasPagarService;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private int idProduto;
        public ucGerenciadorEstoque(ProdutoService service, MovimentacaoEstoqueService movimentacaoEstoqueService , ContasPagarService contasPagarService)
        {
            InitializeComponent();
            _service = service;
            _contasPagarService = contasPagarService;
            _movimentacaoEstoqueService = movimentacaoEstoqueService;
            DesingModerno();
            _listaLabelsTotais = new List<Label> { lblProdutosCadastrados, lblMinimo, lblSemEstoque, lblValorEstoque };
            AtualizarGrid();
        }

        #region DesingModerno

        private void DesingModerno()
        {
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.StyleDataGridView(dgvEstoque, DataGridViewAutoSizeColumnsMode.Fill);
        }
        

        #endregion
        private void FormatGrid()
        {
            if (dgvEstoque.Columns.Count <= 0) return;

            dgvEstoque.Columns[0].HeaderText = "ID_PRODUTO";
            dgvEstoque.Columns[1].HeaderText = "Descricao";
            dgvEstoque.Columns[2].HeaderText = "Unidade";
            dgvEstoque.Columns[3].HeaderText = "Preço de Venda";
            dgvEstoque.Columns[4].HeaderText = "Preço de Compra";
            dgvEstoque.Columns[5].HeaderText = "Quantidade";
            dgvEstoque.Columns[6].HeaderText = "Quantidade Minima";
        }

        
        private void AtualizarGrid()
        {
            dgvEstoque.DataSource = _service.ObterProdutos();

            var totais = _service.obterTotais();
            _listaLabelsTotais[0].Text = totais.totalCadastrado.ToString();
            _listaLabelsTotais[1].Text = totais.abaixoMinimo.ToString();
            _listaLabelsTotais[2].Text = totais.semEstoque.ToString();
            _listaLabelsTotais[3].Text = totais.valorEstoque.ToString();

            idProduto = 0;
            MudarEstadoBotoes(false);
            FormatGrid();
            
        }
        private void MudarEstadoBotoes(bool ativo)
        {
            btnEditar.Enabled = ativo;
            btnStatus.Enabled = ativo;
            btnEntrada.Enabled = ativo;
            btnSaida.Enabled = ativo;
        }
        private void ConfigurarSubComponente(UserControl uc)
        {
            uc.Disposed += (s, e) => AtualizarGrid();
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Location = new Point((this.Width - uc.Width) / 2, (this.Height - uc.Height) / 2);
        }

        #region Funções dos botões
        private void btnNew_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucFormularioProduto(idProduto, 1 , _service, _contasPagarService,_movimentacaoEstoqueService));           
        }
        

        #endregion

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucFormularioProduto(idProduto, 2 , _service, _contasPagarService,_movimentacaoEstoqueService));
        }

        private void dgvEstoque_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var grid = dgvEstoque;
            var colunas = new[] { "IdProduto", "ID_PRODUTO" };

            foreach (var nomeColuna in colunas)
            {
                if (!grid.Columns.Contains(nomeColuna)) continue;

                var valor = grid.Rows[e.RowIndex].Cells[nomeColuna].Value;
                if (valor != null && valor != DBNull.Value && int.TryParse(valor.ToString(), out int id))
                {
                    idProduto = id;
                    MudarEstadoBotoes(true);
                    return;
                }
            }

            idProduto = 0;
            MudarEstadoBotoes(false);
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            if (_service.ObterProdutoPorId(idProduto).status == "Ativado")
            {
                DialogResult result = MessageBox.Show("Deseja desativar o produto ?", "Desativar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _service.alterarStatus(idProduto);
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Deseja ativar o produto ?", "Ativar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _service.alterarStatus(idProduto);
                }
            }
            AtualizarGrid();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void btnEntrada_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucRegistrarEntrada(idProduto, _service, _movimentacaoEstoqueService));
        }
    }
}