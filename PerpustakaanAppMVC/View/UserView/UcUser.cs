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
using System.Web.Security;
using System.Windows.Forms;

namespace PerpustakaanAppMVC.View.UserView
{

    public partial class UcUser : BaseUserControl
    {
        public override string PageTitle => "Manajemen User";
        private UserController _controller = new UserController();
        private List<User> users = new List<User>();
        private int _editingIndex = -1;

        public UcUser()
        {
            InitializeComponent();
            InitDataGridView();
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
            dgvUser.Rows.Clear();
            users = _controller.ReadAll();

            foreach (var u in users)
            {
                // Add a new row with values
                int rowIndex = dgvUser.Rows.Add();
                var row = dgvUser.Rows[rowIndex];

                row.Cells["No"].Value = (rowIndex + 1).ToString();
                row.Cells["Name"].Value = u.Name;
                row.Cells["Email"].Value = u.Email;
                row.Cells["Role"].Value = u.RoleName;
                row.Cells["Status"].Value = u.Status == 1 ? "Aktif" : "Nonaktif";
                // The button columns don't need values since we're using UseColumnTextForButtonValue
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var frm = new FrmEntryUser("Tambah User", _controller);
            frm.OnCreate += OnCreateEventHandler;
            frm.ShowDialog();
        }

        private void OnCreateEventHandler(User user)
        {
            users.Add(user);

            // Add a new row with values
            int rowIndex = dgvUser.Rows.Add();
            var row = dgvUser.Rows[rowIndex];

            row.Cells["No"].Value = (rowIndex + 1).ToString();
            row.Cells["Name"].Value = user.Name;
            row.Cells["Email"].Value = user.Email;
            row.Cells["Role"].Value = user.RoleName;
            row.Cells["Status"].Value = user.Status == 1 ? "Aktif" : "Nonaktif";
            // The button columns don't need values since we're using UseColumnTextForButtonValue
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
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
            if (_editingIndex < 0) return;

            users[_editingIndex] = user;

            // Update the specific row in the grid
            var row = dgvUser.Rows[_editingIndex];
            row.Cells["Name"].Value = user.Name;
            row.Cells["Email"].Value = user.Email;
            row.Cells["Role"].Value = user.RoleName;
            row.Cells["Status"].Value = user.Status == 1 ? "Aktif" : "Nonaktif";
            // The button columns don't need to be updated since they use static text

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
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
            // Check if the click was on the "Action" column
            if (e.ColumnIndex == dgvUser.Columns["Action"].Index && e.RowIndex >= 0)
            {
                // Calculate which button was clicked based on mouse position
                var cellRect = dgvUser.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int totalButtons = 4; // View, Edit, Delete, Reset
                int buttonWidth = cellRect.Width / totalButtons; // Divide cell width by number of buttons

                // Get mouse position relative to the cell
                Point mousePos = dgvUser.PointToClient(Control.MousePosition);

                // Calculate which button region was clicked
                int relativeX = mousePos.X - cellRect.Left;

                if (relativeX < buttonWidth) // View button
                {
                    // Get the corresponding user data
                    var user = users[e.RowIndex];
                    ShowUserData(user);
                }
                else if (relativeX < buttonWidth * 2) // Edit button
                {
                    // Handle edit button click
                    EditUser(e.RowIndex);
                }
                else if (relativeX < buttonWidth * 3) // Delete button
                {
                    // Handle delete button click
                    DeleteUser(e.RowIndex);
                }
                else // Reset Password button
                {
                    // Handle reset password button click
                    ResetPasswordUser(e.RowIndex);
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

            var frm = new FrmEntryUser("Edit User", user, _controller);
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
                    users.RemoveAt(rowIndex);
                    dgvUser.Rows.RemoveAt(rowIndex);

                    // Update row numbers
                    UpdateRowNumbers();
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
            var frm = new FrmReset("Reset Password", user, _controller);
            frm.OnReset += OnResetEventHandler;
            frm.ShowDialog();
        }

        private void ShowUserData(User user)
        {
            // Show user details in a message box
            string userDetails = $"User Details:\n\n" +
                                $"ID: {user.Id}\n" +
                                $"Name: {user.Name}\n" +
                                $"Email: {user.Email}\n" +
                                $"Role: {user.RoleName}\n" +
                                $"Status: {(user.Status == 1 ? "Aktif" : "Nonaktif")}";

            MessageBox.Show(userDetails, "User Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // Define button dimensions
                int totalButtons = 4; // View, Edit, Delete, Reset
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

                // Draw Reset Password button with light yellow background
                Rectangle resetRect = new Rectangle(e.CellBounds.Left + buttonWidth * 3, e.CellBounds.Top, buttonWidth, buttonHeight);
                using (Brush brush = new SolidBrush(Color.LightYellow))
                {
                    e.Graphics.FillRectangle(brush, resetRect);
                }
                ControlPaint.DrawBorder(e.Graphics, resetRect, Color.Gray, ButtonBorderStyle.Solid);
                TextRenderer.DrawText(
                    e.Graphics,
                    "Reset",
                    e.CellStyle.Font,
                    resetRect,
                    Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Handled = true;
            }
        }
    }
}
