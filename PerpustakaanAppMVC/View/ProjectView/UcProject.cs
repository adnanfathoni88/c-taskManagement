using PerpustakaanAppMVC.Controller;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.View.TaskViewMain;
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
        private int _userLoginId;
        private string _userRole;
        private List<string> _allowActions;

        // Event to notify parent form to load UcTask
        public event Action<int, string> LoadTaskViewRequested;

        public UcProject()
        {
            _userLoginId = Session.SessionManager.GetCurrentUserId();
            _userRole = Session.SessionManager.GetCurrentUserRole();

            InitializeComponent();
            InitDataGridView();
            AllowActionByRole();
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

            // **Set background color header**
            dgvProject.EnableHeadersVisualStyles = false; 
            dgvProject.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#3C467B"); // warna latar belakang
            dgvProject.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFFFFF"); // warna teks
            dgvProject.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);


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

            txtSearch.Text = "Cari nama project...";
            txtSearch.ForeColor = Color.Gray;
        }

        private void LoadProjects()
        {
            dgvProject.Rows.Clear();
            projects = _controller.GetAllProjects(_userRole, _userLoginId.ToString());
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

        private void EditProject(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= projects.Count)
            {
                MessageBox.Show("Invalid project selection");
                return;
            }

            _editingIndex = rowIndex;
            var project = projects[_editingIndex];

            var frm = new FrmEntryProject("Edit Project", _FormMode.Update, project);
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

            // Raise event to notify parent form to load UcTask view
            LoadTaskViewRequested?.Invoke(project.Id, project.Nama);
        }

        private void ShowProjectData(Project project)
        {
            var frm = new FrmEntryProject("View Project", _FormMode.View, project);
            frm.ShowDialog();
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dgvProject.Rows.Count; i++)
            {
                dgvProject.Rows[i].Cells["No"].Value = (i + 1).ToString();
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Cari nama project...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Cari nama project...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            // hindari placeholder text
            if (txtSearch.Text == "Cari nama project...") return;

            string searchTerm = txtSearch.Text.ToLower();
            dgvProject.Rows.Clear();

            // filter
            var filteredProjects = projects.Where(p =>
                p.Nama.ToLower().Contains(searchTerm) ||
                p.Deskripsi.ToLower().Contains(searchTerm) ||
                p.Status.ToLower().Contains(searchTerm)
            ).ToList();

            foreach (var project in filteredProjects)
            {
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

        private void btnTambah_Click_1(object sender, EventArgs e)
        {
            var frm = new FrmEntryProject("Tambah Project", _FormMode.Create);
            frm.OnCreate += OnCreateEventHandler;
            frm.ShowDialog();

        }

        private void UcProject_Load(object sender, EventArgs e)
        {
            string role = Session.SessionManager.GetCurrentUserRole();
            var allowedRoles = new List<string> { "Admin", "Project Manager" };

            if (!allowedRoles.Contains(role))
            {
                btnTambah.Visible = false;
            }
        }


        private bool canEdit()
        {
            return _userRole == "Admin" || _userRole == "Project Manager";
        }

        private bool canDelete()
        {
            return _userRole == "Admin" || _userRole == "Project Manager";
        }

        private bool canView()
        {
            return true;
        }

        private bool canTask()
        {
            return true;
        }

        private void AllowActionByRole()
        {
            _allowActions = new List<string>();

            if (canView()) _allowActions.Add("view");
            if (canEdit()) _allowActions.Add("edit");
            if (canDelete()) _allowActions.Add("delete");
            if (canTask()) _allowActions.Add("task");
        }


        private void DgvProject_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvProject.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(dgvProject.DefaultCellStyle.BackColor), e.CellBounds);

                int totalButtons = _allowActions.Count; // tombol sesuai akses
                if (totalButtons == 0) return; // kalau gak ada akses

                int buttonWidth = e.CellBounds.Width / totalButtons;
                int buttonHeight = e.CellBounds.Height;

                for (int i = 0; i < totalButtons; i++)
                {
                    string action = _allowActions[i];
                    Rectangle rect = new Rectangle(e.CellBounds.Left + i * buttonWidth, e.CellBounds.Top, buttonWidth, buttonHeight);
                    Color bgColor = Color.LightGray;

                    switch (action)
                    {
                        case "view": bgColor = Color.LightBlue; break;
                        case "edit": bgColor = Color.LightGreen; break;
                        case "delete": bgColor = Color.LightCoral; break;
                        case "task": bgColor = Color.LightYellow; break;
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

                e.Handled = true;
            }
        }

        private void DgvProject_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvProject.Columns["Action"].Index && e.RowIndex >= 0)
            {
                var cellRect = dgvProject.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point mousePos = dgvProject.PointToClient(Control.MousePosition);
                int relativeX = mousePos.X - cellRect.Left;

                int totalButtons = _allowActions.Count;
                int buttonWidth = cellRect.Width / totalButtons;

                int clickedIndex = relativeX / buttonWidth;
                if (clickedIndex < 0 || clickedIndex >= totalButtons) return;

                string action = _allowActions[clickedIndex];
                var project = projects[e.RowIndex];

                switch (action)
                {
                    case "view":
                        ShowProjectData(project);
                        break;
                    case "edit":
                        EditProject(e.RowIndex);
                        break;
                    case "delete":
                        DeleteProject(e.RowIndex);
                        break;
                    case "task":
                        ShowTaskModal(e.RowIndex);
                        break;
                }
            }
        }

    }
}