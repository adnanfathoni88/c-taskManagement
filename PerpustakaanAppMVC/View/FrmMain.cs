using PerpustakaanAppMVC.View.Dashboard;
using PerpustakaanAppMVC.View.ProjectView;
using PerpustakaanAppMVC.View.RoleView;
using PerpustakaanAppMVC.View.TaskViewMain;
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
        private Button _activeButton = null;

        public FrmMain()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Set initial active button (Dashboard)
            _activeButton = btnDashboard;
            HighlightActiveButton(_activeButton);

            // Add hover event handlers programmatically
            btnDashboard.MouseEnter += Button_MouseEnter;
            btnDashboard.MouseLeave += Button_MouseLeave;
            btnRole.MouseEnter += Button_MouseEnter;
            btnRole.MouseLeave += Button_MouseLeave;
            btnUser.MouseEnter += Button_MouseEnter;
            btnUser.MouseLeave += Button_MouseLeave;
            btnProject.MouseEnter += Button_MouseEnter;
            btnProject.MouseLeave += Button_MouseLeave;
        }

        private void LoadUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            this.panelContent.Controls.Clear();
            this.panelContent.Controls.Add(uc);
        }

        private void HighlightActiveButton(Button button)
        {
            // Reset all buttons to default style
            btnDashboard.BackColor = Color.Transparent;
            btnRole.BackColor = Color.Transparent;
            btnUser.BackColor = Color.Transparent;
            btnProject.BackColor = Color.Transparent;
            //btnTask.BackColor = Color.Transparent;

            // Apply active style to the selected button
            if (button != null)
            {
                // Create a semi-transparent white brush (30% opacity)
                button.BackColor = Color.FromArgb(77, 255, 255, 255); // ARGB: A=77 (30% opacity), RGB=white
            }
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button != _activeButton)
            {
                // Make button slightly more opaque on hover (40% opacity)
                button.BackColor = Color.FromArgb(102, 255, 255, 255); // 40% opacity when hovered
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button != _activeButton)
            {
                // Return to transparent when not hovered (unless it's the active button)
                button.BackColor = Color.Transparent;
            }
            else if (button != null && button == _activeButton)
            {
                // If it's the active button, restore its active state
                button.BackColor = Color.FromArgb(77, 255, 255, 255); // 30% opacity for active
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UcDashboard());
            _activeButton = btnDashboard;
            HighlightActiveButton(_activeButton);
        }

        private void btnRole_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UcRole1());
            _activeButton = btnRole;
            HighlightActiveButton(_activeButton);
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            LoadUserControl(new UcUser());
            _activeButton = btnUser;
            HighlightActiveButton(_activeButton);
        }

        private void btnProject_Click(object sender, EventArgs e)
        {
            var projectControl = new UcProject();
            projectControl.LoadTaskViewRequested += OnLoadTaskViewRequested;
            LoadUserControl(projectControl);
            _activeButton = btnProject;
            HighlightActiveButton(_activeButton);
        }

        private void OnLoadTaskViewRequested(int projectId, string projectName)
        {
            // Load the UcTask control with the specific project
            LoadUserControl(new UcTask(projectId, projectName));
        }
    }
}
