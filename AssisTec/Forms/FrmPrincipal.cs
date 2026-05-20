using System;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.UserControls;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes;
using Guna.UI2.WinForms;

namespace AssisTec
{
    public partial class FrmPrincipal : Form
    {
        private Guna2Button botaoAtivo;

        Panel panelUsuario;
        Panel linha;
        Label lblNome;
        Label lblFuncao;
        Label lblStatus;
        Guna2Button btnLogout;

        public FrmPrincipal()
        {
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            ConfigurarPanelUsuario();
            ConfigurarNavbar();
        }

        private void ConfigurarPanelUsuario()
        {
            string[] partesNome = Sessao.usuarioLogado.nome.Trim().Split(' ');

            string nomeExibicao = partesNome.Length >= 2 ? $"{partesNome[0]} {partesNome[1]}" : partesNome[0];
            
            panelUsuario = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                BackColor = Color.FromArgb(35, 35, 38),
                Padding = new Padding(10)
            };

            linha = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(60, 60, 65)
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
                (s, e) => AbrirUserControl(new ucHome(), s));

            Guna2Button btnUsuario = CriarBotaoMenu(
                "👤 Usuários",
                (s, e) => AbrirUserControl(new ucGerenciador_Usuario(), s)
            );

            Guna2Button btnClientes = CriarBotaoMenu(
                "👥 Clientes",
                (s, e) => AbrirUserControl(new ucGerenciador_Clientes(), s)
            );

            Guna2Button btnEstoque = CriarBotaoMenu(
                "📦 Estoque",
                (s, e) => AbrirUserControl(new ucGerenciadorEstoque(), s)
            );

            Guna2Button btnPedidos = CriarBotaoMenu(
                "📨 Ordens de Serviço",
                (s, e) => AbrirUserControl(new ucGerenciadorOS(), s)
            );

            Guna2Button btnContasReceber = CriarBotaoMenu(
                "💰 Contas a receber",
                (s, e) => AbrirUserControl(new ucContasReceber(), s)
            );

            Guna2Button btnContasPagar = CriarBotaoMenu(
                "🧾 Contas a pagar",
                (s, e) => AbrirUserControl(new ucContasPagar(), s)
            );

            Guna2Button btnBackupImportar = CriarBotaoMenu(
                "☁︎Backup/Importar",
                (s, e) => AbrirUserControl(new ucBackupImportar(), s)
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

        #region Usuario

        private string ObterFuncao(int nivel)
        {
            if (nivel == 1)
            {
                return "Gerente";
            }
            else if (nivel == 2)
            {
                return "Atendente";
            }
            else
            {
                return "Técnico";
            }
        }

        #endregion

        private void AbrirUserControl(UserControl uc, object btnSender)
        {
            AtivarBotao(btnSender);

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

        private void funcaoLogOut(Object sender, EventArgs e)
        {
            Sessao.usuarioLogado = null;
            Application.Restart();
        }
    }
}