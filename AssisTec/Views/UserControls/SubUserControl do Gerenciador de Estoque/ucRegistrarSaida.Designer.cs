using System.ComponentModel;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    partial class ucRegistrarSaida
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
            this.label33 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEstoque = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtQuantidade = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbMotivo = new System.Windows.Forms.ComboBox();
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.btnFechar = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.mtbValor = new System.Windows.Forms.MaskedTextBox();
            this.cbProduto = new System.Windows.Forms.ComboBox();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbFormaPagamento = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panelBotoes.SuspendLayout();
            this.SuspendLayout();
            // 
            // label33
            // 
            this.label33.Dock = System.Windows.Forms.DockStyle.Top;
            this.label33.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label33.Location = new System.Drawing.Point(0, 0);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(339, 34);
            this.label33.TabIndex = 223;
            this.label33.Text = "REGISTRAR SAÍDA DO PRODUTO";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(16, 34);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 18);
            this.label5.TabIndex = 239;
            this.label5.Text = "Produto:";
            // 
            // txtEstoque
            // 
            this.txtEstoque.Location = new System.Drawing.Point(16, 99);
            this.txtEstoque.Name = "txtEstoque";
            this.txtEstoque.Size = new System.Drawing.Size(306, 20);
            this.txtEstoque.TabIndex = 242;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(16, 78);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 18);
            this.label1.TabIndex = 241;
            this.label1.Text = "Quantidade em estoque:";
            // 
            // txtQuantidade
            // 
            this.txtQuantidade.Location = new System.Drawing.Point(16, 143);
            this.txtQuantidade.Name = "txtQuantidade";
            this.txtQuantidade.Size = new System.Drawing.Size(306, 20);
            this.txtQuantidade.TabIndex = 244;
            this.txtQuantidade.TextChanged += new System.EventHandler(this.txtQuantidade_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(16, 122);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 18);
            this.label2.TabIndex = 243;
            this.label2.Text = "Quantidade para saída:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(16, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 18);
            this.label3.TabIndex = 245;
            this.label3.Text = "Motivo da saída:";
            // 
            // cbMotivo
            // 
            this.cbMotivo.FormattingEnabled = true;
            this.cbMotivo.Location = new System.Drawing.Point(16, 187);
            this.cbMotivo.Name = "cbMotivo";
            this.cbMotivo.Size = new System.Drawing.Size(306, 21);
            this.cbMotivo.TabIndex = 246;
            this.cbMotivo.SelectedIndexChanged += new System.EventHandler(this.cbMotivo_SelectedIndexChanged);
            // 
            // panelBotoes
            // 
            this.panelBotoes.Controls.Add(this.btnFechar);
            this.panelBotoes.Controls.Add(this.btnSave);
            this.panelBotoes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBotoes.Location = new System.Drawing.Point(0, 358);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new System.Drawing.Size(339, 62);
            this.panelBotoes.TabIndex = 247;
            // 
            // btnFechar
            // 
            this.btnFechar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnFechar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnFechar.Location = new System.Drawing.Point(162, 14);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(79, 33);
            this.btnFechar.TabIndex = 102;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.UseVisualStyleBackColor = false;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.SeaGreen;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSave.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSave.Location = new System.Drawing.Point(78, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 33);
            this.btnSave.TabIndex = 100;
            this.btnSave.Text = "Salvar";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(16, 211);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 248;
            this.label4.Text = "Valor:";
            // 
            // mtbValor
            // 
            this.mtbValor.Location = new System.Drawing.Point(16, 232);
            this.mtbValor.Name = "mtbValor";
            this.mtbValor.Size = new System.Drawing.Size(306, 20);
            this.mtbValor.TabIndex = 262;
            this.mtbValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mtbValor_KeyPress);
            this.mtbValor.Leave += new System.EventHandler(this.mtbValor_Leave);
            // 
            // cbProduto
            // 
            this.cbProduto.FormattingEnabled = true;
            this.cbProduto.Location = new System.Drawing.Point(16, 55);
            this.cbProduto.Name = "cbProduto";
            this.cbProduto.Size = new System.Drawing.Size(306, 21);
            this.cbProduto.TabIndex = 263;
            // 
            // cbStatus
            // 
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(16, 279);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(306, 21);
            this.cbStatus.TabIndex = 270;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label9.Location = new System.Drawing.Point(16, 255);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 18);
            this.label9.TabIndex = 269;
            this.label9.Text = "Status do pagamento:";
            // 
            // cbFormaPagamento
            // 
            this.cbFormaPagamento.FormattingEnabled = true;
            this.cbFormaPagamento.Location = new System.Drawing.Point(16, 327);
            this.cbFormaPagamento.Name = "cbFormaPagamento";
            this.cbFormaPagamento.Size = new System.Drawing.Size(306, 21);
            this.cbFormaPagamento.TabIndex = 268;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(16, 306);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 18);
            this.label6.TabIndex = 267;
            this.label6.Text = "Forma de Pagamento";
            // 
            // ucRegistrarSaida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbFormaPagamento);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbProduto);
            this.Controls.Add(this.mtbValor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.cbMotivo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtQuantidade);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEstoque);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label33);
            this.Name = "ucRegistrarSaida";
            this.Size = new System.Drawing.Size(339, 420);
            this.panelBotoes.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbFormaPagamento;
        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.ComboBox cbProduto;

        private System.Windows.Forms.MaskedTextBox mtbValor;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Panel panelBotoes;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Button btnSave;

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEstoque;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtQuantidade;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbMotivo;

        private System.Windows.Forms.Label label33;

        #endregion
    }
}