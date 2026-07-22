using System.ComponentModel;

namespace AssisTec.UserControls
{
    partial class ucGerenciadorOS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucGerenciadorOS));
            this.dgvOS = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.btnRelatorio = new System.Windows.Forms.Button();
            this.btnRecibo = new System.Windows.Forms.Button();
            this.btnPagamento = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnGerenciar = new System.Windows.Forms.Button();
            this.panelExibicao = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblRetirada = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblReceber = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblEmAndamento = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTotalOS = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelFiltro = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAtualizar = new System.Windows.Forms.PictureBox();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOS)).BeginInit();
            this.panelBotoes.SuspendLayout();
            this.panelExibicao.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelFiltro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOS
            // 
            this.dgvOS.AllowUserToAddRows = false;
            this.dgvOS.AllowUserToDeleteRows = false;
            this.dgvOS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOS.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOS.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvOS.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvOS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOS.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dgvOS.Location = new System.Drawing.Point(18, 179);
            this.dgvOS.MultiSelect = false;
            this.dgvOS.Name = "dgvOS";
            this.dgvOS.ReadOnly = true;
            this.dgvOS.Size = new System.Drawing.Size(1138, 503);
            this.dgvOS.TabIndex = 53;
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
            this.label4.TabIndex = 147;
            this.label4.Text = "Gerenciador de Ordens de Serviço";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelBotoes
            // 
            this.panelBotoes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelBotoes.Controls.Add(this.btnRelatorio);
            this.panelBotoes.Controls.Add(this.btnRecibo);
            this.panelBotoes.Controls.Add(this.btnPagamento);
            this.panelBotoes.Controls.Add(this.btnNew);
            this.panelBotoes.Controls.Add(this.btnDelete);
            this.panelBotoes.Controls.Add(this.btnGerenciar);
            this.panelBotoes.Location = new System.Drawing.Point(229, 688);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new System.Drawing.Size(692, 61);
            this.panelBotoes.TabIndex = 148;
            // 
            // btnRelatorio
            // 
            this.btnRelatorio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRelatorio.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRelatorio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRelatorio.Enabled = false;
            this.btnRelatorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRelatorio.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRelatorio.Location = new System.Drawing.Point(408, 15);
            this.btnRelatorio.Name = "btnRelatorio";
            this.btnRelatorio.Size = new System.Drawing.Size(133, 33);
            this.btnRelatorio.TabIndex = 106;
            this.btnRelatorio.Text = "Emitir Relatorio";
            this.btnRelatorio.UseVisualStyleBackColor = false;
            // 
            // btnRecibo
            // 
            this.btnRecibo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRecibo.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRecibo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecibo.Enabled = false;
            this.btnRecibo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecibo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRecibo.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRecibo.Location = new System.Drawing.Point(547, 15);
            this.btnRecibo.Name = "btnRecibo";
            this.btnRecibo.Size = new System.Drawing.Size(133, 33);
            this.btnRecibo.TabIndex = 105;
            this.btnRecibo.Text = "Emitir Recibo";
            this.btnRecibo.UseVisualStyleBackColor = false;
            // 
            // btnPagamento
            // 
            this.btnPagamento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPagamento.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnPagamento.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPagamento.Enabled = false;
            this.btnPagamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagamento.ForeColor = System.Drawing.SystemColors.Control;
            this.btnPagamento.Location = new System.Drawing.Point(266, 15);
            this.btnPagamento.Name = "btnPagamento";
            this.btnPagamento.Size = new System.Drawing.Size(136, 33);
            this.btnPagamento.TabIndex = 104;
            this.btnPagamento.Text = "Registrar Pagamento";
            this.btnPagamento.UseVisualStyleBackColor = false;
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
            // btnGerenciar
            // 
            this.btnGerenciar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGerenciar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnGerenciar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGerenciar.Enabled = false;
            this.btnGerenciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGerenciar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnGerenciar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnGerenciar.Location = new System.Drawing.Point(100, 15);
            this.btnGerenciar.Name = "btnGerenciar";
            this.btnGerenciar.Size = new System.Drawing.Size(78, 33);
            this.btnGerenciar.TabIndex = 103;
            this.btnGerenciar.Text = "Gerenciar";
            this.btnGerenciar.UseVisualStyleBackColor = false;
            // 
            // panelExibicao
            // 
            this.panelExibicao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelExibicao.Controls.Add(this.panel4);
            this.panelExibicao.Controls.Add(this.panel5);
            this.panelExibicao.Controls.Add(this.panel3);
            this.panelExibicao.Controls.Add(this.panel1);
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
            this.panel4.Controls.Add(this.lblRetirada);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.pictureBox3);
            this.panel4.Location = new System.Drawing.Point(581, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(239, 56);
            this.panel4.TabIndex = 164;
            // 
            // lblRetirada
            // 
            this.lblRetirada.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRetirada.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblRetirada.Location = new System.Drawing.Point(66, 26);
            this.lblRetirada.Name = "lblRetirada";
            this.lblRetirada.Size = new System.Drawing.Size(115, 23);
            this.lblRetirada.TabIndex = 4;
            this.lblRetirada.Text = "0";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label11.Location = new System.Drawing.Point(66, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(115, 23);
            this.label11.TabIndex = 1;
            this.label11.Text = "Para Retirada";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::AssisTec.Properties.Resources.ParaRetirada;
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
            this.panel5.Controls.Add(this.lblReceber);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Controls.Add(this.pictureBox4);
            this.panel5.Location = new System.Drawing.Point(826, 13);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(239, 56);
            this.panel5.TabIndex = 165;
            // 
            // lblReceber
            // 
            this.lblReceber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceber.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblReceber.Location = new System.Drawing.Point(66, 26);
            this.lblReceber.Name = "lblReceber";
            this.lblReceber.Size = new System.Drawing.Size(115, 23);
            this.lblReceber.TabIndex = 5;
            this.lblReceber.Text = "R$";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label13.Location = new System.Drawing.Point(66, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 23);
            this.label13.TabIndex = 1;
            this.label13.Text = "A receber";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::AssisTec.Properties.Resources.cifrao;
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
            this.panel3.Controls.Add(this.lblEmAndamento);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Location = new System.Drawing.Point(336, 13);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 56);
            this.panel3.TabIndex = 163;
            // 
            // lblEmAndamento
            // 
            this.lblEmAndamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmAndamento.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblEmAndamento.Location = new System.Drawing.Point(66, 26);
            this.lblEmAndamento.Name = "lblEmAndamento";
            this.lblEmAndamento.Size = new System.Drawing.Size(115, 23);
            this.lblEmAndamento.TabIndex = 3;
            this.lblEmAndamento.Text = "0";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label9.Location = new System.Drawing.Point(66, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 23);
            this.label9.TabIndex = 1;
            this.label9.Text = "Em atendimento";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AssisTec.Properties.Resources.EmAndamento;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(3, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(57, 51);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblTotalOS);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(91, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(239, 56);
            this.panel1.TabIndex = 162;
            // 
            // lblTotalOS
            // 
            this.lblTotalOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalOS.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTotalOS.Location = new System.Drawing.Point(71, 26);
            this.lblTotalOS.Name = "lblTotalOS";
            this.lblTotalOS.Size = new System.Drawing.Size(115, 23);
            this.lblTotalOS.TabIndex = 2;
            this.lblTotalOS.Text = "0";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(66, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 23);
            this.label7.TabIndex = 1;
            this.label7.Text = "Total de OS";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::AssisTec.Properties.Resources.ordemServico;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 51);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelFiltro
            // 
            this.panelFiltro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFiltro.Controls.Add(this.label5);
            this.panelFiltro.Controls.Add(this.btnAtualizar);
            this.panelFiltro.Controls.Add(this.txtBusca);
            this.panelFiltro.Controls.Add(this.btnBuscar);
            this.panelFiltro.Controls.Add(this.cbStatus);
            this.panelFiltro.Controls.Add(this.label3);
            this.panelFiltro.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFiltro.Location = new System.Drawing.Point(0, 113);
            this.panelFiltro.Name = "panelFiltro";
            this.panelFiltro.Size = new System.Drawing.Size(1175, 59);
            this.panelFiltro.TabIndex = 165;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(158, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(306, 18);
            this.label5.TabIndex = 156;
            this.label5.Text = "Buscar (Nº, Cliente, Técnico ou Equipamento ):";
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAtualizar.BackgroundImage")));
            this.btnAtualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAtualizar.Location = new System.Drawing.Point(1116, 14);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(38, 38);
            this.btnAtualizar.TabIndex = 158;
            this.btnAtualizar.TabStop = false;
            // 
            // txtBusca
            // 
            this.txtBusca.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtBusca.BackColor = System.Drawing.Color.White;
            this.txtBusca.Location = new System.Drawing.Point(91, 32);
            this.txtBusca.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(440, 20);
            this.txtBusca.TabIndex = 155;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBuscar.Location = new System.Drawing.Point(1009, 19);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(77, 33);
            this.btnBuscar.TabIndex = 157;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            // 
            // cbStatus
            // 
            this.cbStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(538, 31);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(158, 21);
            this.cbStatus.TabIndex = 154;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(587, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 18);
            this.label3.TabIndex = 153;
            this.label3.Text = "Status:";
            // 
            // ucGerenciadorOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.panelFiltro);
            this.Controls.Add(this.panelExibicao);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvOS);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "ucGerenciadorOS";
            this.Size = new System.Drawing.Size(1175, 749);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOS)).EndInit();
            this.panelBotoes.ResumeLayout(false);
            this.panelExibicao.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelFiltro.ResumeLayout(false);
            this.panelFiltro.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelFiltro;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox btnAtualizar;
        private System.Windows.Forms.TextBox txtBusca;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.Panel panelExibicao;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblRetirada;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblReceber;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblEmAndamento;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTotalOS;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;

        private System.Windows.Forms.Panel panelBotoes;
        private System.Windows.Forms.Button btnRecibo;
        private System.Windows.Forms.Button btnPagamento;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnGerenciar;
        private System.Windows.Forms.Button btnRelatorio;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.DataGridView dgvOS;

        #endregion
    }
}