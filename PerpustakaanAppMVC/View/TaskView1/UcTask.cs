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

namespace PerpustakaanAppMVC.View.TaskView1
{
    public partial class UcTask : UserControl
    {
        private DataGridView dgvTasks;
        private TaskController _controller = new TaskController();
        private List<TaskItem> tasks = new List<TaskItem>();

        public UcTask()
        {
            InitializeComponent();
            InitComponents();
            LoadTasks();
        }

        private void InitComponents()
        {
            // Create and configure the DataGridView
            dgvTasks = new DataGridView();
            dgvTasks.Name = "dgvTasks";
            dgvTasks.Dock = DockStyle.Fill;
            dgvTasks.AutoGenerateColumns = false;
            dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTasks.ReadOnly = true;
            dgvTasks.AllowUserToAddRows = false;
            dgvTasks.AllowUserToDeleteRows = false;
            dgvTasks.MultiSelect = false;
            dgvTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add columns
            var idColumn = new DataGridViewTextBoxColumn();
            idColumn.Name = "Id";
            idColumn.HeaderText = "ID";
            idColumn.DataPropertyName = "Id";
            idColumn.FillWeight = 1;
            dgvTasks.Columns.Add(idColumn);

            var titleColumn = new DataGridViewTextBoxColumn();
            titleColumn.Name = "Title";
            titleColumn.HeaderText = "Title";
            titleColumn.DataPropertyName = "Title";
            titleColumn.FillWeight = 3;
            dgvTasks.Columns.Add(titleColumn);

            var statusColumn = new DataGridViewTextBoxColumn();
            statusColumn.Name = "Status";
            statusColumn.HeaderText = "Status";
            statusColumn.DataPropertyName = "StatusText";
            statusColumn.FillWeight = 2;
            dgvTasks.Columns.Add(statusColumn);

            var priorityColumn = new DataGridViewTextBoxColumn();
            priorityColumn.Name = "Priority";
            priorityColumn.HeaderText = "Priority";
            priorityColumn.DataPropertyName = "PriorityText";
            priorityColumn.FillWeight = 2;
            dgvTasks.Columns.Add(priorityColumn);

            var projectColumn = new DataGridViewTextBoxColumn();
            projectColumn.Name = "Project";
            projectColumn.HeaderText = "Project";
            projectColumn.DataPropertyName = "ProjectName";
            projectColumn.FillWeight = 2;
            dgvTasks.Columns.Add(projectColumn);

            var assignedToColumn = new DataGridViewTextBoxColumn();
            assignedToColumn.Name = "AssignedTo";
            assignedToColumn.HeaderText = "Assigned To";
            assignedToColumn.DataPropertyName = "AssignedToName";
            assignedToColumn.FillWeight = 2;
            dgvTasks.Columns.Add(assignedToColumn);

            var deadlineColumn = new DataGridViewTextBoxColumn();
            deadlineColumn.Name = "Deadline";
            deadlineColumn.HeaderText = "Deadline";
            deadlineColumn.DataPropertyName = "Deadline";
            deadlineColumn.FillWeight = 2;
            dgvTasks.Columns.Add(deadlineColumn);

            var descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.Name = "Description";
            descriptionColumn.HeaderText = "Description";
            descriptionColumn.DataPropertyName = "Description";
            descriptionColumn.FillWeight = 4;
            dgvTasks.Columns.Add(descriptionColumn);

            // Add the DataGridView to the control
            this.Controls.Add(dgvTasks);
        }

        private void LoadTasks()
        {
            dgvTasks.Rows.Clear();
            tasks.Clear();

            tasks = _controller.ReadAll();

            foreach (var task in tasks)
            {
                // Add a new row with values
                int rowIndex = dgvTasks.Rows.Add();
                var row = dgvTasks.Rows[rowIndex];

                row.Cells["Id"].Value = task.Id;
                row.Cells["Title"].Value = task.Title;
                row.Cells["Status"].Value = task.StatusText;
                row.Cells["Priority"].Value = task.PriorityText;
                row.Cells["Project"].Value = task.ProjectName;
                row.Cells["AssignedTo"].Value = task.AssignedToName;
                row.Cells["Deadline"].Value = task.Deadline.ToString("dd/MM/yyyy");
                row.Cells["Description"].Value = task.Description;
            }
        }
    }
}
