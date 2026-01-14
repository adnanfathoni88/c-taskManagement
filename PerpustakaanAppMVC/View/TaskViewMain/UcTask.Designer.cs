namespace PerpustakaanAppMVC.View.TaskViewMain
{
    partial class UcTask
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTambah = new System.Windows.Forms.Button();
            this.dgvTask = new System.Windows.Forms.DataGridView();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTask)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnTambah);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(541, 51);
            this.panelHeader.TabIndex = 0;
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(3, 3);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(117, 48);
            this.btnTambah.TabIndex = 0;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // dgvTask
            // 
            this.dgvTask.BackgroundColor = System.Drawing.Color.White;
            this.dgvTask.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTask.Location = new System.Drawing.Point(0, 51);
            this.dgvTask.Name = "dgvTask";
            this.dgvTask.RowHeadersVisible = false;
            this.dgvTask.RowHeadersWidth = 51;
            this.dgvTask.Size = new System.Drawing.Size(541, 338);
            this.dgvTask.TabIndex = 1;
            // 
            // UcTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvTask);
            this.Controls.Add(this.panelHeader);
            this.Name = "UcTask";
            this.Size = new System.Drawing.Size(541, 389);
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTask)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel panelHeader;
        private System.Windows.Forms.DataGridView dgvTask;
        private System.Windows.Forms.Button btnTambah;
    }
}
