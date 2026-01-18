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
        private int _userId;
        private string _roleName;
        private List<Project> projects = new List<Project>();
        private ProjectController _projectController = new ProjectController();
        private TaskController _taskController = new TaskController();

        public override string PageTitle => "Dashboard";

        public UcDashboard()
        {
            // get info user
            _userId = SessionManager.GetCurrentUserId();
            _roleName = SessionManager.GetCurrentUserRole();

            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (!SessionManager.IsLoggedIn) { MessageBox.Show("User is not logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            try
            {
                // total projects
                var countProjects = _projectController.GetTotalProjects(_userId, _roleName);
                lbProyek.Text = countProjects.ToString();

                // total tasks
                var countTotalTasks = _taskController.GetTotalTasks(_userId, _roleName);
                lbDetailTask.Text = countTotalTasks.ToString();

                // Get task counts by status based on user role
                var taskCountsByStatus = GetTaskCountsByStatusAndRole(_userId, _roleName);

                // Update the labels
                lbPending.Text = taskCountsByStatus.ContainsKey("Pending") ? taskCountsByStatus["Pending"].ToString() : "0";
                lbProses.Text = taskCountsByStatus.ContainsKey("In Progress") ? taskCountsByStatus["In Progress"].ToString() : "0";
                lbSelesai.Text = taskCountsByStatus.ContainsKey("Completed") ? taskCountsByStatus["Completed"].ToString() : "0";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Dictionary<string, int> GetTaskCountsByStatusAndRole(int userId, string roleName)
        {
            var taskCounts = new Dictionary<string, int>();

            switch (roleName)
            {
                case "Admin":
                    // Admin sees all tasks grouped by status
                    taskCounts = _taskController.GetTaskCountByStatus();
                    break;

                case "Project Manager":
                    taskCounts = _taskController.GetTaskCountByStatusForProjectManager(userId);
                    break;

                default: 
                    taskCounts = _taskController.GetTaskCountByStatus(userId.ToString());
                    break;
            }

            // Ensure all status types are present in the dictionary
            if (!taskCounts.ContainsKey("Pending")) taskCounts["Pending"] = 0;
            if (!taskCounts.ContainsKey("In Progress")) taskCounts["In Progress"] = 0;
            if (!taskCounts.ContainsKey("Completed")) taskCounts["Completed"] = 0;

            return taskCounts;
        }
    }
}
