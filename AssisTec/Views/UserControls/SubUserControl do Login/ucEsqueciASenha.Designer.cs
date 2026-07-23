using System.ComponentModel;

namespace AssisTec.UserControls.SubUserControl_do_Login
{
    partial class ucEsqueciASenha
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
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnVerificarCodigo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.mtbCodigo = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnAlterarSenha = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblReenviarCodigo = new System.Windows.Forms.Label();
            this.lblAlterarEmail = new System.Windows.Forms.Label();
            this.btnFechar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(652, 38);
            this.label4.TabIndex = 149;
            this.label4.Text = "Alterar a Senha";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnBuscar);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 76);
            this.panel1.TabIndex = 150;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.btnBuscar.Location = new System.Drawing.Point(509, 29);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(121, 25);
            this.btnBuscar.TabIndex = 184;
            this.btnBuscar.Text = "Solicitar Código";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(27, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 18);
            this.label1.TabIndex = 183;
            this.label1.Text = "Digite seu E-mail";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(31, 32);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(472, 20);
            this.txtEmail.TabIndex = 182;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnVerificarCodigo);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.mtbCodigo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 114);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(652, 100);
            this.panel2.TabIndex = 151;
            // 
            // btnVerificarCodigo
            // 
            this.btnVerificarCodigo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVerificarCodigo.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.btnVerificarCodigo.Location = new System.Drawing.Point(510, 57);
            this.btnVerificarCodigo.Name = "btnVerificarCodigo";
            this.btnVerificarCodigo.Size = new System.Drawing.Size(121, 23);
            this.btnVerificarCodigo.TabIndex = 185;
            this.btnVerificarCodigo.Text = "Verificar Código";
            this.btnVerificarCodigo.UseVisualStyleBackColor = true;
            this.btnVerificarCodigo.Click += new System.EventHandler(this.btnVerificarCodigo_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(262, 23);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 18);
            this.label3.TabIndex = 184;
            this.label3.Text = "Digite o código";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mtbCodigo
            // 
            this.mtbCodigo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtbCodigo.Location = new System.Drawing.Point(209, 44);
            this.mtbCodigo.Mask = "___-___";
            this.mtbCodigo.Name = "mtbCodigo";
            this.mtbCodigo.Size = new System.Drawing.Size(209, 38);
            this.mtbCodigo.TabIndex = 0;
            this.mtbCodigo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(28, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 18);
            this.label2.TabIndex = 185;
            this.label2.Text = "Digite a nova senha:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(28, 35);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(476, 20);
            this.txtSenha.TabIndex = 184;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnAlterarSenha);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.txtSenha);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 214);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(652, 81);
            this.panel3.TabIndex = 186;
            // 
            // btnAlterarSenha
            // 
            this.btnAlterarSenha.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAlterarSenha.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.btnAlterarSenha.Location = new System.Drawing.Point(510, 33);
            this.btnAlterarSenha.Name = "btnAlterarSenha";
            this.btnAlterarSenha.Size = new System.Drawing.Size(121, 23);
            this.btnAlterarSenha.TabIndex = 186;
            this.btnAlterarSenha.Text = "Alterar senha";
            this.btnAlterarSenha.UseVisualStyleBackColor = true;
            this.btnAlterarSenha.Click += new System.EventHandler(this.btnAlterarSenha_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(28, 332);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 18);
            this.label5.TabIndex = 187;
            this.label5.Text = "Não recebeu o código?";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReenviarCodigo
            // 
            this.lblReenviarCodigo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblReenviarCodigo.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReenviarCodigo.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblReenviarCodigo.Location = new System.Drawing.Point(197, 332);
            this.lblReenviarCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReenviarCodigo.Name = "lblReenviarCodigo";
            this.lblReenviarCodigo.Size = new System.Drawing.Size(146, 18);
            this.lblReenviarCodigo.TabIndex = 187;
            this.lblReenviarCodigo.Text = "Enviar novo código";
            this.lblReenviarCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblReenviarCodigo.Click += new System.EventHandler(this.lblReenviarCodigo_Click);
            // 
            // lblAlterarEmail
            // 
            this.lblAlterarEmail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAlterarEmail.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlterarEmail.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblAlterarEmail.Location = new System.Drawing.Point(341, 332);
            this.lblAlterarEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlterarEmail.Name = "lblAlterarEmail";
            this.lblAlterarEmail.Size = new System.Drawing.Size(110, 18);
            this.lblAlterarEmail.TabIndex = 188;
            this.lblAlterarEmail.Text = "Alterar E-mail";
            this.lblAlterarEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAlterarEmail.Click += new System.EventHandler(this.lblAlterarEmail_Click);
            // 
            // btnFechar
            // 
            this.btnFechar.BackColor = System.Drawing.Color.Red;
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnFechar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnFechar.Location = new System.Drawing.Point(528, 325);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(78, 33);
            this.btnFechar.TabIndex = 189;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.UseVisualStyleBackColor = false;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // ucEsqueciASenha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.lblAlterarEmail);
            this.Controls.Add(this.lblReenviarCodigo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Name = "ucEsqueciASenha";
            this.Size = new System.Drawing.Size(652, 384);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnFechar;

        private System.Windows.Forms.Label lblAlterarEmail;

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblReenviarCodigo;

        private System.Windows.Forms.Button btnAlterarSenha;

        private System.Windows.Forms.Button btnVerificarCodigo;

        private System.Windows.Forms.Button btnBuscar;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.MaskedTextBox mtbCodigo;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Panel panel2;

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;

        #endregion
    }
}