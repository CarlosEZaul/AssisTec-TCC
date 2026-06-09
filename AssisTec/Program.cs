using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            FrmLogin f = new FrmLogin();
            if (f.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FrmPrincipal());
            }
        }
    }
}