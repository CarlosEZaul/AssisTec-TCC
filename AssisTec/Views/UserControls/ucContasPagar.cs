using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Models;
using AssisTec.UserControls.SubUserControl_do_Financeiro;

namespace AssisTec.UserControls
{
    public partial class ucContasPagar : UserControl
    {
        private readonly ContasPagarService _contasPagarService;
        private readonly PagamentoService _pagamentoService;
        private int idConta;
        private List<Label> listaLabelsTotais;

        public ucContasPagar(ContasPagarService contasPagarService, PagamentoService pagamentoService)
        {
            InitializeComponent();
            _contasPagarService = contasPagarService ?? throw new ArgumentNullException(nameof(contasPagarService));
            _pagamentoService = pagamentoService ?? throw new ArgumentNullException(nameof(pagamentoService));
            
            listaLabelsTotais = new List<Label> { 
                lblTotalPagar, 
                lblPago, 
                lblPendente, 
                lblAtrasado 
            };

            applyDesingModerno();
            configurarComboBox();
            atualizar();
        }

        #region DesingModerno

        private void applyDesingModerno()
        {
            this.Text = "Contas a Pagar";
            this.BackColor = Color.FromArgb(39, 55, 76);
            DesignComponentes.StyleDataGridView(dgvContasPagar, DataGridViewAutoSizeColumnsMode.Fill);
            DesignComponentes.centralizarPanel(panelBotoes, this.Width);
            DesignComponentes.centralizarPanel(panelExibicao, this.Width);
            DesignComponentes.StyleButton(btnDelete, Color.Red);
        }

        #endregion

        #region Funções ou métodos

        private void filtro()
        {
            try
            {
                var (dados, totalGeral, totalPago, totalPendente, totalAtrasado) = _contasPagarService.Filtrar(
                    mtbDataInicio.Text,
                    mtbDataFim.Text,
                    txtBusca.Text,
                    cbStatus.SelectedIndex,
                    cbStatus.SelectedItem?.ToString(),
                    cbFormaPagamento.SelectedValue
                );

                dgvContasPagar.DataSource = dados;
                formatgrid();

                lblTotalPagar.Text = $"R$ {totalGeral:N2}";
                lblPago.Text       = $"R$ {totalPago:N2}";
                lblPendente.Text   = $"R$ {totalPendente:N2}";
                lblAtrasado.Text   = $"R$ {totalAtrasado:N2}";
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao filtrar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void configurarComboBox()
        {
            cbFormaPagamento.DataSource = _pagamentoService.CarregarFormasPagamento(incluirOpcaoTodas: true);
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";

            cbStatus.Items.Clear();
            cbStatus.Items.AddRange(new[] { "Todos os Status", "PENDENTE", "PAGA", "ATRASADO" });
            cbStatus.SelectedIndex = 0;
            cbFormaPagamento.SelectedIndex = 0;
        }

        private void listGrid()
        {
            try
            {
                dgvContasPagar.DataSource = _contasPagarService.ObterTodasContas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void formatgrid()
        {
            try
            {
                if (dgvContasPagar.Columns.Count <= 0) return;

                if (dgvContasPagar.Columns.Contains("id_conta_pagar"))
                    dgvContasPagar.Columns["id_conta_pagar"].HeaderText = "ID_CONTA";
                else if (dgvContasPagar.Columns.Contains("IdContaPagar"))
                    dgvContasPagar.Columns["IdContaPagar"].HeaderText = "ID_CONTA";

                if (dgvContasPagar.Columns.Contains("descricao"))
                    dgvContasPagar.Columns["descricao"].HeaderText = "Descrição";
                else if (dgvContasPagar.Columns.Contains("Descricao"))
                    dgvContasPagar.Columns["Descricao"].HeaderText = "Descrição";

                if (dgvContasPagar.Columns.Contains("valor"))
                    dgvContasPagar.Columns["valor"].HeaderText = "Valor";
                else if (dgvContasPagar.Columns.Contains("Valor"))
                    dgvContasPagar.Columns["Valor"].HeaderText = "Valor";

                if (dgvContasPagar.Columns.Contains("data_emissao"))
                    dgvContasPagar.Columns["data_emissao"].HeaderText = "Data de Emissão";
                else if (dgvContasPagar.Columns.Contains("DataEmissao"))
                    dgvContasPagar.Columns["DataEmissao"].HeaderText = "Data de Emissão";

                if (dgvContasPagar.Columns.Contains("data_vencimento"))
                    dgvContasPagar.Columns["data_vencimento"].HeaderText = "Data de Vencimento";
                else if (dgvContasPagar.Columns.Contains("DataVencimento"))
                    dgvContasPagar.Columns["DataVencimento"].HeaderText = "Data de Vencimento";

                if (dgvContasPagar.Columns.Contains("data_pagamento"))
                    dgvContasPagar.Columns["data_pagamento"].HeaderText = "Data de Pagamento";
                else if (dgvContasPagar.Columns.Contains("DataPagamento"))
                    dgvContasPagar.Columns["DataPagamento"].HeaderText = "Data de Pagamento";

                if (dgvContasPagar.Columns.Contains("status"))
                    dgvContasPagar.Columns["status"].HeaderText = "Status";
                else if (dgvContasPagar.Columns.Contains("Status"))
                    dgvContasPagar.Columns["Status"].HeaderText = "Status";

                if (dgvContasPagar.Columns.Contains("observacoes"))
                    dgvContasPagar.Columns["observacoes"].HeaderText = "Observações";
                else if (dgvContasPagar.Columns.Contains("Observacoes"))
                    dgvContasPagar.Columns["Observacoes"].HeaderText = "Observações";

                if (dgvContasPagar.Columns.Contains("FormaPagamentoDescricao"))
                    dgvContasPagar.Columns["FormaPagamentoDescricao"].HeaderText = "Forma de Pagamento";
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao formatar grid: " + e.Message, "Erro", MessageBoxButtons.OK);
            }
        }

        private void atualizar()
        {
            _contasPagarService.ProcessarContasAtrasadas();
            listGrid();

            if (cbStatus.Items.Count > 0) cbStatus.SelectedIndex = 0;
            if (cbFormaPagamento.Items.Count > 0) cbFormaPagamento.SelectedIndex = 0;

            var totais = _contasPagarService.ObterTotaisPadrao();

            listaLabelsTotais[0].Text = totais.TotalGeral.ToString("C2");
            listaLabelsTotais[1].Text = totais.TotalPagar.ToString("C2");
            listaLabelsTotais[2].Text = totais.TotalPendente.ToString("C2");
            listaLabelsTotais[3].Text = totais.TotalAtrasado.ToString("C2");

            DisableBtn();
            formatgrid();
        }

        private void EnableBtn()
        {
            btnEditar.Enabled = true;
            btnDelete.Enabled = true;
            btnRecibo.Enabled = true;
            btnRegistrarPagamento.Enabled = true;
        }

        private void DisableBtn()
        {
            btnEditar.Enabled = false;
            btnDelete.Enabled = false;
            btnRecibo.Enabled = false;
            btnRegistrarPagamento.Enabled = false;
            idConta = 0;
        }

        #endregion

        #region Funções dos componentes

        private void dgvContasPagar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvContasPagar.Columns[e.ColumnIndex].Name.Equals("status", StringComparison.OrdinalIgnoreCase) ||
                dgvContasPagar.Columns[e.ColumnIndex].Name.Equals("Status", StringComparison.OrdinalIgnoreCase))
            {
                if (e.Value != null && e.Value.ToString() == "ATRASADO")
                {
                    dgvContasPagar.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    dgvContasPagar.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (idConta <= 0) return;

            if (MessageBox.Show("Deseja realmente excluir esta conta?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                _contasPagarService.Excluir(idConta);
                DisableBtn();
                atualizar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ucRegistrarSaidaFinanceiro ucRegistrarSaida = new ucRegistrarSaidaFinanceiro(0, 1, _contasPagarService, _pagamentoService);
            ucRegistrarSaida.Disposed += (s, ev) => atualizar();
            
            this.Controls.Add(ucRegistrarSaida);
            ucRegistrarSaida.BringToFront();
            ucRegistrarSaida.Left = (this.ClientSize.Width - ucRegistrarSaida.Width) / 2;
            ucRegistrarSaida.Top = (this.ClientSize.Height - ucRegistrarSaida.Height) / 2;
            ucRegistrarSaida.Show();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            atualizar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            filtro();
        }

        private void dgvContasPagar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvContasPagar.Rows.Count > 0)
            {
                try
                {
                    EnableBtn();
                    idConta = Convert.ToInt32(dgvContasPagar.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idConta <= 0) return;

            ucRegistrarSaidaFinanceiro ucRegistrarSaida = new ucRegistrarSaidaFinanceiro(idConta, 2, _contasPagarService, _pagamentoService);
            ucRegistrarSaida.Disposed += (s, ev) => atualizar();

            this.Controls.Add(ucRegistrarSaida);
            ucRegistrarSaida.BringToFront();
            ucRegistrarSaida.Left = (this.ClientSize.Width - ucRegistrarSaida.Width) / 2;
            ucRegistrarSaida.Top = (this.ClientSize.Height - ucRegistrarSaida.Height) / 2;
            ucRegistrarSaida.Show();
        }

        private void btnRegistrarPagamento_Click(object sender, EventArgs e)
        {
            if (dgvContasPagar.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma conta para registrar o pagamento.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _contasPagarService.ValidarPagamento(dgvContasPagar.CurrentRow);

                var ucPagamento = new ucRegistrarPagamentoSaida(idConta, _contasPagarService, _pagamentoService);

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
            uc.Disposed += (s, e) => atualizar();
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Location = new Point((this.Width - uc.Width) / 2, (this.Height - uc.Height) / 2);
        }

        #endregion

        private void btnRelatorio_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                    sfd.FileName = $"Relatorio_Contas_Pagar_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string nomeFormaPagamento = cbFormaPagamento.Text;
                        
                        _contasPagarService.GerarRelatorioFiltradoPdf(
                            mtbDataInicio.Text,
                            mtbDataFim.Text,
                            txtBusca.Text,
                            cbStatus.SelectedIndex,
                            cbStatus.SelectedItem?.ToString(),
                            cbFormaPagamento.SelectedValue,
                            nomeFormaPagamento,
                            sfd.FileName
                        );

                        MessageBox.Show("Relatório em PDF gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar relatório: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRecibo_Click_1(object sender, EventArgs e)
        {
            if (idConta <= 0)
            {
                MessageBox.Show("Selecione uma conta para gerar o comprovante/detalhamento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                    sfd.FileName = $"Comprovante_Conta_Pagar_{idConta}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        _contasPagarService.GerarRelatorioIndividualPdf(idConta, sfd.FileName);
                        MessageBox.Show("Comprovante individual gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar o comprovante: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}