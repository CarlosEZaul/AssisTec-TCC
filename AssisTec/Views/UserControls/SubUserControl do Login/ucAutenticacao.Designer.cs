using System.ComponentModel;

namespace AssisTec.UserControls
{
    partial class ucAutenticacao
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnVerificarCodigo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.mtbCodigo = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFechar = new System.Windows.Forms.Button();
            this.lblReenviarCodigo = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnVerificarCodigo);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.mtbCodigo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(751, 100);
            this.panel2.TabIndex = 152;
            // 
            // btnVerificarCodigo
            // 
            this.btnVerificarCodigo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVerificarCodigo.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.btnVerificarCodigo.Location = new System.Drawing.Point(424, 51);
            this.btnVerificarCodigo.Name = "btnVerificarCodigo";
            this.btnVerificarCodigo.Size = new System.Drawing.Size(224, 23);
            this.btnVerificarCodigo.TabIndex = 185;
            this.btnVerificarCodigo.Text = "Verificar código e entrar no sistema";
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
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(751, 38);
            this.label4.TabIndex = 153;
            this.label4.Text = "Autenticação em Dois Fatores (2FA)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnFechar);
            this.panel1.Controls.Add(this.lblReenviarCodigo);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 136);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(751, 100);
            this.panel1.TabIndex = 186;
            // 
            // btnFechar
            // 
            this.btnFechar.BackColor = System.Drawing.Color.Red;
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnFechar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnFechar.Location = new System.Drawing.Point(585, 33);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(78, 33);
            this.btnFechar.TabIndex = 193;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.UseVisualStyleBackColor = false;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // lblReenviarCodigo
            // 
            this.lblReenviarCodigo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblReenviarCodigo.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReenviarCodigo.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblReenviarCodigo.Location = new System.Drawing.Point(254, 40);
            this.lblReenviarCodigo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReenviarCodigo.Name = "lblReenviarCodigo";
            this.lblReenviarCodigo.Size = new System.Drawing.Size(146, 18);
            this.lblReenviarCodigo.TabIndex = 190;
            this.lblReenviarCodigo.Text = "Enviar novo código";
            this.lblReenviarCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblReenviarCodigo.Click += new System.EventHandler(this.lblReenviarCodigo_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(85, 40);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 18);
            this.label5.TabIndex = 191;
            this.label5.Text = "Não recebeu o código?";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucAutenticacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(44)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label4);
            this.Name = "ucAutenticacao";
            this.Size = new System.Drawing.Size(751, 236);
            this.Load += new System.EventHandler(this.ucAutenticacao_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Label lblReenviarCodigo;
        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Panel panel2;

        private System.Windows.Forms.Button btnVerificarCodigo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox mtbCodigo;

        #endregion
    }
}