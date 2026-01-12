using System.Drawing;
using System.Windows.Forms;

namespace PerpustakaanAppMVC.View
{
    partial class FrmMain
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
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnToggleSidebar = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnRole = new System.Windows.Forms.Button();
            this.btnUser = new System.Windows.Forms.Button();
            this.btnProject = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelSidebar.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            //
            // panelSidebar
            //
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(123)))));
            this.panelSidebar.Controls.Add(this.btnToggleSidebar);
            this.panelSidebar.Controls.Add(this.btnDashboard);
            this.panelSidebar.Controls.Add(this.btnRole);
            this.panelSidebar.Controls.Add(this.btnUser);
            this.panelSidebar.Controls.Add(this.btnProject);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(200, 450);
            this.panelSidebar.TabIndex = 0;
            //
            // btnToggleSidebar
            //
            this.btnToggleSidebar.FlatAppearance.BorderSize = 0;
            this.btnToggleSidebar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleSidebar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleSidebar.ForeColor = System.Drawing.Color.White;
            this.btnToggleSidebar.Location = new System.Drawing.Point(3, 3);
            this.btnToggleSidebar.Name = "btnToggleSidebar";
            this.btnToggleSidebar.Size = new System.Drawing.Size(35, 35);
            this.btnToggleSidebar.TabIndex = 4;
            this.btnToggleSidebar.Text = "<";
            this.btnToggleSidebar.UseVisualStyleBackColor = true;
            this.btnToggleSidebar.Click += new System.EventHandler(this.btnToggleSidebar_Click);
            //
            // btnDashboard
            //
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(3, 44);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(194, 43);
            this.btnDashboard.TabIndex = 1;
            this.btnDashboard.Text = "D Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            //
            // btnRole
            //
            this.btnRole.FlatAppearance.BorderSize = 0;
            this.btnRole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRole.ForeColor = System.Drawing.Color.White;
            this.btnRole.Location = new System.Drawing.Point(3, 93);
            this.btnRole.Name = "btnRole";
            this.btnRole.Size = new System.Drawing.Size(194, 43);
            this.btnRole.TabIndex = 0;
            this.btnRole.Text = "R Role";
            this.btnRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRole.UseVisualStyleBackColor = true;
            this.btnRole.Click += new System.EventHandler(this.btnRole_Click);
            //
            // btnUser
            //
            this.btnUser.FlatAppearance.BorderSize = 0;
            this.btnUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUser.ForeColor = System.Drawing.Color.White;
            this.btnUser.Location = new System.Drawing.Point(3, 142);
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(194, 43);
            this.btnUser.TabIndex = 2;
            this.btnUser.Text = "U User";
            this.btnUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUser.UseVisualStyleBackColor = true;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            //
            // btnProject
            //
            this.btnProject.FlatAppearance.BorderSize = 0;
            this.btnProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProject.ForeColor = System.Drawing.Color.White;
            this.btnProject.Location = new System.Drawing.Point(3, 191);
            this.btnProject.Name = "btnProject";
            this.btnProject.Size = new System.Drawing.Size(194, 43);
            this.btnProject.TabIndex = 3;
            this.btnProject.Text = "P Project";
            this.btnProject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProject.UseVisualStyleBackColor = true;
            this.btnProject.Click += new System.EventHandler(this.btnProject_Click);
            //
            // lbTitle
            //
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.8209F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(20, 15);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(0, 29);
            this.lbTitle.TabIndex = 0;
            //
            // panelHeader
            //
            this.panelHeader.Controls.Add(this.lbTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(200, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(600, 45);
            this.panelHeader.TabIndex = 2;
            //
            // panelContent
            //
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(200, 45);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(600, 405);
            this.panelContent.TabIndex = 1;
            //
            // FrmMain
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSidebar);
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            this.panelSidebar.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnRole;
        private System.Windows.Forms.Button btnUser;
        private Button btnProject;
        private Label lbTitle;
        private Panel panelHeader;
        private Panel panelContent;
        private Button btnToggleSidebar;
    }
}