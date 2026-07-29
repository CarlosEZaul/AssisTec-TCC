using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;
using AssisTec.SubForms_do_Gerenciador_de_Pedidos;
using AssisTec.Utils;
using AssisTec.Views.UserControls.SubUserControl_do_Gerenciador_de_OS;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls
{
    public partial class ucGerenciadorOS : UserControl
    {
        private readonly OrdemServicoService  _ordemServicoService;
        private int _idOS;
        public ucGerenciadorOS(OrdemServicoService ordemServico)
        {
            InitializeComponent();
            DesignModerno();
            _ordemServicoService = ordemServico ?? throw new ArgumentNullException(nameof(ordemServico));
            AtualizarGrid();
            
        }

        #region Design

        private void DesignModerno()
        {
            DesignComponentes.StyleDataGridView(dgvOS);
            DesignComponentes.centralizarPanel(panelBotoes, this.Width);
        }
        

        #endregion

        #region Funções e Métodos

        private void AtualizarGrid()
        {
            dgvOS.DataSource = _ordemServicoService.ObterTodasOSAtuais();
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvOS.Columns.Count <= 0) return;

            dgvOS.Columns[0].HeaderText = "ID_OS";
            dgvOS.Columns[1].HeaderText = "Técnico Responsável";
            dgvOS.Columns[2].HeaderText = "Cliente";
            dgvOS.Columns[3].HeaderText = "Equipamento";
            dgvOS.Columns[4].HeaderText = "Status";
            dgvOS.Columns[5].HeaderText = "Data de Abertura";
            dgvOS.Columns[6].HeaderText = "Ultima Atualização";
            dgvOS.Columns[7].HeaderText = "Data de Conclusão";
            dgvOS.Columns[8].HeaderText = "Valor Total";
            
        }
        
        private void ConfigurarSubComponente(UserControl uc)
        {
            uc.Disposed += (s, e) => AtualizarGrid();
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Location = new Point((this.Width - uc.Width) / 2, (this.Height - uc.Height) / 2);
        }

        #endregion

        private void btnNew_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucFormularioOS(_ordemServicoService));
        }

        private void btnGerenciar_Click(object sender, EventArgs e)
        {
            FrmGerenciarOS frmGerenciarOs = new FrmGerenciarOS(_idOS, _ordemServicoService);
            frmGerenciarOs.ShowDialog();
            AtualizarGrid();
        }

        private void dgvOS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvOS.Rows.Count > 0)
            {
                try
                {
                    MudarEstadoBotoes(true);
                    _idOS = Convert.ToInt32(dgvOS.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar os: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void MudarEstadoBotoes(bool ativo)
        {
            btnGerenciar.Enabled = ativo;
            btnPagamento.Enabled = ativo;
            btnRecibo.Enabled = ativo;
        }

        private void dgvOS_CellDoubleClick(object sender, DataGridViewCellEventArgs e) => btnGerenciar_Click(sender, e);

        private void btnPagamento_Click(object sender, EventArgs e)
        {
            if (_ordemServicoService.ObterPorId(_idOS).status != "AGUARDANDO_RETIRADA")
            {
                MessageBox.Show("Somente OS para retirada podem registrar o pagamento",  "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ConfigurarSubComponente(new ucRegistrarPagamentoOS(_idOS, _ordemServicoService));
        }
        
        private void btnRecibo_Click_1(object sender, EventArgs e)
        {
            if (_idOS <= 0)
            {
                MessageBox.Show("Selecione uma Ordem de Serviço válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var dadosRelatorio = _ordemServicoService.GerarReciboOS(_idOS);

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                    sfd.FileName = $"OS_{_idOS}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        GeradorPdfOS.GerarRecibo(dadosRelatorio, sfd.FileName);
                        MessageBox.Show("Relatório Gerado com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}