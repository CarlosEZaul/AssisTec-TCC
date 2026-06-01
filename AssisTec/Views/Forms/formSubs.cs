using System.Drawing;
using System.Windows.Forms;

namespace AssisTec
{
    public partial class formSubs : Form
    {
        public formSubs(UserControl uc, Size size)
        {
            InitializeComponent();
            uc.Dock = DockStyle.Fill;
            this.Controls.Add(uc);
            this.Size = size;
        }
    }
}