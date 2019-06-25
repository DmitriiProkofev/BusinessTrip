namespace Client.BusinessTrip.Views
{
    partial class DirectoryTransportsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryTransportsView));
            this.ss = new System.Windows.Forms.StatusStrip();
            this.btnCancel = new System.Windows.Forms.ToolStripSplitButton();
            this.btnOK = new System.Windows.Forms.ToolStripSplitButton();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TabCard = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMark = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabList = new System.Windows.Forms.TabPage();
            this.dataGridViewTransports = new System.Windows.Forms.DataGridView();
            this.ts = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnAddCopy = new System.Windows.Forms.ToolStripButton();
            this.btnDel = new System.Windows.Forms.ToolStripButton();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.tss2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.tss3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.textBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            this.comboBoxSearch = new System.Windows.Forms.ToolStripComboBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.ss.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.TabCard.SuspendLayout();
            this.tabList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransports)).BeginInit();
            this.ts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
            this.ss.Location = new System.Drawing.Point(0, 238);
            this.ss.Name = "ss";
            this.ss.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.ss.Size = new System.Drawing.Size(629, 34);
            this.ss.TabIndex = 8;
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
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_ButtonClick);
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
            this.btnOK.Click += new System.EventHandler(this.btnOK_ButtonClick);
            // 
            // lblCount
            // 
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(19, 29);
            this.lblCount.Text = "00";
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.TabCard);
            this.TabControl.Controls.Add(this.tabList);
            this.TabControl.Location = new System.Drawing.Point(6, 36);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(617, 201);
            this.TabControl.TabIndex = 9;
            this.TabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabControl_Selecting);
            // 
            // TabCard
            // 
            this.TabCard.Controls.Add(this.label3);
            this.TabCard.Controls.Add(this.textBoxMark);
            this.TabCard.Controls.Add(this.label1);
            this.TabCard.ImageIndex = 0;
            this.TabCard.Location = new System.Drawing.Point(4, 23);
            this.TabCard.Name = "TabCard";
            this.TabCard.Padding = new System.Windows.Forms.Padding(3);
            this.TabCard.Size = new System.Drawing.Size(609, 174);
            this.TabCard.TabIndex = 0;
            this.TabCard.Text = "Карточка";
            this.TabCard.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(589, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 14);
            this.label3.TabIndex = 18;
            this.label3.Text = "*";
            // 
            // textBoxMark
            // 
            this.textBoxMark.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMark.Location = new System.Drawing.Point(9, 20);
            this.textBoxMark.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxMark.MaxLength = 250;
            this.textBoxMark.Multiline = true;
            this.textBoxMark.Name = "textBoxMark";
            this.textBoxMark.Size = new System.Drawing.Size(580, 148);
            this.textBoxMark.TabIndex = 17;
            this.textBoxMark.TextChanged += new System.EventHandler(this.textBoxMark_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 14);
            this.label1.TabIndex = 16;
            this.label1.Text = "Марка:";
            // 
            // tabList
            // 
            this.tabList.Controls.Add(this.dataGridViewTransports);
            this.tabList.ImageIndex = 1;
            this.tabList.Location = new System.Drawing.Point(4, 23);
            this.tabList.Name = "tabList";
            this.tabList.Padding = new System.Windows.Forms.Padding(3);
            this.tabList.Size = new System.Drawing.Size(609, 174);
            this.tabList.TabIndex = 1;
            this.tabList.Text = "Список";
            this.tabList.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTransports
            // 
            this.dataGridViewTransports.AllowUserToAddRows = false;
            this.dataGridViewTransports.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridViewTransports.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTransports.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTransports.ColumnHeadersHeight = 35;
            this.dataGridViewTransports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTransports.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewTransports.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewTransports.Name = "dataGridViewTransports";
            this.dataGridViewTransports.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewTransports.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(196)))), ((int)(((byte)(128)))));
            this.dataGridViewTransports.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewTransports.RowTemplate.Height = 30;
            this.dataGridViewTransports.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTransports.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewTransports.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTransports.ShowEditingIcon = false;
            this.dataGridViewTransports.Size = new System.Drawing.Size(603, 168);
            this.dataGridViewTransports.TabIndex = 0;
            this.dataGridViewTransports.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTransports_CellMouseDown);
            this.dataGridViewTransports.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTransports_ColumnHeaderMouseClick);
            this.dataGridViewTransports.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewTransports_RowPostPaint);
            this.dataGridViewTransports.SelectionChanged += new System.EventHandler(this.dataGridViewTransports_SelectionChanged);
            this.dataGridViewTransports.DoubleClick += new System.EventHandler(this.dataGridViewTransports_DoubleClick);
            this.dataGridViewTransports.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewTransports_KeyDown);
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
            this.tss2,
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
            this.ts.TabIndex = 10;
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
            // tss2
            // 
            this.tss2.Name = "tss2";
            this.tss2.Size = new System.Drawing.Size(6, 29);
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // DirectoryTransportsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 272);
            this.Controls.Add(this.ts);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.ss);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(645, 310);
            this.Name = "DirectoryTransportsView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник транспортных средств";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DirectoryTransportsView_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DirectoryTransportsView_FormClosed);
            this.Shown += new System.EventHandler(this.DirectoryTransportsView_Shown);
            this.SizeChanged += new System.EventHandler(this.DirectoryTransportsView_SizeChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DirectoryTransportsView_KeyUp);
            this.ss.ResumeLayout(false);
            this.ss.PerformLayout();
            this.TabControl.ResumeLayout(false);
            this.TabCard.ResumeLayout(false);
            this.TabCard.PerformLayout();
            this.tabList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransports)).EndInit();
            this.ts.ResumeLayout(false);
            this.ts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.StatusStrip ss;
        internal System.Windows.Forms.ToolStripSplitButton btnCancel;
        internal System.Windows.Forms.ToolStripSplitButton btnOK;
        internal System.Windows.Forms.ToolStripStatusLabel lblCount;
        internal System.Windows.Forms.TabControl TabControl;
        internal System.Windows.Forms.TabPage TabCard;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox textBoxMark;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TabPage tabList;
        internal System.Windows.Forms.DataGridView dataGridViewTransports;
        internal System.Windows.Forms.ToolStrip ts;
        internal System.Windows.Forms.ToolStripButton btnAdd;
        internal System.Windows.Forms.ToolStripButton btnAddCopy;
        internal System.Windows.Forms.ToolStripButton btnDel;
        internal System.Windows.Forms.ToolStripSeparator tss1;
        internal System.Windows.Forms.ToolStripButton btnSave;
        internal System.Windows.Forms.ToolStripSeparator tss2;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        internal System.Windows.Forms.ToolStripButton btnPrev;
        internal System.Windows.Forms.ToolStripButton btnNext;
        internal System.Windows.Forms.ToolStripSeparator tss3;
        internal System.Windows.Forms.ToolStripButton btnClear;
        internal System.Windows.Forms.ToolStripTextBox textBoxSearch;
        private System.Windows.Forms.ToolStripComboBox comboBoxSearch;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}