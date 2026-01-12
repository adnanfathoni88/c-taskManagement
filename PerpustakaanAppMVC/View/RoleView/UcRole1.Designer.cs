namespace PerpustakaanAppMVC.View.RoleView
{
    partial class UcRole1
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
            this.panelCard = new System.Windows.Forms.Panel();
            this.lvRole = new System.Windows.Forms.ListView();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.panelCard.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.SuspendLayout();
            //
            // panelCard
            //
            this.panelCard.BackColor = System.Drawing.Color.White;
            this.panelCard.Controls.Add(this.lvRole);
            this.panelCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCard.Location = new System.Drawing.Point(10, 50);
            this.panelCard.Name = "panelCard";
            this.panelCard.Padding = new System.Windows.Forms.Padding(15);
            this.panelCard.Size = new System.Drawing.Size(523, 317);
            this.panelCard.TabIndex = 7;
            //
            // lvRole
            //
            this.lvRole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRole.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lvRole.FullRowSelect = true;
            this.lvRole.HideSelection = false;
            this.lvRole.Location = new System.Drawing.Point(15, 15);
            this.lvRole.Name = "lvRole";
            this.lvRole.Size = new System.Drawing.Size(493, 287);
            this.lvRole.TabIndex = 0;
            this.lvRole.UseCompatibleStateImageBehavior = false;
            //
            // btnEdit
            //
            this.btnEdit.Location = new System.Drawing.Point(94, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(180)))), ((int)(((byte)(75)))));
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnHapus
            //
            this.btnHapus.Location = new System.Drawing.Point(175, 3);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(75, 23);
            this.btnHapus.TabIndex = 4;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = false;
            this.btnHapus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnHapus.ForeColor = System.Drawing.Color.White;
            this.btnHapus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHapus.FlatAppearance.BorderSize = 0;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            //
            // btnTambah
            //
            this.btnTambah.Location = new System.Drawing.Point(13, 3);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(75, 23);
            this.btnTambah.TabIndex = 1;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = false;
            this.btnTambah.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnTambah.ForeColor = System.Drawing.Color.White;
            this.btnTambah.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTambah.FlatAppearance.BorderSize = 0;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            //
            // panelToolbar
            //
            this.panelToolbar.BackColor = System.Drawing.Color.Transparent;
            this.panelToolbar.Controls.Add(this.btnTambah);
            this.panelToolbar.Controls.Add(this.btnHapus);
            this.panelToolbar.Controls.Add(this.btnEdit);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(10, 10);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(523, 34);
            this.panelToolbar.TabIndex = 6;
            //
            // UcRole1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelCard);
            this.Controls.Add(this.panelToolbar);
            this.Name = "UcRole1";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(543, 377);
            this.panelCard.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCard;
        private System.Windows.Forms.ListView lvRole;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.Panel panelToolbar;
    }
}
