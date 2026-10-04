using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AssisTec.Models;
using AssisTec.Properties;
using AssisTec.Service;
using AssisTec.Views.UserControls.SubUserControl_do_Gerenciador_de_OS;
using Exception = System.Exception;
using Font = System.Drawing.Font;
using Image = iTextSharp.text.Image;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucDetalhesOS : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private readonly int _idOS;
        public ucDetalhesOS(int IdOS, OrdemServicoService ordemServicoService)
        {
            InitializeComponent();
            _idOS = IdOS;
            _ordemServicoService = ordemServicoService ??  throw new ArgumentNullException(nameof(ordemServicoService));
            configurarComboBox();
            CarregarDetalhesOS();
            

        }

        #region Metodos de Manipulação de Dados

        private void configurarComboBox()
        {
            string Normalizar(string s)
            {
                if (string.IsNullOrEmpty(s)) return "";
                var sb = new StringBuilder();
                foreach (char c in s.Normalize(NormalizationForm.FormD))
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        sb.Append(c);
                return sb.ToString().ToLowerInvariant();
            }

            string SoDigitos(string s) => new string((s ?? "").Where(char.IsDigit).ToArray());
            
            void ConfigurarBusca(ComboBox cb, List<dynamic> listaOriginal)
            {
                cb.DisplayMember = "Exibicao";
                cb.ValueMember = "Id";
                cb.DataSource = listaOriginal;
                cb.AutoCompleteMode = AutoCompleteMode.None;
                cb.DropDownStyle = ComboBoxStyle.DropDown;
                cb.SelectedIndex = -1;

                var timer = new Timer { Interval = 200 };

                cb.TextUpdate += (s, e) =>
                {
                    timer.Stop();
                    timer.Start();
                };

                timer.Tick += (s, e) =>
                {
                    timer.Stop();

                    string textoAtual = cb.Text;
                    int posicaoCursor = cb.SelectionStart;
                    string termo = Normalizar(textoAtual.Trim());
                    string digitos = SoDigitos(textoAtual);

                    cb.BeginUpdate();
                    try
                    {
                        if (string.IsNullOrWhiteSpace(termo))
                        {
                            cb.DroppedDown = false;
                            cb.DataSource = listaOriginal;
                            cb.DisplayMember = "Exibicao";
                            cb.ValueMember = "Id";
                            cb.SelectedIndex = -1;
                            cb.Text = "";
                            return;
                        }

                        var listaFiltrada = listaOriginal
                            .Where(x => Normalizar((string)x.Nome).Contains(termo) ||
                                        Normalizar((string)x.Exibicao).Contains(termo) ||
                                        (digitos.Length > 0 && SoDigitos((string)x.Cpf).Contains(digitos)))
                            .ToList();

                        cb.DataSource = listaFiltrada;
                        cb.DisplayMember = "Exibicao";
                        cb.ValueMember = "Id";
                        cb.SelectedIndex = -1;         

                        cb.Text = textoAtual;
                        cb.SelectionStart = Math.Min(posicaoCursor, textoAtual.Length);
                        cb.SelectionLength = 0;

                        if (listaFiltrada.Count > 0 && !cb.DroppedDown)
                            cb.DroppedDown = true;
                    }
                    finally
                    {
                        cb.EndUpdate();
                        Cursor.Current = Cursors.Default;
                    }
                };
            }

            var clientes = _ordemServicoService.ObterClientes()
                .Where(c => c.Status == "Ativado")
                .Select(c => (dynamic)new
                {
                    c.Id,
                    c.Nome,
                    c.Cpf,
                    Exibicao = $"{c.Nome} - {c.Cpf}"
                })
                .OrderBy(c => (string)c.Nome)
                .ToList();

            var tecnicos = _ordemServicoService.ObterTecnicos()
                .Select(t => (dynamic)new
                {
                    t.Id,
                    t.Nome,
                    t.Cpf,
                    Exibicao = $"{t.Nome} - {t.Cpf}"
                })
                .OrderBy(t => (string)t.Nome)
                .ToList();

            ConfigurarBusca(cbCliente, clientes);
            ConfigurarBusca(cbTecnico, tecnicos);

            
            
        }
        
        
        public void CarregarDetalhesOS()
        {
            OrdemServico os = _ordemServicoService.ObterPorId(_idOS);
            try
            {
                txtId.Text = os.id_os.ToString();
                cbCliente.SelectedValue = os.id_cliente;
                cbTecnico.SelectedValue = os.id_tecnico;
                txtEquipamento.Text = os.Equipamento.Descricao;
                txtStatus.Text = os.status;
                txtDataAbertura.Text = os.data_abertura.ToString();
                txtUltimaAtualizacao.Text = os.data_atualizacao.ToString();
                txtValorMaoObra.Text = "R$ "+os.valor_mao_obra.ToString();
                txtValorPecas.Text = "R$ " + os.valor_pecas.ToString();
                txtValorTotal.Text = "R$ " + os.valor_total.ToString();
                txtProblema.Text = os.problema_relatado;
                txtObservacoes.Text = os.observacoes;
                txtDiagnostico.Text = os.diagnostico;

            }
            catch (Exception e)
            {
                throw new ArgumentNullException("Falha ao carregar OS");
            }
        }

        public void SalvarAlteracoes()
        {
            OrdemServico os =  _ordemServicoService.ObterPorId(_idOS);
            try
            {
                os.id_cliente = Convert.ToInt32(cbCliente.SelectedValue);
                os.id_tecnico = Convert.ToInt32(cbTecnico.SelectedValue);
                os.data_atualizacao = DateTime.Now;
                os.problema_relatado = txtProblema.Text;
                os.observacoes = txtObservacoes.Text;
                os.diagnostico = txtDiagnostico.Text;
                if (_ordemServicoService.SalvarAlteracoesOS(os))
                {
                    MessageBox.Show("Alteraçõse feitas com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception e)
            {
                throw new ArgumentNullException("Falha ao salvar altereções");
            }
        }
        
        

        

        #endregion


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var uc = new ucDetalhesEquipamento(_ordemServicoService, _idOS);
            uc.Disposed += (s, e2) => CarregarDetalhesOS();
    
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Location = new Point((this.Width - uc.Width) / 2, (this.Height - uc.Height) / 2);
        }
        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var uc = new ucHistoricoAlteracao(_ordemServicoService, _idOS);
            uc.Disposed += (s, e2) => CarregarDetalhesOS();
    
            uc.Dock = DockStyle.Fill;
            this.Controls.Add(uc);
            uc.BringToFront();
            uc.Visible = true;
            uc.Focus();
        }
    }
}