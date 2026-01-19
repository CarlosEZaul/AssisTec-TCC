using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace AssisTec
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            ConfigurarNavbar(); // cria a navbar ao iniciar
        }

        private void ConfigurarNavbar()
        {
            // Define estilo geral do painel de navegação
            panelNavegacao.Dock = DockStyle.Top;
            panelNavegacao.Height = 50;
            panelNavegacao.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);

            // ======== BOTÃO GERENCIAR USUÁRIO ========
            Guna2Button btnUsuario = new Guna2Button();
            btnUsuario.Text = "👤 Gerenciar Usuários/Técnicos";
            btnUsuario.Width = 160;
            btnUsuario.Height = 50;
            btnUsuario.Location = new System.Drawing.Point(0, 0);
            btnUsuario.FillColor = System.Drawing.Color.FromArgb(94, 148, 255);
            btnUsuario.ForeColor = System.Drawing.Color.White;
            btnUsuario.Font = new System.Drawing.Font("Segoe UI Emoji", 10F, System.Drawing.FontStyle.Bold);
            btnUsuario.Click += (s, e) => AbrirForm(new Gerenciador_Usuarios());

            // ======== BOTÃO GERENCIAR CLIENTE ========
            Guna2Button btnCliente = new Guna2Button();
            btnCliente.Text = "👨‍👩‍👧‍👦 Gerenciar Clientes";
            btnCliente.Width = 160;
            btnCliente.Height = 50;
            btnCliente.Location = new System.Drawing.Point(160, 0);
            btnCliente.FillColor = System.Drawing.Color.FromArgb(94, 148, 255);
            btnCliente.ForeColor = System.Drawing.Color.White;
            btnCliente.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCliente.Click += (s, e) => AbrirForm(new Gerenciador_Clientes());
            
            // ======== BOTÃO GERENCIAR PEDIDO ========
            Guna2Button btnPedido = new Guna2Button();
            btnPedido.Text = "📑 Gerenciar Pedidos";
            btnPedido.Width = 160;
            btnPedido.Height = 50;
            btnPedido.Location = new System.Drawing.Point(320, 0);
            btnPedido.FillColor = System.Drawing.Color.FromArgb(94, 148, 255);
            btnPedido.ForeColor = System.Drawing.Color.White;
            btnPedido.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnPedido.Click += (s, e) => AbrirForm(new Gerenciador_Pedidos());
            
            // ======== BOTÃO GERENCIAR PRODUTO ========
            Guna2Button btnProduto = new Guna2Button();
            btnProduto.Text = "📲 Produtos";
            btnProduto.Width = 160;
            btnProduto.Height = 50;
            btnProduto.Location = new System.Drawing.Point(480, 0);
            btnProduto.FillColor = System.Drawing.Color.FromArgb(94, 148, 255);
            btnProduto.ForeColor = System.Drawing.Color.White;
            btnProduto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnProduto.Click += (s, e) => AbrirForm(new FrmProduto());

            
            // Adiciona os botões lado a lado no painel
            panelNavegacao.Controls.Add(btnCliente);
            panelNavegacao.Controls.Add(btnUsuario);
            panelNavegacao.Controls.Add(btnPedido);
            panelNavegacao.Controls.Add(btnProduto);
        }

        private void AbrirForm(Form formFilho)
        {
            // Limpa o painel de conteúdo antes de abrir o novo form
            panelConteudo.Controls.Clear();

            // Configura o form para ser exibido dentro do painel
            formFilho.TopLevel = false;
            formFilho.FormBorderStyle = FormBorderStyle.None;
            formFilho.Dock = DockStyle.Fill;

            // Adiciona e mostra dentro do panelConteudo
            panelConteudo.Controls.Add(formFilho);
            formFilho.Show();
        }
    }
}
