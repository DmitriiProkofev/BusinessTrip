namespace Core.BusinessTrip.ProjectBase.Utils.AsyncWorking
{
    partial class AsyncLoaderForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_name = new System.Windows.Forms.Label();
            this.progressBarWork = new System.Windows.Forms.ProgressBar();
            this.backgroundWorkerMaster = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_name);
            this.groupBox1.Controls.Add(this.progressBarWork);
            this.groupBox1.Location = new System.Drawing.Point(5, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(358, 70);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(6, 16);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(183, 13);
            this.label_name.TabIndex = 1;
            this.label_name.Text = "Ожидайте завершения операции...";
            // 
            // progressBarWork
            // 
            this.progressBarWork.Location = new System.Drawing.Point(8, 32);
            this.progressBarWork.Name = "progressBarWork";
            this.progressBarWork.Size = new System.Drawing.Size(343, 23);
            this.progressBarWork.TabIndex = 0;
            // 
            // backgroundWorkerMaster
            // 
            this.backgroundWorkerMaster.WorkerReportsProgress = true;
            this.backgroundWorkerMaster.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorkerMasterProgressChanged);
            this.backgroundWorkerMaster.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerMasterRunWorkerCompleted);
            // 
            // AsyncLoaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 75);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AsyncLoaderForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ожидайте завершения операции";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AsyncLoaderForm_FormClosed);
            this.Load += new System.EventHandler(this.WaitFormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.ProgressBar progressBarWork;
        private System.ComponentModel.BackgroundWorker backgroundWorkerMaster;
    }
}