using System;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.DTO;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.UserControls
{
    public partial class ucHome : UserControl
    {
        private readonly OrdemServicoService _ordemServicoService;
        CultureInfo culturaBrasil = new CultureInfo("pt-BR");
        private LucroMesDTO _lucroMesDTO = new LucroMesDTO();

        public ucHome(OrdemServicoService  ordemServicoService)
        {
            InitializeComponent();
           
            _ordemServicoService = ordemServicoService ??  throw new ArgumentNullException(nameof(ordemServicoService));
            ConfigurarComponentes();
        }

        private void ConfigurarComponentes()
        {
            string dia = DateTime.Now.Day.ToString();
            string mes = DateTime.Now.ToString("MMMM",  culturaBrasil);
            string ano = DateTime.Now.Year.ToString();
            string diaDaSemana = DateTime.Now.ToString("dddd", culturaBrasil).ToUpper();
            lblNome.Text = $"Bem-vindo de volta, {Sessao.usuarioLogado.Nome}";
            lblData.Text = $"{diaDaSemana}, {dia} De {mes} De {ano}";

            lblOrdemServico.Text = _ordemServicoService.obterOsAbertas().ToString();
            
            var (totalRecebido, totalPago, lucroLiquido) = _lucroMesDTO.ObterLucroDoMes(DateTime.Now.Month, DateTime.Now.Year);
            
            lblFaturamento.Text = totalRecebido.ToString("C", culturaBrasil);
        }
        
        
    }
}