using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Models;
using AssisTec.UserControls.SubUserControl_do_Financeiro;

namespace AssisTec.UserControls
{
    public partial class ucContasReceber : UserControl
    {
        private readonly ContasReceberService _service;
        private readonly PagamentoService _pagamentoService;
        private int _idConta;
        private readonly List<Label> _listaLabelsTotais;

        public ucContasReceber(ContasReceberService service, PagamentoService pagamentoService)
        {
            InitializeComponent();
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _pagamentoService = pagamentoService ?? throw new ArgumentNullException(nameof(pagamentoService));

            _listaLabelsTotais = new List<Label> { lblTotalReceber, lblRecebido, lblPendente, lblAtrasado };

            ApplyDesignModerno();
            ConfigurarComboBox();
            AtualizarGrid();
        }

        private void ApplyDesignModerno()
        {
            this.BackColor = Color.FromArgb(39, 55, 76);
            DesignComponentes.StyleDataGridView(dgvContasReceber, DataGridViewAutoSizeColumnsMode.Fill);
            DesignComponentes.centralizarPanel(panelBotoes, this.Width);
            DesignComponentes.centralizarPanel(panelExibicao, this.Width);
            DesignComponentes.StyleButton(btnDelete, Color.Red);
            DesignComponentes.centralizarPanel(panelFiltro, this.Width);
        }

        private void ConfigurarComboBox()
        {
            cbFormaPagamento.DataSource = _pagamentoService.CarregarFormasPagamento(incluirOpcaoTodas: true);
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";

            cbStatus.Items.AddRange(new[] { "Todos os Status", "PENDENTE", "PAGA", "ATRASADO" });
            cbStatus.SelectedIndex = 0;
            cbFormaPagamento.SelectedIndex = 0;
        }

        private void FormatGrid()
        {
            if (dgvContasReceber.Columns.Count <= 0) return;

            dgvContasReceber.Columns[0].HeaderText = "ID_CONTA";
            dgvContasReceber.Columns[1].HeaderText = "Descricao";
            dgvContasReceber.Columns[2].HeaderText = "Valor";
            dgvContasReceber.Columns[3].HeaderText = "Data de Emissao";
            dgvContasReceber.Columns[4].HeaderText = "Data de Vencimento";
            dgvContasReceber.Columns[5].HeaderText = "Data de Pagamento";
            dgvContasReceber.Columns[6].HeaderText = "Status";
            dgvContasReceber.Columns[7].HeaderText = "Observacoes";
            dgvContasReceber.Columns[8].HeaderText = "IdOrdemServico";
            dgvContasReceber.Columns[9].HeaderText = "Forma de Pagamento";
        }

        private void ExecutarFiltro()
        {
            var resultado = _service.Filtrar(
                mtbDataInicio.Text,
                mtbDataFim.Text,
                txtBusca.Text,
                cbStatus.SelectedIndex,
                cbStatus.SelectedItem?.ToString(),
                cbFormaPagamento.SelectedValue
            );

            dgvContasReceber.DataSource = resultado.Dados;

            lblTotalReceber.Text = resultado.TotalGeral.ToString("C2");
            lblRecebido.Text = resultado.TotalRecebido.ToString("C2");
            lblPendente.Text = resultado.TotalPendente.ToString("C2");
            lblAtrasado.Text = resultado.TotalAtrasado.ToString("C2");
        }

        private void UpdateMascaraMonetariaGrid()
        {
            if (dgvContasReceber.Columns.Contains("Valor"))
            {
                dgvContasReceber.Columns["Valor"].DefaultCellStyle.Format = "C2";
            }
        }

        private void AtualizarGrid()
        {
            dgvContasReceber.DataSource = _service.CarregarTodas();

            var totais = _service.ObterTotaisPadrao();
            _listaLabelsTotais[0].Text = totais.TotalGeral.ToString("C2");
            _listaLabelsTotais[1].Text = totais.TotalRecebido.ToString("C2");
            _listaLabelsTotais[2].Text = totais.TotalPendente.ToString("C2");
            _listaLabelsTotais[3].Text = totais.TotalAtrasado.ToString("C2");

            _idConta = 0;
            MudarEstadoBotoes(false);
            FormatGrid();
            UpdateMascaraMonetariaGrid();
            _service.ProcessarContasAtrasadas();
        }

        private void MudarEstadoBotoes(bool ativo)
        {
            btnEditar.Enabled = ativo;
            btnDelete.Enabled = ativo;
            btnRecibo.Enabled = ativo;
            btnRegistrarPagamento.Enabled = ativo;
        }

        private void dgvContasReceber_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var nomeColuna = dgvContasReceber.Columns[e.ColumnIndex].Name;

            if (string.Equals(nomeColuna, "status", StringComparison.OrdinalIgnoreCase)
                && e.Value?.ToString() == "ATRASADO")
            {
                dgvContasReceber.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                dgvContasReceber.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void dgvContasReceber_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var grid = dgvContasReceber;
            var colunas = new[] { "IdContaReceber", "ID_CONTA" };

            foreach (var nomeColuna in colunas)
            {
                if (!grid.Columns.Contains(nomeColuna)) continue;

                var valor = grid.Rows[e.RowIndex].Cells[nomeColuna].Value;
                if (valor != null && valor != DBNull.Value && int.TryParse(valor.ToString(), out int id))
                {
                    _idConta = id;
                    MudarEstadoBotoes(true);
                    return;
                }
            }

            _idConta = 0;
            MudarEstadoBotoes(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_idConta <= 0) return;

            var confirmacao = MessageBox.Show(
                "Tem certeza que deseja excluir esta conta a receber?",
                "Confirmar exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacao != DialogResult.Yes) return;

            try
            {
                _service.Excluir(_idConta);
                AtualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucRegistrarEntradaFinanceiro(0, 1, _service, _pagamentoService));
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucRegistrarEntradaFinanceiro(_idConta, 2, _service, _pagamentoService));
        }

        private void btnRegistrarPagamento_Click(object sender, EventArgs e)
        {
            if (dgvContasReceber.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma conta para registrar o pagamento.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _service.ValidarPagamento(dgvContasReceber.CurrentRow);

                var ucPagamento = new ucRegistrarPagamentoEntrada(_idConta, _service, _pagamentoService);

                ConfigurarSubComponente(ucPagamento);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Operação não permitida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message, "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarSubComponente(UserControl uc)
        {
            uc.Disposed += (s, e) => AtualizarGrid();
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Location = new Point((this.Width - uc.Width) / 2, (this.Height - uc.Height) / 2);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ExecutarFiltro();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void btnRecibo_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}