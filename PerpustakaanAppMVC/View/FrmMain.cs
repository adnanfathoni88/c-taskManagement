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
        private const int SIDEBAR_EXPANDED_WIDTH = 200;
        private const int SIDEBAR_COLLAPSED_WIDTH = 50;
        private bool isSidebarExpanded = true;

        public FrmMain()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Set initial state for the toggle button
            UpdateToggleButtonAppearance();
        }

        private void LoadUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            this.panelContent.Controls.Clear();
            this.panelContent.Controls.Add(uc);

            lbTitle.Text = (uc as BaseUserControl).PageTitle;
        }

        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            ToggleSidebar();
        }

        private void ToggleSidebar()
        {
            if (isSidebarExpanded)
            {
                // Collapse the sidebar
                panelSidebar.Width = SIDEBAR_COLLAPSED_WIDTH;

                // Show only letters
                btnDashboard.Text = "D";
                btnRole.Text = "R";
                btnUser.Text = "U";
                btnProject.Text = "P";

                // Adjust button sizes and positions
                btnDashboard.Size = new Size(SIDEBAR_COLLAPSED_WIDTH - 6, 40);
                btnRole.Size = new Size(SIDEBAR_COLLAPSED_WIDTH - 6, 40);
                btnUser.Size = new Size(SIDEBAR_COLLAPSED_WIDTH - 6, 40);
                btnProject.Size = new Size(SIDEBAR_COLLAPSED_WIDTH - 6, 40);

                btnDashboard.Location = new Point(3, 44);
                btnRole.Location = new Point(3, 87);
                btnUser.Location = new Point(3, 130);
                btnProject.Location = new Point(3, 173);
            }
            else
            {
                // Expand the sidebar
                panelSidebar.Width = SIDEBAR_EXPANDED_WIDTH;

                // Show full text with letters
                btnDashboard.Text = "D Dashboard";
                btnRole.Text = "R Role";
                btnUser.Text = "U User";
                btnProject.Text = "P Project";

                // Restore button sizes and positions
                btnDashboard.Size = new Size(SIDEBAR_EXPANDED_WIDTH - 6, 43);
                btnRole.Size = new Size(SIDEBAR_EXPANDED_WIDTH - 6, 43);
                btnUser.Size = new Size(SIDEBAR_EXPANDED_WIDTH - 6, 43);
                btnProject.Size = new Size(SIDEBAR_EXPANDED_WIDTH - 6, 43);

                btnDashboard.Location = new Point(3, 44);
                btnRole.Location = new Point(3, 93);
                btnUser.Location = new Point(3, 142);
                btnProject.Location = new Point(3, 191);
            }

            isSidebarExpanded = !isSidebarExpanded;
            UpdateToggleButtonAppearance();
        }

        private void UpdateToggleButtonAppearance()
        {
            if (isSidebarExpanded)
            {
                btnToggleSidebar.Text = "<";
            }
            else
            {
                btnToggleSidebar.Text = ">";
            }
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
