using System.ComponentModel;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    partial class ucProdutosUtilizados
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
            this.txtQntd = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbProduto = new System.Windows.Forms.ComboBox();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemover = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvItensOS = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItensOS)).BeginInit();
            this.SuspendLayout();
            // 
            // txtQntd
            // 
            this.txtQntd.BackColor = System.Drawing.Color.White;
            this.txtQntd.Location = new System.Drawing.Point(258, 41);
            this.txtQntd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtQntd.Name = "txtQntd";
            this.txtQntd.Size = new System.Drawing.Size(83, 20);
            this.txtQntd.TabIndex = 115;
            this.txtQntd.TextChanged += new System.EventHandler(this.txtQntd_TextChanged);
            this.txtQntd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQntd_KeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label14.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label14.Location = new System.Drawing.Point(258, 20);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 18);
            this.label14.TabIndex = 123;
            this.label14.Text = "Quantidade:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(4, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 18);
            this.label1.TabIndex = 124;
            this.label1.Text = "Produto:";
            // 
            // cbProduto
            // 
            this.cbProduto.FormattingEnabled = true;
            this.cbProduto.Location = new System.Drawing.Point(3, 41);
            this.cbProduto.Name = "cbProduto";
            this.cbProduto.Size = new System.Drawing.Size(248, 21);
            this.cbProduto.TabIndex = 125;
            this.cbProduto.SelectedIndexChanged += new System.EventHandler(this.cbProduto_SelectedIndexChanged);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnAdicionar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdicionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnAdicionar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnAdicionar.Location = new System.Drawing.Point(471, 39);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(96, 23);
            this.btnAdicionar.TabIndex = 126;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = false;
            this.btnAdicionar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(558, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 127;
            this.label2.Text = "Total:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lblTotal.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblTotal.Location = new System.Drawing.Point(610, 6);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(17, 18);
            this.lblTotal.TabIndex = 128;
            this.lblTotal.Text = "...";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRemover);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtValor);
            this.panel1.Controls.Add(this.btnAdicionar);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.txtQntd);
            this.panel1.Controls.Add(this.cbProduto);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(685, 79);
            this.panel1.TabIndex = 130;
            // 
            // btnRemover
            // 
            this.btnRemover.BackColor = System.Drawing.Color.Red;
            this.btnRemover.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemover.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRemover.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRemover.Location = new System.Drawing.Point(573, 39);
            this.btnRemover.Name = "btnRemover";
            this.btnRemover.Size = new System.Drawing.Size(109, 23);
            this.btnRemover.TabIndex = 129;
            this.btnRemover.Text = "Remover";
            this.btnRemover.UseVisualStyleBackColor = false;
            this.btnRemover.Click += new System.EventHandler(this.btnRemover_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(349, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 18);
            this.label3.TabIndex = 128;
            this.label3.Text = "Valor";
            // 
            // txtValor
            // 
            this.txtValor.BackColor = System.Drawing.Color.White;
            this.txtValor.Location = new System.Drawing.Point(349, 41);
            this.txtValor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(115, 20);
            this.txtValor.TabIndex = 127;
            this.txtValor.Text = "R$";
            this.txtValor.Click += new System.EventHandler(this.TxtValor_Click);
            this.txtValor.TextChanged += new System.EventHandler(this.TxtValor_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblTotal);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 386);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(685, 56);
            this.panel2.TabIndex = 131;
            // 
            // dgvItensOS
            // 
            this.dgvItensOS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItensOS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItensOS.Location = new System.Drawing.Point(0, 79);
            this.dgvItensOS.Name = "dgvItensOS";
            this.dgvItensOS.Size = new System.Drawing.Size(685, 307);
            this.dgvItensOS.TabIndex = 132;
            this.dgvItensOS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItensOS_CellClick);
            // 
            // ucProdutosUtilizados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(44)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.dgvItensOS);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ucProdutosUtilizados";
            this.Size = new System.Drawing.Size(685, 442);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItensOS)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dgvItensOS;

        private System.Windows.Forms.Button btnRemover;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtValor;

        private System.Windows.Forms.Panel panel2;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotal;

        private System.Windows.Forms.Button btnAdicionar;

        private System.Windows.Forms.ComboBox cbProduto;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Label label14;

        private System.Windows.Forms.TextBox txtQntd;

        #endregion
    }
}