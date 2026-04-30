using System.ComponentModel;

namespace AssisTec.UserControls
{
    partial class ucGerenciadorEstoque
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucGerenciadorEstoque));
            this.label4 = new System.Windows.Forms.Label();
            this.dgvEstoque = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.cbConcluidas = new System.Windows.Forms.CheckBox();
            this.btnAtualizar = new System.Windows.Forms.PictureBox();
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.btnSaida = new System.Windows.Forms.Button();
            this.btnVisualizacoes = new System.Windows.Forms.Button();
            this.btnEntrada = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoque)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).BeginInit();
            this.panelBotoes.SuspendLayout();
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
            this.label4.Size = new System.Drawing.Size(1175, 38);
            this.label4.TabIndex = 148;
            this.label4.Text = "Gerenciador do Estoque";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvEstoque
            // 
            this.dgvEstoque.AllowUserToAddRows = false;
            this.dgvEstoque.AllowUserToDeleteRows = false;
            this.dgvEstoque.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEstoque.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEstoque.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvEstoque.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvEstoque.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEstoque.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dgvEstoque.Location = new System.Drawing.Point(18, 85);
            this.dgvEstoque.MultiSelect = false;
            this.dgvEstoque.Name = "dgvEstoque";
            this.dgvEstoque.ReadOnly = true;
            this.dgvEstoque.Size = new System.Drawing.Size(1138, 579);
            this.dgvEstoque.TabIndex = 149;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(319, 59);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 18);
            this.label1.TabIndex = 152;
            this.label1.Text = "Buscar:";
            // 
            // txtBusca
            // 
            this.txtBusca.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtBusca.BackColor = System.Drawing.Color.White;
            this.txtBusca.Location = new System.Drawing.Point(380, 59);
            this.txtBusca.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(471, 20);
            this.txtBusca.TabIndex = 151;
            // 
            // cbConcluidas
            // 
            this.cbConcluidas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConcluidas.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cbConcluidas.Location = new System.Drawing.Point(922, 53);
            this.cbConcluidas.Name = "cbConcluidas";
            this.cbConcluidas.Size = new System.Drawing.Size(190, 24);
            this.cbConcluidas.TabIndex = 155;
            this.cbConcluidas.Text = "Exibir produtos abaixo do minímo";
            this.cbConcluidas.UseVisualStyleBackColor = true;
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAtualizar.BackgroundImage")));
            this.btnAtualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAtualizar.Location = new System.Drawing.Point(1118, 39);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(38, 38);
            this.btnAtualizar.TabIndex = 154;
            this.btnAtualizar.TabStop = false;
            // 
            // panelBotoes
            // 
            this.panelBotoes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelBotoes.Controls.Add(this.btnSaida);
            this.panelBotoes.Controls.Add(this.btnVisualizacoes);
            this.panelBotoes.Controls.Add(this.btnEntrada);
            this.panelBotoes.Controls.Add(this.btnNew);
            this.panelBotoes.Controls.Add(this.btnDelete);
            this.panelBotoes.Controls.Add(this.btnEditar);
            this.panelBotoes.Location = new System.Drawing.Point(240, 688);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new System.Drawing.Size(698, 61);
            this.panelBotoes.TabIndex = 156;
            // 
            // btnSaida
            // 
            this.btnSaida.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaida.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSaida.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaida.Enabled = false;
            this.btnSaida.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaida.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSaida.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSaida.Location = new System.Drawing.Point(408, 15);
            this.btnSaida.Name = "btnSaida";
            this.btnSaida.Size = new System.Drawing.Size(133, 33);
            this.btnSaida.TabIndex = 106;
            this.btnSaida.Text = "Registrar Saida";
            this.btnSaida.UseVisualStyleBackColor = false;
            // 
            // btnVisualizacoes
            // 
            this.btnVisualizacoes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVisualizacoes.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnVisualizacoes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVisualizacoes.Enabled = false;
            this.btnVisualizacoes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVisualizacoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnVisualizacoes.ForeColor = System.Drawing.SystemColors.Control;
            this.btnVisualizacoes.Location = new System.Drawing.Point(547, 15);
            this.btnVisualizacoes.Name = "btnVisualizacoes";
            this.btnVisualizacoes.Size = new System.Drawing.Size(142, 33);
            this.btnVisualizacoes.TabIndex = 105;
            this.btnVisualizacoes.Text = "Visualizar Movimentações";
            this.btnVisualizacoes.UseVisualStyleBackColor = false;
            // 
            // btnEntrada
            // 
            this.btnEntrada.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEntrada.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnEntrada.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEntrada.Enabled = false;
            this.btnEntrada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEntrada.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEntrada.ForeColor = System.Drawing.SystemColors.Control;
            this.btnEntrada.Location = new System.Drawing.Point(266, 15);
            this.btnEntrada.Name = "btnEntrada";
            this.btnEntrada.Size = new System.Drawing.Size(136, 33);
            this.btnEntrada.TabIndex = 104;
            this.btnEntrada.Text = "Registrar Entrada";
            this.btnEntrada.UseVisualStyleBackColor = false;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNew.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnNew.ForeColor = System.Drawing.SystemColors.Control;
            this.btnNew.Location = new System.Drawing.Point(14, 15);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(78, 33);
            this.btnNew.TabIndex = 99;
            this.btnNew.Text = "Novo";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnDelete.ForeColor = System.Drawing.SystemColors.Control;
            this.btnDelete.Location = new System.Drawing.Point(182, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(78, 33);
            this.btnDelete.TabIndex = 101;
            this.btnDelete.Text = "Excluir";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnEditar
            // 
            this.btnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditar.Enabled = false;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnEditar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnEditar.Location = new System.Drawing.Point(100, 15);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(78, 33);
            this.btnEditar.TabIndex = 103;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            // 
            // ucGerenciadorEstoque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.cbConcluidas);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBusca);
            this.Controls.Add(this.dgvEstoque);
            this.Controls.Add(this.label4);
            this.Name = "ucGerenciadorEstoque";
            this.Size = new System.Drawing.Size(1175, 749);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoque)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).EndInit();
            this.panelBotoes.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panelBotoes;
        private System.Windows.Forms.Button btnSaida;
        private System.Windows.Forms.Button btnVisualizacoes;
        private System.Windows.Forms.Button btnEntrada;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEditar;

        private System.Windows.Forms.CheckBox cbConcluidas;
        private System.Windows.Forms.PictureBox btnAtualizar;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBusca;

        private System.Windows.Forms.DataGridView dgvEstoque;

        private System.Windows.Forms.Label label4;

        #endregion
    }
}