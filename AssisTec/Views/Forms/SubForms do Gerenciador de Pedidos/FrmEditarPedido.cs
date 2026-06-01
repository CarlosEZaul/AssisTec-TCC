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
using Exception = System.Exception;
using Font = System.Drawing.Font;
using Image = iTextSharp.text.Image;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class FrmEditarPedido : Form
    {
        private ucDetalhesPedidos detalhes; 
        private ucProdutosUtilizados produtos;
        private ucServicos servicos;
        OrdemServico ordemServico;
        private int id;
        
        public FrmEditarPedido(OrdemServico _ordemServico)
        {
            InitializeComponent();
            ApplyModernDesign();
            ordemServico = _ordemServico;
        }

        private void FrmEditarPedido_Load(object sender, EventArgs e)
        {
            //detalhes = new ucDetalhesPedidos(ordemServico);
            //produtos = new ucProdutosUtilizados(ordemServico);
            //servicos = new ucServicos(ordemServico);
            detalhes.Dock = DockStyle.Fill;
            produtos.Dock = DockStyle.Fill;
            panelConteudo.Controls.Add(detalhes);
            panelConteudo.Controls.Add(produtos);
            panelConteudo.Controls.Add(servicos);
            MostrarTela(detalhes);
        }
        private void MostrarTela(UserControl tela)
        {
            foreach (Control ctrl in panelConteudo.Controls)
                ctrl.Visible = false;

            tela.Visible = true;
            tela.BringToFront();
        }

        #region Desing Moderno

        private void ApplyModernDesign()
        {
            try
            {
                // Propriedades do formulário (específicas deste form)
                this.BackColor = Color.FromArgb(240, 240, 240);
                this.Font = new Font("Segoe UI", 9F);
                

                // Estilo dos painéis (específicos deste form)
                //panel1.BackColor = Color.FromArgb(39, 54, 77);
                panel2.BackColor = Color.FromArgb(32, 45, 64);

                // Estilo das labels: Usando o método estático
                //DesingComponentes.ApplyLabelStyles(this);

                // Estilo das caixas de texto com máscara: Usando o método estático para cada controle


                // Estilo dos botões: Usando o método estático para cada controle
                
                ;
                DesingComponentes.StyleButton(btnImprimir, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnFechar, Color.FromArgb(209, 17, 65));
                // ... (outros Buttons)


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion
        
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            
        }

        

        private void btnDetalhes_Click(object sender, EventArgs e)
        {
            MostrarTela(detalhes);
        }


        private void btnProdutos_Click(object sender, EventArgs e)
        {
            MostrarTela(produtos);
        }

        private void btnServiços_Click(object sender, EventArgs e)
        {
            MostrarTela(servicos);
        }
    }
}