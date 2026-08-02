using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using AssisTec.DTO;
using AssisTec.Service;

namespace AssisTec.UserControls
{
    public partial class ucHistoricoOS : UserControl
    {
        private readonly UsuarioService _usuarioService;
        private readonly ClienteService _clienteService;
        private readonly OrdemServicoService _ordemServicoService;

        private int? _idClienteOrigem;
        private int? _idUsuarioOrigem;
        private int _idOS;

        public ucHistoricoOS(int id, UsuarioService usuarioService, OrdemServicoService ordemServicoService)
        {
            InitializeComponent();
            DesingModerno();
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _ordemServicoService = ordemServicoService ?? throw new ArgumentNullException(nameof(ordemServicoService));
            _idUsuarioOrigem = id;
            CarregarComboStatus();
            listGridUsuario(id);
        }

        public ucHistoricoOS(int id, ClienteService clienteService, OrdemServicoService ordemServicoService)
        {
            InitializeComponent();
            DesingModerno();
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            _ordemServicoService = ordemServicoService ?? throw new ArgumentNullException(nameof(ordemServicoService));
            _idClienteOrigem = id;
            CarregarComboStatus();
            listGridCliente(id);
        }

        #region DesingModerno

        private void DesingModerno()
        {
            DesignComponentes.StyleDataGridView(dgvOS, DataGridViewAutoSizeColumnsMode.Fill);
            DesignComponentes.centralizarPanel(panelBotoes, this.Width);
        }

        #endregion

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

        private void ConfigurarComponentes(DataTable dtHistorico)
        {
            dgvOS.DataSource = dtHistorico;

            if (dgvOS.Rows.Count > 0)
            {
                dgvOS.Rows[0].Selected = true;
                AtualizarIdSelecionado(0);
            }
            else
            {
                _idOS = 0;
            }
        }

        private void listGridUsuario(int id)
        {
            DataTable dtHistorico = _usuarioService.obterHistoricoOs(id);
            ConfigurarComponentes(dtHistorico);
        }

        private void listGridCliente(int id)
        {
            DataTable dtHistorico = _clienteService.ObterHistoricoOS(id);
            ConfigurarComponentes(dtHistorico);
        }

        private void AtualizarIdSelecionado(int rowIndex)
        {
            if (rowIndex >= 0 && dgvOS.Rows.Count > rowIndex)
            {
                var cellValue = dgvOS.Columns.Contains("ID")
                    ? dgvOS.Rows[rowIndex].Cells["ID"].Value
                    : dgvOS.Rows[rowIndex].Cells[0].Value;

                if (cellValue != null && cellValue != DBNull.Value)
                {
                    _idOS = Convert.ToInt32(cellValue);
                }
            }
        }

        private void dgvOS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvOS.Rows.Count > 0)
            {
                try
                {
                    AtualizarIdSelecionado(e.RowIndex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar OS: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ExecutarFiltro();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            mtbDataInicio.Clear();
            mtbDataFim.Clear();
            txtBusca.Clear();
            if (cbStatus.Items.Count > 0) cbStatus.SelectedIndex = 0;

            if (_idUsuarioOrigem.HasValue)
            {
                listGridUsuario(_idUsuarioOrigem.Value);
            }
            else if (_idClienteOrigem.HasValue)
            {
                listGridCliente(_idClienteOrigem.Value);
            }
        }

        private void ExecutarFiltro()
        {
            string dataInicio = ObterDataValida(mtbDataInicio.Text);
            string dataFim = ObterDataValida(mtbDataFim.Text);
            string busca = txtBusca.Text;
            string status = cbStatus.SelectedItem != null ? cbStatus.SelectedItem.ToString() : string.Empty;

            DataTable dtFiltrado = _ordemServicoService.FiltrarHistorico(
                idCliente: _idClienteOrigem,
                idTecnico: _idUsuarioOrigem,
                dataInicio: dataInicio,
                dataFim: dataFim,
                busca: busca,
                status: status
            );

            ConfigurarComponentes(dtFiltrado);
        }

        private void btnRelatorioGeral_Click(object sender, EventArgs e)
        {
            DataTable dtAtual = dgvOS.DataSource as DataTable;

            if (dtAtual == null || dtAtual.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados na tabela para gerar o relatório geral.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                sfd.FileName = $"Relatorio_Geral_OS_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DateTime? dtInicioParsed = ParseDataNullable(mtbDataInicio.Text);
                        DateTime? dtFimParsed = ParseDataNullable(mtbDataFim.Text);
                        string status = cbStatus.SelectedItem != null ? cbStatus.SelectedItem.ToString() : "TODOS";
                        string caminhoLogo = Path.Combine(Application.StartupPath, "Resources", "logo.png");

                        _ordemServicoService.GerarRelatorioGeralPdf(
                            dtAtual,
                            dtInicioParsed,
                            dtFimParsed,
                            status,
                            sfd.FileName,
                            caminhoLogo
                        );

                        if (File.Exists(sfd.FileName))
                        {
                            Process.Start(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao gerar relatório geral: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnRelatorioIndividual_Click(object sender, EventArgs e)
        {
            if (_idOS <= 0)
            {
                MessageBox.Show("Selecione uma Ordem de Serviço na tabela para emitir o relatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                sfd.FileName = $"Ordem_Servico_{_idOS:D6}.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string caminhoLogo = Path.Combine(Application.StartupPath, "Resources", "logo.png");

                        _ordemServicoService.ExportarReciboPdf(_idOS, sfd.FileName, caminhoLogo);

                        if (File.Exists(sfd.FileName))
                        {
                            Process.Start(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao gerar relatório individual: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string ObterDataValida(string mascaraData)
        {
            return DateTime.TryParseExact(mascaraData, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                ? mascaraData
                : null;
        }

        private DateTime? ParseDataNullable(string mascaraData)
        {
            if (DateTime.TryParseExact(mascaraData, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtParsed))
            {
                return dtParsed;
            }
            return null;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}