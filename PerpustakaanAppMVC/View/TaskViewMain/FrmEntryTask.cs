using PerpustakaanAppMVC.Controller;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerpustakaanAppMVC.View.TaskViewMain
{

    public enum _FormMode
    {
        Create,
        Update,
        View
    }

    public partial class FrmEntryTask : Form
    {
        private TaskController _controller = new TaskController();
        private _FormMode _formMode;
        private TaskItem _currentTask;
        private int? _projectIdFilter;
        private string _roleName;

        // Event for handling create/update operations
        public event Action<TaskItem> OnCreate;
        public event Action<TaskItem> OnUpdate;

        public FrmEntryTask(string formCaption, _FormMode formMode, TaskController controller, int? projectIdFilter = null)
        {
            InitializeComponent();
            _formMode = formMode;
            _controller = controller;
            _projectIdFilter = projectIdFilter;
            this.Text = formCaption;

            // get current user role
            _roleName = SessionManager.GetCurrentUserRole();

            LoadProjectsAndUsers();

            // Set default values for new task
            cmbStatus.SelectedIndex = 0; // "Pending"
            cmbPriority.SelectedIndex = 1; // "Medium"


            if (_projectIdFilter.HasValue)
            {
                // Find and select the project by ID
                for (int i = 0; i < cmbProject.Items.Count; i++)
                {
                    var project = cmbProject.Items[i] as Model.Entity.Project;
                    if (project != null && project.Id == _projectIdFilter.Value)
                    {
                        cmbProject.SelectedIndex = i;
                        break;
                    }
                }
                cmbProject.Enabled = false; // Disable if it's filtered by project
            }

            SetFormMode();
        }

        public FrmEntryTask(string formCaption, _FormMode formMode, TaskItem task, TaskController controller) : this(formCaption, formMode, controller)
        {
            _currentTask = task;
            FillFormData();
            SetFormMode();
        }

        private void SetFormMode()
        {
            switch (_formMode)
            {
                case _FormMode.Create:
                    btnSimpan.Text = "Tambah";
                    lbForm.Text = "Add Task";
                    break;
                case _FormMode.Update:
                    btnSimpan.Text = "Ubah";
                    lbForm.Text = "Edit Task";
                    cmbProject.Enabled = false;

                    // jika bukan admin dan project manager, hanya bisa ubah status
                    if (_roleName != "Admin" && _roleName != "Project Manager")
                    {
                        txtTitle.ReadOnly = true;
                        txtDescription.ReadOnly = true;
                        cmbPriority.Enabled = false;
                        cmbAssignedTo.Enabled = false;
                        dtpDeadline.Enabled = false;

                        // hide priority label
                        lbPriority.Visible = false;
                        cmbPriority.Visible = false;
                        lbAssigned.Visible = false;
                        cmbAssignedTo.Visible = false;
                        lbProject.Visible = false;
                        cmbProject.Visible = false;
                        lbDeadline.Visible = false;
                        dtpDeadline.Visible = false;
                    }

                    break;
                case _FormMode.View:
                    btnSimpan.Visible = false;
                    lbForm.Text = "View Task";

                    // Disable all input controls
                    txtTitle.ReadOnly = true;
                    txtDescription.ReadOnly = true;
                    cmbStatus.Enabled = false;
                    cmbPriority.Enabled = false;
                    cmbProject.Enabled = false;
                    cmbAssignedTo.Enabled = false;
                    dtpDeadline.Enabled = false;
                    break;
            }
        }

        private void LoadProjectsAndUsers()
        {
            // Load projects
            var projectController = new ProjectController();
            var projects = projectController.GetAllProjects("admin"); // test
            cmbProject.DataSource = projects;
            cmbProject.DisplayMember = "Nama";
            cmbProject.ValueMember = "Id";
            cmbProject.SelectedIndex = -1;

            // Load users
            var userController = new UserController();
            var users = userController.ReadAll();
            cmbAssignedTo.DataSource = users;
            cmbAssignedTo.DisplayMember = "Name";
            cmbAssignedTo.ValueMember = "Id";
            cmbAssignedTo.SelectedIndex = -1;
        }

        private void FillFormData()
        {
            txtTitle.Text = _currentTask.Title;
            txtDescription.Text = _currentTask.Description;
            cmbStatus.SelectedItem = _currentTask.Status;
            cmbPriority.SelectedItem = _currentTask.Priority;
            cmbProject.SelectedValue = _currentTask.ProjectId;
            cmbAssignedTo.SelectedValue = _currentTask.AssignedTo;
            if (!string.IsNullOrEmpty(_currentTask.Deadline))
            {
                dtpDeadline.Value = DateTime.Parse(_currentTask.Deadline);
            }
        }

        private TaskItem CollectFormData()
        {
            var task = new TaskItem
            {
                Title = txtTitle.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Status = cmbStatus.SelectedItem?.ToString() ?? "Pending",
                Priority = cmbPriority.SelectedItem?.ToString() ?? "Medium",
                ProjectId = cmbProject.SelectedValue as int?,
                AssignedTo = cmbAssignedTo.SelectedValue as int?,
                Deadline = dtpDeadline.Value.ToString("yyyy-MM-dd")
            };

            // Set ID only for updates
            if (_currentTask != null)
            {
                task.Id = _currentTask.Id;
            }

            return task;
        }

        private void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                throw new ArgumentException("Title harus diisi");
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                throw new ArgumentException("Status harus dipilih");
            }

            if (cmbPriority.SelectedIndex == -1)
            {
                throw new ArgumentException("Priority harus dipilih");
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateInput();
                var task = CollectFormData();

                if (_formMode == _FormMode.Create)
                {
                    int newId = _controller.Create(task);
                    if (newId > 0)
                    {
                        task.Id = newId;

                        // Create a log entry for the task creation
                        CreateLogEntry(newId, "Created", "Task created successfully");

                        OnCreate?.Invoke(task);
                        MessageBox.Show("Data tugas berhasil ditambahkan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menambahkan data tugas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (_formMode == _FormMode.Update && _currentTask != null)
                {
                    int result = _controller.Update(task);
                    if (result > 0)
                    {
                        // Create a log entry for the task update
                        CreateLogEntry(task.Id,task.Status, "Task status updated to " + task.Status);

                        OnUpdate?.Invoke(task);
                        MessageBox.Show("Data tugas berhasil diubah", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Gagal mengubah data tugas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateLogEntry(int taskId, string action, string description)
        {
            var logController = new LogController();
            var log = new Model.Entity.Log
            {
                TaskId = taskId,
                Action = action,
                UserId = Session.SessionManager.GetCurrentUserId(), // Assuming session manager exists
                Description = description,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            logController.Create(log);
        }

    }
}