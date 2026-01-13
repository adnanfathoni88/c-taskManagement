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
using System.Windows.Forms;

namespace PerpustakaanAppMVC.View.TaskView
{
    public delegate void CreateUpdateTaskEventHandler(TaskItem task);

    public partial class FrmEntryTask : Form
    {
        public event CreateUpdateTaskEventHandler OnCreate;
        public event CreateUpdateTaskEventHandler OnUpdate;

        private TaskController _controller;
        private ProjectController _projectController;
        private UserController _userController;

        private bool isNewData = true;
        private TaskItem task;

        public FrmEntryTask()
        {
            InitializeComponent();
        }

        public FrmEntryTask(string title, TaskController controller) : this()
        {
            this.Text = title;
            _controller = controller;
            _projectController = new ProjectController();
            _userController = new UserController();

            LoadProjects();
            LoadUsers();
            LoadPriorities();
            LoadStatuses();
        }

        private void LoadProjects()
        {
            using (var ctx = new DbContext())
            {
                var repo = new ProjectRepository(ctx);
                cmbProject.DataSource = repo.ReadAll();
                cmbProject.DisplayMember = "Nama";
                cmbProject.ValueMember = "Id";
            }
        }

        private void LoadUsers()
        {
            using (var ctx = new DbContext())
            {
                var repo = new UserRepository(ctx);
                cmbAssignedTo.DataSource = repo.ReadAll();
                cmbAssignedTo.DisplayMember = "Name";
                cmbAssignedTo.ValueMember = "Id";
            }
        }

        private void LoadPriorities()
        {
            // Priority: 1-Low, 2-Medium, 3-High
            cmbPriority.SelectedIndex = 0; // Default to Low
        }

        private void LoadStatuses()
        {
            // Status: 1-Pending, 2-In Progress, 3-Completed
            cmbStatus.SelectedIndex = 0; // Default to Pending
        }

        public FrmEntryTask(string title, TaskItem obj, TaskController controller) : this()
        {
            this.Text = title;
            _controller = controller;
            _projectController = new ProjectController();
            _userController = new UserController();

            isNewData = false;
            task = obj;

            txtTitle.Text = task.Title;
            txtDescription.Text = task.Description;
            dtpDeadline.Value = task.Deadline;

            // Set priority
            switch(task.Priority)
            {
                case 1: cmbPriority.SelectedIndex = 0; break; // Low
                case 2: cmbPriority.SelectedIndex = 1; break; // Medium
                case 3: cmbPriority.SelectedIndex = 2; break; // High
            }

            // Set status
            switch(task.Status)
            {
                case 1: cmbStatus.SelectedIndex = 0; break; // Pending
                case 2: cmbStatus.SelectedIndex = 1; break; // In Progress
                case 3: cmbStatus.SelectedIndex = 2; break; // Completed
            }

            LoadProjects();
            LoadUsers();

            // Set selected values after loading data
            cmbProject.SelectedValue = task.ProjectId;
            cmbAssignedTo.SelectedValue = task.AssignedTo;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                // Validasi input
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Title is required", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTitle.Focus();
                    return;
                }

                if (cmbProject.SelectedValue == null)
                {
                    MessageBox.Show("Project is required", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbProject.Focus();
                    return;
                }

                if (cmbAssignedTo.SelectedValue == null)
                {
                    MessageBox.Show("Assignee is required", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbAssignedTo.Focus();
                    return;
                }

                // Buat objek task baru atau ambil dari parameter
                TaskItem newTask = isNewData ? new TaskItem() : task;

                // Set properti
                newTask.Title = txtTitle.Text;
                newTask.Description = txtDescription.Text;
                newTask.Deadline = dtpDeadline.Value;
                newTask.ProjectId = (int)cmbProject.SelectedValue;
                newTask.AssignedTo = (int)cmbAssignedTo.SelectedValue;

                // Set priority based on selected index
                switch(cmbPriority.SelectedIndex)
                {
                    case 0: newTask.Priority = 1; break; // Low
                    case 1: newTask.Priority = 2; break; // Medium
                    case 2: newTask.Priority = 3; break; // High
                }

                // Set status based on selected index
                switch(cmbStatus.SelectedIndex)
                {
                    case 0: newTask.Status = 1; break; // Pending
                    case 1: newTask.Status = 2; break; // In Progress
                    case 2: newTask.Status = 3; break; // Completed
                }

                if (isNewData)
                {
                    // Tambah data baru
                    int result = _controller.Create(newTask);
                    if (result > 0)
                    {
                        OnCreate?.Invoke(newTask);
                        MessageBox.Show("Task added successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add task", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Update data
                    newTask.Id = task.Id;
                    int result = _controller.Update(newTask);
                    if (result > 0)
                    {
                        OnUpdate?.Invoke(newTask);
                        MessageBox.Show("Task updated successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update task", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}