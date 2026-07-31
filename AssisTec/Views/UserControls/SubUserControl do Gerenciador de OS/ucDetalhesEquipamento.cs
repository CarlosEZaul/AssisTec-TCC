using System;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.Views.UserControls.SubUserControl_do_Gerenciador_de_OS
{
    public partial class ucDetalhesEquipamento : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        private int idOS;
        public ucDetalhesEquipamento(OrdemServicoService ordemServico, int id)
        {
            InitializeComponent();
            idOS = id;
            _ordemServicoService = ordemServico ?? throw new ArgumentNullException(nameof(ordemServico));
            CarregarComboBox();
            CarregarEquipamento();
            DesativarBotão();
        }

        private void CarregarComboBox()
        {
            cbEstado.Items.Add("Perfeito");
            cbEstado.Items.Add("Marcas de Uso");
            cbEstado.Items.Add("Danificado");
            cbEstado.Items.Add("Incompleto");
        }

        private void CarregarEquipamento()
        {
            var os = _ordemServicoService.ObterPorId(idOS);
            if (os == null)
            {
                MessageBox.Show("Ordem de Serviço não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Equipamento equipamento = _ordemServicoService.ObterEquipamentoPorId(os.Equipamento.Id_equipamento);

            if (equipamento == null)
            {
                MessageBox.Show("Equipamento não encontrado para esta OS.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtDescricao.Text = equipamento.Descricao;
            txtMarca.Text = equipamento.Marca;
            txtAcessorio.Text = equipamento.acessorios;
            txtModelo.Text = equipamento.Modelo;
            txtNdeSerie.Text = equipamento.Numero_Serie;
            cbEstado.SelectedItem = equipamento.estado_entrada;
            txtObservacoes.Text = equipamento.Observacoes;
        }

        private void SalvarEquipamento()
        {
            var os = _ordemServicoService.ObterPorId(idOS);
            if (os == null) return;

            Equipamento equipamento = _ordemServicoService.ObterEquipamentoPorId(os.Equipamento.Id_equipamento);
            if (equipamento == null) return;

            equipamento.Descricao = txtDescricao.Text;
            equipamento.estado_entrada = cbEstado.SelectedItem?.ToString() ?? cbEstado.Text;
            equipamento.acessorios = txtAcessorio.Text;
            equipamento.Numero_Serie = txtNdeSerie.Text;
            equipamento.Marca = txtMarca.Text;
            equipamento.Modelo = txtModelo.Text;
            equipamento.Observacoes = txtObservacoes.Text;
    
            var historicoAlteracaoOS = new HistoricoAlteracaoOS
            {
                idOS = idOS,
                idUsuario = os.id_tecnico.GetValueOrDefault(),
                tipo = "Alteracao de Dados",
                descricao = $"Alteração de dados do equipamento {equipamento.Descricao}",
                dataAlteracao = DateTime.Now
            };

            if (_ordemServicoService.AtualizarEquipamento(equipamento))
            {
                _ordemServicoService.RegistrarHistoricoOS(historicoAlteracaoOS);
                MessageBox.Show("Equipamento atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarEquipamento();
            }
            else
            {
                MessageBox.Show("Falha ao alterar equipamento", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DesativarBotão()
        {
            var os =  _ordemServicoService.ObterPorId(idOS);
            if (os != null)
            {
                if (os.status == "FINALIZADA")
                {
                    btnSalvar.Enabled = false;
                }
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
           
            SalvarEquipamento();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}