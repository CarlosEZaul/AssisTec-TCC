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
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnImportar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Engravers MT", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.label1.Font = new System.Drawing.Font("Engravers MT", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(157, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(890, 97);
            this.label1.TabIndex = 149;
            this.label1.Text = "⚠️ Aviso: Ao importar os dados, as informações atuais podem ser substituídas. Ver" + "ifique o arquivo antes de continuar.";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBackup.BackColor = System.Drawing.Color.Green;
            this.btnBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackup.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBackup.Location = new System.Drawing.Point(170, 285);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(203, 124);
            this.btnBackup.TabIndex = 158;
            this.btnBackup.Text = "💾Backup";
            this.btnBackup.UseVisualStyleBackColor = false;
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
            // 
            // ucBackupImportar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Name = "ucBackupImportar";
            this.Size = new System.Drawing.Size(1175, 749);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnImportar;

        private System.Windows.Forms.Button btnBackup;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Label label4;

        #endregion
    }
}