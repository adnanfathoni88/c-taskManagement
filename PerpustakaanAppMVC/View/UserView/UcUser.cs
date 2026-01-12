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
            statusColumn.FillWeight = 1; // Take small portion of available space
            dgvUser.Columns.Add(statusColumn);

            // Add button column for View action
            var viewColumn = new DataGridViewButtonColumn();
            viewColumn.Name = "Action";
            viewColumn.HeaderText = "Action";
            viewColumn.Text = "View";
            viewColumn.UseColumnTextForButtonValue = true;
            viewColumn.FillWeight = 1; // Take small portion of available space
            dgvUser.Columns.Add(viewColumn);

            // Event handler for cell clicks
            dgvUser.CellClick += DgvUser_CellClick;

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
                // The button column doesn't need a value since we're using UseColumnTextForButtonValue
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
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (dgvUser.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan diubah");
                return;
            }

            _editingIndex = dgvUser.SelectedRows[0].Index;
            var user = users[_editingIndex];

            var frm = new FrmEntryUser("Edit User", user, _controller);
            frm.OnUpdate += OnUpdateEventHandler;
            frm.ShowDialog();

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
            var user = users[index];
            var confirm = MessageBox.Show($"Hapus user {user.Name}?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                _controller.Delete(user.Id);

                users.RemoveAt(index);
                dgvUser.Rows.RemoveAt(index);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan direset passwordnya");
                return;
            }
            var index = dgvUser.SelectedRows[0].Index;
            var user = users[index];
            var frm = new FrmReset("Reset Password", user, _controller);
            frm.OnReset += OnResetEventHandler;
            frm.ShowDialog();

        }

        private void OnResetEventHandler(User user)
        {
            MessageBox.Show($"Password user {user.Name} telah direset.");
        }



        private void DgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click was on the "View" button column
            if (e.ColumnIndex == dgvUser.Columns["Action"].Index && e.RowIndex >= 0)
            {
                // Get the corresponding user data
                var user = users[e.RowIndex];
                ShowUserData(user);
            }
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
    }
}
