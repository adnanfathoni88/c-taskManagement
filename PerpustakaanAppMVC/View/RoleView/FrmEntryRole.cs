using PerpustakaanAppMVC.Controller;
using PerpustakaanAppMVC.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerpustakaanAppMVC.View.RoleView
{
    public enum _FormMode
    {
        Create,
        Update,
        View
    }

    public delegate void CreateUpdateEventHandler(Role role);
    public partial class FrmEntryRole : Form
    {

        public event CreateUpdateEventHandler OnCreate;

        public event CreateUpdateEventHandler OnUpdate;

        private RoleController _controller;

        private _FormMode _mode;

        private Role role;

        public FrmEntryRole()
        {
            InitializeComponent();
        }

        public FrmEntryRole(string title, _FormMode mode, RoleController controller) : this()
        {
            this.Text = title;
            _mode = mode;
            _controller = controller;

            if (_mode == _FormMode.View) { setViewMode(); }
        }

        public FrmEntryRole(string title, _FormMode mode, Role obj, RoleController controller) : this()
        {
            this.Text = title;
            _mode = mode;
            _controller = controller;
            role = obj;
            txtName.Text = role.Name;

            if (_mode == _FormMode.View) { setViewMode(); }
        }

        private void setViewMode()
        {
            txtName.ReadOnly = true;
            btnSimpan.Visible = false;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // Validate role name
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Nama role harus diisi.", "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if role name already exists (for Create mode or Update mode when name changed)
            if (_mode == _FormMode.Create || (role != null && !role.Name.Equals(txtName.Text, StringComparison.OrdinalIgnoreCase)))
            {
                var existingRole = _controller.GetByName(txtName.Text);
                if (existingRole != null)
                {
                    MessageBox.Show("Nama role sudah digunakan. Silakan gunakan nama yang berbeda.", "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (_mode == _FormMode.Create) role = new Role();

            role.Name = txtName.Text;
            int result = 0;

            try
            {
                if (_mode == _FormMode.Create)
                {
                    result = _controller.Create(role);
                    if (result > 0)
                    {
                        OnCreate?.Invoke(role);
                        MessageBox.Show("Data role berhasil disimpan.", "Info");
                        this.Close();
                    }
                }
                else
                {
                    result = _controller.Update(role);
                    if (result > 0)
                    {
                        OnUpdate?.Invoke(role);
                        MessageBox.Show("Data role berhasil diupdate.", "Info");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Check for specific database constraint violations
                if (ex.Message.Contains("UNIQUE constraint failed") || ex.Message.Contains("duplicate"))
                {
                    MessageBox.Show("Nama role sudah digunakan. Silakan gunakan nama yang berbeda.", "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error");
                }
            }
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
