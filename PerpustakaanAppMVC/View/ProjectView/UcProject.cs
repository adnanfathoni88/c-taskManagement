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
            InitListView();
        }

        private void InitListView()
        {
            lvProject.View = System.Windows.Forms.View.Details;
            lvProject.FullRowSelect = true;
            lvProject.GridLines = true;

            lvProject.Columns.Add("No", 40);
            lvProject.Columns.Add("Nama", 150);
            lvProject.Columns.Add("Start Date", 100);
            lvProject.Columns.Add("End Date", 100);
            lvProject.Columns.Add("Status", 80);
            lvProject.Columns.Add("Deskripsi", 250);

            LoadProjects();
        }

        private void LoadProjects()
        {
            lvProject.Items.Clear();
            projects = _controller.GetAllProjects();

            foreach (var project in projects)
            {
                var item = new ListViewItem((lvProject.Items.Count + 1).ToString());
                item.SubItems.Add(project.Nama);
                item.SubItems.Add(project.StartDate.ToShortDateString());
                item.SubItems.Add(project.EndDate.ToShortDateString());
                item.SubItems.Add(project.Status);
                item.SubItems.Add(project.Deskripsi);
                lvProject.Items.Add(item);
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

            var item = new ListViewItem((lvProject.Items.Count + 1).ToString());
            item.SubItems.Add(project.Nama);
            item.SubItems.Add(project.StartDate.ToShortDateString());
            item.SubItems.Add(project.EndDate.ToShortDateString());
            item.SubItems.Add(project.Status);
            item.SubItems.Add(project.Deskripsi);
            lvProject.Items.Add(item);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvProject.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan diubah");
                return;
            }

            _editingIndex = lvProject.SelectedIndices[0];
            var project = projects[_editingIndex];

            var frm = new FrmEntryProject("Edit Project", project);
            frm.OnUpdate += OnUpdateEventHandler;
            frm.ShowDialog();
        }

        private void OnUpdateEventHandler(Project project)
        {
            if (_editingIndex < 0) return;

            projects[_editingIndex] = project;
            LoadProjects();

            _editingIndex = -1;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (lvProject.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus");
                return;
            }

            var index = lvProject.SelectedIndices[0];
            var project = projects[index];
            var confirm = MessageBox.Show($"Hapus project {project.Nama}?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                _controller.Delete(project.Id);

                projects.RemoveAt(index);
                lvProject.Items.RemoveAt(index);
            }
        }
    }
}
