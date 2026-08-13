using System;
using System.Windows.Forms;
using AssisTec.Repository;
using Microsoft.EntityFrameworkCore;

namespace AssisTec
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
            }

            FrmLogin f = new FrmLogin();
            if (f.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FrmPrincipal());
            }
        }
    }
}