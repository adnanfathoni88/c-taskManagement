using TWEEKLE.Controller;
using TWEEKLE.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TWEEKLE.View.TaskViewMain
{
    public partial class UcTask : BaseUserControl
    {
        private TaskController _controller = new TaskController();
        private List<TaskItem> tasks = new List<TaskItem>();
        private int _editingIndex = -1;
        private int? _projectIdFilter = null;
        private string _projectNameFilter = null;

        private int _userLoginId;
        private string _userRole;
        private List<string> _allowActions;

        public UcTask()
        {
            _userLoginId = Session.SessionManager.GetCurrentUserId();
            _userRole = Session.SessionManager.GetCurrentUserRole();

            InitializeComponent();
            InitDataGridView();
            AllowActionByRole();
        }

        // Constructor to initialize with a specific project
        public UcTask(int projectId, string projectName)
        {
            _userLoginId = Session.SessionManager.GetCurrentUserId();
            _userRole = Session.SessionManager.GetCurrentUserRole();

            InitializeComponent();
            _projectIdFilter = projectId;
            _projectNameFilter = projectName;

            InitDataGridView();
            AllowActionByRole();
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

            // **Set background color header**
            dgvTask.EnableHeadersVisualStyles = false;
            dgvTask.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#3C467B"); // warna latar belakang
            dgvTask.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFFFFF"); // warna teks
            dgvTask.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

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
            if (_projectIdFilter.HasValue)
            {
                tasks = _controller.GetTasksByRole(_userLoginId, _userRole, _projectIdFilter.Value);
            }
            else
            {
                MessageBox.Show("Project Tidak Ada");
            }

            // Store the current search term before reloading
            string currentSearch = txtSearch.Text;
            if (currentSearch == "Cari nama tugas...")
            {
                currentSearch = "";
            }
            else
            {
                currentSearch = currentSearch.ToLower();
            }

            // Display tasks based on current search filter
            PerformSearch(currentSearch);
        }


        private void OnCreateEventHandler(TaskItem task)
        {
            // Reload all tasks to ensure ProjectName and AssignedToName are properly populated
            LoadTasks();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!canEdit())
            {
                MessageBox.Show("Anda tidak memiliki izin untuk mengedit tugas.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            // Reload all tasks to ensure ProjectName and AssignedToName are properly populated
            LoadTasks();

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!canDelete())
            {
                MessageBox.Show("Anda tidak memiliki izin untuk menghapus tugas.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvTask.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus");
                return;
            }

            var index = dgvTask.SelectedRows[0].Index;
            DeleteTask(index);
        }


        private void DgvTask_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvTask.Columns["Action"].Index && e.RowIndex >= 0)
            {
                var cellRect = dgvTask.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point mousePos = dgvTask.PointToClient(Control.MousePosition);
                int relativeX = mousePos.X - cellRect.Left;

                var task = tasks[e.RowIndex];

                if (_allowActions.Count == 0)
                {
                    // Only View Log button is available
                    // Handle view log button click
                    ViewTaskLog(e.RowIndex);
                }
                else
                {
                    int totalButtons = _allowActions.Count + 1; // +1 for View Log
                    int buttonWidth = cellRect.Width / totalButtons;

                    int clickedIndex = relativeX / buttonWidth;

                    if (clickedIndex < 0 || clickedIndex >= totalButtons) return;

                    if (clickedIndex < _allowActions.Count)
                    {
                        // Handle dynamic actions (view, edit, delete)
                        string action = _allowActions[clickedIndex];

                        switch (action)
                        {
                            case "view":
                                ShowTaskData(task);
                                break;
                            case "edit":
                                EditTask(e.RowIndex);
                                break;
                            case "delete":
                                DeleteTask(e.RowIndex);
                                break;
                        }
                    }
                    else if (clickedIndex == _allowActions.Count) // View Log button
                    {
                        // Handle view log button click
                        ViewTaskLog(e.RowIndex);
                    }
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

            var frm = new FrmEntryTask("Edit Tugas", _FormMode.Update, task, _controller);
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
                    // Create a log entry for the task deletion
                    CreateLogEntry(task.Id, "Deleted", $"Task '{task.Title}' deleted by user {_userLoginId}");

                    // Reload all tasks to ensure the list is properly updated
                    LoadTasks();
                }
                else
                {
                    MessageBox.Show("Gagal menghapus tugas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CreateLogEntry(int taskId, string action, string description)
        {
            var logController = new LogController();
            var log = new Model.Entity.Log
            {
                TaskId = taskId,
                Action = action,
                UserId = _userLoginId,
                Description = description,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            logController.Create(log);
        }


        private void ShowTaskData(TaskItem task)
        {
            // Show task details in the entry form in view mode
            var frm = new FrmEntryTask("View Tugas", _FormMode.View, task, _controller);
            frm.ShowDialog();
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

                // Total buttons: allowed actions + View Log (removed the Log button that was for adding logs)
                int totalButtons = _allowActions.Count + 1;

                // If there are no allowed actions, only show View Log button
                if (_allowActions.Count == 0)
                {
                    int buttonWidth = e.CellBounds.Width; // Only View Log button
                    int buttonHeight = e.CellBounds.Height;

                    // Draw View Log button with light cyan background
                    Rectangle viewLogRect = new Rectangle(e.CellBounds.Left, e.CellBounds.Top, buttonWidth, buttonHeight);
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
                }
                else
                {
                    int buttonWidth = e.CellBounds.Width / totalButtons;
                    int buttonHeight = e.CellBounds.Height;

                    // Draw allowed action buttons (view, edit, delete)
                    for (int i = 0; i < _allowActions.Count; i++)
                    {
                        string action = _allowActions[i];
                        Rectangle rect = new Rectangle(e.CellBounds.Left + i * buttonWidth, e.CellBounds.Top, buttonWidth, buttonHeight);
                        Color bgColor = Color.LightGray;

                        switch (action)
                        {
                            case "view": bgColor = Color.LightBlue; break;
                            case "edit": bgColor = Color.LightGreen; break;
                            case "delete": bgColor = Color.LightCoral; break;
                        }

                        using (Brush brush = new SolidBrush(bgColor))
                        {
                            e.Graphics.FillRectangle(brush, rect);
                        }

                        ControlPaint.DrawBorder(e.Graphics, rect, Color.Gray, ButtonBorderStyle.Solid);
                        TextRenderer.DrawText(
                            e.Graphics,
                            action.Substring(0, 1).ToUpper() + action.Substring(1),
                            e.CellStyle.Font,
                            rect,
                            Color.Black,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                        );
                    }

                    // Draw View Log button with light cyan background
                    Rectangle viewLogRect = new Rectangle(e.CellBounds.Left + _allowActions.Count * buttonWidth, e.CellBounds.Top, buttonWidth, buttonHeight);
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
                }

                e.Handled = true;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!canCreate())
            {
                MessageBox.Show("Anda tidak memiliki izin untuk menambah tugas.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var frm = new FrmEntryTask("Tambah Tugas", _FormMode.Create, _controller, _projectIdFilter);
            frm.OnCreate += OnCreateEventHandler;
            frm.ShowDialog();
        }

        private void UcTask_Load(object sender, EventArgs e)
        {
            lbProjectName.Text = "Task - " + _projectNameFilter;

            // Initialize search textbox
            txtSearch.Text = "Cari nama tugas...";
            txtSearch.ForeColor = Color.Gray;

            // Hide add button if user doesn't have create permission
            btnAdd.Visible = canCreate();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Cari nama tugas...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Cari nama tugas...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Avoid placeholder text
            if (txtSearch.Text == "Cari nama tugas...") return;

            string searchTerm = txtSearch.Text.ToLower();
            PerformSearch(searchTerm);
        }

        private void PerformSearch(string searchTerm)
        {
            // Clear all existing rows to prevent duplicates
            dgvTask.Rows.Clear();

            // Filter tasks based on search term with null checks
            var filteredTasks = tasks.Where(t =>
                string.IsNullOrEmpty(searchTerm) || // Show all if search term is empty
                (t.Title != null && t.Title.ToLower().Contains(searchTerm)) ||
                (t.Description != null && t.Description.ToLower().Contains(searchTerm)) ||
                (t.Status != null && t.Status.ToLower().Contains(searchTerm)) ||
                (t.Priority != null && t.Priority.ToLower().Contains(searchTerm)) ||
                (t.ProjectName != null && t.ProjectName.ToLower().Contains(searchTerm)) ||
                (t.AssignedToName != null && t.AssignedToName.ToLower().Contains(searchTerm)) ||
                (t.Deadline != null && t.Deadline.ToString().ToLower().Contains(searchTerm))
            ).ToList();

            // Add filtered tasks to the DataGridView
            foreach (var task in filteredTasks)
            {
                int rowIndex = dgvTask.Rows.Add();
                var row = dgvTask.Rows[rowIndex];

                // Set the row number based on its position in the filtered list
                row.Cells["No"].Value = (rowIndex + 1).ToString();
                row.Cells["Title"].Value = task.Title ?? "";
                row.Cells["Description"].Value = task.Description ?? "";
                row.Cells["Status"].Value = task.Status ?? "";
                row.Cells["Priority"].Value = task.Priority ?? "";
                row.Cells["Project"].Value = task.ProjectName ?? "";
                row.Cells["AssignedTo"].Value = task.AssignedToName ?? "";
                row.Cells["Deadline"].Value = task.Deadline?.ToString() ?? "";
            }

            // Update row numbers to ensure they are sequential after filtering
            UpdateRowNumbers();
        }

        private bool canView()
        {
            return true; // Everyone can view tasks
        }

        private bool canEdit()
        {
            // Only the task owner or admin can edit
            return true;
        }

        private bool canDelete()
        {
            // Only the task owner or admin can delete
            return _userRole == "Admin" || _userRole == "Project Manager";
        }

        private bool canCreate()
        {
            // Users with certain roles can create tasks
            return _userRole == "Admin" || _userRole == "Project Manager";
        }

        private void AllowActionByRole()
        {
            _allowActions = new List<string>();

            if (canView()) _allowActions.Add("view");
            if (canEdit()) _allowActions.Add("edit");
            if (canDelete()) _allowActions.Add("delete");
            // Note: Create action is typically handled by a button outside the grid
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
    }
}
