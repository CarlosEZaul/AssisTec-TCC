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
            btnUsuario.Text = "👤 Gerenciar Usuário";
            btnUsuario.Width = 160;
            btnUsuario.Height = 50;
            btnUsuario.Location = new System.Drawing.Point(0, 0);
            btnUsuario.FillColor = System.Drawing.Color.FromArgb(94, 148, 255);
            btnUsuario.ForeColor = System.Drawing.Color.White;
            btnUsuario.Font = new System.Drawing.Font("Segoe UI Emoji", 10F, System.Drawing.FontStyle.Bold);
            btnUsuario.Click += (s, e) => AbrirForm(new Gerenciador_Usuarios());

            // ======== BOTÃO GERENCIAR CLIENTE ========
            Guna2Button btnCliente = new Guna2Button();
            btnCliente.Text = "Gerenciar Cliente";
            btnCliente.Width = 160;
            btnCliente.Height = 50;
            btnCliente.Location = new System.Drawing.Point(160, 0);
            btnCliente.FillColor = System.Drawing.Color.FromArgb(94, 148, 255);
            btnCliente.ForeColor = System.Drawing.Color.White;
            btnCliente.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnCliente.Click += (s, e) => AbrirForm(new Gerenciador_Clientes());

            // Adiciona os botões lado a lado no painel
            panelNavegacao.Controls.Add(btnCliente);
            panelNavegacao.Controls.Add(btnUsuario);
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
