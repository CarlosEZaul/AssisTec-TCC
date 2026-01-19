using System.Windows.Forms;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucServicos : UserControl
    {
        private Pedido _pedido;
        public ucServicos(Pedido pedido)
        {
            InitializeComponent();
            _pedido = pedido;
        }
    }
}