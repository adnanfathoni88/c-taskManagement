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
using WinFormsView = System.Windows.Forms.View;

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
            InitListView();
        }

        private void InitListView()
        {
            lvRole.View = WinFormsView.Details;
            lvRole.FullRowSelect = true;
            lvRole.GridLines = true;

            lvRole.Columns.Add("No", 40);
            lvRole.Columns.Add("Name", 150);

            LoadRoles();
        }

        private void LoadRoles()
        {
            lvRole.Items.Clear();
            roles = _controller.ReadAll();

            foreach (var role in roles)
            {
                var item = new ListViewItem((lvRole.Items.Count + 1).ToString());
                item.SubItems.Add(role.Name);
                lvRole.Items.Add(item);
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

            var item = new ListViewItem((lvRole.Items.Count + 1).ToString());
            item.SubItems.Add(role.Name);
            lvRole.Items.Add(item);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvRole.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan diubah");
                return;
            }

            _editingIndex = lvRole.SelectedIndices[0];
            var role = roles[_editingIndex];

            var frm = new FrmEntryRole("Edit Role", role, _controller);
            frm.OnUpdate += OnUpdateEventHandler;
            frm.ShowDialog();
        }

        private void OnUpdateEventHandler(Role role)
        {
            if (_editingIndex < 0) return;

            roles[_editingIndex] = role;
            LoadRoles();

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (lvRole.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus");
                return;
            }

            var index = lvRole.SelectedIndices[0];
            var role = roles[index];
            var confirm = MessageBox.Show($"Hapus role {role.Name}?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                _controller.Delete(role.Id);

                roles.RemoveAt(index);
                lvRole.Items.RemoveAt(index);
            }
        }
    }
}
