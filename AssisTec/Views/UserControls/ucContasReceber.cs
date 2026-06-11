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
        private int _idConta;
        private readonly List<Label> _listaLabelsTotais;

        public ucContasReceber(ContasReceberService service)
        {
            InitializeComponent();
            _service = service;
            
            _listaLabelsTotais = new List<Label> { lblTotalReceber, lblRecebido, lblPendente, lblAtrasado };

            ApplyDesignModerno();
            ConfigurarComboBox();
            AtualizarGrid();
        }

        private void ApplyDesignModerno()
        {
            this.BackColor = Color.FromArgb(39, 55, 76);
            DesingComponentes.StyleDataGridView(dgvContasReceber, DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.centralizarPanel(panelExibicao, this.Width);
            DesingComponentes.StyleButton(btnDelete, Color.Red);
        }

        private void ConfigurarComboBox()
        {
            cbFormaPagamento.DataSource = _service.CarregarFormasPagamento(incluirOpcaoTodas: true);
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";

            cbStatus.Items.AddRange(new[] { "Todos os Status", "PENDENTE", "PAGA", "ATRASADO" });
            cbStatus.SelectedIndex = 0;
            cbFormaPagamento.SelectedIndex = 0;
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

        private void AtualizarGrid()
        {
            dgvContasReceber.DataSource = _service.CarregarTodas();
            
            var totais = _service.ObterTotaisPadrao();
            _listaLabelsTotais[0].Text = totais.TotalGeral.ToString("C2");
            _listaLabelsTotais[1].Text = totais.TotalRecebido.ToString("C2");
            _listaLabelsTotais[2].Text = totais.TotalPendente.ToString("C2");
            _listaLabelsTotais[3].Text = totais.TotalAtrasado.ToString("C2");
            
            MudarEstadoBotoes(false);
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
            if (dgvContasReceber.Columns[e.ColumnIndex].Name == "status" && e.Value?.ToString() == "ATRASADO")
            {
                dgvContasReceber.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                dgvContasReceber.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void dgvContasReceber_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MudarEstadoBotoes(true);
                _idConta = (int)dgvContasReceber.Rows[e.RowIndex].Cells["id_conta_receber"].Value;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _service.Excluir(_idConta);
            AtualizarGrid();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Passa 0 como ID para inserção
            //ConfigurarSubComponente(new ucRegistrarEntradaFinanceiro(dgvContasReceber, 0, 1, _listaLabelsTotais, _service));
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucRegistrarEntradaFinanceiro(_idConta, 2, _service));
        }

        private void btnRegistrarPagamento_Click(object sender, EventArgs e)
        {
            _service.ValidarPagamento(dgvContasReceber.CurrentRow);
            // Certifique-se que o construtor de ucRegistrarPagamento também receba o _service
            ConfigurarSubComponente(new ucRegistrarPagamento(_idConta, dgvContasReceber, _listaLabelsTotais, _service));
        }

        private void ConfigurarSubComponente(UserControl uc)
        {
            uc.Disposed += (s, e) => AtualizarGrid();
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Location = new Point((this.Width - uc.Width) / 2, (this.Height - uc.Height) / 2);
        }
    }
}