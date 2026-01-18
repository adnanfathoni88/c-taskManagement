using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PerpustakaanAppMVC.Controller;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Session;

namespace PerpustakaanAppMVC.View.Dashboard
{
    public partial class UcDashboard : BaseUserControl
    {
        private Label lblUserName;
        private Label lblUserEmail;
        private Label lblUserRole;
        private string _userId;
        private string _roleName;
        private List<Project> projects = new List<Project>();
        private ProjectController _projectController = new ProjectController();
        private TaskController _taskController = new TaskController();

        public override string PageTitle => "Dashboard";

        public UcDashboard()
        {
            // get info user
            _userId = SessionManager.GetCurrentUserId().ToString();
            _roleName = SessionManager.GetCurrentUserRole();

            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            // check login
            if (!SessionManager.IsLoggedIn) { MessageBox.Show("User is not logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            // check isAdmin
            bool isAdmin = _roleName == "Admin";

            // getData
            try
            {
                var countProjects = _projectController.GetTotalProjects(_userId);
                var countTasks = _taskController.GetTotalTasks(_userId);

                lbProyek.Text = countProjects.ToString();
                lbTask.Text = countTasks.ToString();
                lbTask2.Text = countTasks.ToString();


                MessageBox.Show("Total Projects: " + countProjects, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
