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

namespace TWEEKLE.View.UserView
{

    public partial class UcUser : BaseUserControl
    {
        public override string PageTitle => "Manajemen User";
        private UserController _controller = new UserController();
        private List<User> users = new List<User>();
        private int _editingIndex = -1;

        private int _userLoginId;
        private string _userRole;
        private List<string> _allowActions;

        public UcUser()
        {
            _userLoginId = Session.SessionManager.GetCurrentUserId();
            _userRole = Session.SessionManager.GetCurrentUserRole();

            InitializeComponent();
            InitDataGridView();
            AllowActionByRole();
        }

        private void InitDataGridView()
        {
            dgvUser.AutoGenerateColumns = false;
            dgvUser.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUser.ReadOnly = true;
            dgvUser.AllowUserToAddRows = false;
            dgvUser.AllowUserToDeleteRows = false;
            dgvUser.MultiSelect = false;
            dgvUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Make columns fill available space


            // **Set background color header**
            dgvUser.EnableHeadersVisualStyles = false;
            dgvUser.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#3C467B"); // warna latar belakang
            dgvUser.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFFFFF"); // warna teks
            dgvUser.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);


            // Add columns
            var noColumn = new DataGridViewTextBoxColumn();
            noColumn.Name = "No";
            noColumn.HeaderText = "No";
            noColumn.DataPropertyName = "No";
            noColumn.FillWeight = 1; // Take small portion of available space
            dgvUser.Columns.Add(noColumn);

            var nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.Name = "Name";
            nameColumn.HeaderText = "Name";
            nameColumn.DataPropertyName = "Name";
            nameColumn.FillWeight = 3; // Take larger portion of available space
            dgvUser.Columns.Add(nameColumn);

            var emailColumn = new DataGridViewTextBoxColumn();
            emailColumn.Name = "Email";
            emailColumn.HeaderText = "Email";
            emailColumn.DataPropertyName = "Email";
            emailColumn.FillWeight = 3; // Take larger portion of available space
            dgvUser.Columns.Add(emailColumn);

            var roleColumn = new DataGridViewTextBoxColumn();
            roleColumn.Name = "Role";
            roleColumn.HeaderText = "Role";
            roleColumn.DataPropertyName = "RoleName";
            roleColumn.FillWeight = 2; // Take medium portion of available space
            dgvUser.Columns.Add(roleColumn);

            var statusColumn = new DataGridViewTextBoxColumn();
            statusColumn.Name = "Status";
            statusColumn.HeaderText = "Status";
            statusColumn.DataPropertyName = "StatusText";
            statusColumn.FillWeight = 4; // Take small portion of available space
            dgvUser.Columns.Add(statusColumn);

            // Add single button column for all actions
            var actionColumn = new DataGridViewTextBoxColumn();
            actionColumn.Name = "Action";
            actionColumn.HeaderText = "Action";
            actionColumn.ReadOnly = true;
            actionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            actionColumn.Width = 160; // Width to accommodate 4 buttons (40x4)
            actionColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUser.Columns.Add(actionColumn);



            // Event handler for cell clicks
            dgvUser.CellClick += DgvUser_CellClick;

            // Add CellFormatting event to handle tooltips
            dgvUser.CellPainting += DgvUser_CellPainting;

            // Set row height for more vertical space
            dgvUser.RowTemplate.Height = 20; // Fixed row height for more space

            LoadUsers();
        }

        private void LoadUsers()
        {
            users = _controller.ReadAll();

            // Display all users initially
            PerformSearch("");
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var frm = new FrmEntryUser("Tambah User", _FormMode.Create, _controller);
            frm.OnCreate += OnCreateEventHandler;
            frm.ShowDialog();
        }

        private void OnCreateEventHandler(User user)
        {
            // Reload the entire user list to ensure RoleName is populated
            LoadUsers();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!canEdit())
            {
                MessageBox.Show("Anda tidak memiliki izin untuk mengedit user.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvUser.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan diubah");
                return;
            }

            var index = dgvUser.SelectedRows[0].Index;
            EditUser(index);
        }

        private void OnUpdateEventHandler(User user)
        {
            // Reload the entire user list to ensure RoleName is populated
            LoadUsers();

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!canDelete())
            {
                MessageBox.Show("Anda tidak memiliki izin untuk menghapus user.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvUser.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus");
                return;
            }

            var index = dgvUser.SelectedRows[0].Index;
            DeleteUser(index);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (!canReset())
            {
                MessageBox.Show("Anda tidak memiliki izin untuk mereset password user.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvUser.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan direset passwordnya");
                return;
            }

            var index = dgvUser.SelectedRows[0].Index;
            ResetPasswordUser(index);
        }

        private void OnResetEventHandler(User user)
        {
            MessageBox.Show($"Password user {user.Name} telah direset.");
        }



        private void DgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvUser.Columns["Action"].Index && e.RowIndex >= 0)
            {
                var cellRect = dgvUser.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point mousePos = dgvUser.PointToClient(Control.MousePosition);
                int relativeX = mousePos.X - cellRect.Left;

                int totalButtons = _allowActions.Count;
                int buttonWidth = cellRect.Width / totalButtons;

                int clickedIndex = relativeX / buttonWidth;
                if (clickedIndex < 0 || clickedIndex >= totalButtons) return;

                string action = _allowActions[clickedIndex];
                var user = users[e.RowIndex];

                switch (action)
                {
                    case "view":
                        ShowUserData(user);
                        break;
                    case "edit":
                        EditUser(e.RowIndex);
                        break;
                    case "delete":
                        DeleteUser(e.RowIndex);
                        break;
                    case "reset":
                        ResetPasswordUser(e.RowIndex);
                        break;
                }
            }
        }

        private void EditUser(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= users.Count)
            {
                MessageBox.Show("Invalid user selection");
                return;
            }

            _editingIndex = rowIndex;
            var user = users[_editingIndex];

            var frm = new FrmEntryUser("Edit User", _FormMode.Update, user, _controller);
            frm.OnUpdate += OnUpdateEventHandler;
            frm.ShowDialog();
        }

        private void DeleteUser(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= users.Count)
            {
                MessageBox.Show("Invalid user selection");
                return;
            }

            var user = users[rowIndex];
            var confirm = MessageBox.Show($"Hapus user {user.Name}?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                int result = _controller.Delete(user.Id);
                if (result > 0)
                {
                    // Reload the entire user list to ensure proper display
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Gagal menghapus user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResetPasswordUser(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= users.Count)
            {
                MessageBox.Show("Invalid user selection");
                return;
            }

            var user = users[rowIndex];
            var frm = new FrmReset("Reset Password", _FormMode.Update, user, _controller);
            frm.OnReset += OnResetEventHandler;
            frm.ShowDialog();
        }

        private void ShowUserData(User user)
        {
            var frm = new FrmEntryUser("View User", _FormMode.View, user, _controller);
            frm.ShowDialog();
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dgvUser.Rows.Count; i++)
            {
                dgvUser.Rows[i].Cells["No"].Value = (i + 1).ToString();
            }
        }

        private void DgvUser_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvUser.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(dgvUser.DefaultCellStyle.BackColor), e.CellBounds);

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
                        case "reset": bgColor = Color.LightYellow; break;
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

        private void UcUser_Load(object sender, EventArgs e)
        {
            lbUser.Text = "User Management";

            // Initialize search textbox
            txtSearch.Text = "Cari nama user...";
            txtSearch.ForeColor = Color.Gray;

            string role = Session.SessionManager.GetCurrentUserRole();
            var allowedRoles = new List<string> { "Admin" }; // Only Admin can manage users

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

        private bool canReset()
        {
            return _userRole == "Admin";
        }

        private void AllowActionByRole()
        {
            _allowActions = new List<string>();

            if (canView()) _allowActions.Add("view");
            if (canEdit()) _allowActions.Add("edit");
            if (canDelete()) _allowActions.Add("delete");
            if (canReset()) _allowActions.Add("reset");
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Cari nama user...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Cari nama user...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Avoid placeholder text
            if (txtSearch.Text == "Cari nama user...") return;

            string searchTerm = txtSearch.Text.ToLower();
            PerformSearch(searchTerm);
        }

        private void PerformSearch(string searchTerm)
        {
            dgvUser.Rows.Clear();

            // Filter users based on search term
            var filteredUsers = users.Where(u =>
                u != null && // Check if user is not null
                (
                    (u.Name != null && u.Name.ToLower().Contains(searchTerm)) ||
                    (u.Email != null && u.Email.ToLower().Contains(searchTerm)) ||
                    (u.RoleName != null && u.RoleName.ToLower().Contains(searchTerm))
                )
            ).ToList(); 

            foreach (var user in filteredUsers)
            {
                if (user != null) // Double check user is not null
                {
                    int rowIndex = dgvUser.Rows.Add();
                    var row = dgvUser.Rows[rowIndex];

                    row.Cells["No"].Value = (rowIndex + 1).ToString();
                    row.Cells["Name"].Value = user.Name ?? "";
                    row.Cells["Email"].Value = user.Email ?? "";
                    row.Cells["Role"].Value = user.RoleName ?? "";
                    row.Cells["Status"].Value = user.Status == 1 ? "Aktif" : "Nonaktif";
                }
            }
        }
    }
}