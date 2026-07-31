using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucProdutosUtilizados : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly OrdemServico _ordemServico;
        private int _idProduto;
        private Produto _produto;

        public ucProdutosUtilizados()
        {
            InitializeComponent();
            this.Load += ucProdutosUtilizados_Load;
            DesignModerno();
        }

        public ucProdutosUtilizados(OrdemServicoService ordemServicoService, int idOrdemServico) : this()
        {
            _ordemServicoService = ordemServicoService ?? throw new ArgumentNullException(nameof(ordemServicoService));
            _ordemServico = _ordemServicoService.ObterPorId(idOrdemServico);
            ConfigurarCampoValor();
        }

        private void DesignModerno()
        {
            DesignComponentes.StyleDataGridView(dgvItensOS);
            dgvItensOS.ReadOnly = true;
            dgvItensOS.AllowUserToAddRows = false;
            dgvItensOS.AllowUserToDeleteRows = false;
            dgvItensOS.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void ucProdutosUtilizados_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            if (_ordemServicoService != null)
            {
                CarregarProdutos();
                if (_ordemServico != null && _ordemServico.id_os > 0)
                {
                    CarregarItensGrid(); 
                }
            }
        }

        public void AtualizarDados()
        {
            CarregarProdutos();
            if (_ordemServico != null && _ordemServico.id_os > 0)
            {
                CarregarItensGrid();
            }
        }

        private void CarregarProdutos()
        {
            try
            {
                var produtos = _ordemServicoService.ObterProdutos();

                cbProduto.SelectedIndexChanged -= cbProduto_SelectedIndexChanged;

                cbProduto.DataSource = null;
                cbProduto.DisplayMember = "descricao";
                cbProduto.ValueMember = "idProduto";
                cbProduto.DataSource = produtos;

                cbProduto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbProduto.AutoCompleteSource = AutoCompleteSource.ListItems;
                cbProduto.DropDownStyle = ComboBoxStyle.DropDown;

                if (_idProduto > 0)
                {
                    cbProduto.SelectedValue = _idProduto;
                }
                else
                {
                    cbProduto.SelectedIndex = -1;
                }

                cbProduto.SelectedIndexChanged += cbProduto_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar lista de produtos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarItensGrid()
        {
            if (_ordemServico == null || _ordemServico.id_os <= 0) return;

            try
            {
                var listaItens = _ordemServicoService.ObterItensDaOS(_ordemServico.id_os).ToList();

                dgvItensOS.DataSource = null;
                dgvItensOS.AutoGenerateColumns = true;
                dgvItensOS.DataSource = listaItens;
                dgvItensOS.Refresh();

                FormatadorGrid();
                CalcularTotalOS(listaItens);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar itens da OS: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatadorGrid()
        {
            if (dgvItensOS.Columns["Id"] != null)
                dgvItensOS.Columns["Id"].Visible = false;

            if (dgvItensOS.Columns["id_produto"] != null)
                dgvItensOS.Columns["id_produto"].Visible = false;

            if (dgvItensOS.Columns["ValorUnitario"] != null)
                dgvItensOS.Columns["ValorUnitario"].DefaultCellStyle.Format = "C2";

            if (dgvItensOS.Columns["ValorTotal"] != null)
                dgvItensOS.Columns["ValorTotal"].DefaultCellStyle.Format = "C2";
        }

        private void CalcularTotalOS(List<dynamic> itens)
        {
            decimal totalGeral = 0;

            if (itens != null)
            {
                foreach (var item in itens)
                {
                    totalGeral += (decimal)item.ValorTotal;
                }
            }

            lblTotal.Text = totalGeral.ToString("C2");
        }

        private int ConverterParaInt(object valor)
        {
            if (valor == null || valor == DBNull.Value) return 0;
            if (int.TryParse(valor.ToString(), out int resultado)) return resultado;
            return 0;
        }

        private void txtQntd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtQntd_TextChanged(object sender, EventArgs e)
        {
            CalcularValorTotal();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (_idProduto <= 0 || _produto == null)
            {
                MessageBox.Show("Selecione um produto válido da lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQntd.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Informe uma quantidade válida maior que zero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool sucesso = _ordemServicoService.AdicionarOuAtualizarItemOS(_ordemServico, _idProduto, quantidade);
                
                var historico = new HistoricoAlteracaoOS
                {
                    idOS = _ordemServico.id_os,
                    idUsuario = _ordemServico.id_tecnico.GetValueOrDefault(),
                    tipo = "INCLUSAO_PRODUTO",
                    descricao = $"Adicionado {quantidade} do produto {_produto.descricao} na Ordem de Serviço",
                    dataAlteracao = DateTime.Now
                };

                if (sucesso)
                {
                    _ordemServicoService.RegistrarHistoricoOS(historico);
                    MessageBox.Show("Produto processado com sucesso na Ordem de Serviço!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparSelecaoProduto();
                    CarregarProdutos();
                    CarregarItensGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (dgvItensOS.CurrentRow == null || dgvItensOS.CurrentRow.Index < 0 || dgvItensOS.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Selecione um item da tabela para remover.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idItem = 0;

            if (dgvItensOS.Columns.Contains("Id") && dgvItensOS.CurrentRow.Cells["Id"].Value != null)
            {
                idItem = ConverterParaInt(dgvItensOS.CurrentRow.Cells["Id"].Value);
            }

            if (idItem <= 0)
            {
                MessageBox.Show("Não foi possível identificar o ID do item selecionado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtQntd.Text, out int qtdRemover) || qtdRemover <= 0)
            {
                MessageBox.Show("Informe uma quantidade válida a ser removida no campo de Quantidade.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string nomeProduto = _produto != null ? _produto.descricao : "Produto";

                bool sucesso = _ordemServicoService.ReduzirOuRemoverItemOS(idItem, qtdRemover);
                if (sucesso)
                {
                    var historico = new HistoricoAlteracaoOS
                    {
                        idOS = _ordemServico.id_os,
                        idUsuario = _ordemServico.id_tecnico.GetValueOrDefault(),
                        tipo = "REMOCAO_PRODUTO",
                        descricao = $"Removido {qtdRemover} do produto {nomeProduto} na Ordem de Serviço",
                        dataAlteracao = DateTime.Now
                    };
                    _ordemServicoService.RegistrarHistoricoOS(historico);
                    MessageBox.Show("Operação de remoção/redução realizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparSelecaoProduto();
                    CarregarProdutos();
                    CarregarItensGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarProdutoSelecionado();
        }

        private void AtualizarProdutoSelecionado()
        {
            if (cbProduto.SelectedValue != null)
            {
                int idSelecionado = 0;

                if (cbProduto.SelectedValue is int idInt)
                {
                    idSelecionado = idInt;
                }
                else if (int.TryParse(cbProduto.SelectedValue.ToString(), out int idParsed))
                {
                    idSelecionado = idParsed;
                }

                if (idSelecionado > 0)
                {
                    _idProduto = idSelecionado;
                    _produto = _ordemServicoService.ObterProdutoPorId(_idProduto);

                    if (_produto != null)
                    {
                        CalcularValorTotal();
                        return;
                    }
                }
            }

            LimparSelecaoProduto();
        }

        private void CalcularValorTotal()
        {
            if (_produto == null)
            {
                txtValor.Text = "0,00";
                return;
            }

            if (!int.TryParse(txtQntd.Text, out int quantidade) || quantidade < 0)
            {
                quantidade = 0;
            }

            decimal valorTotal = _produto.preco_venda * quantidade;
            txtValor.Text = valorTotal.ToString("N2");
        }

        private void LimparSelecaoProduto()
        {
            _idProduto = 0;
            _produto = null;
            txtQntd.Text = "0";
            txtValor.Text = "0,00";
            cbProduto.SelectedIndex = -1;
        }

        private void dgvItensOS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvItensOS.Rows.Count) return;

            DataGridViewRow row = dgvItensOS.Rows[e.RowIndex];
            if (row.IsNewRow) return;

            int idProd = 0;

            if (dgvItensOS.Columns.Contains("id_produto") && row.Cells["id_produto"].Value != null)
            {
                idProd = ConverterParaInt(row.Cells["id_produto"].Value);
            }

            if (idProd > 0)
            {
                _idProduto = idProd;
                cbProduto.SelectedValue = _idProduto;

                if (dgvItensOS.Columns.Contains("Quantidade") && row.Cells["Quantidade"].Value != null)
                {
                    txtQntd.Text = ConverterParaInt(row.Cells["Quantidade"].Value).ToString();
                }

                _produto = _ordemServicoService.ObterProdutoPorId(_idProduto);
                CalcularValorTotal();
            }
        }

        private void ConfigurarCampoValor()
        {
            txtValor.Text = 0.ToString("C2");
            txtValor.TextAlign = HorizontalAlignment.Right;
        }

        private void TxtValor_TextChanged(object sender, EventArgs e)
        {
            txtValor.TextChanged -= TxtValor_TextChanged;

            string apenasNumeros = new string(txtValor.Text.Where(char.IsDigit).ToArray());

            if (decimal.TryParse(apenasNumeros, out decimal valorSemVirgula))
            {
                decimal valorFinal = valorSemVirgula / 100m;
                txtValor.Text = valorFinal.ToString("C2");
            }
            else
            {
                txtValor.Text = 0.ToString("C2");
            }

            txtValor.SelectionStart = txtValor.Text.Length;

            txtValor.TextChanged += TxtValor_TextChanged;
        }

        private void TxtValor_Click(object sender, EventArgs e)
        {
            txtValor.SelectionStart = txtValor.Text.Length;
        }
    }
}