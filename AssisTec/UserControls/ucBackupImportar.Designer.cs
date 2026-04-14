using System.ComponentModel;

namespace AssisTec.UserControls
{
    partial class ucBackupImportar
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnImportar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1175, 38);
            this.label4.TabIndex = 148;
            this.label4.Text = "Backup e importação de Dados";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(157, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(890, 97);
            this.label1.TabIndex = 149;
            this.label1.Text = "⚠️ Aviso: Ao importar os dados, as informações atuais podem ser substituídas. Ver" + "ifique o arquivo antes de continuar.";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnImportar.BackColor = System.Drawing.Color.Red;
            this.btnImportar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportar.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.btnImportar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnImportar.Location = new System.Drawing.Point(758, 285);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(203, 124);
            this.btnImportar.TabIndex = 159;
            this.btnImportar.Text = "📥Importar";
            this.btnImportar.UseVisualStyleBackColor = false;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(128, 218);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 216);
            this.panel1.TabIndex = 160;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 49);
            this.label2.TabIndex = 0;
            this.label2.Text = "💾";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(49, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 49);
            this.label3.TabIndex = 1;
            this.label3.Text = "Backup";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(19, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(310, 53);
            this.label5.TabIndex = 2;
            this.label5.Text = "Exporte seus dados em um arquivo .SQL para cópia de segurança";
            // 
            // ucBackupImportar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "ucBackupImportar";
            this.Size = new System.Drawing.Size(1175, 749);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Button btnImportar;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Label label4;

        #endregion
    }
}