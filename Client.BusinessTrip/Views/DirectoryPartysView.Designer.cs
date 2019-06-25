namespace Client.BusinessTrip.Views
{
    partial class DirectoryPartysView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryPartysView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ss = new System.Windows.Forms.StatusStrip();
            this.btnCancel = new System.Windows.Forms.ToolStripSplitButton();
            this.btnOK = new System.Windows.Forms.ToolStripSplitButton();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabCard = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.grParti = new System.Windows.Forms.GroupBox();
            this.dataGridViewPersons = new System.Windows.Forms.DataGridView();
            this.partyPartyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.personPersonIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.personDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.personnelNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.departmentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isResponsibleDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bindingSourcePersons = new System.Windows.Forms.BindingSource(this.components);
            this.ts2 = new System.Windows.Forms.ToolStrip();
            this.labelCountPersons = new System.Windows.Forms.ToolStripLabel();
            this.btnAddPerson = new System.Windows.Forms.ToolStripButton();
            this.btnDelPerson = new System.Windows.Forms.ToolStripButton();
            this.lblName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.tabList = new System.Windows.Forms.TabPage();
            this.dataGridViewPartys = new System.Windows.Forms.DataGridView();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.ts = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnAddCopy = new System.Windows.Forms.ToolStripButton();
            this.btnDel = new System.Windows.Forms.ToolStripButton();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.tss3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.textBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            this.comboBoxSearch = new System.Windows.Forms.ToolStripComboBox();
            this.bindingSourcePartys = new System.Windows.Forms.BindingSource(this.components);
            this.ss.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.TabCard.SuspendLayout();
            this.grParti.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPersons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePersons)).BeginInit();
            this.ts2.SuspendLayout();
            this.tabList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPartys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.ts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePartys)).BeginInit();
            this.SuspendLayout();
            // 
            // ss
            // 
            this.ss.AutoSize = false;
            this.ss.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCancel,
            this.btnOK,
            this.lblCount});
            this.ss.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ss.Location = new System.Drawing.Point(0, 315);
            this.ss.Name = "ss";
            this.ss.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.ss.Size = new System.Drawing.Size(629, 32);
            this.ss.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnCancel.AutoSize = false;
            this.btnCancel.DropDownButtonWidth = 0;
            this.btnCancel.Image = global::Client.BusinessTrip.Properties.Resources.ImageCancal;
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.ToolTipText = "Отмена и выход";
            this.btnCancel.ButtonClick += new System.EventHandler(this.btnCancel_ButtonClick);
            // 
            // btnOK
            // 
            this.btnOK.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnOK.AutoSize = false;
            this.btnOK.DropDownButtonWidth = 0;
            this.btnOK.Image = global::Client.BusinessTrip.Properties.Resources.ImageOk;
            this.btnOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.Text = "OK";
            this.btnOK.ButtonClick += new System.EventHandler(this.btnOK_ButtonClick);
            // 
            // lblCount
            // 
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(19, 27);
            this.lblCount.Text = "00";
            // 
            // TabControl1
            // 
            this.TabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl1.Controls.Add(this.TabCard);
            this.TabControl1.Controls.Add(this.tabList);
            this.TabControl1.Location = new System.Drawing.Point(5, 32);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(620, 282);
            this.TabControl1.TabIndex = 1;
            this.TabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabControl1_Selecting);
            // 
            // TabCard
            // 
            this.TabCard.Controls.Add(this.label2);
            this.TabCard.Controls.Add(this.grParti);
            this.TabCard.Controls.Add(this.lblName);
            this.TabCard.Controls.Add(this.textBoxName);
            this.TabCard.ImageIndex = 43;
            this.TabCard.Location = new System.Drawing.Point(4, 23);
            this.TabCard.Name = "TabCard";
            this.TabCard.Padding = new System.Windows.Forms.Padding(3);
            this.TabCard.Size = new System.Drawing.Size(612, 255);
            this.TabCard.TabIndex = 0;
            this.TabCard.Text = "Карточка";
            this.TabCard.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(591, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 14);
            this.label2.TabIndex = 16;
            this.label2.Text = "*";
            // 
            // grParti
            // 
            this.grParti.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grParti.Controls.Add(this.dataGridViewPersons);
            this.grParti.Controls.Add(this.ts2);
            this.grParti.Location = new System.Drawing.Point(7, 34);
            this.grParti.Name = "grParti";
            this.grParti.Size = new System.Drawing.Size(597, 215);
            this.grParti.TabIndex = 2;
            this.grParti.TabStop = false;
            this.grParti.Text = "Состав партии:  ";
            // 
            // dataGridViewPersons
            // 
            this.dataGridViewPersons.AllowUserToAddRows = false;
            this.dataGridViewPersons.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridViewPersons.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewPersons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPersons.AutoGenerateColumns = false;
            this.dataGridViewPersons.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPersons.ColumnHeadersHeight = 35;
            this.dataGridViewPersons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.partyPartyIdDataGridViewTextBoxColumn,
            this.personPersonIdDataGridViewTextBoxColumn,
            this.partyDataGridViewTextBoxColumn,
            this.personDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.positionDataGridViewTextBoxColumn,
            this.personnelNumberDataGridViewTextBoxColumn,
            this.departmentDataGridViewTextBoxColumn,
            this.isResponsibleDataGridViewCheckBoxColumn});
            this.dataGridViewPersons.DataSource = this.bindingSourcePersons;
            this.dataGridViewPersons.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewPersons.Location = new System.Drawing.Point(7, 51);
            this.dataGridViewPersons.Name = "dataGridViewPersons";
            this.dataGridViewPersons.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewPersons.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(196)))), ((int)(((byte)(128)))));
            this.dataGridViewPersons.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewPersons.RowTemplate.Height = 30;
            this.dataGridViewPersons.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewPersons.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewPersons.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPersons.ShowEditingIcon = false;
            this.dataGridViewPersons.Size = new System.Drawing.Size(583, 158);
            this.dataGridViewPersons.TabIndex = 0;
            this.dataGridViewPersons.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPersons_CellContentClick);
            this.dataGridViewPersons.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPersons_CellMouseDown);
            this.dataGridViewPersons.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPersons_CellValueChanged);
            this.dataGridViewPersons.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewPersons_RowPostPaint);
            // 
            // partyPartyIdDataGridViewTextBoxColumn
            // 
            this.partyPartyIdDataGridViewTextBoxColumn.DataPropertyName = "Party_PartyId";
            this.partyPartyIdDataGridViewTextBoxColumn.HeaderText = "Party_PartyId";
            this.partyPartyIdDataGridViewTextBoxColumn.Name = "partyPartyIdDataGridViewTextBoxColumn";
            this.partyPartyIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // personPersonIdDataGridViewTextBoxColumn
            // 
            this.personPersonIdDataGridViewTextBoxColumn.DataPropertyName = "Person_PersonId";
            this.personPersonIdDataGridViewTextBoxColumn.HeaderText = "Person_PersonId";
            this.personPersonIdDataGridViewTextBoxColumn.Name = "personPersonIdDataGridViewTextBoxColumn";
            this.personPersonIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // partyDataGridViewTextBoxColumn
            // 
            this.partyDataGridViewTextBoxColumn.DataPropertyName = "Party";
            this.partyDataGridViewTextBoxColumn.HeaderText = "Party";
            this.partyDataGridViewTextBoxColumn.Name = "partyDataGridViewTextBoxColumn";
            this.partyDataGridViewTextBoxColumn.Visible = false;
            // 
            // personDataGridViewTextBoxColumn
            // 
            this.personDataGridViewTextBoxColumn.DataPropertyName = "Person";
            this.personDataGridViewTextBoxColumn.HeaderText = "Person";
            this.personDataGridViewTextBoxColumn.Name = "personDataGridViewTextBoxColumn";
            this.personDataGridViewTextBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.FillWeight = 141F;
            this.nameDataGridViewTextBoxColumn.HeaderText = "ФИО";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // positionDataGridViewTextBoxColumn
            // 
            this.positionDataGridViewTextBoxColumn.DataPropertyName = "Position";
            this.positionDataGridViewTextBoxColumn.FillWeight = 114F;
            this.positionDataGridViewTextBoxColumn.HeaderText = "Должность";
            this.positionDataGridViewTextBoxColumn.Name = "positionDataGridViewTextBoxColumn";
            this.positionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // personnelNumberDataGridViewTextBoxColumn
            // 
            this.personnelNumberDataGridViewTextBoxColumn.DataPropertyName = "PersonnelNumber";
            this.personnelNumberDataGridViewTextBoxColumn.FillWeight = 71F;
            this.personnelNumberDataGridViewTextBoxColumn.HeaderText = "Таб. №";
            this.personnelNumberDataGridViewTextBoxColumn.Name = "personnelNumberDataGridViewTextBoxColumn";
            this.personnelNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // departmentDataGridViewTextBoxColumn
            // 
            this.departmentDataGridViewTextBoxColumn.DataPropertyName = "Department";
            this.departmentDataGridViewTextBoxColumn.FillWeight = 71F;
            this.departmentDataGridViewTextBoxColumn.HeaderText = "Отдел";
            this.departmentDataGridViewTextBoxColumn.Name = "departmentDataGridViewTextBoxColumn";
            this.departmentDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isResponsibleDataGridViewCheckBoxColumn
            // 
            this.isResponsibleDataGridViewCheckBoxColumn.DataPropertyName = "IsResponsible";
            this.isResponsibleDataGridViewCheckBoxColumn.HeaderText = "Ответственный";
            this.isResponsibleDataGridViewCheckBoxColumn.Name = "isResponsibleDataGridViewCheckBoxColumn";
            this.isResponsibleDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isResponsibleDataGridViewCheckBoxColumn.TrueValue = "true";
            // 
            // bindingSourcePersons
            // 
            this.bindingSourcePersons.DataSource = typeof(Core.BusinessTrip.Domain.PartyPerson);
            // 
            // ts2
            // 
            this.ts2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ts2.Dock = System.Windows.Forms.DockStyle.None;
            this.ts2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelCountPersons,
            this.btnAddPerson,
            this.btnDelPerson});
            this.ts2.Location = new System.Drawing.Point(482, 18);
            this.ts2.Name = "ts2";
            this.ts2.Size = new System.Drawing.Size(108, 25);
            this.ts2.TabIndex = 3;
            this.ts2.Text = "ToolStrip1";
            // 
            // labelCountPersons
            // 
            this.labelCountPersons.AutoSize = false;
            this.labelCountPersons.Name = "labelCountPersons";
            this.labelCountPersons.Size = new System.Drawing.Size(50, 22);
            this.labelCountPersons.Text = "0";
            // 
            // btnAddPerson
            // 
            this.btnAddPerson.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddPerson.Image = ((System.Drawing.Image)(resources.GetObject("btnAddPerson.Image")));
            this.btnAddPerson.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(23, 22);
            this.btnAddPerson.ToolTipText = "Добавить в партию";
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);
            // 
            // btnDelPerson
            // 
            this.btnDelPerson.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelPerson.Image = ((System.Drawing.Image)(resources.GetObject("btnDelPerson.Image")));
            this.btnDelPerson.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelPerson.Name = "btnDelPerson";
            this.btnDelPerson.Size = new System.Drawing.Size(23, 22);
            this.btnDelPerson.ToolTipText = "Удалить из партии";
            this.btnDelPerson.Click += new System.EventHandler(this.btnDelPerson_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(11, 9);
            this.lblName.Margin = new System.Windows.Forms.Padding(3, 0, 7, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(94, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Наименование:";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(115, 6);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxName.MaxLength = 150;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(476, 22);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // tabList
            // 
            this.tabList.Controls.Add(this.dataGridViewPartys);
            this.tabList.ImageKey = "list.png";
            this.tabList.Location = new System.Drawing.Point(4, 23);
            this.tabList.Name = "tabList";
            this.tabList.Padding = new System.Windows.Forms.Padding(3);
            this.tabList.Size = new System.Drawing.Size(612, 255);
            this.tabList.TabIndex = 1;
            this.tabList.Text = "Список";
            this.tabList.UseVisualStyleBackColor = true;
            // 
            // dataGridViewPartys
            // 
            this.dataGridViewPartys.AllowUserToAddRows = false;
            this.dataGridViewPartys.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridViewPartys.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewPartys.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPartys.ColumnHeadersHeight = 35;
            this.dataGridViewPartys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPartys.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewPartys.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewPartys.Name = "dataGridViewPartys";
            this.dataGridViewPartys.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewPartys.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(196)))), ((int)(((byte)(128)))));
            this.dataGridViewPartys.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewPartys.RowTemplate.Height = 30;
            this.dataGridViewPartys.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewPartys.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewPartys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPartys.ShowEditingIcon = false;
            this.dataGridViewPartys.Size = new System.Drawing.Size(606, 249);
            this.dataGridViewPartys.TabIndex = 0;
            this.dataGridViewPartys.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPartys_CellMouseDown);
            this.dataGridViewPartys.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPartys_ColumnHeaderMouseClick);
            this.dataGridViewPartys.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewPartys_RowPostPaint);
            this.dataGridViewPartys.SelectionChanged += new System.EventHandler(this.dataGridViewPartys_SelectionChanged);
            this.dataGridViewPartys.DoubleClick += new System.EventHandler(this.dataGridViewPartys_DoubleClick);
            this.dataGridViewPartys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewPartys_KeyDown);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ts
            // 
            this.ts.AutoSize = false;
            this.ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnAddCopy,
            this.btnDel,
            this.tss1,
            this.btnSave,
            this.btnRefresh,
            this.toolStripSeparator1,
            this.btnPrev,
            this.btnNext,
            this.tss3,
            this.btnClear,
            this.textBoxSearch,
            this.comboBoxSearch});
            this.ts.Location = new System.Drawing.Point(0, 0);
            this.ts.Name = "ts";
            this.ts.Size = new System.Drawing.Size(629, 29);
            this.ts.TabIndex = 0;
            this.ts.Text = "ToolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = global::Client.BusinessTrip.Properties.Resources.ImageAdd;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 26);
            this.btnAdd.ToolTipText = "Добавить новую запись";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddCopy
            // 
            this.btnAddCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCopy.Image = global::Client.BusinessTrip.Properties.Resources.ImageAddCopy;
            this.btnAddCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCopy.Name = "btnAddCopy";
            this.btnAddCopy.Size = new System.Drawing.Size(23, 26);
            this.btnAddCopy.Text = "Добавить копию";
            this.btnAddCopy.Click += new System.EventHandler(this.btnAddCopy_Click);
            // 
            // btnDel
            // 
            this.btnDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDel.Image = global::Client.BusinessTrip.Properties.Resources.ImageDelete;
            this.btnDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(23, 26);
            this.btnDel.ToolTipText = "Удалить выбранные записи";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // tss1
            // 
            this.tss1.Name = "tss1";
            this.tss1.Size = new System.Drawing.Size(6, 29);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = global::Client.BusinessTrip.Properties.Resources.ImageSaveUpdate;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 26);
            this.btnSave.Text = "ToolStripButton3";
            this.btnSave.ToolTipText = "Сохранить изменения";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = global::Client.BusinessTrip.Properties.Resources.ImageRefresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 26);
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // btnPrev
            // 
            this.btnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrev.Image = global::Client.BusinessTrip.Properties.Resources.ImagePrevious;
            this.btnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 26);
            this.btnPrev.Text = "ToolStripButton5";
            this.btnPrev.ToolTipText = "Предыдущая запись";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNext.Image = global::Client.BusinessTrip.Properties.Resources.ImageNext;
            this.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 26);
            this.btnNext.Text = "ToolStripButton6";
            this.btnNext.ToolTipText = "Следующая запись";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // tss3
            // 
            this.tss3.Name = "tss3";
            this.tss3.Size = new System.Drawing.Size(6, 29);
            // 
            // btnClear
            // 
            this.btnClear.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnClear.AutoToolTip = false;
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClear.Image = global::Client.BusinessTrip.Properties.Resources.ImageClear;
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Padding = new System.Windows.Forms.Padding(0, 0, 13, 0);
            this.btnClear.Size = new System.Drawing.Size(33, 26);
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.textBoxSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(202, 29);
            this.textBoxSearch.ToolTipText = "Быстрый поиск";
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // comboBoxSearch
            // 
            this.comboBoxSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.comboBoxSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearch.Name = "comboBoxSearch";
            this.comboBoxSearch.Size = new System.Drawing.Size(189, 29);
            this.comboBoxSearch.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearch_SelectedIndexChanged);
            // 
            // DirectoryPartysView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 347);
            this.Controls.Add(this.ts);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.ss);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(645, 360);
            this.Name = "DirectoryPartysView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник партий";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DirectoryPartysView_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DirectoryPartysView_FormClosed);
            this.Shown += new System.EventHandler(this.DirectoryPartysView_Shown);
            this.SizeChanged += new System.EventHandler(this.DirectoryPartysView_SizeChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DirectoryPartysView_KeyUp);
            this.ss.ResumeLayout(false);
            this.ss.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.TabCard.ResumeLayout(false);
            this.TabCard.PerformLayout();
            this.grParti.ResumeLayout(false);
            this.grParti.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPersons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePersons)).EndInit();
            this.ts2.ResumeLayout(false);
            this.ts2.PerformLayout();
            this.tabList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPartys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ts.ResumeLayout(false);
            this.ts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePartys)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.StatusStrip ss;
        internal System.Windows.Forms.ToolStripSplitButton btnCancel;
        internal System.Windows.Forms.ToolStripSplitButton btnOK;
        internal System.Windows.Forms.ToolStripStatusLabel lblCount;
        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.TabPage TabCard;
        internal System.Windows.Forms.ToolStrip ts2;
        internal System.Windows.Forms.ToolStripLabel labelCountPersons;
        internal System.Windows.Forms.ToolStripButton btnAddPerson;
        internal System.Windows.Forms.ToolStripButton btnDelPerson;
        internal System.Windows.Forms.DataGridView dataGridViewPersons;
        private System.Windows.Forms.BindingSource bindingSourcePersons;
        internal System.Windows.Forms.Label lblName;
        internal System.Windows.Forms.TextBox textBoxName;
        internal System.Windows.Forms.TabPage tabList;
        internal System.Windows.Forms.DataGridView dataGridViewPartys;
        private System.Windows.Forms.BindingSource bindingSourcePartys;
        private System.Windows.Forms.ErrorProvider errorProvider;
        internal System.Windows.Forms.GroupBox grParti;
        internal System.Windows.Forms.ToolStrip ts;
        internal System.Windows.Forms.ToolStripButton btnAdd;
        internal System.Windows.Forms.ToolStripButton btnAddCopy;
        internal System.Windows.Forms.ToolStripButton btnDel;
        internal System.Windows.Forms.ToolStripSeparator tss1;
        internal System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        internal System.Windows.Forms.ToolStripButton btnPrev;
        internal System.Windows.Forms.ToolStripButton btnNext;
        internal System.Windows.Forms.ToolStripSeparator tss3;
        internal System.Windows.Forms.ToolStripButton btnClear;
        internal System.Windows.Forms.ToolStripTextBox textBoxSearch;
        private System.Windows.Forms.ToolStripComboBox comboBoxSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn partyPartyIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn personPersonIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn partyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn personDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn positionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn personnelNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn departmentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isResponsibleDataGridViewCheckBoxColumn;
    }
}