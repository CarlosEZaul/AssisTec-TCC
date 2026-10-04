using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AssisTec.Models;

using AssisTec.Repository;
using AssisTec.Service;
using MySql.Data.MySqlClient;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucFormularioOS : UserControl
    {
        
        private readonly OrdemServicoService _ordemServicoService;
        
        public ucFormularioOS(OrdemServicoService ordemServico)
        {
            InitializeComponent();
            _ordemServicoService = ordemServico ?? throw new ArgumentNullException(nameof(ordemServico));
            configurarComboBox();
        }

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

            cbEstado.Items.Clear();
            cbEstado.Items.AddRange(new object[] { "Perfeito", "Marcas de Uso", "Danificado", "Incompleto" }); 
            
        }
        
        private void LimparTxt()
        {
            cbTecnico.SelectedIndex = -1;
            cbCliente.SelectedIndex = -1;
            txtDescricao.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtNdeSerie.Text = "";
            txtAcessorio.Text = "";
            cbEstado.SelectedIndex = -1;
            txtObservacoes.Text = "";
            txtProblemas.Text="";
        }
        
        
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Equipamento equipamento = new Equipamento
                {
                    Descricao = txtDescricao.Text.Trim(),
                    Marca = txtMarca.Text.Trim(),
                    Modelo = txtModelo.Text.Trim(),
                    Numero_Serie = txtNdeSerie.Text.Trim(),
                    estado_entrada = cbEstado.SelectedValue?.ToString() ?? cbEstado.Text,
                    acessorios = txtAcessorio.Text.Trim(),
                    Observacoes = txtObservacoes.Text.Trim()
                };

                OrdemServico os = new OrdemServico
                {
                    id_cliente = cbCliente.SelectedValue != null ? Convert.ToInt32(cbCliente.SelectedValue) : (int?)null,
                    id_tecnico = cbTecnico.SelectedValue != null ? Convert.ToInt32(cbTecnico.SelectedValue) : (int?)null,
                    problema_relatado = txtProblemas.Text
                };

                if (_ordemServicoService.SalvarOS(os, equipamento))
                {
                    MessageBox.Show("Ordem de Serviço salva com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar os dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparTxt();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cbCliente_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is Usuario tecnico)
            {
                e.Value = $"{tecnico.Nome} - {tecnico.Cpf}";
            }
        }

        private void cbTecnico_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is Cliente cliente)
            {
                e.Value = $"{cliente.Nome} - {cliente.Cpf}";
            }
        }
    }
}