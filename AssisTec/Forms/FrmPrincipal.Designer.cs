using System.ComponentModel;

namespace AssisTec
{
    partial class FrmPrincipal
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
            this.panelNavegacao = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelConteudo = new System.Windows.Forms.Panel();
            this.panelNavegacao.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelNavegacao
            // 
            this.panelNavegacao.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelNavegacao.Controls.Add(this.panel1);
            this.panelNavegacao.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNavegacao.Location = new System.Drawing.Point(0, 0);
            this.panelNavegacao.Name = "panelNavegacao";
            this.panelNavegacao.Size = new System.Drawing.Size(1350, 58);
            this.panelNavegacao.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(3, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1351, 675);
            this.panel1.TabIndex = 1;
            // 
            // panelConteudo
            // 
            this.panelConteudo.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panelConteudo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelConteudo.Location = new System.Drawing.Point(0, 58);
            this.panelConteudo.Name = "panelConteudo";
            this.panelConteudo.Size = new System.Drawing.Size(1350, 671);
            this.panelConteudo.TabIndex = 1;
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.panelConteudo);
            this.Controls.Add(this.panelNavegacao);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelNavegacao.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelConteudo;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Panel panelNavegacao;

        #endregion
    }
}