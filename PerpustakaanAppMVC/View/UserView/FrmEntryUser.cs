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

    public delegate void CreateUpdateEventHandler(User user);

    public partial class FrmEntryUser : Form
    {
        public event CreateUpdateEventHandler OnCreate;

        public event CreateUpdateEventHandler OnUpdate;

        private UserController _controller;

        private bool isNewData = true;

        private User user;


        public FrmEntryUser()
        {
            InitializeComponent();
        }
        public FrmEntryUser(string title, UserController controller) : this()
        {
            this.Text = title;

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

        public FrmEntryUser(string title, User obj, UserController controller) : this()
        {
            this.Text = title;
            _controller = controller;
            isNewData = false;
            user = obj;


            txtName.Text = user.Name;
            txtEmail.Text = user.Email;
            txtPassword.Text = user.Password;
            cmbRole.SelectedValue = user.RoleId;
            chkAktif.Checked = user.Status == 1;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                if (isNewData) user = new User();

                user.Name = txtName.Text;
                user.Email = txtEmail.Text;
                user.Password = txtPassword.Text;
                user.RoleId = (int)cmbRole.SelectedValue;
                user.Status = chkAktif.Checked ? 1 : 0;

                int result = isNewData
                    ? _controller.Create(user)
                    : _controller.Update(user);

                if (result > 0)
                {
                    if (isNewData)
                        OnCreate?.Invoke(user);
                    else
                        OnUpdate?.Invoke(user);

                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void FrmEntryUser_Load(object sender, EventArgs e)
        {
            LoadRoles();

            if (!isNewData)
            {
                txtName.Text = user.Name;
                txtEmail.Text = user.Email;
                txtPassword.Text = user.Password;
                cmbRole.SelectedValue = user.RoleId;
                chkAktif.Checked = user.Status == 1;
            }
        }
    }
}
