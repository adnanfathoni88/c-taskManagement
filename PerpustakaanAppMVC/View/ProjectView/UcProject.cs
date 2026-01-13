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

namespace PerpustakaanAppMVC.View.ProjectView
{
    public partial class UcProject : BaseUserControl
    {
        public override string PageTitle => "Manajemen Project";
        private ProjectController _controller = new ProjectController();
        private List<Project> projects = new List<Project>();
        private int _editingIndex = -1;

        public UcProject()
        {
            InitializeComponent();
            InitDataGridView();
        }

        private void InitDataGridView()
        {
            dgvProject.AutoGenerateColumns = false;
            dgvProject.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProject.ReadOnly = true;
            dgvProject.AllowUserToAddRows = false;
            dgvProject.AllowUserToDeleteRows = false;
            dgvProject.MultiSelect = false;
            dgvProject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add columns
            var noColumn = new DataGridViewTextBoxColumn();
            noColumn.Name = "No";
            noColumn.HeaderText = "No";
            noColumn.DataPropertyName = "No";
            noColumn.FillWeight = 1;
            dgvProject.Columns.Add(noColumn);

            var namaColumn = new DataGridViewTextBoxColumn();
            namaColumn.Name = "Nama";
            namaColumn.HeaderText = "Nama";
            namaColumn.DataPropertyName = "Nama";
            namaColumn.FillWeight = 3;
            dgvProject.Columns.Add(namaColumn);

            var startDateColumn = new DataGridViewTextBoxColumn();
            startDateColumn.Name = "StartDate";
            startDateColumn.HeaderText = "Start Date";
            startDateColumn.DataPropertyName = "StartDate";
            startDateColumn.FillWeight = 2;
            dgvProject.Columns.Add(startDateColumn);

            var endDateColumn = new DataGridViewTextBoxColumn();
            endDateColumn.Name = "EndDate";
            endDateColumn.HeaderText = "End Date";
            endDateColumn.DataPropertyName = "EndDate";
            endDateColumn.FillWeight = 2;
            dgvProject.Columns.Add(endDateColumn);

            var statusColumn = new DataGridViewTextBoxColumn();
            statusColumn.Name = "Status";
            statusColumn.HeaderText = "Status";
            statusColumn.DataPropertyName = "Status";
            statusColumn.FillWeight = 2;
            dgvProject.Columns.Add(statusColumn);

            var deskripsiColumn = new DataGridViewTextBoxColumn();
            deskripsiColumn.Name = "Deskripsi";
            deskripsiColumn.HeaderText = "Deskripsi";
            deskripsiColumn.DataPropertyName = "Deskripsi";
            deskripsiColumn.FillWeight = 4;
            dgvProject.Columns.Add(deskripsiColumn);

            // Add single button column for all actions
            var actionColumn = new DataGridViewTextBoxColumn();
            actionColumn.Name = "Action";
            actionColumn.HeaderText = "Action";
            actionColumn.ReadOnly = true;
            actionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            actionColumn.Width = 200; // Width to accommodate 4 buttons (50x4)
            actionColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProject.Columns.Add(actionColumn);

            // Event handler for cell clicks
            dgvProject.CellClick += DgvProject_CellClick;

            // Add CellPainting event to draw buttons
            dgvProject.CellPainting += DgvProject_CellPainting;

            // Set row height for more vertical space
            dgvProject.RowTemplate.Height = 20;

            LoadProjects();
        }

        private void LoadProjects()
        {
            dgvProject.Rows.Clear();
            projects = _controller.GetAllProjects();

            foreach (var project in projects)
            {
                // Add a new row with values
                int rowIndex = dgvProject.Rows.Add();
                var row = dgvProject.Rows[rowIndex];

                row.Cells["No"].Value = (rowIndex + 1).ToString();
                row.Cells["Nama"].Value = project.Nama;
                row.Cells["StartDate"].Value = project.StartDate.ToShortDateString();
                row.Cells["EndDate"].Value = project.EndDate.ToShortDateString();
                row.Cells["Status"].Value = project.Status;
                row.Cells["Deskripsi"].Value = project.Deskripsi;
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var frm = new FrmEntryProject("Tambah Project");
            frm.OnCreate += OnCreateEventHandler;
            frm.ShowDialog();
        }

        private void OnCreateEventHandler(Project project)
        {
            projects.Add(project);

            // Add a new row with values
            int rowIndex = dgvProject.Rows.Add();
            var row = dgvProject.Rows[rowIndex];

            row.Cells["No"].Value = (rowIndex + 1).ToString();
            row.Cells["Nama"].Value = project.Nama;
            row.Cells["StartDate"].Value = project.StartDate.ToShortDateString();
            row.Cells["EndDate"].Value = project.EndDate.ToShortDateString();
            row.Cells["Status"].Value = project.Status;
            row.Cells["Deskripsi"].Value = project.Deskripsi;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProject.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan diubah");
                return;
            }

            var index = dgvProject.SelectedRows[0].Index;
            EditProject(index);
        }

        private void OnUpdateEventHandler(Project project)
        {
            if (_editingIndex < 0) return;

            projects[_editingIndex] = project;

            // Update the specific row in the grid
            var row = dgvProject.Rows[_editingIndex];
            row.Cells["Nama"].Value = project.Nama;
            row.Cells["StartDate"].Value = project.StartDate.ToShortDateString();
            row.Cells["EndDate"].Value = project.EndDate.ToShortDateString();
            row.Cells["Status"].Value = project.Status;
            row.Cells["Deskripsi"].Value = project.Deskripsi;

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvProject.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus");
                return;
            }

            var index = dgvProject.SelectedRows[0].Index;
            DeleteProject(index);
        }

        private void DgvProject_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click was on the "Action" column
            if (e.ColumnIndex == dgvProject.Columns["Action"].Index && e.RowIndex >= 0)
            {
                // Calculate which button was clicked based on mouse position
                var cellRect = dgvProject.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int totalButtons = 4; // View, Edit, Delete, Task
                int buttonWidth = cellRect.Width / totalButtons; // Divide cell width by number of buttons

                // Get mouse position relative to the cell
                Point mousePos = dgvProject.PointToClient(Control.MousePosition);

                // Calculate which button region was clicked
                int relativeX = mousePos.X - cellRect.Left;

                if (relativeX < buttonWidth) // View button
                {
                    // Get the corresponding project data
                    var project = projects[e.RowIndex];
                    ShowProjectData(project);
                }
                else if (relativeX < buttonWidth * 2) // Edit button
                {
                    // Handle edit button click
                    EditProject(e.RowIndex);
                }
                else if (relativeX < buttonWidth * 3) // Delete button
                {
                    // Handle delete button click
                    DeleteProject(e.RowIndex);
                }
                else // Task button
                {
                    // Handle task button click
                    ShowTaskModal(e.RowIndex);
                }
            }
        }

        private void EditProject(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= projects.Count)
            {
                MessageBox.Show("Invalid project selection");
                return;
            }

            _editingIndex = rowIndex;
            var project = projects[_editingIndex];

            var frm = new FrmEntryProject("Edit Project", project);
            frm.OnUpdate += OnUpdateEventHandler;
            frm.ShowDialog();
        }

        private void DeleteProject(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= projects.Count)
            {
                MessageBox.Show("Invalid project selection");
                return;
            }

            var project = projects[rowIndex];
            var confirm = MessageBox.Show($"Hapus project {project.Nama}?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                int result = _controller.Delete(project.Id);
                if (result > 0)
                {
                    projects.RemoveAt(rowIndex);
                    dgvProject.Rows.RemoveAt(rowIndex);

                    // Update row numbers
                    UpdateRowNumbers();
                }
                else
                {
                    MessageBox.Show("Gagal menghapus project", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowTaskModal(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= projects.Count)
            {
                MessageBox.Show("Invalid project selection");
                return;
            }

            var project = projects[rowIndex];

            // Show a simple message box as a placeholder for the task modal
            // In a real implementation, you would create a task management form here
            string taskInfo = $"Task Management for Project: {project.Nama}\n\n" +
                              $"This would open a task management interface for the selected project.\n" +
                              $"Project ID: {project.Id}\n" +
                              $"Project Name: {project.Nama}\n" +
                              $"Start Date: {project.StartDate}\n" +
                              $"End Date: {project.EndDate}\n" +
                              $"Status: {project.Status}";

            MessageBox.Show(taskInfo, "Task Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowProjectData(Project project)
        {
            // Show project details in a message box
            string projectDetails = $"Project Details:\n\n" +
                                    $"ID: {project.Id}\n" +
                                    $"Name: {project.Nama}\n" +
                                    $"Start Date: {project.StartDate}\n" +
                                    $"End Date: {project.EndDate}\n" +
                                    $"Status: {project.Status}\n" +
                                    $"Description: {project.Deskripsi}";

            MessageBox.Show(projectDetails, "Project Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dgvProject.Rows.Count; i++)
            {
                dgvProject.Rows[i].Cells["No"].Value = (i + 1).ToString();
            }
        }

        private void DgvProject_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvProject.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(dgvProject.DefaultCellStyle.BackColor), e.CellBounds);

                // Define button dimensions
                int totalButtons = 4; // View, Edit, Delete, Task
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

                // Draw Task button with light yellow background
                Rectangle taskRect = new Rectangle(e.CellBounds.Left + buttonWidth * 3, e.CellBounds.Top, buttonWidth, buttonHeight);
                using (Brush brush = new SolidBrush(Color.LightYellow))
                {
                    e.Graphics.FillRectangle(brush, taskRect);
                }
                ControlPaint.DrawBorder(e.Graphics, taskRect, Color.Gray, ButtonBorderStyle.Solid);
                TextRenderer.DrawText(
                    e.Graphics,
                    "Task",
                    e.CellStyle.Font,
                    taskRect,
                    Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Handled = true;
            }
        }
    }
}
