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
    public delegate void CreateUpdateEventHandler(Role role);
    public partial class FrmEntryRole : Form
    {

        public event CreateUpdateEventHandler OnCreate;

        public event CreateUpdateEventHandler OnUpdate;

        private RoleController _controller;

        private bool isNewData = true;

        private Role role;

        public FrmEntryRole()
        {
            InitializeComponent();
        }

        public FrmEntryRole(string title, RoleController controller) : this()
        {
            this.Text = title;
            _controller = controller;
        }

        public FrmEntryRole(string title, Role obj, RoleController controller) : this()
        {
            this.Text = title;
            _controller = controller;
            isNewData = false;
            role = obj;
            txtName.Text = role.Name;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (isNewData) role = new Role();

            role.Name = txtName.Text;
            int result = 0;

            try
            {
                if (isNewData)
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
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error");
            }
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
