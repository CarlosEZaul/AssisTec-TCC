using System;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.UserControls;
using Guna.UI2.WinForms;

namespace AssisTec
{
    public partial class FrmPrincipal : Form
    {
        private Guna2Button botaoAtivo;
        
        ucGerenciador_Usuario ucUsuarios = new ucGerenciador_Usuario();
        ucGerenciador_Clientes ucClientes = new ucGerenciador_Clientes();
        ucGerenciadorEstoque ucEstoque = new ucGerenciadorEstoque();
        //ucProdutos ucProdutos = new ucProdutos();
        //ucGerenciadorPedidos ucPedidos = new ucGerenciadorPedidos();

        public FrmPrincipal()
        {
            InitializeComponent();
            ConfigurarNavbar();
        }

        private void ConfigurarNavbar()
        {
            panelNavegacao.Dock = DockStyle.Left;
            panelNavegacao.Width = 230;
            panelNavegacao.BackColor = Color.FromArgb(45, 45, 48);

            panelNavegacao.Controls.Clear();

            // Botão Usuários
            Guna2Button btnUsuario = CriarBotaoMenu(
                "👤 Usuários",
                (s, e) => AbrirUserControl(ucUsuarios, s)
            );

            
            
            // Botão Clientes
            Guna2Button btnClientes = CriarBotaoMenu(
                "👥 Clientes",
                (s, e) => AbrirUserControl(ucClientes, s)
            );
            
            
            // Botão Estoque
            Guna2Button btnEstoque = CriarBotaoMenu(
                "📦 Estoque",
                (s, e) => AbrirUserControl(ucEstoque, s)
            );
            
            //panelNavegacao.Controls.Add(btnProdutos);
            
            //Botão OS
            Guna2Button btnPedidos = CriarBotaoMenu(
                "📨 Ordens de Serviço",
                (s, e) => AbrirUserControl(new ucGerenciadorOS(), s)
            );
            
            
            
            
            // Logo
            Label lblLogo = new Label
            {
                Text = "ASSISTEC",
                Anchor = AnchorStyles.Left  | AnchorStyles.Top,
                Dock = DockStyle.Top,
                Height = 80,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelNavegacao.Controls.Add(btnEstoque);
            panelNavegacao.Controls.Add(btnPedidos);
            panelNavegacao.Controls.Add(btnClientes);
            panelNavegacao.Controls.Add(btnUsuario);
            panelNavegacao.Controls.Add(lblLogo);
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

            panelConteudo.Controls.Clear(); // sempre limpa

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
        
    }
}
