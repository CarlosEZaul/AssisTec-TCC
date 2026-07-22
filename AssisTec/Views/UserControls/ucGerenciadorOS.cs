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
            _ordemServicoService = ordemServico ?? throw new ArgumentNullException(nameof(ordemServico));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
        }

        #region Funções e Métodos

        private void AtualizarGrid()
        {
            
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