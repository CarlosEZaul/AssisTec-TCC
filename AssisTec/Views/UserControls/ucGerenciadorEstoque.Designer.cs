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
            this.cbSemEstoque = new System.Windows.Forms.CheckBox();
            this.btnAtualizar = new System.Windows.Forms.PictureBox();
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.btnSaida = new System.Windows.Forms.Button();
            this.btnVisualizacoes = new System.Windows.Forms.Button();
            this.btnEntrada = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnStatus = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDesativados = new System.Windows.Forms.CheckBox();
            this.cbAbaixoMinimo = new System.Windows.Forms.CheckBox();
            this.panelExibicao = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblSemEstoque = new System.Windows.Forms.Label();
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
            this.lblProdutosCadastrados = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoque)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).BeginInit();
            this.panelBotoes.SuspendLayout();
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
            this.dgvEstoque.Location = new System.Drawing.Point(18, 179);
            this.dgvEstoque.MultiSelect = false;
            this.dgvEstoque.Name = "dgvEstoque";
            this.dgvEstoque.ReadOnly = true;
            this.dgvEstoque.Size = new System.Drawing.Size(1138, 503);
            this.dgvEstoque.TabIndex = 149;
            this.dgvEstoque.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEstoque_CellClick);
            this.dgvEstoque.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvEstoque_CellFormatting);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(356, 19);
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
            this.txtBusca.Location = new System.Drawing.Point(417, 19);
            this.txtBusca.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(471, 20);
            this.txtBusca.TabIndex = 151;
            this.txtBusca.TextChanged += new System.EventHandler(this.txtBusca_TextChanged);
            // 
            // cbSemEstoque
            // 
            this.cbSemEstoque.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSemEstoque.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cbSemEstoque.Location = new System.Drawing.Point(938, 19);
            this.cbSemEstoque.Name = "cbSemEstoque";
            this.cbSemEstoque.Size = new System.Drawing.Size(190, 24);
            this.cbSemEstoque.TabIndex = 155;
            this.cbSemEstoque.Text = "Exibir produtos sem estoque";
            this.cbSemEstoque.UseVisualStyleBackColor = true;
            this.cbSemEstoque.CheckedChanged += new System.EventHandler(this.cbSemEstoque_CheckedChanged);
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAtualizar.BackgroundImage")));
            this.btnAtualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAtualizar.Location = new System.Drawing.Point(1134, 3);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(38, 38);
            this.btnAtualizar.TabIndex = 154;
            this.btnAtualizar.TabStop = false;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // panelBotoes
            // 
            this.panelBotoes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelBotoes.Controls.Add(this.btnSaida);
            this.panelBotoes.Controls.Add(this.btnVisualizacoes);
            this.panelBotoes.Controls.Add(this.btnEntrada);
            this.panelBotoes.Controls.Add(this.btnNew);
            this.panelBotoes.Controls.Add(this.btnStatus);
            this.panelBotoes.Controls.Add(this.btnEditar);
            this.panelBotoes.Location = new System.Drawing.Point(240, 688);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new System.Drawing.Size(721, 61);
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
            this.btnSaida.Location = new System.Drawing.Point(427, 15);
            this.btnSaida.Name = "btnSaida";
            this.btnSaida.Size = new System.Drawing.Size(133, 33);
            this.btnSaida.TabIndex = 106;
            this.btnSaida.Text = "Registrar Saida";
            this.btnSaida.UseVisualStyleBackColor = false;
            this.btnSaida.Click += new System.EventHandler(this.btnSaida_Click);
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
            this.btnVisualizacoes.Location = new System.Drawing.Point(566, 15);
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
            this.btnEntrada.Location = new System.Drawing.Point(285, 15);
            this.btnEntrada.Name = "btnEntrada";
            this.btnEntrada.Size = new System.Drawing.Size(136, 33);
            this.btnEntrada.TabIndex = 104;
            this.btnEntrada.Text = "Registrar Entrada";
            this.btnEntrada.UseVisualStyleBackColor = false;
            this.btnEntrada.Click += new System.EventHandler(this.btnEntrada_Click);
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
            // btnStatus
            // 
            this.btnStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStatus.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStatus.Enabled = false;
            this.btnStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnStatus.ForeColor = System.Drawing.SystemColors.Control;
            this.btnStatus.Location = new System.Drawing.Point(182, 15);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(97, 33);
            this.btnStatus.TabIndex = 101;
            this.btnStatus.Text = "Ativar/Desativar";
            this.btnStatus.UseVisualStyleBackColor = false;
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click);
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
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.cbDesativados);
            this.panel1.Controls.Add(this.cbAbaixoMinimo);
            this.panel1.Controls.Add(this.btnAtualizar);
            this.panel1.Controls.Add(this.cbSemEstoque);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtBusca);
            this.panel1.Location = new System.Drawing.Point(0, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1175, 60);
            this.panel1.TabIndex = 157;
            // 
            // cbDesativados
            // 
            this.cbDesativados.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDesativados.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cbDesativados.Location = new System.Drawing.Point(938, 39);
            this.cbDesativados.Name = "cbDesativados";
            this.cbDesativados.Size = new System.Drawing.Size(190, 21);
            this.cbDesativados.TabIndex = 157;
            this.cbDesativados.Text = "Exibir produtos desativados";
            this.cbDesativados.UseVisualStyleBackColor = true;
            this.cbDesativados.CheckedChanged += new System.EventHandler(this.cbDesativados_CheckedChanged);
            // 
            // cbAbaixoMinimo
            // 
            this.cbAbaixoMinimo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAbaixoMinimo.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cbAbaixoMinimo.Location = new System.Drawing.Point(938, 0);
            this.cbAbaixoMinimo.Name = "cbAbaixoMinimo";
            this.cbAbaixoMinimo.Size = new System.Drawing.Size(190, 24);
            this.cbAbaixoMinimo.TabIndex = 156;
            this.cbAbaixoMinimo.Text = "Exibir produtos abaixo do minímo";
            this.cbAbaixoMinimo.UseVisualStyleBackColor = true;
            this.cbAbaixoMinimo.CheckedChanged += new System.EventHandler(this.cbAbaixoMinimo_CheckedChanged);
            // 
            // panelExibicao
            // 
            this.panelExibicao.Controls.Add(this.panel4);
            this.panelExibicao.Controls.Add(this.panel5);
            this.panelExibicao.Controls.Add(this.panel3);
            this.panelExibicao.Controls.Add(this.panel2);
            this.panelExibicao.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExibicao.Location = new System.Drawing.Point(0, 38);
            this.panelExibicao.Name = "panelExibicao";
            this.panelExibicao.Size = new System.Drawing.Size(1175, 75);
            this.panelExibicao.TabIndex = 164;
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lblSemEstoque);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.pictureBox3);
            this.panel4.Location = new System.Drawing.Point(582, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(239, 56);
            this.panel4.TabIndex = 164;
            // 
            // lblSemEstoque
            // 
            this.lblSemEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSemEstoque.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblSemEstoque.Location = new System.Drawing.Point(66, 26);
            this.lblSemEstoque.Name = "lblSemEstoque";
            this.lblSemEstoque.Size = new System.Drawing.Size(115, 23);
            this.lblSemEstoque.TabIndex = 4;
            this.lblSemEstoque.Text = "0";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label11.Location = new System.Drawing.Point(66, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(115, 23);
            this.label11.TabIndex = 1;
            this.label11.Text = "Sem estoque";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::AssisTec.Properties.Resources.fora_de_estoque;
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
            this.pictureBox4.BackgroundImage = global::AssisTec.Properties.Resources.valor_estoque;
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
            this.label9.Size = new System.Drawing.Size(115, 23);
            this.label9.TabIndex = 1;
            this.label9.Text = "Abaixo do Mínimo";
            // 
            // fifgurinha
            // 
            this.fifgurinha.BackgroundImage = global::AssisTec.Properties.Resources.abaixo_minimo;
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
            this.panel2.Controls.Add(this.lblProdutosCadastrados);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(92, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 56);
            this.panel2.TabIndex = 162;
            // 
            // lblProdutosCadastrados
            // 
            this.lblProdutosCadastrados.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdutosCadastrados.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblProdutosCadastrados.Location = new System.Drawing.Point(71, 26);
            this.lblProdutosCadastrados.Name = "lblProdutosCadastrados";
            this.lblProdutosCadastrados.Size = new System.Drawing.Size(115, 23);
            this.lblProdutosCadastrados.TabIndex = 2;
            this.lblProdutosCadastrados.Text = "0";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(66, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 23);
            this.label7.TabIndex = 1;
            this.label7.Text = "Produtos Cadastrados";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::AssisTec.Properties.Resources.produto;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::AssisTec.Properties.Resources.produto;
            this.pictureBox1.Location = new System.Drawing.Point(3, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 51);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ucGerenciadorEstoque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.panelExibicao);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.dgvEstoque);
            this.Controls.Add(this.label4);
            this.Name = "ucGerenciadorEstoque";
            this.Size = new System.Drawing.Size(1175, 749);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstoque)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).EndInit();
            this.panelBotoes.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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

        private System.Windows.Forms.CheckBox cbDesativados;

        private System.Windows.Forms.CheckBox cbAbaixoMinimo;

        private System.Windows.Forms.Label lblMinimo;

        private System.Windows.Forms.PictureBox fifgurinha;

        private System.Windows.Forms.Panel panelExibicao;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblSemEstoque;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblValorEstoque;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel panel3;

        private System.Windows.Forms.Label label9;

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblProdutosCadastrados;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Panel panelBotoes;
        private System.Windows.Forms.Button btnSaida;
        private System.Windows.Forms.Button btnVisualizacoes;
        private System.Windows.Forms.Button btnEntrada;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnStatus;
        private System.Windows.Forms.Button btnEditar;

        private System.Windows.Forms.CheckBox cbSemEstoque;
        private System.Windows.Forms.PictureBox btnAtualizar;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBusca;

        private System.Windows.Forms.DataGridView dgvEstoque;

        private System.Windows.Forms.Label label4;

        #endregion
    }
}