using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucServicos : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private int _idOrdemServico;
        private int _idAcaoSelecionada;

        public ucServicos(OrdemServicoService ordemServicoService, int idOrdemServico)
        {
            InitializeComponent();
            _ordemServicoService = ordemServicoService ?? throw new ArgumentNullException(nameof(ordemServicoService));
            _idOrdemServico = idOrdemServico;
            this.Load += ucServicos_Load;
            DesignModerno();
            ConfigurarCampoValor();
        }

        private void DesignModerno()
        {
            DesignComponentes.StyleDataGridView(dgvServicosOS);
            dgvServicosOS.ReadOnly = true;
            dgvServicosOS.AllowUserToAddRows = false;
            dgvServicosOS.AllowUserToDeleteRows = false;
            dgvServicosOS.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvServicosOS.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvServicosOS.MultiSelect = false;
        }

        private void ucServicos_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;

            if (_idOrdemServico > 0)
            {
                CarregarGrid();
            }
        }

        public void AtualizarDados()
        {
            if (_idOrdemServico > 0)
            {
                CarregarGrid();
            }
        }

        private void CarregarGrid()
        {
            if (_idOrdemServico <= 0 || _ordemServicoService == null) return;

            try
            {
                List<ServicosOS> lista = _ordemServicoService.ListarAcaoOSPorOS(_idOrdemServico);

                dgvServicosOS.DataSource = null;
                dgvServicosOS.AutoGenerateColumns = true;
                dgvServicosOS.DataSource = lista;
                dgvServicosOS.Refresh();

                FormatadorGrid();
                CalcularTotalOS();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar serviços da OS: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatadorGrid()
        {
            if (dgvServicosOS.Columns.Count == 0) return;

            if (dgvServicosOS.Columns.Contains("idServico"))
                dgvServicosOS.Columns["idServico"].Visible = false;

            if (dgvServicosOS.Columns.Contains("id_OS"))
                dgvServicosOS.Columns["id_OS"].Visible = false;

            if (dgvServicosOS.Columns.Contains("OrdemServico"))
                dgvServicosOS.Columns["OrdemServico"].Visible = false;

            if (dgvServicosOS.Columns.Contains("descricao"))
            {
                dgvServicosOS.Columns["descricao"].HeaderText = "Serviço / Ação";
                dgvServicosOS.Columns["descricao"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvServicosOS.Columns.Contains("valor_cobrado"))
            {
                dgvServicosOS.Columns["valor_cobrado"].HeaderText = "Valor (R$)";
                dgvServicosOS.Columns["valor_cobrado"].DefaultCellStyle.Format = "C2";
                dgvServicosOS.Columns["valor_cobrado"].Width = 120;
            }
        }

        private void CalcularTotalOS()
        {
            decimal totalGeral = 0;

            if (dgvServicosOS.DataSource is List<ServicosOS> lista)
            {
                foreach (var item in lista)
                {
                    totalGeral += item.valor_cobrado;
                }
            }

            lblTotal.Text = totalGeral.ToString("C2");
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (_idOrdemServico <= 0)
            {
                MessageBox.Show("Ordem de Serviço inválida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtServico.Text))
            {
                MessageBox.Show("Informe a descrição do serviço.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServico.Focus();
                return;
            }

            if (!decimal.TryParse(txtValor.Text, out decimal valorCobrado) || valorCobrado < 0)
            {
                MessageBox.Show("Informe um valor válido para o serviço.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtValor.Focus();
                return;
            }

            try
            {
                var servico = new ServicosOS
                {
                    idServico = _idAcaoSelecionada,
                    id_OS = _idOrdemServico,
                    descricao = txtServico.Text.Trim(),
                    valor_cobrado = valorCobrado
                };

                var ordemServico = _ordemServicoService.ObterPorId(_idOrdemServico);
                
                var historicoAlteracaoOS = new HistoricoAlteracaoOS
                {
                    idOS = _idOrdemServico,
                    idUsuario = ordemServico != null ? ordemServico.id_tecnico.GetValueOrDefault() : 0,
                    tipo = _idAcaoSelecionada > 0 ? "ALTERACAO_SERVICO" : "INCLUSAO_SERVICO",
                    descricao = _idAcaoSelecionada > 0 
                        ? $"Alterado serviço para: {servico.descricao} na Ordem de Serviço" 
                        : $"Adicionado {servico.descricao} na Ordem de Serviço",
                    dataAlteracao = DateTime.Now
                };

                bool sucesso = _ordemServicoService.SalvarServicoOS(servico);
                if (sucesso)
                {
                    _ordemServicoService.RegistrarHistoricoOS(historicoAlteracaoOS);
                    MessageBox.Show("Serviço registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (_idAcaoSelecionada <= 0)
            {
                MessageBox.Show("Selecione um serviço na tabela para remover.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Deseja realmente remover este serviço da OS?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            try
            {
                bool sucesso = _ordemServicoService.ExcluirServicoOS(_idAcaoSelecionada);
                if (sucesso)
                {
                    var ordemServico = _ordemServicoService.ObterPorId(_idOrdemServico);

                    var historicoAlteracaoOS = new HistoricoAlteracaoOS
                    {
                        idOS = _idOrdemServico,
                        idUsuario = ordemServico != null ? ordemServico.id_tecnico.GetValueOrDefault() : 0,
                        tipo = "REMOCAO_SERVICO",
                        descricao = $"Removido o serviço de {txtServico.Text} na Ordem de Serviço",
                        dataAlteracao = DateTime.Now
                    };

                    _ordemServicoService.RegistrarHistoricoOS(historicoAlteracaoOS);
                    MessageBox.Show("Serviço removido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimparCampos();
                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvServicosOS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvServicosOS.Rows.Count) return;

            DataGridViewRow row = dgvServicosOS.Rows[e.RowIndex];
            if (row.IsNewRow) return;

            if (row.DataBoundItem != null)
            {
                dynamic item = row.DataBoundItem;

                try
                {
                    _idAcaoSelecionada = item.idServico != null ? Convert.ToInt32(item.idServico) : (item.ID != null ? Convert.ToInt32(item.ID) : 0);
                }
                catch
                {
                    _idAcaoSelecionada = 0;
                }

                try
                {
                    txtServico.Text = item.descricao != null ? Convert.ToString(item.descricao) : (item.Descricao != null ? Convert.ToString(item.Descricao) : string.Empty);
                }
                catch
                {
                    txtServico.Text = string.Empty;
                }

                try
                {
                    decimal valor = 0;
                    if (item.valor_cobrado != null) valor = Convert.ToDecimal(item.valor_cobrado);
                    else if (item.ValorCobrado != null) valor = Convert.ToDecimal(item.ValorCobrado);

                    txtValor.Text = valor.ToString("F2");
                }
                catch
                {
                    txtValor.Text = "0,00";
                }
            }
        }

        private void LimparCampos()
        {
            _idAcaoSelecionada = 0;
            txtServico.Text = string.Empty;
            txtValor.Text = string.Empty;
        }

        private void txtValorServico_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.Contains(","))
            {
                e.Handled = true;
            }
        }
        
        private void ConfigurarCampoValor()
        {
            txtValor.Text = 0.ToString("C2");
            txtValor.TextAlign = HorizontalAlignment.Right;

            txtValor.TextChanged += TxtValor_TextChanged;
            txtValor.Click += TxtValor_Click;
        }

        private void TxtValor_TextChanged(object sender, EventArgs e)
        {
            txtValor.TextChanged -= TxtValor_TextChanged;

            string apenasNumeros = new string(txtValor.Text.Where(char.IsDigit).ToArray());

            if (decimal.TryParse(apenasNumeros, out decimal valorSemVirgula))
            {
                decimal valorFinal = valorSemVirgula / 100m;
                txtValor.Text = valorFinal.ToString("C2");
            }
            else
            {
                txtValor.Text = 0.ToString("C2");
            }

            txtValor.SelectionStart = txtValor.Text.Length;

            txtValor.TextChanged += TxtValor_TextChanged;
        }

        private void TxtValor_Click(object sender, EventArgs e)
        {
            txtValor.SelectionStart = txtValor.Text.Length;
        }
    }
}