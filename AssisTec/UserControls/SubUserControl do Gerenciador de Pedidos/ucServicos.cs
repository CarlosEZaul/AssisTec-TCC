using System.Windows.Forms;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucServicos : UserControl
    {
        private OrdemDeServico _ordemServico;
        public ucServicos(OrdemDeServico ordemServico)
        {
            InitializeComponent();
            _ordemServico = ordemServico;
        }
    }
}