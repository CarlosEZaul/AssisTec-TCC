using System.ComponentModel;

namespace AssisTec.UserControls
{
    partial class ucContasReceber
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucContasReceber));
            this.dgvContasReceber = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mtbDataInicio = new System.Windows.Forms.MaskedTextBox();
            this.mtbDataFim = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnAtualizar = new System.Windows.Forms.PictureBox();
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnRecibo = new System.Windows.Forms.Button();
            this.btnRelatorio = new System.Windows.Forms.Button();
            this.cbFormaPagamento = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTotalReceber = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelExibicao = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblPendente = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblAtrasado = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblRecebido = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContasReceber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).BeginInit();
            this.panelBotoes.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelExibicao.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvContasReceber
            // 
            this.dgvContasReceber.AllowUserToAddRows = false;
            this.dgvContasReceber.AllowUserToDeleteRows = false;
            this.dgvContasReceber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContasReceber.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvContasReceber.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvContasReceber.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvContasReceber.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContasReceber.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dgvContasReceber.Location = new System.Drawing.Point(18, 179);
            this.dgvContasReceber.MultiSelect = false;
            this.dgvContasReceber.Name = "dgvContasReceber";
            this.dgvContasReceber.ReadOnly = true;
            this.dgvContasReceber.Size = new System.Drawing.Size(1138, 503);
            this.dgvContasReceber.TabIndex = 53;
            this.dgvContasReceber.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContasReceber_CellClick);
            this.dgvContasReceber.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvContasReceber_CellFormatting);
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
            this.label4.TabIndex = 147;
            this.label4.Text = "Contas a Receber";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(42, 134);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 18);
            this.label1.TabIndex = 149;
            this.label1.Text = "Data de início:";
            // 
            // mtbDataInicio
            // 
            this.mtbDataInicio.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtbDataInicio.Location = new System.Drawing.Point(41, 152);
            this.mtbDataInicio.Mask = "00/00/0000";
            this.mtbDataInicio.Name = "mtbDataInicio";
            this.mtbDataInicio.Size = new System.Drawing.Size(100, 20);
            this.mtbDataInicio.TabIndex = 150;
            this.mtbDataInicio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mtbDataFim
            // 
            this.mtbDataFim.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtbDataFim.Location = new System.Drawing.Point(177, 152);
            this.mtbDataFim.Mask = "00/00/0000";
            this.mtbDataFim.Name = "mtbDataFim";
            this.mtbDataFim.Size = new System.Drawing.Size(100, 20);
            this.mtbDataFim.TabIndex = 152;
            this.mtbDataFim.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(194, 134);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 18);
            this.label2.TabIndex = 151;
            this.label2.Text = "Data fim:";
            // 
            // cbStatus
            // 
            this.cbStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(574, 152);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(158, 21);
            this.cbStatus.TabIndex = 154;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(618, 134);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 18);
            this.label3.TabIndex = 153;
            this.label3.Text = "Status:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(370, 134);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 18);
            this.label5.TabIndex = 156;
            this.label5.Text = "Motivo ou descrição:";
            // 
            // txtBusca
            // 
            this.txtBusca.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtBusca.BackColor = System.Drawing.Color.White;
            this.txtBusca.Location = new System.Drawing.Point(299, 152);
            this.txtBusca.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(268, 20);
            this.txtBusca.TabIndex = 155;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBuscar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnBuscar.Location = new System.Drawing.Point(942, 140);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(139, 33);
            this.btnBuscar.TabIndex = 157;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAtualizar.BackgroundImage")));
            this.btnAtualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAtualizar.Location = new System.Drawing.Point(1118, 135);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(38, 38);
            this.btnAtualizar.TabIndex = 158;
            this.btnAtualizar.TabStop = false;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // panelBotoes
            // 
            this.panelBotoes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelBotoes.Controls.Add(this.btnDelete);
            this.panelBotoes.Controls.Add(this.btnEditar);
            this.panelBotoes.Controls.Add(this.btnRegistrar);
            this.panelBotoes.Controls.Add(this.btnRecibo);
            this.panelBotoes.Controls.Add(this.btnRelatorio);
            this.panelBotoes.Location = new System.Drawing.Point(370, 688);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new System.Drawing.Size(484, 61);
            this.panelBotoes.TabIndex = 159;
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
            this.btnDelete.Location = new System.Drawing.Point(204, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(78, 33);
            this.btnDelete.TabIndex = 104;
            this.btnDelete.Text = "Excluir";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            this.btnEditar.Location = new System.Drawing.Point(120, 15);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(78, 33);
            this.btnEditar.TabIndex = 105;
            this.btnEditar.Text = "Gerenciar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRegistrar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRegistrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegistrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRegistrar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRegistrar.Location = new System.Drawing.Point(14, 15);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(100, 33);
            this.btnRegistrar.TabIndex = 99;
            this.btnRegistrar.Text = "Registrar Entrada";
            this.btnRegistrar.UseVisualStyleBackColor = false;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
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
            this.btnRecibo.Location = new System.Drawing.Point(384, 15);
            this.btnRecibo.Name = "btnRecibo";
            this.btnRecibo.Size = new System.Drawing.Size(88, 33);
            this.btnRecibo.TabIndex = 101;
            this.btnRecibo.Text = "Emitir Recibo";
            this.btnRecibo.UseVisualStyleBackColor = false;
            this.btnRecibo.Click += new System.EventHandler(this.btnRecibo_Click);
            // 
            // btnRelatorio
            // 
            this.btnRelatorio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRelatorio.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRelatorio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRelatorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRelatorio.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRelatorio.Location = new System.Drawing.Point(288, 15);
            this.btnRelatorio.Name = "btnRelatorio";
            this.btnRelatorio.Size = new System.Drawing.Size(90, 33);
            this.btnRelatorio.TabIndex = 103;
            this.btnRelatorio.Text = "Gerar Relatório";
            this.btnRelatorio.UseVisualStyleBackColor = false;
            this.btnRelatorio.Click += new System.EventHandler(this.btnRelatorio_Click);
            // 
            // cbFormaPagamento
            // 
            this.cbFormaPagamento.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbFormaPagamento.FormattingEnabled = true;
            this.cbFormaPagamento.Location = new System.Drawing.Point(738, 152);
            this.cbFormaPagamento.Name = "cbFormaPagamento";
            this.cbFormaPagamento.Size = new System.Drawing.Size(158, 21);
            this.cbFormaPagamento.TabIndex = 161;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(747, 134);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 18);
            this.label6.TabIndex = 160;
            this.label6.Text = "Forma de Pagamento:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblTotalReceber);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(4, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(239, 56);
            this.panel1.TabIndex = 162;
            // 
            // lblTotalReceber
            // 
            this.lblTotalReceber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalReceber.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTotalReceber.Location = new System.Drawing.Point(71, 26);
            this.lblTotalReceber.Name = "lblTotalReceber";
            this.lblTotalReceber.Size = new System.Drawing.Size(115, 23);
            this.lblTotalReceber.TabIndex = 2;
            this.lblTotalReceber.Text = "R$";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(66, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 23);
            this.label7.TabIndex = 1;
            this.label7.Text = "Total a receber";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::AssisTec.Properties.Resources.cifrao;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 51);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelExibicao
            // 
            this.panelExibicao.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelExibicao.Controls.Add(this.panel4);
            this.panelExibicao.Controls.Add(this.panel5);
            this.panelExibicao.Controls.Add(this.panel3);
            this.panelExibicao.Controls.Add(this.panel1);
            this.panelExibicao.Location = new System.Drawing.Point(86, 59);
            this.panelExibicao.Name = "panelExibicao";
            this.panelExibicao.Size = new System.Drawing.Size(986, 75);
            this.panelExibicao.TabIndex = 163;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lblPendente);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.pictureBox3);
            this.panel4.Location = new System.Drawing.Point(494, 11);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(239, 56);
            this.panel4.TabIndex = 164;
            // 
            // lblPendente
            // 
            this.lblPendente.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPendente.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblPendente.Location = new System.Drawing.Point(66, 26);
            this.lblPendente.Name = "lblPendente";
            this.lblPendente.Size = new System.Drawing.Size(115, 23);
            this.lblPendente.TabIndex = 4;
            this.lblPendente.Text = "R$";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label11.Location = new System.Drawing.Point(66, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(115, 23);
            this.label11.TabIndex = 1;
            this.label11.Text = "Pendente";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::AssisTec.Properties.Resources.relogio_pendente;
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
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.lblAtrasado);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Controls.Add(this.pictureBox4);
            this.panel5.Location = new System.Drawing.Point(739, 11);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(239, 56);
            this.panel5.TabIndex = 165;
            // 
            // lblAtrasado
            // 
            this.lblAtrasado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAtrasado.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblAtrasado.Location = new System.Drawing.Point(66, 26);
            this.lblAtrasado.Name = "lblAtrasado";
            this.lblAtrasado.Size = new System.Drawing.Size(115, 23);
            this.lblAtrasado.TabIndex = 5;
            this.lblAtrasado.Text = "R$";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label13.Location = new System.Drawing.Point(66, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 23);
            this.label13.TabIndex = 1;
            this.label13.Text = "Atrasado";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::AssisTec.Properties.Resources.atencao;
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
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lblRecebido);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Location = new System.Drawing.Point(249, 11);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 56);
            this.panel3.TabIndex = 163;
            // 
            // lblRecebido
            // 
            this.lblRecebido.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecebido.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblRecebido.Location = new System.Drawing.Point(66, 26);
            this.lblRecebido.Name = "lblRecebido";
            this.lblRecebido.Size = new System.Drawing.Size(115, 23);
            this.lblRecebido.TabIndex = 3;
            this.lblRecebido.Text = "R$";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label9.Location = new System.Drawing.Point(66, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 23);
            this.label9.TabIndex = 1;
            this.label9.Text = "Recebido";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AssisTec.Properties.Resources.aprovado;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(3, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(57, 51);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // ucContasReceber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.Controls.Add(this.panelExibicao);
            this.Controls.Add(this.cbFormaPagamento);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBusca);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mtbDataFim);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mtbDataInicio);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvContasReceber);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "ucContasReceber";
            this.Size = new System.Drawing.Size(1175, 749);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContasReceber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).EndInit();
            this.panelBotoes.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelExibicao.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEditar;

        private System.Windows.Forms.Label lblRecebido;
        private System.Windows.Forms.Label lblPendente;
        private System.Windows.Forms.Label lblAtrasado;

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox4;

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox3;

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox2;

        private System.Windows.Forms.Panel panelExibicao;

        private System.Windows.Forms.Label lblTotalReceber;

        private System.Windows.Forms.Label label7;

        private System.Windows.Forms.PictureBox pictureBox1;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbFormaPagamento;

        private System.Windows.Forms.Panel panelBotoes;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnRecibo;
        private System.Windows.Forms.Button btnRelatorio;

        private System.Windows.Forms.PictureBox btnAtualizar;

        private System.Windows.Forms.Button btnBuscar;

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBusca;

        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.MaskedTextBox mtbDataFim;
        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox mtbDataInicio;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.DataGridView dgvContasReceber;

        #endregion
    }
}