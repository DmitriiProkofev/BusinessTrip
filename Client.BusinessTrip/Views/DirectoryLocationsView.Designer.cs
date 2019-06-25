namespace Client.BusinessTrip.Views
{
    partial class DirectoryLocationsView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryLocationsView));
            this.tabList = new System.Windows.Forms.TabPage();
            this.dataGridViewLocations = new System.Windows.Forms.DataGridView();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.textBoxShortAddress = new System.Windows.Forms.TextBox();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.TabCard = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.ss = new System.Windows.Forms.StatusStrip();
            this.btnCancel = new System.Windows.Forms.ToolStripSplitButton();
            this.btnOK = new System.Windows.Forms.ToolStripSplitButton();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.img = new System.Windows.Forms.ImageList(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
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
            this.bindingSourceLocations = new System.Windows.Forms.BindingSource(this.components);
            this.tabList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLocations)).BeginInit();
            this.TabCard.SuspendLayout();
            this.ss.SuspendLayout();
            this.TabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.ts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceLocations)).BeginInit();
            this.SuspendLayout();
            // 
            // tabList
            // 
            this.tabList.Controls.Add(this.dataGridViewLocations);
            this.tabList.ImageIndex = 1;
            this.tabList.Location = new System.Drawing.Point(4, 23);
            this.tabList.Name = "tabList";
            this.tabList.Padding = new System.Windows.Forms.Padding(3);
            this.tabList.Size = new System.Drawing.Size(617, 204);
            this.tabList.TabIndex = 1;
            this.tabList.Text = "Список";
            this.tabList.UseVisualStyleBackColor = true;
            // 
            // dataGridViewLocations
            // 
            this.dataGridViewLocations.AllowUserToAddRows = false;
            this.dataGridViewLocations.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridViewLocations.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewLocations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLocations.ColumnHeadersHeight = 35;
            this.dataGridViewLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLocations.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewLocations.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewLocations.Name = "dataGridViewLocations";
            this.dataGridViewLocations.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewLocations.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(196)))), ((int)(((byte)(128)))));
            this.dataGridViewLocations.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewLocations.RowTemplate.Height = 30;
            this.dataGridViewLocations.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewLocations.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewLocations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLocations.ShowEditingIcon = false;
            this.dataGridViewLocations.Size = new System.Drawing.Size(611, 198);
            this.dataGridViewLocations.TabIndex = 0;
            this.dataGridViewLocations.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewLocations_CellMouseDown);
            this.dataGridViewLocations.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewLocations_ColumnHeaderMouseClick);
            this.dataGridViewLocations.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewLocations_RowPostPaint);
            this.dataGridViewLocations.SelectionChanged += new System.EventHandler(this.dataGridViewLocations_SelectionChanged);
            this.dataGridViewLocations.DoubleClick += new System.EventHandler(this.dataGridViewLocations_DoubleClick);
            this.dataGridViewLocations.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewLocations_KeyDown);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(6, 45);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(92, 14);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Полный адрес:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(6, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(46, 14);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Адрес:";
            // 
            // textBoxShortAddress
            // 
            this.textBoxShortAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxShortAddress.Location = new System.Drawing.Point(6, 20);
            this.textBoxShortAddress.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.textBoxShortAddress.MaxLength = 100;
            this.textBoxShortAddress.Name = "textBoxShortAddress";
            this.textBoxShortAddress.Size = new System.Drawing.Size(584, 22);
            this.textBoxShortAddress.TabIndex = 3;
            this.textBoxShortAddress.TextChanged += new System.EventHandler(this.textBoxShortAddress_TextChanged);
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAddress.Location = new System.Drawing.Point(6, 62);
            this.textBoxAddress.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxAddress.MaxLength = 250;
            this.textBoxAddress.Multiline = true;
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(584, 136);
            this.textBoxAddress.TabIndex = 1;
            this.textBoxAddress.TextChanged += new System.EventHandler(this.textBoxAddress_TextChanged);
            // 
            // TabCard
            // 
            this.TabCard.Controls.Add(this.label2);
            this.TabCard.Controls.Add(this.textBoxAddress);
            this.TabCard.Controls.Add(this.Label1);
            this.TabCard.Controls.Add(this.lblName);
            this.TabCard.Controls.Add(this.textBoxShortAddress);
            this.TabCard.ImageIndex = 0;
            this.TabCard.Location = new System.Drawing.Point(4, 23);
            this.TabCard.Name = "TabCard";
            this.TabCard.Padding = new System.Windows.Forms.Padding(3);
            this.TabCard.Size = new System.Drawing.Size(612, 204);
            this.TabCard.TabIndex = 0;
            this.TabCard.Text = "Карточка";
            this.TabCard.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(590, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 14);
            this.label2.TabIndex = 15;
            this.label2.Text = "*";
            // 
            // ss
            // 
            this.ss.AutoSize = false;
            this.ss.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCancel,
            this.btnOK,
            this.lblCount});
            this.ss.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ss.Location = new System.Drawing.Point(0, 265);
            this.ss.Name = "ss";
            this.ss.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.ss.Size = new System.Drawing.Size(629, 32);
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
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.TabCard);
            this.TabControl.Controls.Add(this.tabList);
            this.TabControl.ImageList = this.img;
            this.TabControl.Location = new System.Drawing.Point(6, 30);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(620, 231);
            this.TabControl.TabIndex = 1;
            this.TabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabControl_Selecting);
            // 
            // img
            // 
            this.img.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img.ImageStream")));
            this.img.TransparentColor = System.Drawing.Color.Transparent;
            this.img.Images.SetKeyName(0, "ImageEdit.png");
            this.img.Images.SetKeyName(1, "ImageList.png");
            this.img.Images.SetKeyName(2, "ImageBook.png");
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
            // bindingSourceLocations
            // 
            this.bindingSourceLocations.DataSource = typeof(Core.BusinessTrip.Domain.Location);
            // 
            // DirectoryLocationsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 297);
            this.Controls.Add(this.ts);
            this.Controls.Add(this.ss);
            this.Controls.Add(this.TabControl);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(645, 310);
            this.Name = "DirectoryLocationsView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник адресов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DirectoryLocationsView_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DirectoryLocationsView_FormClosed);
            this.Shown += new System.EventHandler(this.DirectoryLocationsView_Shown);
            this.SizeChanged += new System.EventHandler(this.DirectoryLocationsView_SizeChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DirectoryLocationsView_KeyUp);
            this.tabList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLocations)).EndInit();
            this.TabCard.ResumeLayout(false);
            this.TabCard.PerformLayout();
            this.ss.ResumeLayout(false);
            this.ss.PerformLayout();
            this.TabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ts.ResumeLayout(false);
            this.ts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceLocations)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.TabPage tabList;
        internal System.Windows.Forms.DataGridView dataGridViewLocations;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label lblName;
        internal System.Windows.Forms.TextBox textBoxShortAddress;
        internal System.Windows.Forms.TextBox textBoxAddress;
        internal System.Windows.Forms.TabPage TabCard;
        internal System.Windows.Forms.StatusStrip ss;
        internal System.Windows.Forms.ToolStripSplitButton btnCancel;
        internal System.Windows.Forms.ToolStripSplitButton btnOK;
        internal System.Windows.Forms.ToolStripStatusLabel lblCount;
        internal System.Windows.Forms.TabControl TabControl;
        internal System.Windows.Forms.ImageList img;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.BindingSource bindingSourceLocations;
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
        private System.Windows.Forms.Label label2;
    }
}