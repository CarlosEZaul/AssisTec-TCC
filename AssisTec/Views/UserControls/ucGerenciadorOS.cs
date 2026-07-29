using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.SubForms_do_Gerenciador_de_Pedidos;
using AssisTec.Utils;
using AssisTec.Views.UserControls.SubUserControl_do_Gerenciador_de_OS;

namespace AssisTec.UserControls
{
    public partial class ucGerenciadorOS : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private int _idOS;

        public ucGerenciadorOS(OrdemServicoService ordemServico)
        {
            InitializeComponent();
            DesignModerno();
            _ordemServicoService = ordemServico ?? throw new ArgumentNullException(nameof(ordemServico));
            
            CarregarComboStatus();
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

        private void CarregarComboStatus()
        {
            cbStatus.Items.Clear();
    
            cbStatus.Items.Add("TODOS");
            cbStatus.Items.Add("ABERTA");   
            cbStatus.Items.Add("AGUARDANDO_RETIRADA");
            cbStatus.Items.Add("FINALIZADA");
            cbStatus.Items.Add("CANCELADA");

            cbStatus.SelectedIndex = 0;
        }

        private void ExecutarFiltro()
        {
            try
            {
                var resultado = _ordemServicoService.Filtrar(
                    mtbDataInicio.Text,
                    mtbDataFim.Text,
                    txtBusca.Text,
                    cbStatus.SelectedIndex,
                    cbStatus.SelectedItem?.ToString()
                );

                dgvOS.DataSource = resultado.Dados;

                lblTotalOS.Text = resultado.TotalOS.ToString();
                lblEmAndamento.Text = resultado.EmAtendimento.ToString();
                lblRetirada.Text = resultado.ParaRetirada.ToString();
        
                lblRecebidoFinalizado.Text = $"{resultado.TotalRecebido:C2} / {resultado.QntRecebido}";
                lblCancelado.Text = $"{resultado.TotalCancelado:C2} / {resultado.QntCancelado}";

                _idOS = 0;
                MudarEstadoBotoes(false);
                FormatGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao filtrar Ordens de Serviço: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarGrid()
        {
            try
            {
                var resultado = _ordemServicoService.ObterDadosAtuais();

                dgvOS.DataSource = resultado.Dados;

                lblTotalOS.Text = resultado.TotalOS.ToString();
                lblEmAndamento.Text = resultado.EmAtendimento.ToString();
                lblRetirada.Text = resultado.ParaRetirada.ToString();

                lblRecebidoFinalizado.Text = $"{resultado.TotalRecebido:C2} / {resultado.QntRecebido}";
                lblCancelado.Text = $"{resultado.QntCancelado}";

                _idOS = 0;
                MudarEstadoBotoes(false);
                FormatGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar Ordens de Serviço: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormatGrid()
        {
            if (dgvOS.Columns.Count <= 0) return;

            if (dgvOS.Columns.Contains("ID")) dgvOS.Columns["ID"].HeaderText = "ID";
            if (dgvOS.Columns.Contains("Cliente")) dgvOS.Columns["Cliente"].HeaderText = "Cliente";
            if (dgvOS.Columns.Contains("Tecnico")) dgvOS.Columns["Tecnico"].HeaderText = "Técnico";
            if (dgvOS.Columns.Contains("Equipamento")) dgvOS.Columns["Equipamento"].HeaderText = "Equipamento";
            if (dgvOS.Columns.Contains("Status")) dgvOS.Columns["Status"].HeaderText = "Status";
            
            if (dgvOS.Columns.Contains("DataAbertura")) 
            {
                dgvOS.Columns["DataAbertura"].HeaderText = "Data de Abertura";
                dgvOS.Columns["DataAbertura"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            if (dgvOS.Columns.Contains("UltimaAtulizacao")) 
            {
                dgvOS.Columns["UltimaAtulizacao"].HeaderText = "Última Atualização";
                dgvOS.Columns["UltimaAtulizacao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            if (dgvOS.Columns.Contains("DataConclusao")) 
            {
                dgvOS.Columns["DataConclusao"].HeaderText = "Data de Conclusão";
                dgvOS.Columns["DataConclusao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            if (dgvOS.Columns.Contains("ValorTotal"))
            {
                dgvOS.Columns["ValorTotal"].HeaderText = "Valor Total";
                dgvOS.Columns["ValorTotal"].DefaultCellStyle.Format = "C2";
            }
        }

        private void ConfigurarSubComponente(UserControl uc)
        {
            uc.Disposed += (s, e) => AtualizarGrid();
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Location = new Point((this.Width - uc.Width) / 2, (this.Height - uc.Height) / 2);
        }

        private void MudarEstadoBotoes(bool ativo)
        {
            btnGerenciar.Enabled = ativo;
            btnPagamento.Enabled = ativo;
            btnRecibo.Enabled = ativo;
        }

        #endregion

        #region Eventos da Tela

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ExecutarFiltro();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            mtbDataInicio.Clear();
            mtbDataFim.Clear();
            txtBusca.Clear();
            cbStatus.SelectedIndex = 0;

            AtualizarGrid();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucFormularioOS(_ordemServicoService));
        }

        private void btnGerenciar_Click(object sender, EventArgs e)
        {
            if (_idOS <= 0) return;
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
                    var cellValue = dgvOS.Columns.Contains("ID") 
                        ? dgvOS.Rows[e.RowIndex].Cells["ID"].Value 
                        : dgvOS.Rows[e.RowIndex].Cells[0].Value;

                    _idOS = Convert.ToInt32(cellValue);
                    MudarEstadoBotoes(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar OS: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvOS_CellDoubleClick(object sender, DataGridViewCellEventArgs e) => btnGerenciar_Click(sender, e);

        private void btnPagamento_Click(object sender, EventArgs e)
        {
            if (_idOS <= 0) return;

            var os = _ordemServicoService.ObterPorId(_idOS);
            if (os != null && !string.Equals(os.status, "AGUARDANDO_RETIRADA", StringComparison.OrdinalIgnoreCase) && !string.Equals(os.status, "PARA RETIRADA", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Somente OS para retirada podem registrar o pagamento.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = sfd.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}