using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;
using AssisTec.Utils;

namespace AssisTec.Views.UserControls.SubUserControl_do_Gerenciador_de_OS
{
    public partial class ucRegistrarPagamentoOS : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly OrdemServico _ordemServico;
        private readonly int _idOS;

        public ucRegistrarPagamentoOS(int idOS, OrdemServicoService ordemServicoService)
        {
            InitializeComponent();
            _idOS = idOS;
            _ordemServicoService = ordemServicoService ?? throw new ArgumentNullException(nameof(ordemServicoService));
            
            _ordemServico = _ordemServicoService.ObterPorId(idOS);
            
            if (_ordemServico == null)
            {
                MessageBox.Show("Ordem de Serviço não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CarregarFormasPagamento();
            ConfigurarComponentes();
        }
        
        private void CarregarFormasPagamento()
        {
            var dt = _ordemServicoService.CarregarFormasPagamento(incluirOpcaoTodas: false);

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Rows.RemoveAt(0);
            }

            cbFormaPagamento.DataSource = dt;
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";
        }

        private void ConfigurarComponentes()
        {
            mtbDataPagamento.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtValorPeca.Text = _ordemServico.valor_pecas.ToString("C2");
            txtValorServico.Text = _ordemServico.valor_mao_obra.ToString("C2");
            txtValorTotal.Text = _ordemServico.valor_total.ToString("C2");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbFormaPagamento.SelectedValue == null)
                {
                    MessageBox.Show("Selecione uma forma de pagamento válida.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idFormaPagamento = Convert.ToInt32(cbFormaPagamento.SelectedValue);
        
                int idUsuarioAtual = Sessao.usuarioLogado.Id;

                bool sucesso = _ordemServicoService.RegistrarPagamento(_idOS, idUsuarioAtual, idFormaPagamento);

                if (sucesso)
                {
                    MessageBox.Show("Pagamento registrado e OS finalizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
                    var dadosRelatorio = _ordemServicoService.ImprimirOS(_idOS);

                    string caminhoPdf = Path.Combine(Path.GetTempPath(), $"Recibo_OS_{_idOS}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            
                    GeradorPdfOS.ImprimirOS(dadosRelatorio, caminhoPdf);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = caminhoPdf,
                        UseShellExecute = true
                    });

                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Não foi possível registrar o pagamento. Verifique os dados e tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}