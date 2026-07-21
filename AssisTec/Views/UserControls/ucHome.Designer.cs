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
            this.lblContaPagar = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblMinimo = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.picturebox = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblOrdemServico = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblOsRecentes = new System.Windows.Forms.Label();
            this.dgvOS = new System.Windows.Forms.DataGridView();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblEstoque = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.tlpBotoes = new System.Windows.Forms.TableLayoutPanel();
            this.btnOs = new System.Windows.Forms.Button();
            this.btnSaidaEstoque = new System.Windows.Forms.Button();
            this.btnCliente = new System.Windows.Forms.Button();
            this.btnEntradaEstoque = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelExibicao.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOS)).BeginInit();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panelBotoes.SuspendLayout();
            this.tlpBotoes.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(48)))), ((int)(((byte)(66)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblData);
            this.panel1.Controls.Add(this.lblNome);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1152, 82);
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
            this.lblNome.Size = new System.Drawing.Size(1089, 44);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "label1";
            // 
            // panelExibicao
            // 
            this.panelExibicao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelExibicao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(48)))), ((int)(((byte)(66)))));
            this.panelExibicao.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelExibicao.Controls.Add(this.panel4);
            this.panelExibicao.Controls.Add(this.panel5);
            this.panelExibicao.Controls.Add(this.panel3);
            this.panelExibicao.Controls.Add(this.panel2);
            this.panelExibicao.Location = new System.Drawing.Point(0, 82);
            this.panelExibicao.Name = "panelExibicao";
            this.panelExibicao.Size = new System.Drawing.Size(1152, 135);
            this.panelExibicao.TabIndex = 165;
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lblFaturamento);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.pictureBox3);
            this.panel4.Location = new System.Drawing.Point(569, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(239, 91);
            this.panel4.TabIndex = 164;
            // 
            // lblFaturamento
            // 
            this.lblFaturamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.lblFaturamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFaturamento.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblFaturamento.Location = new System.Drawing.Point(66, 41);
            this.lblFaturamento.Name = "lblFaturamento";
            this.lblFaturamento.Size = new System.Drawing.Size(115, 23);
            this.lblFaturamento.TabIndex = 4;
            this.lblFaturamento.Text = "R$";
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
            this.pictureBox3.BackgroundImage = global::AssisTec.Properties.Resources.cifrao;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Image = global::AssisTec.Properties.Resources.cifrao;
            this.pictureBox3.Location = new System.Drawing.Point(3, 26);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(57, 51);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.lblContaPagar);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Controls.Add(this.pictureBox4);
            this.panel5.Location = new System.Drawing.Point(814, 13);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(239, 91);
            this.panel5.TabIndex = 165;
            // 
            // lblContaPagar
            // 
            this.lblContaPagar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContaPagar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblContaPagar.Location = new System.Drawing.Point(62, 41);
            this.lblContaPagar.Name = "lblContaPagar";
            this.lblContaPagar.Size = new System.Drawing.Size(115, 23);
            this.lblContaPagar.TabIndex = 5;
            this.lblContaPagar.Text = "R$";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label13.Location = new System.Drawing.Point(66, 3);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 23);
            this.label13.TabIndex = 1;
            this.label13.Text = "Contas a Pagar";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::AssisTec.Properties.Resources.cifrao_vermelho_removebg_preview;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Location = new System.Drawing.Point(3, 26);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(57, 51);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lblMinimo);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.picturebox);
            this.panel3.Location = new System.Drawing.Point(324, 13);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 91);
            this.panel3.TabIndex = 163;
            // 
            // lblMinimo
            // 
            this.lblMinimo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.lblMinimo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinimo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblMinimo.Location = new System.Drawing.Point(66, 41);
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
            // picturebox
            // 
            this.picturebox.BackgroundImage = global::AssisTec.Properties.Resources.abaixo_minimo;
            this.picturebox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picturebox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturebox.Location = new System.Drawing.Point(3, 26);
            this.picturebox.Name = "picturebox";
            this.picturebox.Size = new System.Drawing.Size(57, 51);
            this.picturebox.TabIndex = 0;
            this.picturebox.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblOrdemServico);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(79, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(239, 91);
            this.panel2.TabIndex = 162;
            // 
            // lblOrdemServico
            // 
            this.lblOrdemServico.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrdemServico.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblOrdemServico.Location = new System.Drawing.Point(66, 41);
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
            this.label7.Size = new System.Drawing.Size(168, 23);
            this.label7.TabIndex = 1;
            this.label7.Text = "Ordens Abertas";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::AssisTec.Properties.Resources.ordemServico;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 51);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.lblOsRecentes);
            this.panel6.Controls.Add(this.dgvOS);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(626, 461);
            this.panel6.TabIndex = 166;
            // 
            // lblOsRecentes
            // 
            this.lblOsRecentes.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOsRecentes.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblOsRecentes.ForeColor = System.Drawing.Color.White;
            this.lblOsRecentes.Location = new System.Drawing.Point(0, 0);
            this.lblOsRecentes.Name = "lblOsRecentes";
            this.lblOsRecentes.Size = new System.Drawing.Size(622, 36);
            this.lblOsRecentes.TabIndex = 2;
            this.lblOsRecentes.Text = "Ordens de Serviço recentes";
            this.lblOsRecentes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvOS
            // 
            this.dgvOS.AllowUserToAddRows = false;
            this.dgvOS.AllowUserToDeleteRows = false;
            this.dgvOS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOS.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOS.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvOS.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvOS.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvOS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOS.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dgvOS.Location = new System.Drawing.Point(3, 49);
            this.dgvOS.MultiSelect = false;
            this.dgvOS.Name = "dgvOS";
            this.dgvOS.ReadOnly = true;
            this.dgvOS.Size = new System.Drawing.Size(602, 380);
            this.dgvOS.TabIndex = 54;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel7.Controls.Add(this.lblEstoque);
            this.panel7.Controls.Add(this.dataGridView1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(626, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(523, 464);
            this.panel7.TabIndex = 167;
            // 
            // lblEstoque
            // 
            this.lblEstoque.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblEstoque.ForeColor = System.Drawing.Color.White;
            this.lblEstoque.Location = new System.Drawing.Point(136, -2);
            this.lblEstoque.Name = "lblEstoque";
            this.lblEstoque.Size = new System.Drawing.Size(294, 36);
            this.lblEstoque.TabIndex = 55;
            this.lblEstoque.Text = "Estoque baixo";
            this.lblEstoque.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Gray;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridView1.Location = new System.Drawing.Point(35, 49);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(469, 380);
            this.dataGridView1.TabIndex = 55;
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Location = new System.Drawing.Point(3, 216);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1149, 464);
            this.panel8.TabIndex = 168;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.panel6);
            this.panel9.Controls.Add(this.panel7);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1149, 464);
            this.panel9.TabIndex = 168;
            // 
            // panelBotoes
            // 
            this.panelBotoes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBotoes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBotoes.Controls.Add(this.tlpBotoes);
            this.panelBotoes.Controls.Add(this.label1);
            this.panelBotoes.Location = new System.Drawing.Point(629, 683);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new System.Drawing.Size(523, 216);
            this.panelBotoes.TabIndex = 169;
            // 
            // tlpBotoes
            // 
            this.tlpBotoes.ColumnCount = 1;
            this.tlpBotoes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBotoes.Controls.Add(this.btnOs, 0, 0);
            this.tlpBotoes.Controls.Add(this.btnSaidaEstoque, 0, 3);
            this.tlpBotoes.Controls.Add(this.btnCliente, 0, 1);
            this.tlpBotoes.Controls.Add(this.btnEntradaEstoque, 0, 2);
            this.tlpBotoes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBotoes.Location = new System.Drawing.Point(0, 36);
            this.tlpBotoes.Name = "tlpBotoes";
            this.tlpBotoes.RowCount = 4;
            this.tlpBotoes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpBotoes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpBotoes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpBotoes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpBotoes.Size = new System.Drawing.Size(519, 176);
            this.tlpBotoes.TabIndex = 57;
            // 
            // btnOs
            // 
            this.btnOs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(48)))), ((int)(((byte)(66)))));
            this.btnOs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnOs.ForeColor = System.Drawing.SystemColors.Control;
            this.btnOs.Location = new System.Drawing.Point(3, 3);
            this.btnOs.Name = "btnOs";
            this.btnOs.Size = new System.Drawing.Size(513, 38);
            this.btnOs.TabIndex = 104;
            this.btnOs.Text = "Nova Ordem de Serviço";
            this.btnOs.UseVisualStyleBackColor = false;
            this.btnOs.Click += new System.EventHandler(this.btnOs_Click);
            // 
            // btnSaidaEstoque
            // 
            this.btnSaidaEstoque.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(48)))), ((int)(((byte)(66)))));
            this.btnSaidaEstoque.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaidaEstoque.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaidaEstoque.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaidaEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSaidaEstoque.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSaidaEstoque.Location = new System.Drawing.Point(3, 135);
            this.btnSaidaEstoque.Name = "btnSaidaEstoque";
            this.btnSaidaEstoque.Size = new System.Drawing.Size(513, 38);
            this.btnSaidaEstoque.TabIndex = 107;
            this.btnSaidaEstoque.Text = "Saída no estoque";
            this.btnSaidaEstoque.UseVisualStyleBackColor = false;
            // 
            // btnCliente
            // 
            this.btnCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(48)))), ((int)(((byte)(66)))));
            this.btnCliente.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnCliente.ForeColor = System.Drawing.SystemColors.Control;
            this.btnCliente.Location = new System.Drawing.Point(3, 47);
            this.btnCliente.Name = "btnCliente";
            this.btnCliente.Size = new System.Drawing.Size(513, 38);
            this.btnCliente.TabIndex = 105;
            this.btnCliente.Text = "Cadastrar Cliente";
            this.btnCliente.UseVisualStyleBackColor = false;
            // 
            // btnEntradaEstoque
            // 
            this.btnEntradaEstoque.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(48)))), ((int)(((byte)(66)))));
            this.btnEntradaEstoque.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEntradaEstoque.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEntradaEstoque.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEntradaEstoque.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnEntradaEstoque.ForeColor = System.Drawing.SystemColors.Control;
            this.btnEntradaEstoque.Location = new System.Drawing.Point(3, 91);
            this.btnEntradaEstoque.Name = "btnEntradaEstoque";
            this.btnEntradaEstoque.Size = new System.Drawing.Size(513, 38);
            this.btnEntradaEstoque.TabIndex = 106;
            this.btnEntradaEstoque.Text = "Entrada no estoque";
            this.btnEntradaEstoque.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(519, 36);
            this.label1.TabIndex = 56;
            this.label1.Text = "Atalhos";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(48)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panelExibicao);
            this.Controls.Add(this.panel1);
            this.Name = "ucHome";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.Size = new System.Drawing.Size(1152, 903);
            this.panel1.ResumeLayout(false);
            this.panelExibicao.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picturebox)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOS)).EndInit();
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panelBotoes.ResumeLayout(false);
            this.tlpBotoes.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tlpBotoes;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button btnCliente;
        private System.Windows.Forms.Button btnEntradaEstoque;
        private System.Windows.Forms.Button btnSaidaEstoque;

        private System.Windows.Forms.Button btnOs;

        private System.Windows.Forms.Panel panelBotoes;

        private System.Windows.Forms.Panel panel9;

        private System.Windows.Forms.Panel panel8;

        private System.Windows.Forms.Label lblEstoque;

        private System.Windows.Forms.DataGridView dataGridView1;

        private System.Windows.Forms.Label lblOsRecentes;

        private System.Windows.Forms.Panel panel7;

        private System.Windows.Forms.DataGridView dgvOS;

        private System.Windows.Forms.Panel panel6;

        private System.Windows.Forms.Panel panelExibicao;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblFaturamento;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblContaPagar;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblMinimo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox picturebox;
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