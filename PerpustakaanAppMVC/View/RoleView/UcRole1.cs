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
using System.Web.Security;
using System.Windows.Forms;

namespace TWEEKLE.View.RoleView
{
    public partial class UcRole1 : UserControl
    {
        private RoleController _controller = new RoleController();
        private List<Role> roles = new List<Role>();
        private int _editingIndex = -1;

        private int _userLoginId;
        private string _userRole;
        private List<string> _allowActions;

        public UcRole1()
        {
            _userLoginId = Session.SessionManager.GetCurrentUserId();
            _userRole = Session.SessionManager.GetCurrentUserRole();

            InitializeComponent();
            InitDataGridView();
            AllowActionByRole();
        }

        private void InitDataGridView()
        {
            dgvRole.AutoGenerateColumns = false;
            dgvRole.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRole.ReadOnly = true;
            dgvRole.AllowUserToAddRows = false;
            dgvRole.AllowUserToDeleteRows = false;
            dgvRole.MultiSelect = false;
            dgvRole.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Make columns fill available space

            // **Set background color header**
            dgvRole.EnableHeadersVisualStyles = false;
            dgvRole.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#3C467B"); // warna latar belakang
            dgvRole.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFFFFF"); // warna teks
            dgvRole.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);


            // Add columns
            var noColumn = new DataGridViewTextBoxColumn();
            noColumn.Name = "No";
            noColumn.HeaderText = "No";
            // Don't set DataPropertyName, we'll set the value manually
            noColumn.FillWeight = 1; // Take small portion of available space
            dgvRole.Columns.Add(noColumn);

            var nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.Name = "Name";
            nameColumn.HeaderText = "Name";
            nameColumn.DataPropertyName = "Name";
            nameColumn.FillWeight = 4; // Take larger portion of available space
            dgvRole.Columns.Add(nameColumn);

            // Add single button column for all actions
            var actionColumn = new DataGridViewTextBoxColumn();
            actionColumn.Name = "Action";
            actionColumn.HeaderText = "Action";
            actionColumn.ReadOnly = true;
            actionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            actionColumn.Width = 120; // Width to accommodate 3 buttons (40x3)
            actionColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRole.Columns.Add(actionColumn);

            // Event handler for cell clicks
            dgvRole.CellClick += DgvRole_CellClick;

            // Add CellPainting event to draw buttons
            dgvRole.CellPainting += DgvRole_CellPainting;

            // Set row height for more vertical space
            dgvRole.RowTemplate.Height = 20; // Fixed row height for more space

            LoadRoles();
        }

        private void LoadRoles()
        {
            roles = _controller.ReadAll();

            // Display all roles initially
            PerformSearch("");
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var frm = new FrmEntryRole("Tambah Role", _FormMode.Create, _controller);
            frm.OnCreate += OnCreateEventHandler;
            frm.ShowDialog();
        }

        private void OnCreateEventHandler(Role role)
        {
            // Reload the entire role list to ensure proper display
            LoadRoles();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!canEdit())
            {
                MessageBox.Show("Anda tidak memiliki izin untuk mengedit role.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvRole.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan diubah");
                return;
            }

            var index = dgvRole.SelectedRows[0].Index;
            EditRole(index);
        }

        private void OnUpdateEventHandler(Role role)
        {
            // Reload the entire role list to ensure proper display
            LoadRoles();

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!canDelete())
            {
                MessageBox.Show("Anda tidak memiliki izin untuk menghapus role.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvRole.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus");
                return;
            }

            var index = dgvRole.SelectedRows[0].Index;
            DeleteRole(index);
        }

        private void DgvRole_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvRole.Columns["Action"].Index && e.RowIndex >= 0)
            {
                var cellRect = dgvRole.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point mousePos = dgvRole.PointToClient(Control.MousePosition);
                int relativeX = mousePos.X - cellRect.Left;

                int totalButtons = _allowActions.Count;
                int buttonWidth = cellRect.Width / totalButtons;

                int clickedIndex = relativeX / buttonWidth;
                if (clickedIndex < 0 || clickedIndex >= totalButtons) return;

                string action = _allowActions[clickedIndex];
                var role = roles[e.RowIndex];

                switch (action)
                {
                    case "view":
                        ShowRoleData(role);
                        break;
                    case "edit":
                        EditRole(e.RowIndex);
                        break;
                    case "delete":
                        DeleteRole(e.RowIndex);
                        break;
                }
            }
        }

        private void EditRole(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= roles.Count)
            {
                MessageBox.Show("Invalid role selection");
                return;
            }

            _editingIndex = rowIndex;
            var role = roles[_editingIndex];

            var frm = new FrmEntryRole("Edit Role", _FormMode.Update, role, _controller);
            frm.OnUpdate += OnUpdateEventHandler;
            frm.ShowDialog();
        }

        private void DeleteRole(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= roles.Count)
            {
                MessageBox.Show("Invalid role selection");
                return;
            }

            var role = roles[rowIndex];

            // Check if the role is in use by any user
            if (_controller.IsRoleInUse(role.Id))
            {
                MessageBox.Show($"Tidak dapat menghapus role '{role.Name}' karena sedang digunakan oleh satu atau lebih pengguna.",
                    "Tidak Dapat Dihapus", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Hapus role {role.Name}?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    int result = _controller.Delete(role.Id);
                    if (result > 0)
                    {
                        // Reload the entire role list to ensure proper display
                        LoadRoles();
                        MessageBox.Show($"Role '{role.Name}' berhasil dihapus.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Gagal menghapus role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi kesalahan saat menghapus role: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowRoleData(Role role)
        {
            var frm = new FrmEntryRole("View Role", _FormMode.View, role, _controller);
            frm.ShowDialog();
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dgvRole.Rows.Count; i++)
            {
                dgvRole.Rows[i].Cells["No"].Value = (i + 1).ToString();
            }
        }

        private void DgvRole_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvRole.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(dgvRole.DefaultCellStyle.BackColor), e.CellBounds);

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

        private void UcRole1_Load(object sender, EventArgs e)
        {
            lbRole.Text = "Role Management";

            // Initialize search textbox
            txtSearch.Text = "Cari nama role...";
            txtSearch.ForeColor = Color.Gray;

            string role = Session.SessionManager.GetCurrentUserRole();
            var allowedRoles = new List<string> { "Admin" }; // Only Admin can manage roles

            if (!allowedRoles.Contains(role))
            {
                btnTambah.Visible = false;
            }
        }

        private bool canEdit()
        {
            return _userRole == "Admin";
        }

        private bool canDelete()
        {
            return _userRole == "Admin";
        }

        private bool canView()
        {
            return true;
        }

        private void AllowActionByRole()
        {
            _allowActions = new List<string>();

            if (canView()) _allowActions.Add("view");
            if (canEdit()) _allowActions.Add("edit");
            if (canDelete()) _allowActions.Add("delete");
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Cari nama role...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Cari nama role...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Avoid placeholder text
            if (txtSearch.Text == "Cari nama role...") return;

            string searchTerm = txtSearch.Text.ToLower();
            PerformSearch(searchTerm);
        }

        private void PerformSearch(string searchTerm)
        {
            dgvRole.Rows.Clear();

            // Filter roles based on search term
            var filteredRoles = roles.Where(r =>
                r.Name.ToLower().Contains(searchTerm)
            ).ToList();

            foreach (var role in filteredRoles)
            {
                int rowIndex = dgvRole.Rows.Add();
                var row = dgvRole.Rows[rowIndex];

                row.Cells["No"].Value = (rowIndex + 1).ToString();
                row.Cells["Name"].Value = role.Name;
            }
        }
    }
}