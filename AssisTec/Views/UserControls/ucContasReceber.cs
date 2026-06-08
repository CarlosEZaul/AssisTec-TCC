using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Models;
using AssisTec.Reports;
using AssisTec.UserControls.SubUserControl_do_Financeiro;

namespace AssisTec.UserControls
{
    public partial class ucContasReceber : UserControl
    {
        private readonly ContasReceberService _service;
        //private readonly ContasReceberRelatorio _relatorio;
        private int _idConta;
        private readonly List<Label> _listaLabelsTotais;

        public ucContasReceber()
        {
            InitializeComponent();
            
            _service = new ContasReceberService();
            //_relatorio = new ContasReceberRelatorio();
            
            _listaLabelsTotais = new List<Label> 
            { 
                lblTotalReceber, 
                lblRecebido, 
                lblPendente, 
                lblAtrasado 
            };

            ApplyDesignModerno();
            ConfigurarComboBox();
            FormatGrid();
            listGrid();
        }

        private void ApplyDesignModerno()
        {
            this.Text = "Contas a Receber";
            this.BackColor = Color.FromArgb(39, 55, 76);
            DesingComponentes.StyleDataGridView(dgvContasReceber, DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.centralizarPanel(panelExibicao, this.Width);
            DesingComponentes.StyleButton(btnDelete, Color.Red);
        }

        private void ConfigurarComboBox()
        {
            try
            {
                cbFormaPagamento.DataSource = _service.CarregarFormasPagamentoComPadrao();
                cbFormaPagamento.DisplayMember = "exibicao";
                cbFormaPagamento.ValueMember = "id_forma_pagamento";

                cbFormaPagamento.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbFormaPagamento.AutoCompleteSource = AutoCompleteSource.ListItems;

                cbStatus.Items.Clear();
                cbStatus.Items.Add("Todos os Status");
                cbStatus.Items.Add("PENDENTE");
                cbStatus.Items.Add("PAGA");
                cbStatus.Items.Add("ATRASADO");

                cbStatus.SelectedIndex = 0;
                cbFormaPagamento.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecutarFiltro()
        {
            try
            {
                var (dados, totalGeral, totalRecebido, totalPendente, totalAtrasado) = _service.FiltrarContas(
                    mtbDataInicio.Text,
                    mtbDataFim.Text,
                    txtBusca.Text,
                    cbStatus.SelectedIndex,
                    cbStatus.SelectedItem?.ToString(),
                    cbFormaPagamento.SelectedValue
                );

                dgvContasReceber.DataSource = dados;
                FormatGrid();

                lblTotalReceber.Text = totalGeral.ToString("C2");
                lblRecebido.Text = totalRecebido.ToString("C2");
                lblPendente.Text = totalPendente.ToString("C2");
                lblAtrasado.Text = totalAtrasado.ToString("C2");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatGrid()
        {
            if (dgvContasReceber.Columns.Count <= 0) return;

            dgvContasReceber.Columns[0].HeaderText = "ID_CONTA";
            dgvContasReceber.Columns[1].HeaderText = "Descrição";
            dgvContasReceber.Columns[2].HeaderText = "Valor";
            dgvContasReceber.Columns[3].HeaderText = "Data de Emissão";
            dgvContasReceber.Columns[4].HeaderText = "Data de Pagamento";
            dgvContasReceber.Columns[5].HeaderText = "Data de Vencimento";
            dgvContasReceber.Columns[6].HeaderText = "Status";
            dgvContasReceber.Columns[7].HeaderText = "Forma de Pagamento";
            dgvContasReceber.Columns[8].HeaderText = "Observações";
            dgvContasReceber.Columns[9].HeaderText = "ID_OS";
        }

        private void listGrid()
        {
            try
            {
                dgvContasReceber.DataSource = _service.CarregarTodasContas();
                
                cbStatus.SelectedIndex = 0;
                cbFormaPagamento.SelectedIndex = 0;

                var (totalGeral, totalRecebido, totalPendente, totalAtrasado) = _service.ObterTotaisPadrao();

                _listaLabelsTotais[0].Text = totalGeral.ToString("C2");
                _listaLabelsTotais[1].Text = totalRecebido.ToString("C2");
                _listaLabelsTotais[2].Text = totalPendente.ToString("C2");
                _listaLabelsTotais[3].Text = totalAtrasado.ToString("C2");
                
                FormatGrid();
                DisableBtn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void dgvContasReceber_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvContasReceber.Columns[e.ColumnIndex].Index == 7 && e.Value != null)
            {
                if (e.Value.ToString() == "ATRASADO")
                {
                    dgvContasReceber.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    dgvContasReceber.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }

        private void dgvContasReceber_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvContasReceber.Rows.Count > 0)
            {
                try
                {
                    EnableBtn();
                    _idConta = Convert.ToInt32(dgvContasReceber.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ExecutarFiltro();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _service.DeletarContasReceber(_idConta);
                listGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ucRegistrarEntradaFinanceiro ucRegistrarEntrada = new ucRegistrarEntradaFinanceiro(dgvContasReceber, _idConta, 1, _listaLabelsTotais);
            ConfigurarSubComponente(ucRegistrarEntrada);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ucRegistrarEntradaFinanceiro ucRegistrarEntrada = new ucRegistrarEntradaFinanceiro(dgvContasReceber, _idConta, 2, _listaLabelsTotais);
            ConfigurarSubComponente(ucRegistrarEntrada);
        }

        private void btnRegistrarPagamento_Click(object sender, EventArgs e)
        {
            try
            {
                _service.ValidarRegistoPagamento(dgvContasReceber.CurrentRow);
                
                ucRegistrarPagamento ucPagamento = new ucRegistrarPagamento(_idConta, dgvContasReceber, _listaLabelsTotais);
                ConfigurarSubComponente(ucPagamento);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            // try
            // {
            //     var (dados, totalGeral, totalRecebido, totalPendente, totalAtrasado) = _service.FiltrarContas(
            //         mtbDataInicio.Text,
            //         mtbDataFim.Text,
            //         txtBusca.Text,
            //         cbStatus.SelectedIndex,
            //         cbStatus.SelectedItem?.ToString(),
            //         cbFormaPagamento.SelectedValue
            //     );
            //
            //     _relatorio.GerarRelatorioGeral(dados, totalGeral, totalRecebido, totalPendente, totalAtrasado);
            // }
            // catch (Exception ex)
            // {
            //     MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // }
        }

        private void btnRecibo_Click(object sender, EventArgs e)
        {
            // var (sucesso, mensagem, _) = _relatorio.GerarRecibo(_idConta);
            //
            // if (sucesso)
            //     MessageBox.Show(mensagem, "Recibo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // else
            //     MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ConfigurarSubComponente(UserControl uc)
        {
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Left = (this.ClientSize.Width - uc.Width) / 2;
            uc.Top = (this.ClientSize.Height - uc.Height) / 2;
            uc.Show();
        }
    }
}