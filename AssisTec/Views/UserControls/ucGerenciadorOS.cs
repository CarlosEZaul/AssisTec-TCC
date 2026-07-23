using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;
using AssisTec.SubForms_do_Gerenciador_de_Pedidos;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls
{
    public partial class ucGerenciadorOS : UserControl
    {
        private readonly OrdemServicoService  _ordemServicoService;
        private readonly UsuarioService _usuarioService;
        private readonly ClienteService _clienteService;
        public ucGerenciadorOS(OrdemServicoService ordemServico, UsuarioService usuarioService, ClienteService clienteService)
        {
            InitializeComponent();
            DesignModerno();
            _ordemServicoService = ordemServico ?? throw new ArgumentNullException(nameof(ordemServico));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
            AtualizarGrid();
            
        }

        #region Design

        private void DesignModerno()
        {
            DesignComponentes.StyleDataGridView(dgvOS);
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
            dgvOS.Columns[6].HeaderText = "Data de Conclusão";
            dgvOS.Columns[7].HeaderText = "Valor Total";
            
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
            ConfigurarSubComponente(new ucFormularioOS(_ordemServicoService,_clienteService, _usuarioService));
        }
    }
}