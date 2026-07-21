using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AssisTec
{
    public class DesignComponentes
    {
        public static void StyleHeaderLabel(Label label)
        {
            label.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label.ForeColor = Color.White;
        }

        public static void StyleSubHeaderLabel(Label label)
        {
            label.Font = new Font("Segoe UI Semibold", 11F);
            label.ForeColor = Color.FromArgb(200, 200, 200);
        }

        public static void StyleFieldLabel(Label label)
        {
            label.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            label.ForeColor = Color.WhiteSmoke;
        }

        public static void StyleHintLabel(Label label)
        {
            label.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            label.ForeColor = Color.FromArgb(160, 160, 160);
        }

        public static void ApplyLabelStyles(Control container)
        {
            foreach (Control control in container.Controls)
            {
                if (control is Label label)
                {
                    if (label.Tag != null && label.Tag.ToString().Equals("Header", StringComparison.OrdinalIgnoreCase))
                    {
                        StyleHeaderLabel(label);
                    }
                    else if (label.Tag != null && label.Tag.ToString().Equals("SubHeader", StringComparison.OrdinalIgnoreCase))
                    {
                        StyleSubHeaderLabel(label);
                    }
                    else if (label.Tag != null && label.Tag.ToString().Equals("Hint", StringComparison.OrdinalIgnoreCase))
                    {
                        StyleHintLabel(label);
                    }
                    else
                    {
                        StyleFieldLabel(label);
                    }
                }

                if (control.HasChildren)
                {
                    ApplyLabelStyles(control);
                }
            }
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

        public static void StyleDataGridView(DataGridView dgv, DataGridViewAutoSizeColumnsMode autoSizeMode = DataGridViewAutoSizeColumnsMode.Fill)
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
            dgv.AutoSizeColumnsMode = autoSizeMode;
            dgv.ScrollBars = ScrollBars.Both;
        }

        public static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.BackColor = Color.White;
            comboBox.ForeColor = Color.FromArgb(60, 60, 60);
            comboBox.Font = new Font("Segoe UI", 9F);
        }

        public static void centralizarPanel(Control control, int w)
        {
            control.Left = (w - control.Width) / 2;
        }

        public static void centralizarWidthControl(Control control, int w)
        {
            control.Left = ((w - control.Width) / 2);
        }

        public static void centralizarControl(Control control, int w, int h)
        {
            control.Left = ((w - control.Width) / 2);
            control.Top = (h - control.Height) / 2;
        }

        public static void AdicionarImagemNaLabel(Label label, Image image)
        {
            label.Image = RedimensionarImagem(image, 30, 30);
            label.ImageAlign = ContentAlignment.TopLeft;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Padding = new Padding(20,0,0,0);
        }

        public static void AdicionarImagemNoBotao(Button button, Image image, int tamanhoIcone = 24)
        {
            if (button == null || image == null) return;

            button.Image = RedimensionarImagem(image, tamanhoIcone, tamanhoIcone);
    
            button.ImageAlign = ContentAlignment.MiddleCenter;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
        }

        private static Image RedimensionarImagem(Image imagemOriginal, int largura, int altura)
        {
            Bitmap bitmap = new Bitmap(largura, altura);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imagemOriginal, 0, 0, largura, altura);
            }
            return bitmap;
        }

        public static void ArredondarPainel(Panel panel, int raio, Color corBorda, int espessuraBorda = 1)
        {
            panel.BorderStyle = BorderStyle.Fixed3D;

            panel.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, panel.Width, panel.Height);
                using (GraphicsPath path = ObterCaminhoArredondado(rect, raio))
                {
                    panel.Region = new Region(path);

                    if (espessuraBorda > 0)
                    {
                        using (Pen pen = new Pen(corBorda, espessuraBorda))
                        {
                            pen.Alignment = PenAlignment.Inset;
                            e.Graphics.DrawPath(pen, path);
                        }
                    }
                }
            };

            panel.Invalidate();
        }

        public static void RecortarCantosDataGridView(DataGridView dgv, int raio)
        {
            Action aplicarRegiao = () =>
            {
                if (dgv.Width <= 0 || dgv.Height <= 0) return;

                Rectangle rect = new Rectangle(0, 0, dgv.Width, dgv.Height);
                using (GraphicsPath path = ObterCaminhoArredondado(rect, raio))
                {
                    dgv.Region = new Region(path);
                }
            };

            dgv.Resize += (sender, e) => aplicarRegiao();
            aplicarRegiao();
        }

        private static GraphicsPath ObterCaminhoArredondado(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (diameter > rect.Width) diameter = rect.Width;
            if (diameter > rect.Height) diameter = rect.Height;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}