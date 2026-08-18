using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.Models;
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
        
                // Adicionada a atualização da label de valor a receber
                lblReceber.Text = resultado.TotalAReceber.ToString("C2");
                lblRecebidoFinalizado.Text = $"{resultado.TotalRecebido:C2} / {resultado.QntRecebido}";
                lblCancelado.Text = $"{resultado.QntCancelado}"; // Removido o total em dinheiro se for para mostrar apenas a quantidade, mantendo o padrão do AtualizarGrid

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

                lblReceber.Text = resultado.TotalAReceber.ToString("C2");
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
            btnImprimir.Enabled = ativo;
            btnRecibo.Enabled = ativo;
            btnAtualizacao.Enabled = ativo;
            btnContatoCliente.Enabled = ativo;
            btnContatoTecnico.Enabled = ativo;
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

            var os = _ordemServicoService.ObterPorId(_idOS);
            if (os == null)
            {
                MessageBox.Show("Ordem de Serviço não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool ehGerente = Sessao.usuarioLogado != null && Sessao.usuarioLogado.Nivel == 1;
            bool ehTecnicoResponsavel = Sessao.usuarioLogado != null && os.id_tecnico == Sessao.usuarioLogado.Id;

            if (!ehGerente && !ehTecnicoResponsavel)
            {
                MessageBox.Show("Acesso Negado! Apenas o Gerente ou o Técnico responsável por esta Ordem de Serviço podem gerenciá-la.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            bool ehGerente = Sessao.usuarioLogado != null && Sessao.usuarioLogado.Nivel == 1;
            bool ehAtendente = Sessao.usuarioLogado != null && Sessao.usuarioLogado.Nivel == 2;

            if (!ehGerente && !ehAtendente)
            {
                MessageBox.Show("Acesso Negado! Apenas Atendentes ou Gerentes podem registrar pagamentos e finalizar Ordens de Serviço.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                var os = _ordemServicoService.ObterPorId(_idOS);
                
                var dadosRelatorio = _ordemServicoService.ImprimirOS(os);

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                    sfd.FileName = $"OS_{_idOS}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        GeradorPdfOS.ImprimirOS(dadosRelatorio, sfd.FileName);

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
        
        private void btnRelatorio_Click_1(object sender, EventArgs e)
        {
            try
            {
                DataTable dtRelatorio = (DataTable)dgvOS.DataSource;

                if (dtRelatorio == null || dtRelatorio.Rows.Count == 0)
                {
                    MessageBox.Show("Não há dados na listagem para gerar o relatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Arquivos PDF (*.pdf)|*.pdf";
                    saveFileDialog.Title = "Salvar Relatório Geral de Ordens de Serviço";
                    saveFileDialog.FileName = $"Relatorio_OS_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        DateTime? dataInicio = null;
                        if (DateTime.TryParse(mtbDataInicio.Text, out DateTime parsedInicio))
                        {
                            dataInicio = parsedInicio.Date;
                        }

                        DateTime? dataFim = null;
                        if (DateTime.TryParse(mtbDataFim.Text, out DateTime parsedFim))
                        {
                            dataFim = parsedFim.Date;
                        }

                        string status = null;
                        if (cbStatus.SelectedIndex > 0 && cbStatus.SelectedItem != null)
                        {
                            status = cbStatus.SelectedItem.ToString();
                        }
                        else if (!string.IsNullOrWhiteSpace(cbStatus.Text) && cbStatus.Text != "Todos")
                        {
                            status = cbStatus.Text;
                        }

                        string caminhoLogo = Path.Combine(Application.StartupPath, "Resources", "logo.png");

                        string caminhoArquivo = _ordemServicoService.GerarRelatorioGeralPdf(dtRelatorio, dataInicio, dataFim, status, saveFileDialog.FileName, caminhoLogo);

                        this.Cursor = Cursors.Default;

                        Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = caminhoArquivo,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Erro ao gerar o relatório em PDF:\n{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnContatoCliente_Click(object sender, EventArgs e)
        {
            if (_idOS <= 0)
            {
                MessageBox.Show("Por favor, selecione um cliente válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnContatoCliente.Enabled = false;

            try
            {
                var os = _ordemServicoService.ObterPorId(_idOS);

                if (os == null)
                {
                    MessageBox.Show("Ordem de Serviço não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Cliente cliente = os.Cliente;

                if (cliente == null)
                {
                    MessageBox.Show("Cliente não encontrado para esta OS.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(cliente.Telefone))
                {
                    MessageBox.Show("Este cliente não possui um telefone cadastrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool sucesso = await ContatoWhatsApp.EntrarContato(cliente.Telefone);

                if (sucesso)
                {
                    MessageBox.Show("Contato iniciado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Falha ao iniciar contato. Verifique a conexão.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro inesperado: {ex.Message}", "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnContatoCliente.Enabled = true;
            }
        }

        private async void btnContatoTecnico_Click(object sender, EventArgs e)
        {
            if (_idOS <= 0)
            {
                MessageBox.Show("Por favor, selecione um cliente válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnContatoTecnico.Enabled = false;

            try
            {
                var os = _ordemServicoService.ObterPorId(_idOS);

                if (os == null)
                {
                    MessageBox.Show("Ordem de Serviço não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Usuario tecnico = os.Tecnico;

                if (tecnico == null)
                {
                    MessageBox.Show("Cliente não encontrado para esta OS.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tecnico.Telefone))
                {
                    MessageBox.Show("Este cliente não possui um telefone cadastrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool sucesso = await ContatoWhatsApp.EntrarContato(tecnico.Telefone);

                if (sucesso)
                {
                    MessageBox.Show("Contato iniciado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Falha ao iniciar contato. Verifique a conexão.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro inesperado: {ex.Message}", "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnContatoTecnico.Enabled = true;
            }
        }

        #endregion


        private void btnRecibo_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvOS.CurrentRow == null)
                {
                    MessageBox.Show("Selecione uma Ordem de Serviço para emitir o recibo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idOS = Convert.ToInt32(dgvOS.CurrentRow.Cells["ID"].Value);
                string status = dgvOS.CurrentRow.Cells["status"].Value?.ToString();

                if (status != "FINALIZADA")
                {
                    MessageBox.Show("O recibo só pode ser emitido para Ordens de Serviço FINALIZADAS.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string caminhoSalvar = null;

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Arquivo PDF (*.pdf)|*.pdf";
                    sfd.FileName = $"Recibo_OS_{idOS}_{DateTime.Now:yyyyMMdd}.pdf";
                    sfd.Title = "Salvar Recibo de OS";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        caminhoSalvar = sfd.FileName;
                    }
                }

                bool sucesso = _ordemServicoService.EmitirReciboOSFinalizada(idOS, caminhoSalvar);

                if (sucesso)
                {
                    MessageBox.Show("Recibo gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao emitir recibo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAtualizacao_Click(object sender, EventArgs e)
        {
            ConfigurarSubComponente(new ucHistoricoAlteracao(_ordemServicoService, _idOS));
        }
    }
}