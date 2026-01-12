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

namespace AssisTec
{
    public partial class Gerenciador_Pedidos : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int id;
        private string uf;
        private bool okCep;
        
        public Gerenciador_Pedidos()
        {
            InitializeComponent();
            ApplyModernDesign();
            
            
        }

        private void Gerenciador_Pedidos_Load(object sender, EventArgs e)
        {
            btnNew.Focus();
            listGrid();
            formatGrid();
        }
        #region Design Moderno

        private void ApplyModernDesign()
        {
            try
            {
                // Propriedades do formulário (específicas deste form)
                this.Text = "Gerenciador de Clientes";
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
                
                DesingComponentes.StyleTextBox(txtBusca);

                // Estilo das caixas de texto com máscara: Usando o método estático para cada controle
                

                // Estilo dos botões: Usando o método estático para cada controle
                DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnCancel, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnImprimir, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));
                // ... (outros Buttons)

                // Estilo do DataGridView: Usando o método estático (se o form tiver um DataGridView)
                DesingComponentes.StyleDataGridView(dgvPedidos, DataGridViewAutoSizeColumnsMode.AllCells); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
        
        #region Métodos de Manipulação de Dados

        private void enableBtn()
        {
            btnDelete.Enabled = true;
            btnGerenciar.Enabled = true;
            btnCancel.Enabled = true;
            btnImprimir.Enabled = true;
        }

        private void disableBtn()
        {
            btnDelete.Enabled = false;
            btnGerenciar.Enabled = false;
            btnCancel.Enabled = false;
            btnImprimir.Enabled = false;
        }
        
        private Pedido formPedido()
        {
            Pedido pedido = new Pedido();
            pedido.id_pedido = id;
            return pedido;
            
        }
        
        private void formatGrid()
        {
            if (dgvPedidos.Columns.Count > 0)
            {
                dgvPedidos.Columns[0].HeaderText = "ID PEDIDO";
                dgvPedidos.Columns[1].HeaderText = "CLIENTE";
                dgvPedidos.Columns[2].HeaderText = "TÉCNICO";
                dgvPedidos.Columns[3].HeaderText = "EQUIPAMENTO";
                dgvPedidos.Columns[4].HeaderText = "PROBLEMA_RELATADO";
                dgvPedidos.Columns[5].HeaderText = "DIAGNÓTICO";
                dgvPedidos.Columns[6].HeaderText = "STATUS";
                dgvPedidos.Columns[7].HeaderText = "DATA_ABERTURA";
                dgvPedidos.Columns[8].HeaderText = "DATA_ATUALIZAÇÃO";
                dgvPedidos.Columns[9].HeaderText = "DATA_FECHAMENTO";
                dgvPedidos.Columns[10].HeaderText = "VALOR_OBRA";
                dgvPedidos.Columns[11].HeaderText = "VALOR_PEÇAS";
                dgvPedidos.Columns[12].HeaderText = "VALOR_TOTAL";
                dgvPedidos.Columns[13].HeaderText = "OBSERVAÇÕES";

                dgvPedidos.Columns[3].Visible = false;
                dgvPedidos.Columns[4].Visible = false;
                dgvPedidos.Columns[5].Visible = false;
                dgvPedidos.Columns[13].Visible = false;

            }
        }
        
        private void listGrid()
        {
            try
            {
                con.OpenConnection();

                sql = @" SELECT 
                                p.id_pedido,
                                c.nome AS cliente,
                                u.nome AS tecnico,
                                e.descricao AS equipamento,
                                p.problema_relatado,
                                p.diagnostico,
                                p.status,
                                p.data_abertura,
                                p.data_atualizacao,
                                p.data_fechamento,
                                p.valor_mao_obra,
                                p.valor_pecas,
                                p.valor_total,
                                p.observacoes
                            FROM pedidos p
                            LEFT JOIN clientes c      ON p.id_cliente = c.id_cliente
                            LEFT JOIN usuarios u      ON p.id_tecnico = u.id_usuario
                            LEFT JOIN equipamentos e  ON p.id_equipamento = e.id_equipamento
                            ORDER BY p.id_pedido ASC;
                ";

                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvPedidos.DataSource = dt;
                
                con.CloseConnection();
                formatGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    cmd.Parameters.AddWithValue("@id", id);

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

        private void btnNew_Click(object sender, EventArgs e)
        {
            Novo_Pedido novo_pedido = new Novo_Pedido();
            novo_pedido.ShowDialog();
            listGrid();
            
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT p.*, e.descricao\nFROM pedidos p\nINNER JOIN equipamentos e \n    ON p.id_equipamento = e.id_equipamento\nWHERE e.descricao LIKE @descricao\nORDER BY e.descricao ASC;\n";  
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@descricao", txtBusca.Text + "%");
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dgvPedidos.DataSource = dt;
                con.CloseConnection();
                formatGrid();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Erro na busca: " + ex.Message);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja excluir pedido?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    con.OpenConnection();
                    sql = "DELETE FROM pedidos WHERE id_pedido = @id";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    listGrid();
                }
                catch(Exception exception)
                {
                    MessageBox.Show("Erro ao excluir pedido: " + exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
                
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Pedido pedido = formPedido();
            if (e.RowIndex >= 0 && dgvPedidos.Rows.Count > 0)
            {
                try
                {
                    pedido.id_pedido= id =Convert.ToInt32(dgvPedidos.Rows[e.RowIndex].Cells[0].Value);
                    enableBtn();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            disableBtn();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Pedido pedido = formPedido();
            Editar_Pedido editar_Pedido = new Editar_Pedido(pedido);
            editar_Pedido.ShowDialog();
        }

        private void dgvPedidos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Pedido pedido = formPedido();
            Editar_Pedido editar_Pedido = new Editar_Pedido(pedido);
            editar_Pedido.ShowDialog();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            ImprimirPedidoPDF();
        }
    }
}