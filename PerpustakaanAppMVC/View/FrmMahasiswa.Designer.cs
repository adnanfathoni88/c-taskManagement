namespace PerpustakaanAppMVC.View
{
    partial class FrmMahasiswa
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtNama = new System.Windows.Forms.TextBox();
            this.btnCari = new System.Windows.Forms.Button();
            this.lvwMahasiswa = new System.Windows.Forms.ListView();
            this.btnTambah = new System.Windows.Forms.Button();
            this.btnPerbaiki = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnSelesai = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cari nama mahasiswa";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(171, 15);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(350, 22);
            this.txtNama.TabIndex = 1;
            // 
            // btnCari
            // 
            this.btnCari.Location = new System.Drawing.Point(540, 13);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(82, 27);
            this.btnCari.TabIndex = 2;
            this.btnCari.Text = "Cari";
            this.btnCari.UseVisualStyleBackColor = true;
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // lvwMahasiswa
            // 
            this.lvwMahasiswa.HideSelection = false;
            this.lvwMahasiswa.Location = new System.Drawing.Point(29, 56);
            this.lvwMahasiswa.Name = "lvwMahasiswa";
            this.lvwMahasiswa.Size = new System.Drawing.Size(593, 317);
            this.lvwMahasiswa.TabIndex = 3;
            this.lvwMahasiswa.UseCompatibleStateImageBehavior = false;
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(29, 393);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(82, 27);
            this.btnTambah.TabIndex = 4;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // btnPerbaiki
            // 
            this.btnPerbaiki.Location = new System.Drawing.Point(117, 393);
            this.btnPerbaiki.Name = "btnPerbaiki";
            this.btnPerbaiki.Size = new System.Drawing.Size(82, 27);
            this.btnPerbaiki.TabIndex = 5;
            this.btnPerbaiki.Text = "Perbaiki";
            this.btnPerbaiki.UseVisualStyleBackColor = true;
            this.btnPerbaiki.Click += new System.EventHandler(this.btnPerbaiki_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(205, 393);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(82, 27);
            this.btnHapus.TabIndex = 6;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnSelesai
            // 
            this.btnSelesai.Location = new System.Drawing.Point(540, 393);
            this.btnSelesai.Name = "btnSelesai";
            this.btnSelesai.Size = new System.Drawing.Size(82, 27);
            this.btnSelesai.TabIndex = 7;
            this.btnSelesai.Text = "Selesai";
            this.btnSelesai.UseVisualStyleBackColor = true;
            this.btnSelesai.Click += new System.EventHandler(this.btnSelesai_Click);
            // 
            // FrmMahasiswa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 450);
            this.Controls.Add(this.btnSelesai);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnPerbaiki);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.lvwMahasiswa);
            this.Controls.Add(this.btnCari);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.label1);
            this.Name = "FrmMahasiswa";
            this.Text = "FrmMahasiswa";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNama;
        private System.Windows.Forms.Button btnCari;
        private System.Windows.Forms.ListView lvwMahasiswa;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.Button btnPerbaiki;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnSelesai;
    }
}