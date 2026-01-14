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

namespace PerpustakaanAppMVC.View.RoleView
{
    public partial class UcRole1 : BaseUserControl
    {
        public override string PageTitle => "Manajemen Role";
        private RoleController _controller = new RoleController();
        private List<Role> roles = new List<Role>();
        private int _editingIndex = -1;

        public UcRole1()
        {
            InitializeComponent();
            InitDataGridView();
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

            // Add columns
            var noColumn = new DataGridViewTextBoxColumn();
            noColumn.Name = "No";
            noColumn.HeaderText = "No";
            noColumn.DataPropertyName = "No";
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
            dgvRole.Rows.Clear();
            roles = _controller.ReadAll();

            foreach (var role in roles)
            {
                // Add a new row with values
                int rowIndex = dgvRole.Rows.Add();
                var row = dgvRole.Rows[rowIndex];

                row.Cells["No"].Value = (rowIndex + 1).ToString();
                row.Cells["Name"].Value = role.Name;
                // The button columns don't need values since we're using UseColumnTextForButtonValue
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var frm = new FrmEntryRole("Tambah Role", _controller);
            frm.OnCreate += OnCreateEventHandler;
            frm.ShowDialog();
        }

        private void OnCreateEventHandler(Role role)
        {
            roles.Add(role);

            // Add a new row with values
            int rowIndex = dgvRole.Rows.Add();
            var row = dgvRole.Rows[rowIndex];

            row.Cells["No"].Value = (rowIndex + 1).ToString();
            row.Cells["Name"].Value = role.Name;
            // The button columns don't need values since we're using UseColumnTextForButtonValue
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
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
            if (_editingIndex < 0) return;

            roles[_editingIndex] = role;

            // Update the specific row in the grid
            var row = dgvRole.Rows[_editingIndex];
            row.Cells["Name"].Value = role.Name;
            // The button columns don't need to be updated since they use static text

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
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
            // Check if the click was on the "Action" column
            if (e.ColumnIndex == dgvRole.Columns["Action"].Index && e.RowIndex >= 0)
            {
                // Calculate which button was clicked based on mouse position
                var cellRect = dgvRole.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int totalButtons = 3; // View, Edit, Delete
                int buttonWidth = cellRect.Width / totalButtons; // Divide cell width by number of buttons

                // Get mouse position relative to the cell
                Point mousePos = dgvRole.PointToClient(Control.MousePosition);

                // Calculate which button region was clicked
                int relativeX = mousePos.X - cellRect.Left;

                if (relativeX < buttonWidth) // View button
                {
                    // Get the corresponding role data
                    var role = roles[e.RowIndex];
                    ShowRoleData(role);
                }
                else if (relativeX < buttonWidth * 2) // Edit button
                {
                    // Handle edit button click
                    EditRole(e.RowIndex);
                }
                else // Delete button
                {
                    // Handle delete button click
                    DeleteRole(e.RowIndex);
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

            var frm = new FrmEntryRole("Edit Role", role, _controller);
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
            var confirm = MessageBox.Show($"Hapus role {role.Name}?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                int result = _controller.Delete(role.Id);
                if (result > 0)
                {
                    roles.RemoveAt(rowIndex);
                    dgvRole.Rows.RemoveAt(rowIndex);

                    // Update row numbers
                    UpdateRowNumbers();
                }
                else
                {
                    MessageBox.Show("Gagal menghapus role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowRoleData(Role role)
        {
            // Show role details in a message box
            string roleDetails = $"Role Details:\n\n" +
                                $"ID: {role.Id}\n" +
                                $"Name: {role.Name}";

            MessageBox.Show(roleDetails, "Role Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // Define button dimensions
                int totalButtons = 3; // View, Edit, Delete
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

                e.Handled = true;
            }
        }
    }
}
