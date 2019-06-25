namespace Client.BusinessTrip.Views
{
    partial class DirectoryDirectionsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryDirectionsView));
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.img = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxReason = new System.Windows.Forms.TextBox();
            this.lblOsnovanie = new System.Windows.Forms.Label();
            this.grDate = new System.Windows.Forms.GroupBox();
            this.textBoxDayCount = new System.Windows.Forms.TextBox();
            this.lblDaycnt = new System.Windows.Forms.Label();
            this.dateTimePickerDateBegin = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerDateEnd = new System.Windows.Forms.DateTimePicker();
            this.lblDatebeg = new System.Windows.Forms.Label();
            this.lblDateend = new System.Windows.Forms.Label();
            this.pLoc = new System.Windows.Forms.Panel();
            this.btnLocClear = new System.Windows.Forms.Button();
            this.textBoxLocation = new System.Windows.Forms.TextBox();
            this.btnLoc = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOrgClear = new System.Windows.Forms.Button();
            this.textBoxOrganization = new System.Windows.Forms.TextBox();
            this.btnOrg = new System.Windows.Forms.Button();
            this.lblLoc = new System.Windows.Forms.Label();
            this.lblOrg = new System.Windows.Forms.Label();
            this.ss = new System.Windows.Forms.StatusStrip();
            this.btnCancel = new System.Windows.Forms.ToolStripSplitButton();
            this.btnOK = new System.Windows.Forms.ToolStripSplitButton();
            this.bindingSourceDirect = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.panel1.SuspendLayout();
            this.grDate.SuspendLayout();
            this.pLoc.SuspendLayout();
            this.panel2.SuspendLayout();
            this.ss.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDirect)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // img
            // 
            this.img.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img.ImageStream")));
            this.img.TransparentColor = System.Drawing.Color.Transparent;
            this.img.Images.SetKeyName(0, "ImageEdit.png");
            this.img.Images.SetKeyName(1, "ImageList.png");
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.textBoxReason);
            this.panel1.Controls.Add(this.lblOsnovanie);
            this.panel1.Controls.Add(this.grDate);
            this.panel1.Controls.Add(this.pLoc);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblLoc);
            this.panel1.Controls.Add(this.lblOrg);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 169);
            this.panel1.TabIndex = 10;
            // 
            // textBoxReason
            // 
            this.textBoxReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReason.Location = new System.Drawing.Point(110, 138);
            this.textBoxReason.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.textBoxReason.MaxLength = 100;
            this.textBoxReason.Name = "textBoxReason";
            this.textBoxReason.Size = new System.Drawing.Size(451, 22);
            this.textBoxReason.TabIndex = 47;
            // 
            // lblOsnovanie
            // 
            this.lblOsnovanie.AutoSize = true;
            this.lblOsnovanie.Location = new System.Drawing.Point(27, 141);
            this.lblOsnovanie.Margin = new System.Windows.Forms.Padding(3, 0, 7, 0);
            this.lblOsnovanie.Name = "lblOsnovanie";
            this.lblOsnovanie.Size = new System.Drawing.Size(73, 14);
            this.lblOsnovanie.TabIndex = 46;
            this.lblOsnovanie.Text = "Основание:";
            // 
            // grDate
            // 
            this.grDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grDate.Controls.Add(this.textBoxDayCount);
            this.grDate.Controls.Add(this.lblDaycnt);
            this.grDate.Controls.Add(this.dateTimePickerDateBegin);
            this.grDate.Controls.Add(this.dateTimePickerDateEnd);
            this.grDate.Controls.Add(this.lblDatebeg);
            this.grDate.Controls.Add(this.lblDateend);
            this.grDate.Location = new System.Drawing.Point(13, 82);
            this.grDate.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.grDate.Name = "grDate";
            this.grDate.Size = new System.Drawing.Size(556, 50);
            this.grDate.TabIndex = 45;
            this.grDate.TabStop = false;
            this.grDate.Text = "Даты: ";
            // 
            // textBoxDayCount
            // 
            this.textBoxDayCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDayCount.Font = new System.Drawing.Font("Tahoma", 8F);
            this.textBoxDayCount.Location = new System.Drawing.Point(393, 16);
            this.textBoxDayCount.Name = "textBoxDayCount";
            this.textBoxDayCount.ReadOnly = true;
            this.textBoxDayCount.Size = new System.Drawing.Size(155, 20);
            this.textBoxDayCount.TabIndex = 30;
            this.textBoxDayCount.Text = "00";
            // 
            // lblDaycnt
            // 
            this.lblDaycnt.AutoSize = true;
            this.lblDaycnt.Location = new System.Drawing.Point(343, 19);
            this.lblDaycnt.Margin = new System.Windows.Forms.Padding(3, 0, 7, 0);
            this.lblDaycnt.Name = "lblDaycnt";
            this.lblDaycnt.Size = new System.Drawing.Size(40, 14);
            this.lblDaycnt.TabIndex = 30;
            this.lblDaycnt.Text = "Дней:";
            // 
            // dateTimePickerDateBegin
            // 
            this.dateTimePickerDateBegin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDateBegin.Location = new System.Drawing.Point(36, 16);
            this.dateTimePickerDateBegin.Name = "dateTimePickerDateBegin";
            this.dateTimePickerDateBegin.Size = new System.Drawing.Size(128, 22);
            this.dateTimePickerDateBegin.TabIndex = 7;
            this.dateTimePickerDateBegin.ValueChanged += new System.EventHandler(this.dateTimePickerDateBegin_ValueChanged);
            // 
            // dateTimePickerDateEnd
            // 
            this.dateTimePickerDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerDateEnd.Location = new System.Drawing.Point(204, 16);
            this.dateTimePickerDateEnd.Name = "dateTimePickerDateEnd";
            this.dateTimePickerDateEnd.Size = new System.Drawing.Size(128, 22);
            this.dateTimePickerDateEnd.TabIndex = 6;
            this.dateTimePickerDateEnd.ValueChanged += new System.EventHandler(this.dateTimePickerDateEnd_ValueChanged);
            // 
            // lblDatebeg
            // 
            this.lblDatebeg.AutoSize = true;
            this.lblDatebeg.Location = new System.Drawing.Point(12, 18);
            this.lblDatebeg.Margin = new System.Windows.Forms.Padding(3, 0, 7, 0);
            this.lblDatebeg.Name = "lblDatebeg";
            this.lblDatebeg.Size = new System.Drawing.Size(14, 14);
            this.lblDatebeg.TabIndex = 0;
            this.lblDatebeg.Text = "С";
            // 
            // lblDateend
            // 
            this.lblDateend.AutoSize = true;
            this.lblDateend.Location = new System.Drawing.Point(173, 19);
            this.lblDateend.Margin = new System.Windows.Forms.Padding(3, 0, 7, 0);
            this.lblDateend.Name = "lblDateend";
            this.lblDateend.Size = new System.Drawing.Size(21, 14);
            this.lblDateend.TabIndex = 2;
            this.lblDateend.Text = "по";
            // 
            // pLoc
            // 
            this.pLoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pLoc.Controls.Add(this.btnLocClear);
            this.pLoc.Controls.Add(this.textBoxLocation);
            this.pLoc.Controls.Add(this.btnLoc);
            this.pLoc.Location = new System.Drawing.Point(110, 13);
            this.pLoc.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.pLoc.Name = "pLoc";
            this.pLoc.Size = new System.Drawing.Size(459, 28);
            this.pLoc.TabIndex = 43;
            // 
            // btnLocClear
            // 
            this.btnLocClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocClear.FlatAppearance.BorderSize = 0;
            this.btnLocClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLocClear.Image = global::Client.BusinessTrip.Properties.Resources.ImageClear;
            this.btnLocClear.Location = new System.Drawing.Point(429, 2);
            this.btnLocClear.Name = "btnLocClear";
            this.btnLocClear.Size = new System.Drawing.Size(27, 24);
            this.btnLocClear.TabIndex = 28;
            this.btnLocClear.UseVisualStyleBackColor = true;
            this.btnLocClear.Click += new System.EventHandler(this.btnLocClear_Click);
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLocation.Location = new System.Drawing.Point(1, 3);
            this.textBoxLocation.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.ReadOnly = true;
            this.textBoxLocation.Size = new System.Drawing.Size(393, 22);
            this.textBoxLocation.TabIndex = 24;
            // 
            // btnLoc
            // 
            this.btnLoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoc.FlatAppearance.BorderSize = 0;
            this.btnLoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoc.Image = global::Client.BusinessTrip.Properties.Resources.ImageBook;
            this.btnLoc.Location = new System.Drawing.Point(399, 2);
            this.btnLoc.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnLoc.Name = "btnLoc";
            this.btnLoc.Size = new System.Drawing.Size(27, 24);
            this.btnLoc.TabIndex = 25;
            this.btnLoc.UseVisualStyleBackColor = true;
            this.btnLoc.Click += new System.EventHandler(this.btnLoc_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.btnOrgClear);
            this.panel2.Controls.Add(this.textBoxOrganization);
            this.panel2.Controls.Add(this.btnOrg);
            this.panel2.Location = new System.Drawing.Point(110, 47);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(459, 28);
            this.panel2.TabIndex = 44;
            // 
            // btnOrgClear
            // 
            this.btnOrgClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrgClear.FlatAppearance.BorderSize = 0;
            this.btnOrgClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrgClear.Image = global::Client.BusinessTrip.Properties.Resources.ImageClear;
            this.btnOrgClear.Location = new System.Drawing.Point(429, 1);
            this.btnOrgClear.Name = "btnOrgClear";
            this.btnOrgClear.Size = new System.Drawing.Size(27, 24);
            this.btnOrgClear.TabIndex = 29;
            this.btnOrgClear.UseVisualStyleBackColor = true;
            this.btnOrgClear.Click += new System.EventHandler(this.btnOrgClear_Click);
            // 
            // textBoxOrganization
            // 
            this.textBoxOrganization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOrganization.Location = new System.Drawing.Point(1, 3);
            this.textBoxOrganization.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxOrganization.Name = "textBoxOrganization";
            this.textBoxOrganization.ReadOnly = true;
            this.textBoxOrganization.Size = new System.Drawing.Size(393, 22);
            this.textBoxOrganization.TabIndex = 24;
            // 
            // btnOrg
            // 
            this.btnOrg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrg.FlatAppearance.BorderSize = 0;
            this.btnOrg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrg.Image = global::Client.BusinessTrip.Properties.Resources.ImageBook;
            this.btnOrg.Location = new System.Drawing.Point(399, 1);
            this.btnOrg.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnOrg.Name = "btnOrg";
            this.btnOrg.Size = new System.Drawing.Size(27, 24);
            this.btnOrg.TabIndex = 25;
            this.btnOrg.UseVisualStyleBackColor = true;
            this.btnOrg.Click += new System.EventHandler(this.btnOrg_Click);
            // 
            // lblLoc
            // 
            this.lblLoc.AutoSize = true;
            this.lblLoc.Location = new System.Drawing.Point(55, 20);
            this.lblLoc.Margin = new System.Windows.Forms.Padding(3, 0, 7, 0);
            this.lblLoc.Name = "lblLoc";
            this.lblLoc.Size = new System.Drawing.Size(46, 14);
            this.lblLoc.TabIndex = 41;
            this.lblLoc.Text = "Адрес:";
            // 
            // lblOrg
            // 
            this.lblOrg.AutoSize = true;
            this.lblOrg.Location = new System.Drawing.Point(17, 53);
            this.lblOrg.Margin = new System.Windows.Forms.Padding(3, 0, 7, 0);
            this.lblOrg.Name = "lblOrg";
            this.lblOrg.Size = new System.Drawing.Size(84, 14);
            this.lblOrg.TabIndex = 42;
            this.lblOrg.Text = "Организация:";
            // 
            // ss
            // 
            this.ss.AutoSize = false;
            this.ss.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCancel,
            this.btnOK});
            this.ss.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ss.Location = new System.Drawing.Point(0, 169);
            this.ss.Name = "ss";
            this.ss.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.ss.Size = new System.Drawing.Size(584, 32);
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
            // bindingSourceDirect
            // 
            this.bindingSourceDirect.DataSource = typeof(Core.BusinessTrip.Domain.Direction);
            // 
            // DirectoryDirectionsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 201);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ss);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(600, 239);
            this.Name = "DirectoryDirectionsView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактор направлений";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DirectoryDirectionsView_FormClosed);
            this.SizeChanged += new System.EventHandler(this.DirectoryDirectionsView_SizeChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DirectoryDirectionsView_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grDate.ResumeLayout(false);
            this.grDate.PerformLayout();
            this.pLoc.ResumeLayout(false);
            this.pLoc.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ss.ResumeLayout(false);
            this.ss.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceDirect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        internal System.Windows.Forms.ImageList img;
        private System.Windows.Forms.BindingSource bindingSourceDirect;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.TextBox textBoxReason;
        internal System.Windows.Forms.Label lblOsnovanie;
        internal System.Windows.Forms.GroupBox grDate;
        internal System.Windows.Forms.TextBox textBoxDayCount;
        internal System.Windows.Forms.Label lblDaycnt;
        internal System.Windows.Forms.DateTimePicker dateTimePickerDateBegin;
        internal System.Windows.Forms.DateTimePicker dateTimePickerDateEnd;
        internal System.Windows.Forms.Label lblDatebeg;
        internal System.Windows.Forms.Label lblDateend;
        internal System.Windows.Forms.Panel pLoc;
        internal System.Windows.Forms.TextBox textBoxLocation;
        internal System.Windows.Forms.Button btnLoc;
        internal System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.TextBox textBoxOrganization;
        internal System.Windows.Forms.Button btnOrg;
        internal System.Windows.Forms.Label lblLoc;
        internal System.Windows.Forms.Label lblOrg;
        internal System.Windows.Forms.StatusStrip ss;
        internal System.Windows.Forms.ToolStripSplitButton btnCancel;
        internal System.Windows.Forms.ToolStripSplitButton btnOK;
        internal System.Windows.Forms.Button btnLocClear;
        internal System.Windows.Forms.Button btnOrgClear;
    }
}