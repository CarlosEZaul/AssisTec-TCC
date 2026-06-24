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
            DesingComponentes.StyleDataGridView(dgvContasPagar, DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.centralizarPanel(panelExibicao, this.Width);
            DesingComponentes.StyleButton(btnDelete, Color.Red);
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

                dgvContasPagar.Columns[0].HeaderText = "ID_CONTA";
                dgvContasPagar.Columns[1].HeaderText = "Descrição";
                dgvContasPagar.Columns[2].HeaderText = "Valor";
                dgvContasPagar.Columns[3].HeaderText = "Data de Emissão";
                dgvContasPagar.Columns[4].HeaderText = "Data de Vencimento";
                dgvContasPagar.Columns[5].HeaderText = "Data de Pagamento";
                dgvContasPagar.Columns[6].HeaderText = "Status";
                dgvContasPagar.Columns[7].HeaderText = "Observações";
                dgvContasPagar.Columns[8].HeaderText = "Forma de Pagamento";
                
                
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao formatar grid: " + e.Message, "Erro", MessageBoxButtons.OK);
            }
        }

        private void atualizar()
        {
            listGrid();
            cbStatus.SelectedIndex = 0;
            cbFormaPagamento.SelectedIndex = 0;

            var totais = _contasPagarService.ObterTotaisPadrao();

            listaLabelsTotais[0].Text = totais.TotalGeral.ToString("C2");
            listaLabelsTotais[1].Text = totais.TotalPagar.ToString("C2");
            listaLabelsTotais[2].Text = totais.TotalPendente.ToString("C2");
            listaLabelsTotais[3].Text = totais.TotalAtrasado.ToString("C2");
            _contasPagarService.ProcessarContasAtrasadas();
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
        }

        #endregion

        #region Funções dos componentes

        private void dgvContasPagar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvContasPagar.Columns[e.ColumnIndex].Index == 6 && e.Value != null)
            {
                string status = e.Value.ToString();

                if (status == "ATRASADO")
                {
                    dgvContasPagar.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    dgvContasPagar.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
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

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            
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

        private void btnRecibo_Click(object sender, EventArgs e)
        {
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
    }
}