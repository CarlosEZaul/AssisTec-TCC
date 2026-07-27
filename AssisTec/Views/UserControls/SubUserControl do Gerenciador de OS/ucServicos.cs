using System;
using System.Collections.Generic;
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
        }
        

        private void DesignModerno()
        {
            DesignComponentes.StyleDataGridView(dgvServicosOS);
            dgvServicosOS.ReadOnly = true;
            dgvServicosOS.AllowUserToAddRows = false;
            dgvServicosOS.AllowUserToDeleteRows = false;
            dgvServicosOS.EditMode = DataGridViewEditMode.EditProgrammatically;
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
            if (dgvServicosOS.Columns["idAcao"] != null)
                dgvServicosOS.Columns["idAcao"].Visible = false;

            if (dgvServicosOS.Columns["id_OS"] != null)
                dgvServicosOS.Columns["id_OS"].Visible = false;

            if (dgvServicosOS.Columns["OrdemServico"] != null)
                dgvServicosOS.Columns["OrdemServico"].Visible = false;

            if (dgvServicosOS.Columns["descricao"] != null)
                dgvServicosOS.Columns["descricao"].HeaderText = "Serviço / Ação";

            if (dgvServicosOS.Columns["valor_cobrado"] != null)
            {
                dgvServicosOS.Columns["valor_cobrado"].HeaderText = "Valor (R$)";
                dgvServicosOS.Columns["valor_cobrado"].DefaultCellStyle.Format = "C2";
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

            if (!decimal.TryParse(txtValorServico.Text, out decimal valorCobrado) || valorCobrado < 0)
            {
                MessageBox.Show("Informe um valor válido para o serviço.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtValorServico.Focus();
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

                bool sucesso = _ordemServicoService.SalvarServicoOS(servico);
                if (sucesso)
                {
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

            if (row.DataBoundItem is ServicosOS acao)
            {
                _idAcaoSelecionada = acao.idServico;
                txtServico.Text = acao.descricao;
                txtValorServico.Text = acao.valor_cobrado.ToString("F2");
            }
        }

        private void LimparCampos()
        {
            _idAcaoSelecionada = 0;
            txtServico.Text = string.Empty;
            txtValorServico.Text = string.Empty;
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
    }
}