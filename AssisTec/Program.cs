using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.Repository;

namespace AssisTec
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmPrincipal());
            FrmLogin f = new FrmLogin();
            if (f.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FrmPrincipal());
            }
        }
    }
}
