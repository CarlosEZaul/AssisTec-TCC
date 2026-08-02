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
            this.btnRelatorioOS = new System.Windows.Forms.Button();
            this.btnRelatorioGeral = new System.Windows.Forms.Button();
            this.btnFecha = new System.Windows.Forms.Button();
            this.panelFiltro = new System.Windows.Forms.Panel();
            this.mtbDataFim = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mtbDataInicio = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAtualizar = new System.Windows.Forms.PictureBox();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOS)).BeginInit();
            this.panelBotoes.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelFiltro.SuspendLayout();
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
            this.dgvOS.Location = new System.Drawing.Point(19, 92);
            this.dgvOS.MultiSelect = false;
            this.dgvOS.Name = "dgvOS";
            this.dgvOS.ReadOnly = true;
            this.dgvOS.Size = new System.Drawing.Size(1119, 518);
            this.dgvOS.TabIndex = 53;
            this.dgvOS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOS_CellClick);
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
            this.panel1.Controls.Add(this.btnRelatorioOS);
            this.panel1.Controls.Add(this.btnRelatorioGeral);
            this.panel1.Controls.Add(this.btnFecha);
            this.panel1.Location = new System.Drawing.Point(415, 616);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(293, 61);
            this.panel1.TabIndex = 253;
            // 
            // btnRelatorioOS
            // 
            this.btnRelatorioOS.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRelatorioOS.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRelatorioOS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorioOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRelatorioOS.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRelatorioOS.Location = new System.Drawing.Point(99, 14);
            this.btnRelatorioOS.Name = "btnRelatorioOS";
            this.btnRelatorioOS.Size = new System.Drawing.Size(104, 33);
            this.btnRelatorioOS.TabIndex = 104;
            this.btnRelatorioOS.Text = "Relatório da OS";
            this.btnRelatorioOS.UseVisualStyleBackColor = false;
            this.btnRelatorioOS.Click += new System.EventHandler(this.btnRelatorioIndividual_Click);
            // 
            // btnRelatorioGeral
            // 
            this.btnRelatorioGeral.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRelatorioGeral.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRelatorioGeral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorioGeral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRelatorioGeral.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRelatorioGeral.Location = new System.Drawing.Point(14, 14);
            this.btnRelatorioGeral.Name = "btnRelatorioGeral";
            this.btnRelatorioGeral.Size = new System.Drawing.Size(79, 33);
            this.btnRelatorioGeral.TabIndex = 103;
            this.btnRelatorioGeral.Text = "Relatório";
            this.btnRelatorioGeral.UseVisualStyleBackColor = false;
            this.btnRelatorioGeral.Click += new System.EventHandler(this.btnRelatorioGeral_Click);
            // 
            // btnFecha
            // 
            this.btnFecha.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnFecha.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFecha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnFecha.ForeColor = System.Drawing.SystemColors.Control;
            this.btnFecha.Location = new System.Drawing.Point(209, 14);
            this.btnFecha.Name = "btnFecha";
            this.btnFecha.Size = new System.Drawing.Size(79, 33);
            this.btnFecha.TabIndex = 102;
            this.btnFecha.Text = "Fechar";
            this.btnFecha.UseVisualStyleBackColor = false;
            this.btnFecha.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // panelFiltro
            // 
            this.panelFiltro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFiltro.Controls.Add(this.mtbDataFim);
            this.panelFiltro.Controls.Add(this.label2);
            this.panelFiltro.Controls.Add(this.mtbDataInicio);
            this.panelFiltro.Controls.Add(this.label1);
            this.panelFiltro.Controls.Add(this.label5);
            this.panelFiltro.Controls.Add(this.btnAtualizar);
            this.panelFiltro.Controls.Add(this.txtBusca);
            this.panelFiltro.Controls.Add(this.btnBuscar);
            this.panelFiltro.Controls.Add(this.cbStatus);
            this.panelFiltro.Controls.Add(this.label3);
            this.panelFiltro.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFiltro.Location = new System.Drawing.Point(0, 38);
            this.panelFiltro.Name = "panelFiltro";
            this.panelFiltro.Size = new System.Drawing.Size(1152, 59);
            this.panelFiltro.TabIndex = 254;
            // 
            // mtbDataFim
            // 
            this.mtbDataFim.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtbDataFim.Location = new System.Drawing.Point(173, 32);
            this.mtbDataFim.Mask = "00/00/0000";
            this.mtbDataFim.Name = "mtbDataFim";
            this.mtbDataFim.Size = new System.Drawing.Size(136, 20);
            this.mtbDataFim.TabIndex = 162;
            this.mtbDataFim.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(182, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 18);
            this.label2.TabIndex = 161;
            this.label2.Text = "Data de Conclusão:";
            // 
            // mtbDataInicio
            // 
            this.mtbDataInicio.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtbDataInicio.Location = new System.Drawing.Point(42, 32);
            this.mtbDataInicio.Mask = "00/00/0000";
            this.mtbDataInicio.Name = "mtbDataInicio";
            this.mtbDataInicio.Size = new System.Drawing.Size(125, 20);
            this.mtbDataInicio.TabIndex = 160;
            this.mtbDataInicio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(43, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 18);
            this.label1.TabIndex = 159;
            this.label1.Text = "Data de Abertura:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(384, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(306, 18);
            this.label5.TabIndex = 156;
            this.label5.Text = "Buscar (Nº, Cliente, Técnico ou Equipamento ):";
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAtualizar.BackgroundImage")));
            this.btnAtualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAtualizar.Location = new System.Drawing.Point(1093, 14);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(38, 38);
            this.btnAtualizar.TabIndex = 158;
            this.btnAtualizar.TabStop = false;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // txtBusca
            // 
            this.txtBusca.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtBusca.BackColor = System.Drawing.Color.White;
            this.txtBusca.Location = new System.Drawing.Point(317, 32);
            this.txtBusca.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(440, 20);
            this.txtBusca.TabIndex = 155;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBuscar.Location = new System.Drawing.Point(986, 19);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(77, 33);
            this.btnBuscar.TabIndex = 157;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // cbStatus
            // 
            this.cbStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(764, 31);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(158, 21);
            this.cbStatus.TabIndex = 154;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(813, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 18);
            this.label3.TabIndex = 153;
            this.label3.Text = "Status:";
            // 
            // ucHistoricoOS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panelFiltro);
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
            this.panelFiltro.ResumeLayout(false);
            this.panelFiltro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnRelatorioOS;

        private System.Windows.Forms.Panel panelFiltro;
        private System.Windows.Forms.MaskedTextBox mtbDataFim;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox mtbDataInicio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox btnAtualizar;
        private System.Windows.Forms.TextBox txtBusca;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRelatorioGeral;
        private System.Windows.Forms.Button btnFecha;
        private System.Windows.Forms.Button button3;

        private System.Windows.Forms.Panel panelBotoes;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Button btnFechar;

        private System.Windows.Forms.Button button2;

        private System.Windows.Forms.DataGridView dgvOS;

        #endregion
    }
}