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
using Refit;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;
using Exception = System.Exception;
using Font = System.Drawing.Font;
using Image = iTextSharp.text.Image;


namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class Editar_Pedido : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int id;
        private string uf;
        private bool okCep;
        private Pedido _pedido;

        public Editar_Pedido(Pedido pedido)
        {
            InitializeComponent();
            _pedido = pedido;
        }


        private void Editar_Pedido_Load(object sender, EventArgs e)
        {
            txtId.Text = _pedido.id_pedido.ToString();
            CarregarDadosPedido();
        }

        #region Desing Moderno

        private void ApplyModernDesign()
        {
            try
            {
                // Propriedades do formulário (específicas deste form)
                this.BackColor = Color.FromArgb(240, 240, 240);
                this.Font = new Font("Segoe UI", 9F);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.StartPosition = FormStartPosition.CenterScreen;

                // Estilo dos painéis (específicos deste form)
                panel1.BackColor = Color.FromArgb(39, 54, 77);
                panel2.BackColor = Color.FromArgb(32, 45, 64);

                // Estilo das labels: Usando o método estático
                DesingComponentes.ApplyLabelStyles(this);

                // Estilo dos cabeçalhos de seção (específicos deste form)
                label1.Font = new Font("Segoe UI Semibold", 14F);
                label1.ForeColor = Color.White;

                // Estilo das caixas de texto com máscara: Usando o método estático para cada controle


                // Estilo dos botões: Usando o método estático para cada controle
                DesingComponentes.StyleButton(btnSalvar, Color.FromArgb(0, 120, 215));
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

        #region Metodos de Manipulação de Dados

        private void CarregarDadosPedido()
        {
            try
            {
                con.OpenConnection();

                string sql = @"
                    SELECT 
                        p.id_pedido,
                        c.nome AS cliente,
                        u.nome AS tecnico,
                        e.descricao AS equipamento,
                        p.status,
                        p.data_abertura,
                        p.data_atualizacao,
                        p.data_fechamento,
                        p.valor_mao_obra,
                        p.valor_pecas,
                        p.valor_total,
                        p.problema_relatado,
                        p.diagnostico,
                        p.observacoes
                    FROM pedidos p
                    LEFT JOIN clientes c ON p.id_cliente = c.id_cliente
                    LEFT JOIN usuarios u ON p.id_tecnico = u.id_usuario
                    LEFT JOIN equipamentos e ON p.id_equipamento = e.id_equipamento
                    WHERE p.id_pedido = @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
                {
                    cmd.Parameters.AddWithValue("@id", _pedido.id_pedido);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtCliente.Text = dr["cliente"]?.ToString();
                            txtTecnico.Text = dr["tecnico"]?.ToString();
                            txtEquipamento.Text = dr["equipamento"]?.ToString();
                            txtStatus.Text = dr["status"]?.ToString();

                            txtDataAbertura.Text = dr["data_abertura"] == DBNull.Value
                                ? ""
                                : Convert.ToDateTime(dr["data_abertura"]).ToString("dd/MM/yyyy");

                            txtUltimaAtualizacao.Text = dr["data_atualizacao"] == DBNull.Value
                                ? ""
                                : Convert.ToDateTime(dr["data_atualizacao"]).ToString("dd/MM/yyyy");

                            txtValorMaoObra.Text = dr["valor_mao_obra"]?.ToString();
                            txtValorPecas.Text = dr["valor_pecas"]?.ToString();
                            txtValorTotal.Text = dr["valor_total"]?.ToString();

                            txtProblema.Text = dr["problema_relatado"]?.ToString();
                            txtDiagnostico.Text = dr["diagnostico"]?.ToString();
                            txtObservacoes.Text = dr["observacoes"]?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados do pedido:\n" + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ImprimirPedidoPDF()
        {
            try
            {
                con.OpenConnection();

                // Query ajustada conforme as imagens das tabelas (PK de usuarios é id_usuario)
                string sql = @"
                    SELECT 
                        p.id_pedido, 
                        p.status, 
                        p.data_abertura, 
                        p.data_atualizacao, 
                        p.data_fechamento,
                        p.valor_mao_obra, 
                        p.valor_pecas, 
                        p.valor_total,
                        p.problema_relatado, 
                        p.diagnostico,
                        p.observacoes,
                        c.nome AS cliente_nome, 
                        c.cpf AS cliente_cpf, 
                        c.telefone AS cliente_telefone,
                        u.nome AS tecnico_nome, 
                        u.telefone AS tecnico_telefone,
                        u.cpf AS tecnico_cpf,
                        e.descricao AS equipamento
                    FROM pedidos p
                    LEFT JOIN clientes c ON p.id_cliente = c.id_cliente
                    LEFT JOIN usuarios u ON p.id_tecnico = u.id_usuario
                    LEFT JOIN equipamentos e ON p.id_equipamento = e.id_equipamento
                    WHERE p.id_pedido = @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
                {
                    cmd.Parameters.AddWithValue("@id", _pedido.id_pedido);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            MessageBox.Show("Pedido não encontrado.");
                            return;
                        }

                        // Configuração do local de salvamento (Desktop)
                        string caminhoArquivo = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                            $"Pedido_{dr["id_pedido"]}.pdf"
                        );

                        Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminhoArquivo, FileMode.Create));
                        doc.Open();

                        // Fontes
                        var fTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        var fSub = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                        var fNormal = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                        
                        //Logo
                        string pastaLogos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img");
                        string caminhoLogo = Path.Combine(pastaLogos, "logopng.png");
                        Image logo = Image.GetInstance(caminhoLogo);
                        logo.ScaleAbsolute(100f, 100f); 
                        logo.Alignment = Element.ALIGN_LEFT;
                        logo.SpacingAfter = 10f;
                        doc.Add(logo);
                        
                        // Cabeçalho
                        Paragraph pHeader = new Paragraph("ASSISTEC - ORDEM DE SERVIÇO", fTitulo);
                        pHeader.Alignment = Element.ALIGN_CENTER;
                        pHeader.SpacingAfter = 20;
                        doc.Add(pHeader);
                        
                        

                        // --- SEÇÃO CLIENTE ---
                        doc.Add(new Paragraph("DADOS DO CLIENTE", fSub));
                        doc.Add(new Paragraph($"Nome: {dr["cliente_nome"]}", fNormal));
                        doc.Add(new Paragraph($"CPF: {dr["cliente_cpf"]}", fNormal));
                        doc.Add(new Paragraph($"Telefone: {dr["cliente_telefone"]}\n\n", fNormal));
                        
                        // --- SEÇÃO Tecnico ---
                        doc.Add(new Paragraph("DADOS DO TÉCNICO", fSub));
                        doc.Add(new Paragraph($"Técnico Responsável: {dr["tecnico_nome"]}", fNormal));
                        doc.Add(new Paragraph($"CPF: {dr["tecnico_cpf"]}", fNormal));
                        doc.Add(new Paragraph($"Telefone: {dr["tecnico_telefone"]}\n\n", fNormal));
                        
                        // --- SEÇÃO EQUIPAMENTO/PEDIDO ---
                        doc.Add(new Paragraph("DETALHES DO SERVIÇO", fSub));
                        doc.Add(new Paragraph($"Equipamento: {dr["equipamento"]}", fNormal));
                        doc.Add(new Paragraph($"Status Atual: {dr["status"]}", fNormal));
                        doc.Add(new Paragraph($"Problema: {dr["problema_relatado"]}", fNormal));
                        doc.Add(new Paragraph($"Diagnóstico: {dr["diagnostico"] ?? "Em análise"}\n\n", fNormal));
                        

                        // --- TABELA DE DATAS E VALORES ---
                        PdfPTable tabela = new PdfPTable(2);
                        tabela.WidthPercentage = 100;
                        tabela.SetWidths(new float[] { 1, 1 });
                        

                        // Datas (Tratando possíveis nulos do banco)
                        tabela.AddCell(new Phrase("Data Abertura", fSub));
                        tabela.AddCell(new Phrase(dr["data_abertura"] != DBNull.Value ? Convert.ToDateTime(dr["data_abertura"]).ToString("dd/MM/yyyy HH:mm") : "-", fNormal));

                        tabela.AddCell(new Phrase("Última Atualização", fSub));
                        tabela.AddCell(new Phrase(dr["data_atualizacao"] != DBNull.Value ? Convert.ToDateTime(dr["data_atualizacao"]).ToString("dd/MM/yyyy HH:mm") : "-", fNormal));

                        // Valores
                        decimal maoObra = dr["valor_mao_obra"] != DBNull.Value ? Convert.ToDecimal(dr["valor_mao_obra"]) : 0;
                        decimal pecas = dr["valor_pecas"] != DBNull.Value ? Convert.ToDecimal(dr["valor_pecas"]) : 0;
                        decimal total = dr["valor_total"] != DBNull.Value ? Convert.ToDecimal(dr["valor_total"]) : (maoObra + pecas);

                        tabela.AddCell(new Phrase("Valor Mão de Obra", fSub));
                        tabela.AddCell(new Phrase(maoObra.ToString("C2"), fNormal));

                        tabela.AddCell(new Phrase("Valor Peças", fSub));
                        tabela.AddCell(new Phrase(pecas.ToString("C2"), fNormal));

                        tabela.AddCell(new Phrase("VALOR TOTAL", fSub));
                        tabela.AddCell(new Phrase(total.ToString("C2"), fSub));

                        doc.Add(tabela);

                        // Observações finais
                        if (dr["observacoes"] != DBNull.Value)
                        {
                            doc.Add(new Paragraph("\nObservações:", fSub));
                            doc.Add(new Paragraph(dr["observacoes"].ToString(), fNormal));
                        }

                        doc.Close();
                        writer.Close();

                        // Abre o arquivo após gerar
                        Process.Start(new ProcessStartInfo(caminhoArquivo) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro crítico ao gerar PDF:\n" + ex.Message);
            }
            finally
            {
                con.CloseConnection();
            }
        }

            #endregion
            
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            ImprimirPedidoPDF();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
