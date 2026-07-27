using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.AtendeClienteService;
using AssisTec.SubForms_do_Gerenciador_de_Pedidos;
using MySql.Data.MySqlClient;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;
using AssisTec.Models;
using AssisTec.Service;
using AssisTec.Views.UserControls.SubUserControl_do_Gerenciador_de_OS;
using Exception = System.Exception;
using Font = System.Drawing.Font;
using Image = iTextSharp.text.Image;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class FrmGerenciarOS : Form
    {
        
        
        private readonly OrdemServicoService _ordemServicoService;
        private int _id;
        private ucDetalhesOS detalhes;
        private ucProdutosUtilizados produtos;
        private ucServicos servicos;
        public FrmGerenciarOS(int id,OrdemServicoService ordemServicoService)
        {
            InitializeComponent();
            _ordemServicoService = ordemServicoService ?? throw new ArgumentNullException(nameof(ordemServicoService));
            _id = id;
            IniciarUserControls();
            ApplyModernDesign();
           
        }

        
        private void FrmGerenciarOS_Load(object sender, EventArgs e)
        {
            MostrarTela(detalhes);
        }

        
        #region Desing Moderno
        

        private void ApplyModernDesign()
        {
            try
            {
                this.BackColor = Color.FromArgb(240, 240, 240);
                this.Font = new Font("Segoe UI", 9F);
                panel2.BackColor = Color.FromArgb(32, 45, 64);
                
                DesignComponentes.StyleButton(btnImprimir, Color.FromArgb(0, 120, 215));
                DesignComponentes.StyleButton(btnFechar, Color.FromArgb(209, 17, 65));
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion
        private void IniciarUserControls()
        {
            detalhes = new ucDetalhesOS(_id, _ordemServicoService);
            produtos = new ucProdutosUtilizados(_ordemServicoService, _id);
            servicos = new ucServicos(_ordemServicoService, _id);
            
            detalhes.Dock = DockStyle.Fill;
            produtos.Dock = DockStyle.Fill;
            servicos.Dock = DockStyle.Fill;

            panelConteudo.Controls.Add(detalhes);
            panelConteudo.Controls.Add(produtos);
            panelConteudo.Controls.Add(servicos);
        }
        
        
        
        private void MostrarTela(UserControl tela)
        {
            if (tela == null) return;

            foreach (Control ctrl in panelConteudo.Controls)
                ctrl.Visible = false;

            tela.Visible = true;
            tela.BringToFront();
        }

        private void MudarVisibilidadeBotoes(bool ativo)
        {
            btnSalvar.Visible = ativo;
        }
        
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            
        }

        

        private void btnDetalhes_Click(object sender, EventArgs e)
        {
            MudarVisibilidadeBotoes(true);
            MostrarTela(detalhes);
        }


        private void btnProdutos_Click(object sender, EventArgs e)
        {
            MudarVisibilidadeBotoes(false);
            produtos.AtualizarDados();
            MostrarTela(produtos);
        }

        private void btnServiços_Click(object sender, EventArgs e)
        {
            MudarVisibilidadeBotoes(false);
            MostrarTela(servicos);
        }

        public void Atualizar()
        {
            detalhes.CarregarDetalhesOS();
            produtos.AtualizarDados();
            servicos.AtualizarDados();
        }


        private void btnDesfazer_Click(object sender, EventArgs e)
        {
            Atualizar();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            detalhes.SalvarAlteracoes();
        }
    }
}