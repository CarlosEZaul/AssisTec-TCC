using System.ComponentModel;

namespace AssisTec.Views.UserControls.SubUserControl_do_Gerenciador_de_OS
{
    partial class ucRegistrarPagamentoOS
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
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.btnFechar = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.mtbDataPagamento = new System.Windows.Forms.MaskedTextBox();
            this.cbFormaPagamento = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.txtValorPeca = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtValorServico = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtValorTotal = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDesconto = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDesconto = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtIdOs = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEquipamento = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTecnico = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelBotoes.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBotoes
            // 
            this.panelBotoes.Controls.Add(this.btnFechar);
            this.panelBotoes.Controls.Add(this.btnSave);
            this.panelBotoes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBotoes.Location = new System.Drawing.Point(0, 440);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new System.Drawing.Size(337, 62);
            this.panelBotoes.TabIndex = 248;
            // 
            // btnFechar
            // 
            this.btnFechar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnFechar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnFechar.Location = new System.Drawing.Point(163, 16);
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
            this.btnSave.Location = new System.Drawing.Point(79, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 33);
            this.btnSave.TabIndex = 100;
            this.btnSave.Text = "Salvar";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // mtbDataPagamento
            // 
            this.mtbDataPagamento.Location = new System.Drawing.Point(24, 32);
            this.mtbDataPagamento.Mask = "00/00/0000";
            this.mtbDataPagamento.Name = "mtbDataPagamento";
            this.mtbDataPagamento.Size = new System.Drawing.Size(269, 20);
            this.mtbDataPagamento.TabIndex = 247;
            this.mtbDataPagamento.ValidatingType = typeof(System.DateTime);
            // 
            // cbFormaPagamento
            // 
            this.cbFormaPagamento.FormattingEnabled = true;
            this.cbFormaPagamento.Location = new System.Drawing.Point(24, 76);
            this.cbFormaPagamento.Name = "cbFormaPagamento";
            this.cbFormaPagamento.Size = new System.Drawing.Size(269, 21);
            this.cbFormaPagamento.TabIndex = 246;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.Location = new System.Drawing.Point(24, 55);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 18);
            this.label7.TabIndex = 245;
            this.label7.Text = "Forma de Pagamento:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(24, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 18);
            this.label5.TabIndex = 244;
            this.label5.Text = "Data de Pagamento:";
            // 
            // label33
            // 
            this.label33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label33.Dock = System.Windows.Forms.DockStyle.Top;
            this.label33.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label33.Location = new System.Drawing.Point(0, 0);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(337, 24);
            this.label33.TabIndex = 243;
            this.label33.Text = "REGISTRAR PAGAMENTO DA OS";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtValorPeca
            // 
            this.txtValorPeca.BackColor = System.Drawing.Color.White;
            this.txtValorPeca.Enabled = false;
            this.txtValorPeca.Location = new System.Drawing.Point(24, 121);
            this.txtValorPeca.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtValorPeca.Name = "txtValorPeca";
            this.txtValorPeca.Size = new System.Drawing.Size(269, 20);
            this.txtValorPeca.TabIndex = 249;
            this.txtValorPeca.Text = "R$";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(24, 100);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 18);
            this.label1.TabIndex = 250;
            this.label1.Text = "Valor p/ peça";
            // 
            // txtValorServico
            // 
            this.txtValorServico.BackColor = System.Drawing.Color.White;
            this.txtValorServico.Enabled = false;
            this.txtValorServico.Location = new System.Drawing.Point(24, 165);
            this.txtValorServico.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtValorServico.Name = "txtValorServico";
            this.txtValorServico.Size = new System.Drawing.Size(269, 20);
            this.txtValorServico.TabIndex = 251;
            this.txtValorServico.Text = "R$";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(24, 144);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 18);
            this.label2.TabIndex = 252;
            this.label2.Text = "Valor p/ serviço";
            // 
            // txtValorTotal
            // 
            this.txtValorTotal.BackColor = System.Drawing.Color.White;
            this.txtValorTotal.Enabled = false;
            this.txtValorTotal.Location = new System.Drawing.Point(24, 253);
            this.txtValorTotal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtValorTotal.Name = "txtValorTotal";
            this.txtValorTotal.Size = new System.Drawing.Size(269, 20);
            this.txtValorTotal.TabIndex = 253;
            this.txtValorTotal.Text = "R$";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(24, 232);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 18);
            this.label3.TabIndex = 254;
            this.label3.Text = "Valor Total";
            // 
            // txtDesconto
            // 
            this.txtDesconto.BackColor = System.Drawing.Color.White;
            this.txtDesconto.Enabled = false;
            this.txtDesconto.Location = new System.Drawing.Point(24, 209);
            this.txtDesconto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDesconto.Name = "txtDesconto";
            this.txtDesconto.Size = new System.Drawing.Size(135, 20);
            this.txtDesconto.TabIndex = 255;
            this.txtDesconto.Text = "R$";
            this.txtDesconto.TextChanged += new System.EventHandler(this.txtDesconto_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(24, 188);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 18);
            this.label4.TabIndex = 256;
            this.label4.Text = "Desconto";
            // 
            // cbDesconto
            // 
            this.cbDesconto.ForeColor = System.Drawing.SystemColors.Control;
            this.cbDesconto.Location = new System.Drawing.Point(166, 207);
            this.cbDesconto.Name = "cbDesconto";
            this.cbDesconto.Size = new System.Drawing.Size(127, 25);
            this.cbDesconto.TabIndex = 257;
            this.cbDesconto.Text = "Aplicar Desconto";
            this.cbDesconto.UseVisualStyleBackColor = true;
            this.cbDesconto.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label16.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label16.Location = new System.Drawing.Point(24, 17);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(70, 18);
            this.label16.TabIndex = 259;
            this.label16.Text = "ID da OS:";
            // 
            // txtIdOs
            // 
            this.txtIdOs.BackColor = System.Drawing.Color.White;
            this.txtIdOs.Location = new System.Drawing.Point(102, 15);
            this.txtIdOs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtIdOs.Name = "txtIdOs";
            this.txtIdOs.Size = new System.Drawing.Size(191, 20);
            this.txtIdOs.TabIndex = 258;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(24, 43);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 18);
            this.label6.TabIndex = 261;
            this.label6.Text = "Equipamento:";
            // 
            // txtEquipamento
            // 
            this.txtEquipamento.BackColor = System.Drawing.Color.White;
            this.txtEquipamento.Location = new System.Drawing.Point(119, 41);
            this.txtEquipamento.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtEquipamento.Name = "txtEquipamento";
            this.txtEquipamento.Size = new System.Drawing.Size(174, 20);
            this.txtEquipamento.TabIndex = 260;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label8.Location = new System.Drawing.Point(24, 69);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 18);
            this.label8.TabIndex = 263;
            this.label8.Text = "Cliente:";
            // 
            // txtCliente
            // 
            this.txtCliente.BackColor = System.Drawing.Color.White;
            this.txtCliente.Location = new System.Drawing.Point(102, 67);
            this.txtCliente.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.Size = new System.Drawing.Size(191, 20);
            this.txtCliente.TabIndex = 262;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label9.Location = new System.Drawing.Point(24, 95);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 18);
            this.label9.TabIndex = 265;
            this.label9.Text = "Técnico:";
            // 
            // txtTecnico
            // 
            this.txtTecnico.BackColor = System.Drawing.Color.White;
            this.txtTecnico.Location = new System.Drawing.Point(102, 93);
            this.txtTecnico.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtTecnico.Name = "txtTecnico";
            this.txtTecnico.Size = new System.Drawing.Size(191, 20);
            this.txtTecnico.TabIndex = 264;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtTecnico);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtCliente);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtEquipamento);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.txtIdOs);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(337, 128);
            this.panel1.TabIndex = 266;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cbDesconto);
            this.panel2.Controls.Add(this.txtDesconto);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtValorTotal);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtValorServico);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtValorPeca);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.mtbDataPagamento);
            this.panel2.Controls.Add(this.cbFormaPagamento);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 152);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(337, 293);
            this.panel2.TabIndex = 267;
            // 
            // ucRegistrarPagamentoOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.label33);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "ucRegistrarPagamentoOS";
            this.Size = new System.Drawing.Size(337, 502);
            this.panelBotoes.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel2;

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTecnico;
        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtIdOs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtEquipamento;

        private System.Windows.Forms.TextBox txtDesconto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbDesconto;

        private System.Windows.Forms.TextBox txtValorPeca;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtValorServico;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtValorTotal;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.Panel panelBotoes;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.MaskedTextBox mtbDataPagamento;
        private System.Windows.Forms.ComboBox cbFormaPagamento;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label33;

        #endregion
    }
}