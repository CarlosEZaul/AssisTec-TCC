using System.ComponentModel;

namespace AssisTec.UserControls
{
    partial class ucHome
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblData = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.panelExibicao = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblFaturamento = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblValorEstoque = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblMinimo = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.fifgurinha = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblOrdemServico = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panelExibicao.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fifgurinha)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblData);
            this.panel1.Controls.Add(this.lblNome);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1175, 82);
            this.panel1.TabIndex = 0;
            // 
            // lblData
            // 
            this.lblData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblData.ForeColor = System.Drawing.Color.White;
            this.lblData.Location = new System.Drawing.Point(32, 44);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(403, 23);
            this.lblData.TabIndex = 1;
            this.lblData.Text = "label1";
            // 
            // lblNome
            // 
            this.lblNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblNome.ForeColor = System.Drawing.Color.White;
            this.lblNome.Location = new System.Drawing.Point(32, 0);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(1140, 44);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "label1";
            // 
            // panelExibicao
            // 
            this.panelExibicao.Controls.Add(this.panel4);
            this.panelExibicao.Controls.Add(this.panel5);
            this.panelExibicao.Controls.Add(this.panel3);
            this.panelExibicao.Controls.Add(this.panel2);
            this.panelExibicao.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExibicao.Location = new System.Drawing.Point(0, 82);
            this.panelExibicao.Name = "panelExibicao";
            this.panelExibicao.Size = new System.Drawing.Size(1175, 69);
            this.panelExibicao.TabIndex = 165;
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lblFaturamento);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.pictureBox3);
            this.panel4.Location = new System.Drawing.Point(582, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(239, 56);
            this.panel4.TabIndex = 164;
            // 
            // lblFaturamento
            // 
            this.lblFaturamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFaturamento.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblFaturamento.Location = new System.Drawing.Point(66, 26);
            this.lblFaturamento.Name = "lblFaturamento";
            this.lblFaturamento.Size = new System.Drawing.Size(115, 23);
            this.lblFaturamento.TabIndex = 4;
            this.lblFaturamento.Text = "0";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label11.Location = new System.Drawing.Point(66, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(137, 23);
            this.label11.TabIndex = 1;
            this.label11.Text = "Faturamento do Mês";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(3, 1);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(57, 51);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.lblValorEstoque);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Controls.Add(this.pictureBox4);
            this.panel5.Location = new System.Drawing.Point(827, 13);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(239, 56);
            this.panel5.TabIndex = 165;
            // 
            // lblValorEstoque
            // 
            this.lblValorEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorEstoque.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblValorEstoque.Location = new System.Drawing.Point(66, 26);
            this.lblValorEstoque.Name = "lblValorEstoque";
            this.lblValorEstoque.Size = new System.Drawing.Size(115, 23);
            this.lblValorEstoque.TabIndex = 5;
            this.lblValorEstoque.Text = "R$";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label13.Location = new System.Drawing.Point(66, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 23);
            this.label13.TabIndex = 1;
            this.label13.Text = "Valor em estoque";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Location = new System.Drawing.Point(3, 1);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(57, 51);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lblMinimo);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.fifgurinha);
            this.panel3.Location = new System.Drawing.Point(337, 13);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 56);
            this.panel3.TabIndex = 163;
            // 
            // lblMinimo
            // 
            this.lblMinimo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinimo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblMinimo.Location = new System.Drawing.Point(66, 26);
            this.lblMinimo.Name = "lblMinimo";
            this.lblMinimo.Size = new System.Drawing.Size(115, 23);
            this.lblMinimo.TabIndex = 3;
            this.lblMinimo.Text = "0";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label9.Location = new System.Drawing.Point(66, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(172, 23);
            this.label9.TabIndex = 1;
            this.label9.Text = "Produtos Abaixo do Mínimo";
            // 
            // fifgurinha
            // 
            this.fifgurinha.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.fifgurinha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fifgurinha.Location = new System.Drawing.Point(3, 1);
            this.fifgurinha.Name = "fifgurinha";
            this.fifgurinha.Size = new System.Drawing.Size(57, 51);
            this.fifgurinha.TabIndex = 0;
            this.fifgurinha.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblOrdemServico);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(92, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 56);
            this.panel2.TabIndex = 162;
            // 
            // lblOrdemServico
            // 
            this.lblOrdemServico.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrdemServico.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblOrdemServico.Location = new System.Drawing.Point(71, 26);
            this.lblOrdemServico.Name = "lblOrdemServico";
            this.lblOrdemServico.Size = new System.Drawing.Size(115, 23);
            this.lblOrdemServico.TabIndex = 2;
            this.lblOrdemServico.Text = "0";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(66, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 23);
            this.label7.TabIndex = 1;
            this.label7.Text = "Ordens Abertas";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 51);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ucHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.panelExibicao);
            this.Controls.Add(this.panel1);
            this.Name = "ucHome";
            this.Size = new System.Drawing.Size(1175, 749);
            this.panel1.ResumeLayout(false);
            this.panelExibicao.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fifgurinha)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelExibicao;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblFaturamento;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblValorEstoque;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblMinimo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox fifgurinha;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblOrdemServico;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;

        private System.Windows.Forms.Label lblData;

        private System.Windows.Forms.Label lblNome;

        private System.Windows.Forms.Panel panel1;

        #endregion
    }
}