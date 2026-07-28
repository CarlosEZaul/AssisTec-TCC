using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.Views.UserControls.SubUserControl_do_Gerenciador_de_OS
{
    public partial class ucHistoricoAlteracao : UserControl
    {
        private OrdemServicoService _historicoAlteracaoOSService;
        private int _idOS;

        public ucHistoricoAlteracao(OrdemServicoService ordemServicoService, int idOs)
        {
            InitializeComponent();
            _historicoAlteracaoOSService = ordemServicoService;
            _idOS = idOs;

            if (!DesignMode)
            {
                CarregarHistorico(_idOS);
            }

            DesignModerno();
        }

        #region Design

        private void DesignModerno()
        {
            DesignComponentes.StyleDataGridView(dgvHistoricoOS);
            //DesignComponentes.centralizarPanel(panel1, panel3.Width);
        }

        #endregion

        #region Funcoes

        private void CarregarHistorico(int idOS)
        {
            _idOS = idOS;

            if (_idOS <= 0 || _historicoAlteracaoOSService == null) return;

            try
            {
                var consulta = _historicoAlteracaoOSService.ObterPorOrdemServico(_idOS);
                

                dgvHistoricoOS.DataSource = null;
                dgvHistoricoOS.AutoGenerateColumns = true;
                dgvHistoricoOS.DataSource = consulta;

                dgvHistoricoOS.BindingContext = new BindingContext();

                FormatadorGrid();
                dgvHistoricoOS.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar histórico: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatadorGrid()
        {
            if (dgvHistoricoOS.Columns.Count == 0) return;

            dgvHistoricoOS.ReadOnly = true;
            dgvHistoricoOS.AllowUserToAddRows = false;
            dgvHistoricoOS.AllowUserToDeleteRows = false;
            dgvHistoricoOS.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistoricoOS.MultiSelect = false;

            if (dgvHistoricoOS.Columns.Contains("ID")) 
                dgvHistoricoOS.Columns["ID"].Visible = false;

            if (dgvHistoricoOS.Columns.Contains("DataAlteracao"))
            {
                dgvHistoricoOS.Columns["DataAlteracao"].HeaderText = "Data/Hora";
                dgvHistoricoOS.Columns["DataAlteracao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvHistoricoOS.Columns["DataAlteracao"].Width = 130;
            }

            if (dgvHistoricoOS.Columns.Contains("Tipo"))
            {
                dgvHistoricoOS.Columns["Tipo"].HeaderText = "Tipo";
                dgvHistoricoOS.Columns["Tipo"].Width = 120;
            }

            if (dgvHistoricoOS.Columns.Contains("Descricao"))
            {
                dgvHistoricoOS.Columns["Descricao"].HeaderText = "Descrição";
                dgvHistoricoOS.Columns["Descricao"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvHistoricoOS.Columns.Contains("Usuario"))
            {
                dgvHistoricoOS.Columns["Usuario"].HeaderText = "Usuário";
                dgvHistoricoOS.Columns["Usuario"].Width = 150;
            }
        }

        #endregion

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}