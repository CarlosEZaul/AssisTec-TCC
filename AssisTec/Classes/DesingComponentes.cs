using System.Drawing;
using System.Windows.Forms;

public static class DesingComponentes
{
    

    public static void StyleDataGridView(DataGridView dgv)
    {
        dgv.BorderStyle = BorderStyle.None;
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 232, 255);
        dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
        dgv.BackgroundColor = Color.White;
        dgv.RowHeadersVisible = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.RowTemplate.Height = 35;
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10);
        dgv.ColumnHeadersHeight = 40;
        dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
    }

    public static void StyleTextBox(TextBox textBox)
    {
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.BackColor = Color.White;
        textBox.Font = new Font("Segoe UI", 9F);
        textBox.ForeColor = Color.FromArgb(60, 60, 60);
    }

    public static void StyleMaskedTextBox(MaskedTextBox maskedTextBox)
    {
        maskedTextBox.BorderStyle = BorderStyle.FixedSingle;
        maskedTextBox.BackColor = Color.White;
        maskedTextBox.Font = new Font("Segoe UI", 9F);
        maskedTextBox.ForeColor = Color.FromArgb(60, 60, 60);
    }

    public static void StyleButton(Button button, Color backgroundColor)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.BackColor = backgroundColor;
        button.ForeColor = Color.White;
        button.Font = new Font("Segoe UI Semibold", 9F);
        button.Cursor = Cursors.Hand;
    }


    public static void ApplyLabelStyles(Form form)
    {
        foreach (Control control in form.Controls)
        {
            if (control is Panel panel)
            {
                foreach (Control panelControl in panel.Controls)
                {
                    if (panelControl is Label label)
                    {
                        label.ForeColor = Color.White;
                        label.Font = new Font("Segoe UI", 9F);
                    }
                }
            }
        }
    }
}