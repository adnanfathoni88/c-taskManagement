using PerpustakaanAppMVC.Controller;
using PerpustakaanAppMVC.Model.Entity;
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
    public partial class UcTask : BaseUserControl
    {
        public override string PageTitle => "Task Management";

        private TaskController _controller = new TaskController();
        private List<TaskItem> tasks = new List<TaskItem>();
        private int _editingIndex = -1;

        public UcTask()
        {
            InitializeComponent();
            InitDataGridView();
        }

        private void InitDataGridView()
        {
            dgvTask.AutoGenerateColumns = false;
            dgvTask.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTask.ReadOnly = true;
            dgvTask.AllowUserToAddRows = false;
            dgvTask.AllowUserToDeleteRows = false;
            dgvTask.MultiSelect = false;
            dgvTask.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add columns
            var noColumn = new DataGridViewTextBoxColumn();
            noColumn.Name = "No";
            noColumn.HeaderText = "No";
            noColumn.DataPropertyName = "No";
            noColumn.FillWeight = 1;
            dgvTask.Columns.Add(noColumn);

            var titleColumn = new DataGridViewTextBoxColumn();
            titleColumn.Name = "Title";
            titleColumn.HeaderText = "Title";
            titleColumn.DataPropertyName = "Title";
            titleColumn.FillWeight = 3;
            dgvTask.Columns.Add(titleColumn);

            var priorityColumn = new DataGridViewTextBoxColumn();
            priorityColumn.Name = "Priority";
            priorityColumn.HeaderText = "Priority";
            priorityColumn.DataPropertyName = "PriorityText";
            priorityColumn.FillWeight = 2;
            dgvTask.Columns.Add(priorityColumn);

            var statusColumn = new DataGridViewTextBoxColumn();
            statusColumn.Name = "Status";
            statusColumn.HeaderText = "Status";
            statusColumn.DataPropertyName = "StatusText";
            statusColumn.FillWeight = 2;
            dgvTask.Columns.Add(statusColumn);

            var projectColumn = new DataGridViewTextBoxColumn();
            projectColumn.Name = "Project";
            projectColumn.HeaderText = "Project";
            projectColumn.DataPropertyName = "ProjectName";
            projectColumn.FillWeight = 2;
            dgvTask.Columns.Add(projectColumn);

            var assignedToColumn = new DataGridViewTextBoxColumn();
            assignedToColumn.Name = "AssignedTo";
            assignedToColumn.HeaderText = "Assigned To";
            assignedToColumn.DataPropertyName = "AssignedToName";
            assignedToColumn.FillWeight = 2;
            dgvTask.Columns.Add(assignedToColumn);

            var deadlineColumn = new DataGridViewTextBoxColumn();
            deadlineColumn.Name = "Deadline";
            deadlineColumn.HeaderText = "Deadline";
            deadlineColumn.DataPropertyName = "Deadline";
            deadlineColumn.FillWeight = 2;
            dgvTask.Columns.Add(deadlineColumn);

            // Add button column for View action
            var viewColumn = new DataGridViewButtonColumn();
            viewColumn.Name = "View";
            viewColumn.HeaderText = "";
            viewColumn.Text = "View";
            viewColumn.UseColumnTextForButtonValue = true;
            viewColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            viewColumn.Width = 40;
            viewColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTask.Columns.Add(viewColumn);

            // Add button column for Edit action
            var editColumn = new DataGridViewButtonColumn();
            editColumn.Name = "Edit";
            editColumn.HeaderText = "";
            editColumn.Text = "Edit";
            editColumn.UseColumnTextForButtonValue = true;
            editColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            editColumn.Width = 40;
            editColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTask.Columns.Add(editColumn);

            // Add button column for Delete action
            var deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.Name = "Delete";
            deleteColumn.HeaderText = "";
            deleteColumn.Text = "Delete";
            deleteColumn.UseColumnTextForButtonValue = true;
            deleteColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            deleteColumn.Width = 40;
            deleteColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTask.Columns.Add(deleteColumn);

            // Event handler for cell clicks
            dgvTask.CellClick += DgvTask_CellClick;

            // Set row height for more vertical space
            dgvTask.RowTemplate.Height = 20;

            LoadTasks();
        }

        private void LoadTasks()
        {
            dgvTask.Rows.Clear();
            tasks = _controller.ReadAll();

            foreach (var t in tasks)
            {
                // Add a new row with values
                int rowIndex = dgvTask.Rows.Add();
                var row = dgvTask.Rows[rowIndex];

                row.Cells["No"].Value = (rowIndex + 1).ToString();
                row.Cells["Title"].Value = t.Title;
                row.Cells["Priority"].Value = t.PriorityText;
                row.Cells["Status"].Value = t.StatusText;
                row.Cells["Project"].Value = t.ProjectName;
                row.Cells["AssignedTo"].Value = t.AssignedToName;
                row.Cells["Deadline"].Value = t.Deadline.ToString("dd/MM/yyyy");
                // The button columns don't need values since we're using UseColumnTextForButtonValue
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var frm = new FrmEntryTask("Add Task", _controller);
            frm.OnCreate += OnCreateEventHandler;
            frm.ShowDialog();
        }

        private void OnCreateEventHandler(TaskItem task)
        {
            tasks.Add(task);

            // Add a new row with values
            int rowIndex = dgvTask.Rows.Add();
            var row = dgvTask.Rows[rowIndex];

            row.Cells["No"].Value = (rowIndex + 1).ToString();
            row.Cells["Title"].Value = task.Title;
            row.Cells["Priority"].Value = task.PriorityText;
            row.Cells["Status"].Value = task.StatusText;
            row.Cells["Project"].Value = task.ProjectName;
            row.Cells["AssignedTo"].Value = task.AssignedToName;
            row.Cells["Deadline"].Value = task.Deadline.ToString("dd/MM/yyyy");
            // The button columns don't need values since we're using UseColumnTextForButtonValue
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTask.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the data to be edited");
                return;
            }

            var index = dgvTask.SelectedRows[0].Index;
            EditTask(index);
        }

        private void OnUpdateEventHandler(TaskItem task)
        {
            if (_editingIndex < 0) return;

            tasks[_editingIndex] = task;

            // Update the specific row in the grid
            var row = dgvTask.Rows[_editingIndex];
            row.Cells["Title"].Value = task.Title;
            row.Cells["Priority"].Value = task.PriorityText;
            row.Cells["Status"].Value = task.StatusText;
            row.Cells["Project"].Value = task.ProjectName;
            row.Cells["AssignedTo"].Value = task.AssignedToName;
            row.Cells["Deadline"].Value = task.Deadline.ToString("dd/MM/yyyy");
            // The button columns don't need to be updated since they use static text

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvTask.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the data to be deleted");
                return;
            }

            var index = dgvTask.SelectedRows[0].Index;
            DeleteTask(index);
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTasks();
        }

        private void DgvTask_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click was on the button columns
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                var columnName = dgvTask.Columns[e.ColumnIndex].Name;
                var task = tasks[e.RowIndex];

                switch (columnName)
                {
                    case "View":
                        ShowTaskDetails(task);
                        break;
                    case "Edit":
                        EditTask(e.RowIndex);
                        break;
                    case "Delete":
                        DeleteTask(e.RowIndex);
                        break;
                }
            }
        }

        private void EditTask(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= tasks.Count)
            {
                MessageBox.Show("Invalid task selection");
                return;
            }

            _editingIndex = rowIndex;
            var task = tasks[_editingIndex];

            var frm = new FrmEntryTask("Edit Task", task, _controller);
            frm.OnUpdate += OnUpdateEventHandler;
            frm.ShowDialog();
        }

        private void DeleteTask(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= tasks.Count)
            {
                MessageBox.Show("Invalid task selection");
                return;
            }

            var task = tasks[rowIndex];
            var confirm = MessageBox.Show($"Delete task {task.Title}?", "Confirmation", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                int result = _controller.Delete(task.Id);
                if (result > 0)
                {
                    tasks.RemoveAt(rowIndex);
                    dgvTask.Rows.RemoveAt(rowIndex);

                    // Update row numbers
                    UpdateRowNumbers();

                    MessageBox.Show($"Task {task.Title} has been deleted.", "Task Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to delete task", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowTaskDetails(TaskItem task)
        {
            // Show task details in a message box
            string taskDetails = $"Task Details:\n\n" +
                                $"ID: {task.Id}\n" +
                                $"Title: {task.Title}\n" +
                                $"Priority: {task.PriorityText}\n" +
                                $"Status: {task.StatusText}\n" +
                                $"Project: {task.ProjectName}\n" +
                                $"Assigned To: {task.AssignedToName}\n" +
                                $"Deadline: {task.Deadline:dd/MM/yyyy}\n" +
                                $"Description: {task.Description}";

            MessageBox.Show(taskDetails, "Task Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dgvTask.Rows.Count; i++)
            {
                dgvTask.Rows[i].Cells["No"].Value = (i + 1).ToString();
            }
        }
    }
}