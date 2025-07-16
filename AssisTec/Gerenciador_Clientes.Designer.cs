namespace AssisTec
{
    partial class Gerenciador_Clientes
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gerenciador_Clientes));
            this.lblNome = new System.Windows.Forms.Label();
            this.lblcpf = new System.Windows.Forms.Label();
            this.lbltel = new System.Windows.Forms.Label();
            this.lbldatanasc = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.mtbCPF = new System.Windows.Forms.MaskedTextBox();
            this.txtRua = new System.Windows.Forms.TextBox();
            this.lblendereco = new System.Windows.Forms.Label();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.lblnum = new System.Windows.Forms.Label();
            this.mtbTel = new System.Windows.Forms.MaskedTextBox();
            this.mtbNasc = new System.Windows.Forms.MaskedTextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblrua = new System.Windows.Forms.Label();
            this.lblbairro = new System.Windows.Forms.Label();
            this.txtBairro = new System.Windows.Forms.TextBox();
            this.lblcidade = new System.Windows.Forms.Label();
            this.txtCidade = new System.Windows.Forms.TextBox();
            this.lblestado = new System.Windows.Forms.Label();
            this.txtEstado = new System.Windows.Forms.TextBox();
            this.mtbCep = new System.Windows.Forms.MaskedTextBox();
            this.lblcep = new System.Windows.Forms.Label();
            this.lblcomplemento = new System.Windows.Forms.Label();
            this.txtComp = new System.Windows.Forms.TextBox();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.lblbusca = new System.Windows.Forms.Label();
            this.dgvClientes = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAtualizar = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNome.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblNome.Location = new System.Drawing.Point(3, 43);
            this.lblNome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(45, 18);
            this.lblNome.TabIndex = 1;
            this.lblNome.Text = "Nome:";
            // 
            // lblcpf
            // 
            this.lblcpf.AutoSize = true;
            this.lblcpf.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lblcpf.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblcpf.Location = new System.Drawing.Point(3, 67);
            this.lblcpf.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblcpf.Name = "lblcpf";
            this.lblcpf.Size = new System.Drawing.Size(35, 18);
            this.lblcpf.TabIndex = 2;
            this.lblcpf.Text = "CPF:";
            // 
            // lbltel
            // 
            this.lbltel.AutoSize = true;
            this.lbltel.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lbltel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbltel.Location = new System.Drawing.Point(3, 93);
            this.lbltel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbltel.Name = "lbltel";
            this.lbltel.Size = new System.Drawing.Size(67, 18);
            this.lbltel.TabIndex = 4;
            this.lbltel.Text = "Telefone:";
            // 
            // lbldatanasc
            // 
            this.lbldatanasc.AutoSize = true;
            this.lbldatanasc.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lbldatanasc.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbldatanasc.Location = new System.Drawing.Point(2, 125);
            this.lbldatanasc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbldatanasc.Name = "lbldatanasc";
            this.lbldatanasc.Size = new System.Drawing.Size(132, 18);
            this.lbldatanasc.TabIndex = 5;
            this.lbldatanasc.Text = "Data de Nascimento:";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Location = new System.Drawing.Point(72, 39);
            this.txtName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(377, 22);
            this.txtName.TabIndex = 6;
            // 
            // mtbCPF
            // 
            this.mtbCPF.BackColor = System.Drawing.Color.White;
            this.mtbCPF.Location = new System.Drawing.Point(72, 66);
            this.mtbCPF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mtbCPF.Mask = "000.000.000-00";
            this.mtbCPF.Name = "mtbCPF";
            this.mtbCPF.Size = new System.Drawing.Size(131, 22);
            this.mtbCPF.TabIndex = 7;
            this.mtbCPF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRua
            // 
            this.txtRua.BackColor = System.Drawing.Color.White;
            this.txtRua.Location = new System.Drawing.Point(67, 236);
            this.txtRua.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtRua.Name = "txtRua";
            this.txtRua.ReadOnly = true;
            this.txtRua.Size = new System.Drawing.Size(199, 22);
            this.txtRua.TabIndex = 9;
            // 
            // lblendereco
            // 
            this.lblendereco.AutoSize = true;
            this.lblendereco.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblendereco.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblendereco.Location = new System.Drawing.Point(215, 181);
            this.lblendereco.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblendereco.Name = "lblendereco";
            this.lblendereco.Size = new System.Drawing.Size(86, 23);
            this.lblendereco.TabIndex = 8;
            this.lblendereco.Text = "Endereço:";
            // 
            // txtNumber
            // 
            this.txtNumber.BackColor = System.Drawing.Color.White;
            this.txtNumber.Location = new System.Drawing.Point(319, 238);
            this.txtNumber.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(60, 22);
            this.txtNumber.TabIndex = 11;
            // 
            // lblnum
            // 
            this.lblnum.AutoSize = true;
            this.lblnum.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lblnum.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblnum.Location = new System.Drawing.Point(292, 237);
            this.lblnum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblnum.Name = "lblnum";
            this.lblnum.Size = new System.Drawing.Size(27, 18);
            this.lblnum.TabIndex = 10;
            this.lblnum.Text = "N°:";
            // 
            // mtbTel
            // 
            this.mtbTel.BackColor = System.Drawing.Color.White;
            this.mtbTel.Location = new System.Drawing.Point(72, 92);
            this.mtbTel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mtbTel.Mask = "(00) 00000-0000";
            this.mtbTel.Name = "mtbTel";
            this.mtbTel.Size = new System.Drawing.Size(131, 22);
            this.mtbTel.TabIndex = 12;
            this.mtbTel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mtbNasc
            // 
            this.mtbNasc.BackColor = System.Drawing.Color.White;
            this.mtbNasc.Location = new System.Drawing.Point(142, 121);
            this.mtbNasc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mtbNasc.Mask = "00/00/0000";
            this.mtbNasc.Name = "mtbNasc";
            this.mtbNasc.Size = new System.Drawing.Size(116, 22);
            this.mtbNasc.TabIndex = 13;
            this.mtbNasc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mtbNasc.ValidatingType = typeof(System.DateTime);
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnNew.ForeColor = System.Drawing.SystemColors.Control;
            this.btnNew.Location = new System.Drawing.Point(85, 375);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 14;
            this.btnNew.Text = "Novo";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSave.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSave.Location = new System.Drawing.Point(247, 375);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Salvar";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnDelete.ForeColor = System.Drawing.SystemColors.Control;
            this.btnDelete.Location = new System.Drawing.Point(328, 375);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Excluir";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.Control;
            this.btnCancel.Location = new System.Drawing.Point(409, 375);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(190, 13);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 23);
            this.label4.TabIndex = 18;
            this.label4.Text = "Gerenciador de Clientes";
            // 
            // lblrua
            // 
            this.lblrua.AutoSize = true;
            this.lblrua.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lblrua.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblrua.Location = new System.Drawing.Point(6, 241);
            this.lblrua.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblrua.Name = "lblrua";
            this.lblrua.Size = new System.Drawing.Size(34, 18);
            this.lblrua.TabIndex = 19;
            this.lblrua.Text = "Rua:";
            // 
            // lblbairro
            // 
            this.lblbairro.AutoSize = true;
            this.lblbairro.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lblbairro.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblbairro.Location = new System.Drawing.Point(292, 278);
            this.lblbairro.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblbairro.Name = "lblbairro";
            this.lblbairro.Size = new System.Drawing.Size(50, 18);
            this.lblbairro.TabIndex = 21;
            this.lblbairro.Text = "Bairro:";
            // 
            // txtBairro
            // 
            this.txtBairro.BackColor = System.Drawing.Color.White;
            this.txtBairro.Location = new System.Drawing.Point(350, 276);
            this.txtBairro.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtBairro.Name = "txtBairro";
            this.txtBairro.ReadOnly = true;
            this.txtBairro.Size = new System.Drawing.Size(165, 22);
            this.txtBairro.TabIndex = 20;
            // 
            // lblcidade
            // 
            this.lblcidade.AutoSize = true;
            this.lblcidade.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lblcidade.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblcidade.Location = new System.Drawing.Point(5, 278);
            this.lblcidade.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblcidade.Name = "lblcidade";
            this.lblcidade.Size = new System.Drawing.Size(54, 18);
            this.lblcidade.TabIndex = 23;
            this.lblcidade.Text = "Cidade:";
            // 
            // txtCidade
            // 
            this.txtCidade.BackColor = System.Drawing.Color.White;
            this.txtCidade.Location = new System.Drawing.Point(67, 277);
            this.txtCidade.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCidade.Name = "txtCidade";
            this.txtCidade.ReadOnly = true;
            this.txtCidade.Size = new System.Drawing.Size(199, 22);
            this.txtCidade.TabIndex = 22;
            // 
            // lblestado
            // 
            this.lblestado.AutoSize = true;
            this.lblestado.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lblestado.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblestado.Location = new System.Drawing.Point(6, 320);
            this.lblestado.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblestado.Name = "lblestado";
            this.lblestado.Size = new System.Drawing.Size(53, 18);
            this.lblestado.TabIndex = 25;
            this.lblestado.Text = "Estado:";
            // 
            // txtEstado
            // 
            this.txtEstado.BackColor = System.Drawing.Color.White;
            this.txtEstado.Location = new System.Drawing.Point(67, 319);
            this.txtEstado.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtEstado.Name = "txtEstado";
            this.txtEstado.ReadOnly = true;
            this.txtEstado.Size = new System.Drawing.Size(199, 22);
            this.txtEstado.TabIndex = 24;
            // 
            // mtbCep
            // 
            this.mtbCep.BackColor = System.Drawing.Color.White;
            this.mtbCep.Font = new System.Drawing.Font("Comic Sans MS", 8.25F);
            this.mtbCep.Location = new System.Drawing.Point(193, 207);
            this.mtbCep.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mtbCep.Mask = "00000-000";
            this.mtbCep.Name = "mtbCep";
            this.mtbCep.Size = new System.Drawing.Size(131, 23);
            this.mtbCep.TabIndex = 27;
            this.mtbCep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblcep
            // 
            this.lblcep.AutoSize = true;
            this.lblcep.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lblcep.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblcep.Location = new System.Drawing.Point(156, 207);
            this.lblcep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblcep.Name = "lblcep";
            this.lblcep.Size = new System.Drawing.Size(35, 18);
            this.lblcep.TabIndex = 26;
            this.lblcep.Text = "CEP:";
            // 
            // lblcomplemento
            // 
            this.lblcomplemento.AutoSize = true;
            this.lblcomplemento.Font = new System.Drawing.Font("Comic Sans MS", 9.75F);
            this.lblcomplemento.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblcomplemento.Location = new System.Drawing.Point(290, 322);
            this.lblcomplemento.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblcomplemento.Name = "lblcomplemento";
            this.lblcomplemento.Size = new System.Drawing.Size(89, 18);
            this.lblcomplemento.TabIndex = 29;
            this.lblcomplemento.Text = "Complemento:";
            // 
            // txtComp
            // 
            this.txtComp.BackColor = System.Drawing.Color.White;
            this.txtComp.Location = new System.Drawing.Point(382, 317);
            this.txtComp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtComp.Name = "txtComp";
            this.txtComp.Size = new System.Drawing.Size(165, 22);
            this.txtComp.TabIndex = 28;
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditar.Enabled = false;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnEditar.ForeColor = System.Drawing.SystemColors.Control;
            this.btnEditar.Location = new System.Drawing.Point(166, 375);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(75, 23);
            this.btnEditar.TabIndex = 30;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.Enabled = false;
            this.btnBuscar.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.btnBuscar.Location = new System.Drawing.Point(331, 207);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 31;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.mtbCep_Leave);
            // 
            // txtBusca
            // 
            this.txtBusca.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtBusca.BackColor = System.Drawing.Color.White;
            this.txtBusca.Location = new System.Drawing.Point(75, 12);
            this.txtBusca.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(363, 22);
            this.txtBusca.TabIndex = 33;
            this.txtBusca.TextChanged += new System.EventHandler(this.txtBusca_TextChanged);
            // 
            // lblbusca
            // 
            this.lblbusca.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblbusca.AutoSize = true;
            this.lblbusca.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblbusca.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblbusca.Location = new System.Drawing.Point(22, 13);
            this.lblbusca.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblbusca.Name = "lblbusca";
            this.lblbusca.Size = new System.Drawing.Size(47, 18);
            this.lblbusca.TabIndex = 32;
            this.lblbusca.Text = "Busca:";
            // 
            // dgvClientes
            // 
            this.dgvClientes.AllowUserToAddRows = false;
            this.dgvClientes.AllowUserToDeleteRows = false;
            this.dgvClientes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvClientes.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvClientes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientes.Location = new System.Drawing.Point(22, 45);
            this.dgvClientes.Name = "dgvClientes";
            this.dgvClientes.ReadOnly = true;
            this.dgvClientes.Size = new System.Drawing.Size(478, 354);
            this.dgvClientes.TabIndex = 34;
            this.dgvClientes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClientes_CellClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnBuscar);
            this.panel1.Controls.Add(this.btnEditar);
            this.panel1.Controls.Add(this.lblcomplemento);
            this.panel1.Controls.Add(this.mtbCep);
            this.panel1.Controls.Add(this.lblcep);
            this.panel1.Controls.Add(this.txtComp);
            this.panel1.Controls.Add(this.lblestado);
            this.panel1.Controls.Add(this.txtEstado);
            this.panel1.Controls.Add(this.lblcidade);
            this.panel1.Controls.Add(this.txtCidade);
            this.panel1.Controls.Add(this.lblbairro);
            this.panel1.Controls.Add(this.txtBairro);
            this.panel1.Controls.Add(this.lblrua);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Controls.Add(this.mtbNasc);
            this.panel1.Controls.Add(this.mtbTel);
            this.panel1.Controls.Add(this.txtNumber);
            this.panel1.Controls.Add(this.lblnum);
            this.panel1.Controls.Add(this.txtRua);
            this.panel1.Controls.Add(this.lblendereco);
            this.panel1.Controls.Add(this.mtbCPF);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.lbldatanasc);
            this.panel1.Controls.Add(this.lbltel);
            this.panel1.Controls.Add(this.lblcpf);
            this.panel1.Controls.Add(this.lblNome);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 411);
            this.panel1.TabIndex = 36;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(44)))), ((int)(((byte)(64)))));
            this.panel2.Controls.Add(this.btnAtualizar);
            this.panel2.Controls.Add(this.lblbusca);
            this.panel2.Controls.Add(this.txtBusca);
            this.panel2.Controls.Add(this.dgvClientes);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(564, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(518, 411);
            this.panel2.TabIndex = 37;
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAtualizar.BackgroundImage")));
            this.btnAtualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAtualizar.Location = new System.Drawing.Point(458, 3);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(42, 36);
            this.btnAtualizar.TabIndex = 35;
            this.btnAtualizar.TabStop = false;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // Gerenciador_Clientes
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(55)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(1076, 411);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(15, 15);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Gerenciador_Clientes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAtualizar)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.PictureBox btnAtualizar;

        private System.Windows.Forms.Panel panel2;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.DataGridView dgvClientes;

        private System.Windows.Forms.TextBox txtBusca;
        private System.Windows.Forms.Label lblbusca;

        private System.Windows.Forms.Button btnBuscar;

        #endregion

        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Label lblcpf;
        private System.Windows.Forms.Label lbltel;
        private System.Windows.Forms.Label lbldatanasc;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.MaskedTextBox mtbCPF;
        private System.Windows.Forms.TextBox txtRua;
        private System.Windows.Forms.Label lblendereco;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Label lblnum;
        private System.Windows.Forms.MaskedTextBox mtbTel;
        private System.Windows.Forms.MaskedTextBox mtbNasc;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblrua;
        private System.Windows.Forms.Label lblbairro;
        private System.Windows.Forms.TextBox txtBairro;
        private System.Windows.Forms.Label lblcidade;
        private System.Windows.Forms.TextBox txtCidade;
        private System.Windows.Forms.Label lblestado;
        private System.Windows.Forms.TextBox txtEstado;
        private System.Windows.Forms.MaskedTextBox mtbCep;
        private System.Windows.Forms.Label lblcep;
        private System.Windows.Forms.Label lblcomplemento;
        private System.Windows.Forms.TextBox txtComp;
        private System.Windows.Forms.Button btnEditar;
    }
}

