using System.Windows.Forms;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucServicos : UserControl
    {
        private OrdemServico _ordemServico;
        public ucServicos(OrdemServico ordemServico)
        {
            InitializeComponent();
            _ordemServico = ordemServico;
        }
    }
}