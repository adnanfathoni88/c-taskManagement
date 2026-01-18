using PerpustakaanAppMVC.Controller;
using PerpustakaanAppMVC.Model.Context;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Model.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerpustakaanAppMVC.View.UserView
{

    public enum FormMode
    {
        Create,
        Update,
        View
    }

    public delegate void CreateUpdateEventHandler(User user);

    public partial class FrmEntryUser : Form
    {
        public event CreateUpdateEventHandler OnCreate;

        public event CreateUpdateEventHandler OnUpdate;

        private UserController _controller;

        private _FormMode _mode;

        private User user;


        public FrmEntryUser()
        {
            InitializeComponent();
        }
        public FrmEntryUser(string title, _FormMode mode, UserController controller) : this()
        {
            this.Text = title;
            _mode = mode;

            _controller = controller;

            LoadRoles();
        }

        private void LoadRoles()
        {
            using (var ctx = new DbContext())
            {
                var repo = new RoleRepository(ctx);
                cmbRole.DataSource = repo.ReadAll();
                cmbRole.DisplayMember = "Name";
                cmbRole.ValueMember = "Id";
            }
        }

        public FrmEntryUser(string title, _FormMode mode, User obj, UserController controller) : this()
        {
            this.Text = title;
            _mode = mode;
            _controller = controller;
            user = obj;


            txtName.Text = user.Name;
            txtEmail.Text = user.Email;
            txtPassword.Text = user.Password;
            cmbRole.SelectedValue = user.RoleId;
            chkAktif.Checked = user.Status == 1;

            if (_mode == _FormMode.View) { setViewMode(); }
        }

        private void setViewMode()
        {
            txtName.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtPassword.ReadOnly = true;
            cmbRole.Enabled = false;
            chkAktif.Enabled = false;
            btnSimpan.Visible = false;

            // Hide password field in view mode
            lbPassword.Visible = false;
            txtPassword.Visible = false;
        }

        private void setEditMode()
        {
            txtName.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtPassword.ReadOnly = false;
            cmbRole.Enabled = true;
            chkAktif.Enabled = true;
            btnSimpan.Visible = true;

            // Hide password field in edit mode
            lbPassword.Visible = false;
            txtPassword.Visible = false;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate email format
                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Email format tidak valid.", "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if email already exists (for Create mode or Update mode when email changed)
                if (_mode == _FormMode.Create || (user != null && user.Email != txtEmail.Text))
                {
                    var existingUser = _controller.GetByEmail(txtEmail.Text);
                    if (existingUser != null)
                    {
                        MessageBox.Show("Email sudah digunakan oleh pengguna lain.", "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (_mode == _FormMode.Create) user = new User();

                user.Name = txtName.Text;
                user.Email = txtEmail.Text;
                // Only update password if it's not empty (for update mode)
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    user.Password = txtPassword.Text;
                }
                user.RoleId = (int)cmbRole.SelectedValue;
                user.Status = chkAktif.Checked ? 1 : 0;

                int result = _mode == _FormMode.Create
                    ? _controller.Create(user)
                    : _controller.Update(user);

                if (result > 0)
                {
                    if (_mode == _FormMode.Create)
                        OnCreate?.Invoke(user);
                    else
                        OnUpdate?.Invoke(user);

                    Close();
                }
                else
                {
                    MessageBox.Show("Operasi gagal. Silakan coba lagi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Check for specific database constraint violations
                if (ex.Message.Contains("UNIQUE constraint failed") || ex.Message.Contains("duplicate"))
                {
                    MessageBox.Show("Email sudah digunakan oleh pengguna lain.", "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void FrmEntryUser_Load(object sender, EventArgs e)
        {
            LoadRoles();

            if (_mode == _FormMode.Update || _mode == _FormMode.View)
            {
                txtName.Text = user.Name;
                txtEmail.Text = user.Email;
                cmbRole.SelectedValue = user.RoleId;
                chkAktif.Checked = user.Status == 1;
            }

            // Apply mode-specific UI settings
            if (_mode == _FormMode.View)
            {
                setViewMode();
                lbForm.Text = "View User";
            }
            else if (_mode == _FormMode.Update)
            {
                setEditMode();
                lbForm.Text = "Edit User";
            }
            else if (_mode == _FormMode.Create)
            {
                // For create mode, show all fields
                lbPassword.Visible = true;
                txtPassword.Visible = true;
                lbForm.Text = "Create User";
            }
        }

    }
}
