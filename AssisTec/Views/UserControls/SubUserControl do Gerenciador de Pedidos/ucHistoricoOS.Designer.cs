using System.ComponentModel;

namespace AssisTec.UserControls
{
    partial class ucHistoricoOS
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucHistoricoOS));
            this.dgvOS = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.btnFechar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.btnRelatorio = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbTipoMovimentação = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAtualizar = new System.Windows.Forms.PictureBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.mtbDataFim = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mtbDataInicio = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTecnico = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOS)).BeginInit();
            this.panelBotoes.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOS
            // 
            this.dgvOS.AllowUserToAddRows = false;
            this.dgvOS.AllowUserToDeleteRows = false;
            this.dgvOS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOS.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOS.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvOS.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvOS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOS.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dgvOS.Location = new System.Drawing.Point(19, 82);
            this.dgvOS.MultiSelect = false;
            this.dgvOS.Name = "dgvOS";
            this.dgvOS.ReadOnly = true;
            this.dgvOS.Size = new System.Drawing.Size(1119, 528);
            this.dgvOS.TabIndex = 53;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.BackColor = System.Drawing.Color.RoyalBlue;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Enabled = false;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button2.ForeColor = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(3, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 33);
            this.button2.TabIndex = 106;
            this.button2.Text = "Imprimir relatório de OS";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // btnFechar
            // 
            this.btnFechar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(17)))), ((int)(((byte)(65)))));
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnFechar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnFechar.Location = new System.Drawing.Point(142, 10);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(81, 33);
            this.btnFechar.TabIndex = 107;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1152, 38);
            this.label4.TabIndex = 149;
            this.label4.Text = "Histórico de OS\'s";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelBotoes
            // 
            this.panelBotoes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelBotoes.Controls.Add(this.button2);
            this.panelBotoes.Controls.Add(this.btnFechar);
            this.panelBotoes.Location = new System.Drawing.Point(395, 1090);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new System.Drawing.Size(231, 46);
            this.panelBotoes.TabIndex = 150;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.btnRelatorio);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(415, 616);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(293, 61);
            this.panel1.TabIndex = 253;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.RoyalBlue;
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button3.ForeColor = System.Drawing.SystemColors.Control;
            this.button3.Location = new System.Drawing.Point(99, 14);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(104, 33);
            this.button3.TabIndex = 104;
            this.button3.Text = "Relatório da OS";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // btnRelatorio
            // 
            this.btnRelatorio.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRelatorio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRelatorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRelatorio.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRelatorio.Location = new System.Drawing.Point(14, 14);
            this.btnRelatorio.Name = "btnRelatorio";
            this.btnRelatorio.Size = new System.Drawing.Size(79, 33);
            this.btnRelatorio.TabIndex = 103;
            this.btnRelatorio.Text = "Relatório";
            this.btnRelatorio.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(209, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 33);
            this.button1.TabIndex = 102;
            this.button1.Text = "Fechar";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtTecnico);
            this.panel2.Controls.Add(this.cbTipoMovimentação);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnAtualizar);
            this.panel2.Controls.Add(this.btnBuscar);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.mtbDataFim);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.mtbDataInicio);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1152, 48);
            this.panel2.TabIndex = 255;
            // 
            // cbTipoMovimentação
            // 
            this.cbTipoMovimentação.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbTipoMovimentação.FormattingEnabled = true;
            this.cbTipoMovimentação.Location = new System.Drawing.Point(687, 21);
            this.cbTipoMovimentação.Name = "cbTipoMovimentação";
            this.cbTipoMovimentação.Size = new System.Drawing.Size(296, 21);
            this.cbTipoMovimentação.TabIndex = 168;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(815, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 18);
            this.label1.TabIndex = 167;
            this.label1.Text = "Cliente";
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAtualizar.BackgroundImage")));
            this.btnAtualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAtualizar.Location = new System.Drawing.Point(1100, 7);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(38, 38);
            this.btnAtualizar.TabIndex = 166;
            this.btnAtualizar.TabStop = false;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBuscar.Location = new System.Drawing.Point(1017, 9);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(77, 33);
            this.btnBuscar.TabIndex = 165;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(506, 2);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 18);
            this.label3.TabIndex = 163;
            this.label3.Text = "Técnico";
            // 
            // mtbDataFim
            // 
            this.mtbDataFim.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtbDataFim.Location = new System.Drawing.Point(279, 21);
            this.mtbDataFim.Mask = "00/00/0000";
            this.mtbDataFim.Name = "mtbDataFim";
            this.mtbDataFim.Size = new System.Drawing.Size(100, 20);
            this.mtbDataFim.TabIndex = 162;
            this.mtbDataFim.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(296, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 18);
            this.label2.TabIndex = 161;
            this.label2.Text = "Data fim:";
            // 
            // mtbDataInicio
            // 
            this.mtbDataInicio.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtbDataInicio.Location = new System.Drawing.Point(173, 21);
            this.mtbDataInicio.Mask = "00/00/0000";
            this.mtbDataInicio.Name = "mtbDataInicio";
            this.mtbDataInicio.Size = new System.Drawing.Size(100, 20);
            this.mtbDataInicio.TabIndex = 160;
            this.mtbDataInicio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(174, 3);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 18);
            this.label5.TabIndex = 159;
            this.label5.Text = "Data de início:";
            // 
            // txtTecnico
            // 
            this.txtTecnico.Location = new System.Drawing.Point(398, 21);
            this.txtTecnico.Name = "txtTecnico";
            this.txtTecnico.Size = new System.Drawing.Size(269, 20);
            this.txtTecnico.TabIndex = 169;
            // 
            // ucHistoricoOS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvOS);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "ucHistoricoOS";
            this.Size = new System.Drawing.Size(1152, 680);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOS)).EndInit();
            this.panelBotoes.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox txtTecnico;

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbTipoMovimentação;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox btnAtualizar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox mtbDataFim;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox mtbDataInicio;
        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRelatorio;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;

        private System.Windows.Forms.Panel panelBotoes;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Button btnFechar;

        private System.Windows.Forms.Button button2;

        private System.Windows.Forms.DataGridView dgvOS;

        #endregion
    }
}