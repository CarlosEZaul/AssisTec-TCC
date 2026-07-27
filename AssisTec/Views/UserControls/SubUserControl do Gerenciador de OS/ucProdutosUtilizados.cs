using System;
using System.Data;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucProdutosUtilizados : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private int _idProduto;
        private int _idOrdemServico;
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
            _idOrdemServico = idOrdemServico;
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
                if (_idOrdemServico > 0)
                {
                    CarregarItensGrid(); 
                }
            }
        }

        public void AtualizarDados()
        {
            CarregarProdutos();
            if (_idOrdemServico > 0)
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
            if (_idOrdemServico <= 0) return;

            try
            {
                DataTable dtItens = _ordemServicoService.ObterItensDaOS(_idOrdemServico);

                dgvItensOS.DataSource = null;
                dgvItensOS.AutoGenerateColumns = true;
                dgvItensOS.DataSource = dtItens;
                dgvItensOS.Refresh();

                FormatadorGrid();
                CalcularTotalOS();
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

            if (dgvItensOS.Columns["idProduto"] != null)
                dgvItensOS.Columns["idProduto"].Visible = false;

            if (dgvItensOS.Columns["ValorUnitario"] != null)
                dgvItensOS.Columns["ValorUnitario"].DefaultCellStyle.Format = "C2";

            if (dgvItensOS.Columns["ValorTotal"] != null)
                dgvItensOS.Columns["ValorTotal"].DefaultCellStyle.Format = "C2";
        }

        private void CalcularTotalOS()
        {
            decimal totalGeral = 0;

            if (dgvItensOS.DataSource is DataTable dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    if (dt.Columns.Contains("ValorTotal") && row["ValorTotal"] != DBNull.Value && row["ValorTotal"] != null)
                    {
                        if (decimal.TryParse(row["ValorTotal"].ToString(), out decimal valor))
                        {
                            totalGeral += valor;
                        }
                    }
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
            if (!int.TryParse(txtQntd.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Informe uma quantidade válida maior que zero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool sucesso = _ordemServicoService.AdicionarOuAtualizarItemOS(_idOrdemServico, _idProduto, quantidade);
                if (sucesso)
                {
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

            if (dgvItensOS.CurrentRow.DataBoundItem is DataRowView rowView)
            {
                idItem = ConverterParaInt(rowView.Row.Table.Columns.Contains("Id") ? rowView["Id"] : null);
            }
            else if (dgvItensOS.Columns.Contains("Id") && dgvItensOS.CurrentRow.Cells["Id"].Value != null)
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
                bool sucesso = _ordemServicoService.ReduzirOuRemoverItemOS(idItem, qtdRemover);
                if (sucesso)
                {
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

            if (row.DataBoundItem is DataRowView rowView && rowView.Row.Table.Columns.Contains("idProduto"))
            {
                idProd = ConverterParaInt(rowView["idProduto"]);
            }
            else if (dgvItensOS.Columns.Contains("idProduto") && row.Cells["idProduto"].Value != null)
            {
                idProd = ConverterParaInt(row.Cells["idProduto"].Value);
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
    }
}