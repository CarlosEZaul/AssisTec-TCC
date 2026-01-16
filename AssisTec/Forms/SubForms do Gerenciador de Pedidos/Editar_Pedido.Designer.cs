using System.ComponentModel;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    partial class Editar_Pedido
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnFechar = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtProblema = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtObservacoes = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtDiagnostico = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtValorTotal = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtValorPecas = new System.Windows.Forms.TextBox();
            this.txtValorMaoObra = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtUltimaAtualizacao = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDataAbertura = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEquipamento = new System.Windows.Forms.TextBox();
            this.txtTecnico = new System.Windows.Forms.TextBox();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnDetalhes = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(44)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.txtId);
            this.panel1.Controls.Add(this.txtProblema);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.txtObservacoes);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.txtDiagnostico);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.txtValorTotal);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.txtValorPecas);
            this.panel1.Controls.Add(this.txtValorMaoObra);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtUltimaAtualizacao);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtDataAbertura);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtStatus);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtEquipamento);
            this.panel1.Controls.Add(this.txtTecnico);
            this.panel1.Controls.Add(this.txtCliente);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(691, 518);
            this.panel1.TabIndex = 41;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnFechar);
            this.panel3.Controls.Add(this.btnImprimir);
            this.panel3.Controls.Add(this.btnSalvar);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 475);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(691, 43);
            this.panel3.TabIndex = 73;
            // 
            // btnFechar
            // 
            this.btnFechar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(17)))), ((int)(((byte)(65)))));
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnFechar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnFechar.Location = new System.Drawing.Point(389, 10);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 47;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.UseVisualStyleBackColor = false;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnImprimir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnImprimir.ForeColor = System.Drawing.SystemColors.Control;
            this.btnImprimir.Location = new System.Drawing.Point(308, 10);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(75, 23);
            this.btnImprimir.TabIndex = 46;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = false;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSalvar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSalvar.Location = new System.Drawing.Point(227, 10);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 45;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.Color.White;
            this.txtId.Enabled = false;
            this.txtId.Location = new System.Drawing.Point(102, 56);
            this.txtId.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(108, 20);
            this.txtId.TabIndex = 72;
            // 
            // txtProblema
            // 
            this.txtProblema.BackColor = System.Drawing.Color.White;
            this.txtProblema.Location = new System.Drawing.Point(92, 202);
            this.txtProblema.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtProblema.Multiline = true;
            this.txtProblema.Name = "txtProblema";
            this.txtProblema.Size = new System.Drawing.Size(582, 82);
            this.txtProblema.TabIndex = 71;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label14.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label14.Location = new System.Drawing.Point(6, 202);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 18);
            this.label14.TabIndex = 70;
            this.label14.Text = "Problema:";
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.BackColor = System.Drawing.Color.White;
            this.txtObservacoes.Location = new System.Drawing.Point(92, 378);
            this.txtObservacoes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.Size = new System.Drawing.Size(582, 91);
            this.txtObservacoes.TabIndex = 69;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label13.Location = new System.Drawing.Point(-1, 378);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 18);
            this.label13.TabIndex = 68;
            this.label13.Text = "Observações:";
            // 
            // txtDiagnostico
            // 
            this.txtDiagnostico.BackColor = System.Drawing.Color.White;
            this.txtDiagnostico.Location = new System.Drawing.Point(92, 290);
            this.txtDiagnostico.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDiagnostico.Multiline = true;
            this.txtDiagnostico.Name = "txtDiagnostico";
            this.txtDiagnostico.Size = new System.Drawing.Size(582, 82);
            this.txtDiagnostico.TabIndex = 67;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label12.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label12.Location = new System.Drawing.Point(6, 290);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 18);
            this.label12.TabIndex = 66;
            this.label12.Text = "Diagnóstico:";
            // 
            // txtValorTotal
            // 
            this.txtValorTotal.BackColor = System.Drawing.Color.White;
            this.txtValorTotal.Enabled = false;
            this.txtValorTotal.Location = new System.Drawing.Point(529, 163);
            this.txtValorTotal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtValorTotal.Name = "txtValorTotal";
            this.txtValorTotal.Size = new System.Drawing.Size(142, 20);
            this.txtValorTotal.TabIndex = 65;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label11.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label11.Location = new System.Drawing.Point(447, 165);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 18);
            this.label11.TabIndex = 64;
            this.label11.Text = "Valor total:";
            // 
            // txtValorPecas
            // 
            this.txtValorPecas.BackColor = System.Drawing.Color.White;
            this.txtValorPecas.Enabled = false;
            this.txtValorPecas.Location = new System.Drawing.Point(313, 163);
            this.txtValorPecas.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtValorPecas.Name = "txtValorPecas";
            this.txtValorPecas.Size = new System.Drawing.Size(126, 20);
            this.txtValorPecas.TabIndex = 63;
            // 
            // txtValorMaoObra
            // 
            this.txtValorMaoObra.BackColor = System.Drawing.Color.White;
            this.txtValorMaoObra.Enabled = false;
            this.txtValorMaoObra.Location = new System.Drawing.Point(102, 165);
            this.txtValorMaoObra.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtValorMaoObra.Name = "txtValorMaoObra";
            this.txtValorMaoObra.Size = new System.Drawing.Size(101, 20);
            this.txtValorMaoObra.TabIndex = 62;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label10.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label10.Location = new System.Drawing.Point(6, 165);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 18);
            this.label10.TabIndex = 61;
            this.label10.Text = "Mão de obra:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label9.Location = new System.Drawing.Point(211, 165);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 18);
            this.label9.TabIndex = 59;
            this.label9.Text = "Valor p/ peça:";
            // 
            // txtUltimaAtualizacao
            // 
            this.txtUltimaAtualizacao.BackColor = System.Drawing.Color.White;
            this.txtUltimaAtualizacao.Enabled = false;
            this.txtUltimaAtualizacao.Location = new System.Drawing.Point(418, 128);
            this.txtUltimaAtualizacao.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUltimaAtualizacao.Name = "txtUltimaAtualizacao";
            this.txtUltimaAtualizacao.Size = new System.Drawing.Size(142, 20);
            this.txtUltimaAtualizacao.TabIndex = 58;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.Location = new System.Drawing.Point(286, 130);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 18);
            this.label7.TabIndex = 57;
            this.label7.Text = "Ultima atualização:";
            // 
            // txtDataAbertura
            // 
            this.txtDataAbertura.BackColor = System.Drawing.Color.White;
            this.txtDataAbertura.Enabled = false;
            this.txtDataAbertura.Location = new System.Drawing.Point(119, 128);
            this.txtDataAbertura.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDataAbertura.Name = "txtDataAbertura";
            this.txtDataAbertura.Size = new System.Drawing.Size(142, 20);
            this.txtDataAbertura.TabIndex = 56;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(4, 130);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 18);
            this.label5.TabIndex = 55;
            this.label5.Text = "Data de abertura:";
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.Color.White;
            this.txtStatus.Enabled = false;
            this.txtStatus.Location = new System.Drawing.Point(506, 93);
            this.txtStatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(172, 20);
            this.txtStatus.TabIndex = 54;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(444, 93);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 18);
            this.label4.TabIndex = 53;
            this.label4.Text = "Status:";
            // 
            // txtEquipamento
            // 
            this.txtEquipamento.BackColor = System.Drawing.Color.White;
            this.txtEquipamento.Enabled = false;
            this.txtEquipamento.Location = new System.Drawing.Point(320, 58);
            this.txtEquipamento.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtEquipamento.Name = "txtEquipamento";
            this.txtEquipamento.Size = new System.Drawing.Size(358, 20);
            this.txtEquipamento.TabIndex = 51;
            // 
            // txtTecnico
            // 
            this.txtTecnico.BackColor = System.Drawing.Color.White;
            this.txtTecnico.Enabled = false;
            this.txtTecnico.Location = new System.Drawing.Point(286, 93);
            this.txtTecnico.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtTecnico.Name = "txtTecnico";
            this.txtTecnico.Size = new System.Drawing.Size(150, 20);
            this.txtTecnico.TabIndex = 50;
            // 
            // txtCliente
            // 
            this.txtCliente.BackColor = System.Drawing.Color.White;
            this.txtCliente.Enabled = false;
            this.txtCliente.Location = new System.Drawing.Point(68, 91);
            this.txtCliente.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.Size = new System.Drawing.Size(142, 20);
            this.txtCliente.TabIndex = 49;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label8.Location = new System.Drawing.Point(225, 58);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 18);
            this.label8.TabIndex = 48;
            this.label8.Text = "Equipamento:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(218, 93);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 18);
            this.label6.TabIndex = 44;
            this.label6.Text = "Técnico:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(6, 93);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 18);
            this.label3.TabIndex = 42;
            this.label3.Text = "Cliente:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.btnDetalhes);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(691, 46);
            this.panel2.TabIndex = 41;
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Left;
            this.button3.Location = new System.Drawing.Point(460, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(231, 46);
            this.button3.TabIndex = 3;
            this.button3.Text = "Ações";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Left;
            this.button2.Location = new System.Drawing.Point(230, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(230, 46);
            this.button2.TabIndex = 2;
            this.button2.Text = "Produtos Utilizados";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnDetalhes
            // 
            this.btnDetalhes.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDetalhes.Location = new System.Drawing.Point(0, 0);
            this.btnDetalhes.Name = "btnDetalhes";
            this.btnDetalhes.Size = new System.Drawing.Size(230, 46);
            this.btnDetalhes.TabIndex = 1;
            this.btnDetalhes.Text = "Detalhes";
            this.btnDetalhes.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(6, 58);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 18);
            this.label1.TabIndex = 37;
            this.label1.Text = "ID do Pedido:";
            // 
            // Editar_Pedido
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(44)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(691, 518);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Editar_Pedido";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.Editar_Pedido_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnFechar;

        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnSalvar;

        private System.Windows.Forms.Panel panel3;

        private System.Windows.Forms.TextBox txtId;

        private System.Windows.Forms.TextBox txtProblema;
        private System.Windows.Forms.Label label14;

        private System.Windows.Forms.TextBox txtObservacoes;
        private System.Windows.Forms.Label label13;

        private System.Windows.Forms.TextBox txtDiagnostico;
        private System.Windows.Forms.Label label12;

        private System.Windows.Forms.TextBox txtValorTotal;
        private System.Windows.Forms.Label label11;

        private System.Windows.Forms.TextBox txtValorMaoObra;
        private System.Windows.Forms.Label label10;

        private System.Windows.Forms.TextBox txtValorPecas;
        private System.Windows.Forms.Label label9;

        private System.Windows.Forms.TextBox txtUltimaAtualizacao;
        private System.Windows.Forms.Label label7;

        private System.Windows.Forms.TextBox txtDataAbertura;
        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.TextBox txtTecnico;
        private System.Windows.Forms.TextBox txtEquipamento;

        private System.Windows.Forms.Label label8;

        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;

        private System.Windows.Forms.Button btnDetalhes;

        private System.Windows.Forms.Panel panel2;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Panel panel1;

        #endregion
    }
}