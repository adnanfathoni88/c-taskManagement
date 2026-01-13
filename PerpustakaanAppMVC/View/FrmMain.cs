using PerpustakaanAppMVC.View.Dashboard;
using PerpustakaanAppMVC.View.ProjectView;
using PerpustakaanAppMVC.View.RoleView;
using PerpustakaanAppMVC.View.UserView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerpustakaanAppMVC.View
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
        }

        private void LoadUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            this.panelContent.Controls.Clear();
            this.panelContent.Controls.Add(uc);

            lbTitle.Text = (uc as BaseUserControl).PageTitle;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UcDashboard());
        }

        private void btnRole_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UcRole1());
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UcUser());
        }

        private void btnProject_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UcProject());
        }
    }
}
