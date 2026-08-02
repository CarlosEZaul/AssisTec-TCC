using System;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Repository;
using AssisTec.Service;
using AssisTec.UserControls;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes;
using Guna.UI2.WinForms;

namespace AssisTec
{
    public partial class FrmPrincipal : Form
    {
        private Guna2Button botaoAtivo;
        private readonly ContasReceberService _contasReceberService;
        private readonly PagamentoService _pagamentoService;
        private readonly ContasPagarService _contasPagarService;
        private readonly ProdutoService _produtoService;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private readonly OrdemServicoService _ordemServicoService;
        private readonly UsuarioService _usuarioService;
        private readonly  ClienteService _clienteService;
        
        Panel panelUsuario;
        Label lblNome;
        Label lblFuncao;
        Label lblStatus;
        Guna2Button btnLogout;

        public FrmPrincipal()
        {
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            
            var context = new AppDbContext();
            
            var contasReceberRepository = new ContasReceberRepository(context);
            var pagamentoRepository = new PagamentoRepository(context);
            var clienteRepository = new ClienteRepository(context);
            var usuarioRepository = new UsuarioRepository(context);
            var contasPagarRepository = new ContasPagarRepository(context);
            var produtoRepository = new ProdutoRepository(context);
            var movimentacaoEstoqueRepository = new MovimentacaoEstoqueRepository(context);
            var OrdemServicoRepository = new OrdemServicoRepository(context);
            var EquipamentoRepository = new EquipamentoRepository(context);
            var ItemOSRepository = new ItemOSRepository(context);
            var ServicosOSRepository = new ServicosOSRepository(context);
            var HistoricoAlteracaoOSRepository = new HistoricoAlteracaoOSRepository(context);
            var OrdemServicoRepostory =  new OrdemServicoRepository(context);

            _usuarioService = new UsuarioService(usuarioRepository, OrdemServicoRepostory);
            _clienteService =  new ClienteService(clienteRepository, OrdemServicoRepostory);
            _produtoService = new ProdutoService(produtoRepository);
            _contasPagarService = new ContasPagarService(contasPagarRepository, pagamentoRepository);
            _contasReceberService = new ContasReceberService(contasReceberRepository, pagamentoRepository, OrdemServicoRepository);
            _pagamentoService = new PagamentoService(contasReceberRepository, contasPagarRepository, pagamentoRepository);
            _movimentacaoEstoqueService = new MovimentacaoEstoqueService(movimentacaoEstoqueRepository);
            _ordemServicoService = new OrdemServicoService(OrdemServicoRepository,
                EquipamentoRepository,
                usuarioRepository,
                clienteRepository,
                ItemOSRepository,
                ServicosOSRepository,
                produtoRepository,
                movimentacaoEstoqueRepository,
                HistoricoAlteracaoOSRepository,
                contasReceberRepository,
                pagamentoRepository);

            ConfigurarPanelUsuario();
            ConfigurarNavbar();
            AbrirUserControl(new ucHome(_ordemServicoService, _produtoService, _movimentacaoEstoqueService, _contasPagarService, _contasReceberService), null);
        }

        private void ConfigurarPanelUsuario()
        {
            string[] partesNome = Sessao.usuarioLogado.Nome.Trim().Split(' ');

            string nomeExibicao = partesNome.Length >= 2 ? $"{partesNome[0]} {partesNome[1]}" : partesNome[0];

            panelUsuario = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                BackColor = Color.FromArgb(35, 35, 38),
                Padding = new Padding(10)
            };

            lblNome = new Label
            {
                Text = nomeExibicao,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            lblFuncao = new Label
            {
                Text = ObterFuncao(Sessao.usuarioLogado.Nivel),
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 9),
                Location = new Point(15, 40),
                AutoSize = true
            };

            lblStatus = new Label
            {
                Text = "● Online",
                ForeColor = Color.LimeGreen,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(15, 62),
                AutoSize = true
            };

            btnLogout = new Guna2Button
            {
                Animated = false,
                Text = "↩ Logout",
                Dock = DockStyle.Bottom,
                Width = 30,
                Height = 30,
                FillColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                TextOffset = new Point(20, 0),
                BorderRadius = 5
            };

            btnLogout.Click += funcaoLogOut;
        }

        private void ConfigurarNavbar()
        {
            panelNavegacao.Dock = DockStyle.Left;
            panelNavegacao.Width = 230;
            panelNavegacao.BackColor = Color.FromArgb(45, 45, 48);

            panelNavegacao.Controls.Clear();

            Guna2Button btnHome = CriarBotaoMenu(
                "🏠 Home",
                (s, e) => AbrirUserControl(new ucHome(_ordemServicoService, _produtoService, _movimentacaoEstoqueService, _contasPagarService, _contasReceberService), s));

            Guna2Button btnUsuario = CriarBotaoMenu(
                "👤 Usuários",
                (s, e) => 
                {
                    if (!ValidarAcessoPermitido()) return;
                    AbrirUserControl(new ucGerenciador_Usuario(_usuarioService, _ordemServicoService), s);
                }
            );

            Guna2Button btnClientes = CriarBotaoMenu(
                "👥 Clientes",
                (s, e) => AbrirUserControl(new ucGerenciador_Clientes(_clienteService,_ordemServicoService), s)
            );

            Guna2Button btnEstoque = CriarBotaoMenu(
                "📦 Estoque",
                (s, e) => 
                {
                    if (!ValidarAcessoEstoque()) return;
                    AbrirUserControl(new ucGerenciadorEstoque(_produtoService, _movimentacaoEstoqueService, _contasPagarService, _contasReceberService), s);
                }
            );

            Guna2Button btnPedidos = CriarBotaoMenu(
                "📨 Ordens de Serviço",
                (s, e) => AbrirUserControl(new ucGerenciadorOS(_ordemServicoService), s)
            );

            Guna2Button btnContasReceber = CriarBotaoMenu(
                "💰 Contas a receber",
                (s, e) => 
                {
                    if (!ValidarAcessoPermitido()) return;
                    AbrirUserControl(new ucContasReceber(_contasReceberService, _pagamentoService), s);
                }
            );

            Guna2Button btnContasPagar = CriarBotaoMenu(
                "🧾 Contas a pagar",
                (s, e) => 
                {
                    if (!ValidarAcessoPermitido()) return;
                    AbrirUserControl(new ucContasPagar(_contasPagarService, _pagamentoService), s);
                }
            );

            Guna2Button btnBackupImportar = CriarBotaoMenu(
                "☁︎Backup/Importar",
                (s, e) => 
                {
                    if (!ValidarAcessoPermitido()) return;
                    AbrirUserControl(new ucBackupImportar(), s);
                }
            );

            Label lblLogo = new Label
            {
                Text = "ASSISTEC",
                Anchor = AnchorStyles.Top,
                Dock = DockStyle.Top,
                Height = 80,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(20, 0, 0, 0),
            };

            panelNavegacao.Controls.Add(btnBackupImportar);
            panelNavegacao.Controls.Add(btnContasPagar);
            panelNavegacao.Controls.Add(btnContasReceber);
            panelNavegacao.Controls.Add(btnEstoque);
            panelNavegacao.Controls.Add(btnPedidos);
            panelNavegacao.Controls.Add(btnClientes);
            panelNavegacao.Controls.Add(btnUsuario);
            panelNavegacao.Controls.Add(btnHome);
            panelNavegacao.Controls.Add(lblLogo);

            panelUsuario.Controls.Add(lblNome);
            panelUsuario.Controls.Add(lblFuncao);
            panelUsuario.Controls.Add(lblStatus);
            panelUsuario.Controls.Add(btnLogout);
            panelNavegacao.Controls.Add(panelUsuario);
        }

        private bool ValidarAcessoPermitido()
        {
            if (Sessao.usuarioLogado != null && (Sessao.usuarioLogado.Nivel == 2 || Sessao.usuarioLogado.Nivel == 3))
            {
                MessageBox.Show("Acesso Negado! Seu nível de usuário não tem permissão para acessar este módulo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarAcessoEstoque()
        {
            if (Sessao.usuarioLogado != null && Sessao.usuarioLogado.Nivel == 3)
            {
                MessageBox.Show("Acesso Negado! Técnicos não possuem permissão para gerenciar o módulo de Estoque.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private Guna2Button CriarBotaoMenu(string texto, EventHandler eventoClick)
        {
            Guna2Button btn = new Guna2Button
            {
                Text = texto,
                Dock = DockStyle.Top,
                Height = 55,
                FillColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Left,
                TextOffset = new Point(20, 0),
                Animated = true,
                BorderRadius = 5
            };

            btn.HoverState.FillColor = Color.FromArgb(60, 60, 65);
            btn.Click += eventoClick;

            return btn;
        }

        private void AbrirUserControl(UserControl uc, object btnSender)
        {
            AtivarBotao(btnSender);

            foreach (Control control in panelConteudo.Controls)
            {
                control.Dispose();
            }
            panelConteudo.Controls.Clear();

            uc.Dock = DockStyle.Fill;

            panelConteudo.Controls.Add(uc);
            uc.BringToFront();
        }

        private void AtivarBotao(object btnSender)
        {
            if (btnSender is Guna2Button btn && botaoAtivo != btn)
            {
                DesativarBotoes();

                botaoAtivo = btn;
                botaoAtivo.FillColor = Color.FromArgb(94, 148, 255);
                botaoAtivo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            }
        }

        private void DesativarBotoes()
        {
            foreach (Control control in panelNavegacao.Controls)
            {
                if (control is Guna2Button btn)
                {
                    btn.FillColor = Color.Transparent;
                    btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
            }
        }

        #region Usuario

        private string ObterFuncao(int nivel)
        {
            switch (nivel)
            {
                case 1:
                    return "Gerente";
                case 2:
                    return "Atendente";
                case 3:
                    return "Técnico";
                default:
                    return "";
            }
        }

        private void funcaoLogOut(Object sender, EventArgs e)
        {
            Sessao.usuarioLogado = null;
            Application.Restart();
        }

        #endregion
    }
}