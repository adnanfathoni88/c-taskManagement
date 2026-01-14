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

namespace PerpustakaanAppMVC.View.TaskViewMain
{
    public partial class UcTask : BaseUserControl
    {
        public override string PageTitle
        {
            get
            {
                return _projectNameFilter != null ?
                    $"Tugas untuk Project: {_projectNameFilter}" :
                    "Manajemen Tugas";
            }
        }
        private TaskController _controller = new TaskController();
        private List<TaskItem> tasks = new List<TaskItem>();
        private int _editingIndex = -1;
        private int? _projectIdFilter = null;
        private string _projectNameFilter = null;

        public UcTask()
        {
            InitializeComponent();
            InitDataGridView();
        }

        // Constructor to initialize with a specific project
        public UcTask(int projectId, string projectName)
        {
            InitializeComponent();
            _projectIdFilter = projectId;
            _projectNameFilter = projectName;
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
            dgvTask.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Make columns fill available space

            // Add columns
            var noColumn = new DataGridViewTextBoxColumn();
            noColumn.Name = "No";
            noColumn.HeaderText = "No";
            noColumn.DataPropertyName = "No";
            noColumn.FillWeight = 1; // Take small portion of available space
            dgvTask.Columns.Add(noColumn);

            var titleColumn = new DataGridViewTextBoxColumn();
            titleColumn.Name = "Title";
            titleColumn.HeaderText = "Title";
            titleColumn.DataPropertyName = "Title";
            titleColumn.FillWeight = 3; // Take larger portion of available space
            dgvTask.Columns.Add(titleColumn);

            var descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.Name = "Description";
            descriptionColumn.HeaderText = "Description";
            descriptionColumn.DataPropertyName = "Description";
            descriptionColumn.FillWeight = 4; // Take larger portion of available space
            dgvTask.Columns.Add(descriptionColumn);

            var statusColumn = new DataGridViewTextBoxColumn();
            statusColumn.Name = "Status";
            statusColumn.HeaderText = "Status";
            statusColumn.DataPropertyName = "Status";
            statusColumn.FillWeight = 2; // Take medium portion of available space
            dgvTask.Columns.Add(statusColumn);

            var priorityColumn = new DataGridViewTextBoxColumn();
            priorityColumn.Name = "Priority";
            priorityColumn.HeaderText = "Priority";
            priorityColumn.DataPropertyName = "Priority";
            priorityColumn.FillWeight = 2; // Take medium portion of available space
            dgvTask.Columns.Add(priorityColumn);

            var projectColumn = new DataGridViewTextBoxColumn();
            projectColumn.Name = "Project";
            projectColumn.HeaderText = "Project";
            projectColumn.DataPropertyName = "ProjectName";
            projectColumn.FillWeight = 2; // Take medium portion of available space
            dgvTask.Columns.Add(projectColumn);

            var assignedToColumn = new DataGridViewTextBoxColumn();
            assignedToColumn.Name = "AssignedTo";
            assignedToColumn.HeaderText = "Assigned To";
            assignedToColumn.DataPropertyName = "AssignedToName";
            assignedToColumn.FillWeight = 2; // Take medium portion of available space
            dgvTask.Columns.Add(assignedToColumn);

            var deadlineColumn = new DataGridViewTextBoxColumn();
            deadlineColumn.Name = "Deadline";
            deadlineColumn.HeaderText = "Deadline";
            deadlineColumn.DataPropertyName = "Deadline";
            deadlineColumn.FillWeight = 2; // Take medium portion of available space
            dgvTask.Columns.Add(deadlineColumn);

            // Add single button column for all actions
            var actionColumn = new DataGridViewTextBoxColumn();
            actionColumn.Name = "Action";
            actionColumn.HeaderText = "Action";
            actionColumn.ReadOnly = true;
            actionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            actionColumn.Width = 200; // Width to accommodate 5 buttons (40x5)
            actionColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTask.Columns.Add(actionColumn);

            // Event handler for cell clicks
            dgvTask.CellClick += DgvTask_CellClick;

            // Add CellFormatting event to handle tooltips
            dgvTask.CellPainting += DgvTask_CellPainting;

            // Set row height for more vertical space
            dgvTask.RowTemplate.Height = 20; // Fixed row height for more space

            LoadTasks();
        }

        private void LoadTasks()
        {
            dgvTask.Rows.Clear();

            if (_projectIdFilter.HasValue)
            {
                // Load tasks for specific project
                tasks = _controller.GetByProjectId(_projectIdFilter.Value);
            }
            else
            {
                // Load all tasks
                tasks = _controller.ReadAll();
            }

            foreach (var t in tasks)
            {
                // Add a new row with values
                int rowIndex = dgvTask.Rows.Add();
                var row = dgvTask.Rows[rowIndex];

                row.Cells["No"].Value = (rowIndex + 1).ToString();
                row.Cells["Title"].Value = t.Title;
                row.Cells["Description"].Value = t.Description;
                row.Cells["Status"].Value = t.Status;
                row.Cells["Priority"].Value = t.Priority;
                row.Cells["Project"].Value = t.ProjectName;
                row.Cells["AssignedTo"].Value = t.AssignedToName;
                row.Cells["Deadline"].Value = t.Deadline;
                // The button columns don't need values since we're using UseColumnTextForButtonValue
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var frm = new FrmEntryTask("Tambah Tugas", _controller, _projectIdFilter);
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
            row.Cells["Description"].Value = task.Description;
            row.Cells["Status"].Value = task.Status;
            row.Cells["Priority"].Value = task.Priority;
            row.Cells["Project"].Value = task.ProjectName;
            row.Cells["AssignedTo"].Value = task.AssignedToName;
            row.Cells["Deadline"].Value = task.Deadline;
            // The button columns don't need values since we're using UseColumnTextForButtonValue
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTask.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan diubah");
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
            row.Cells["Description"].Value = task.Description;
            row.Cells["Status"].Value = task.Status;
            row.Cells["Priority"].Value = task.Priority;
            row.Cells["Project"].Value = task.ProjectName;
            row.Cells["AssignedTo"].Value = task.AssignedToName;
            row.Cells["Deadline"].Value = task.Deadline;
            // The button columns don't need to be updated since they use static text

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvTask.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus");
                return;
            }

            var index = dgvTask.SelectedRows[0].Index;
            DeleteTask(index);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (dgvTask.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan ditambahkan lognya");
                return;
            }

            var index = dgvTask.SelectedRows[0].Index;
            // Show log form
            ShowTaskLog(index);
        }

        private void DgvTask_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click was on the "Action" column
            if (e.ColumnIndex == dgvTask.Columns["Action"].Index && e.RowIndex >= 0)
            {
                // Calculate which button was clicked based on mouse position
                var cellRect = dgvTask.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int totalButtons = 5; // View, Edit, Delete, Log, View Log
                int buttonWidth = cellRect.Width / totalButtons; // Divide cell width by number of buttons

                // Get mouse position relative to the cell
                Point mousePos = dgvTask.PointToClient(Control.MousePosition);

                // Calculate which button region was clicked
                int relativeX = mousePos.X - cellRect.Left;

                if (relativeX < buttonWidth) // View button
                {
                    // Get the corresponding task data
                    var task = tasks[e.RowIndex];
                    ShowTaskData(task);
                }
                else if (relativeX < buttonWidth * 2) // Edit button
                {
                    // Handle edit button click
                    EditTask(e.RowIndex);
                }
                else if (relativeX < buttonWidth * 3) // Delete button
                {
                    // Handle delete button click
                    DeleteTask(e.RowIndex);
                }
                else if (relativeX < buttonWidth * 4) // Log button
                {
                    // Handle log button click
                    ShowTaskLog(e.RowIndex);
                }
                else // View Log button
                {
                    // Handle view log button click
                    ViewTaskLog(e.RowIndex);
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

            var frm = new FrmEntryTask("Edit Tugas", task, _controller);
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
            var confirm = MessageBox.Show($"Hapus tugas {task.Title}?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                int result = _controller.Delete(task.Id);
                if (result > 0)
                {
                    tasks.RemoveAt(rowIndex);
                    dgvTask.Rows.RemoveAt(rowIndex);

                    // Update row numbers
                    UpdateRowNumbers();
                }
                else
                {
                    MessageBox.Show("Gagal menghapus tugas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowTaskLog(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= tasks.Count)
            {
                MessageBox.Show("Invalid task selection");
                return;
            }

            var task = tasks[rowIndex];

            // Open the log entry form for this task
            var frm = new FrmEntryLog("Tambah Log", task.Id);
            frm.OnCreate += (log) => {
                // Optionally refresh or show success message
                MessageBox.Show($"Log berhasil ditambahkan untuk tugas {task.Title}", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            frm.ShowDialog();
        }

        private void ViewTaskLog(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= tasks.Count)
            {
                MessageBox.Show("Invalid task selection");
                return;
            }

            var task = tasks[rowIndex];

            // Open the log viewer form for this task
            var frm = new FrmLogViewer(task.Id, task.Title);
            frm.ShowDialog();
        }

        private void ShowTaskData(TaskItem task)
        {
            // Show task details in a message box
            string taskDetails = $"Task Details:\n\n" +
                                $"ID: {task.Id}\n" +
                                $"Title: {task.Title}\n" +
                                $"Description: {task.Description}\n" +
                                $"Status: {task.Status}\n" +
                                $"Priority: {task.Priority}\n" +
                                $"Project: {task.ProjectName}\n" +
                                $"Assigned To: {task.AssignedToName}\n" +
                                $"Deadline: {task.Deadline}";

            MessageBox.Show(taskDetails, "Task Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dgvTask.Rows.Count; i++)
            {
                dgvTask.Rows[i].Cells["No"].Value = (i + 1).ToString();
            }
        }

        private void DgvTask_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvTask.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(dgvTask.DefaultCellStyle.BackColor), e.CellBounds);

                // Define button dimensions
                int totalButtons = 5; // View, Edit, Delete, Log, View Log
                int buttonWidth = e.CellBounds.Width / totalButtons;
                int buttonHeight = e.CellBounds.Height;

                // Draw View button with light blue background
                Rectangle viewRect = new Rectangle(e.CellBounds.Left, e.CellBounds.Top, buttonWidth, buttonHeight);
                using (Brush brush = new SolidBrush(Color.LightBlue))
                {
                    e.Graphics.FillRectangle(brush, viewRect);
                }
                ControlPaint.DrawBorder(e.Graphics, viewRect, Color.Gray, ButtonBorderStyle.Solid);
                TextRenderer.DrawText(
                    e.Graphics,
                    "View",
                    e.CellStyle.Font,
                    viewRect,
                    Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                // Draw Edit button with light green background
                Rectangle editRect = new Rectangle(e.CellBounds.Left + buttonWidth, e.CellBounds.Top, buttonWidth, buttonHeight);
                using (Brush brush = new SolidBrush(Color.LightGreen))
                {
                    e.Graphics.FillRectangle(brush, editRect);
                }
                ControlPaint.DrawBorder(e.Graphics, editRect, Color.Gray, ButtonBorderStyle.Solid);
                TextRenderer.DrawText(
                    e.Graphics,
                    "Edit",
                    e.CellStyle.Font,
                    editRect,
                    Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                // Draw Delete button with light coral background
                Rectangle deleteRect = new Rectangle(e.CellBounds.Left + buttonWidth * 2, e.CellBounds.Top, buttonWidth, buttonHeight);
                using (Brush brush = new SolidBrush(Color.LightCoral))
                {
                    e.Graphics.FillRectangle(brush, deleteRect);
                }
                ControlPaint.DrawBorder(e.Graphics, deleteRect, Color.Gray, ButtonBorderStyle.Solid);
                TextRenderer.DrawText(
                    e.Graphics,
                    "Delete",
                    e.CellStyle.Font,
                    deleteRect,
                    Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                // Draw Log button with light yellow background
                Rectangle logRect = new Rectangle(e.CellBounds.Left + buttonWidth * 3, e.CellBounds.Top, buttonWidth, buttonHeight);
                using (Brush brush = new SolidBrush(Color.LightYellow))
                {
                    e.Graphics.FillRectangle(brush, logRect);
                }
                ControlPaint.DrawBorder(e.Graphics, logRect, Color.Gray, ButtonBorderStyle.Solid);
                TextRenderer.DrawText(
                    e.Graphics,
                    "Log",
                    e.CellStyle.Font,
                    logRect,
                    Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                // Draw View Log button with light cyan background
                Rectangle viewLogRect = new Rectangle(e.CellBounds.Left + buttonWidth * 4, e.CellBounds.Top, buttonWidth, buttonHeight);
                using (Brush brush = new SolidBrush(Color.LightCyan))
                {
                    e.Graphics.FillRectangle(brush, viewLogRect);
                }
                ControlPaint.DrawBorder(e.Graphics, viewLogRect, Color.Gray, ButtonBorderStyle.Solid);
                TextRenderer.DrawText(
                    e.Graphics,
                    "View Log",
                    e.CellStyle.Font,
                    viewLogRect,
                    Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Handled = true;
            }
        }
    }
}
