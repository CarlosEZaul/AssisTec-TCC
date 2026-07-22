using System.ComponentModel;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class Component1 : Component
    {
        public Component1()
        {
            InitializeComponent();
        }

        public Component1(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}