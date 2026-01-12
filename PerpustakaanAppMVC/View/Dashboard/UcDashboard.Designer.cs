namespace PerpustakaanAppMVC.View.Dashboard
{
    partial class UcDashboard
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
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.btnAddStat = new System.Windows.Forms.Button();
            this.btnAddChart = new System.Windows.Forms.Button();
            this.btnAddReport = new System.Windows.Forms.Button();
            this.panelToolbar.SuspendLayout();
            this.SuspendLayout();
            //
            // panelToolbar
            //
            this.panelToolbar.BackColor = System.Drawing.Color.Transparent;
            this.panelToolbar.Controls.Add(this.btnAddReport);
            this.panelToolbar.Controls.Add(this.btnAddChart);
            this.panelToolbar.Controls.Add(this.btnAddStat);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(399, 34);
            this.panelToolbar.TabIndex = 0;
            //
            // btnAddStat
            //
            this.btnAddStat.Location = new System.Drawing.Point(13, 3);
            this.btnAddStat.Name = "btnAddStat";
            this.btnAddStat.Size = new System.Drawing.Size(75, 23);
            this.btnAddStat.TabIndex = 0;
            this.btnAddStat.Text = "Stat";
            this.btnAddStat.UseVisualStyleBackColor = false;
            this.btnAddStat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(61)))), ((int)(((byte)(156)))));
            this.btnAddStat.ForeColor = System.Drawing.Color.White;
            this.btnAddStat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddStat.FlatAppearance.BorderSize = 0;
            //
            // btnAddChart
            //
            this.btnAddChart.Location = new System.Drawing.Point(94, 3);
            this.btnAddChart.Name = "btnAddChart";
            this.btnAddChart.Size = new System.Drawing.Size(75, 23);
            this.btnAddChart.TabIndex = 1;
            this.btnAddChart.Text = "Chart";
            this.btnAddChart.UseVisualStyleBackColor = false;
            this.btnAddChart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(81)))), ((int)(((byte)(176)))));
            this.btnAddChart.ForeColor = System.Drawing.Color.White;
            this.btnAddChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddChart.FlatAppearance.BorderSize = 0;
            //
            // btnAddReport
            //
            this.btnAddReport.Location = new System.Drawing.Point(175, 3);
            this.btnAddReport.Name = "btnAddReport";
            this.btnAddReport.Size = new System.Drawing.Size(75, 23);
            this.btnAddReport.TabIndex = 2;
            this.btnAddReport.Text = "Report";
            this.btnAddReport.UseVisualStyleBackColor = false;
            this.btnAddReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(101)))), ((int)(((byte)(196)))));
            this.btnAddReport.ForeColor = System.Drawing.Color.White;
            this.btnAddReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddReport.FlatAppearance.BorderSize = 0;
            //
            // UcDashboard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelToolbar);
            this.Name = "UcDashboard";
            this.Size = new System.Drawing.Size(399, 264);
            this.panelToolbar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnAddStat;
        private System.Windows.Forms.Button btnAddChart;
        private System.Windows.Forms.Button btnAddReport;

    }
}
