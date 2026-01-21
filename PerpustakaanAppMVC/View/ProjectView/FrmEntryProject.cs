using TWEEKLE.Controller;
using TWEEKLE.Model.Entity;
using TWEEKLE.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TWEEKLE.View.ProjectView
{

    // delegte 
    public delegate void CreateUpdateEventHandler(Project project);

    public partial class FrmEntryProject : Form
    {
        public event CreateUpdateEventHandler OnCreate;
        public event CreateUpdateEventHandler OnUpdate;

        private ProjectController _controller = new ProjectController();
        private bool _isNewData = true;
        private Project _project;
        private int _userLoginId;
        private string _mode;

        public FrmEntryProject()
        {
            _userLoginId = SessionManager.GetCurrentUserId();
            InitializeComponent();
        }

        public FrmEntryProject(string title, string mode, Project project = null) : this()
        {
            this.Text = title;
            loadStatus();

            _isNewData = (mode == "create");
            if (project != null) { loadUserData(project); }

            // set label form
            switch (mode)
            {
                case "create":
                    lbForm.Text = "Tambah Project";
                    break;
                case "update":
                    lbForm.Text = "Edit Project";
                    break;
                case "view":
                    lbForm.Text = "Lihat Project";
                    setViewMode();
                    break;
            }

            this._mode = mode;
        }

        private void setViewMode()
        {
            txtNama.ReadOnly = true;
            txtNama.BackColor = Color.White;
            txtDeskripsi.ReadOnly = true;
            txtDeskripsi.BackColor = Color.White;
            dateStart.Enabled = false;
            dateStart.ForeColor = Color.White;
            dateEnd.Enabled = false;
            dateEnd.ForeColor = Color.White;
            cmbStatus.Enabled = false;
            cmbStatus.BackColor = Color.White;
            btnSimpan.Visible = false;
        }

        private void loadUserData(Project project)
        {
            _project = project;
            txtNama.Text = project.Nama;
            txtDeskripsi.Text = project.Deskripsi;
            dateStart.Value = project.StartDate;
            dateEnd.Value = project.EndDate;
            // Set the status in the combobox
            int statusIndex = cmbStatus.Items.IndexOf(project.Status);
            if (statusIndex >= 0)
            {
                cmbStatus.SelectedIndex = statusIndex;
            }
            else
            {
                cmbStatus.SelectedIndex = 0; // Default to first item if status not found
            }
        }

        private void loadStatus()
        {
            cmbStatus.Items.Add("Aktif");
            cmbStatus.Items.Add("Nonaktif");
            cmbStatus.SelectedIndex = 0;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // Validate project name
            if (string.IsNullOrWhiteSpace(txtNama.Text))
            {
                MessageBox.Show("Nama project harus diisi.", "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if project name already exists (for Create mode or Update mode when name changed)
            if (_mode == "create" || (_project != null && _project.Nama != txtNama.Text))
            {

                Project existingProject = null;
                if (_mode == "create")
                {
                    existingProject = _controller.GetByName(txtNama.Text);
                }
                else if (_project != null)
                {
                    // For update mode, exclude the current project from the check
                    existingProject = _controller.GetByNameExcludeId(txtNama.Text, _project.Id);
                }

                if (existingProject != null)
                {
                    MessageBox.Show("Nama project sudah digunakan. Silakan gunakan nama yang berbeda.", "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                // Only create a new project object if we're in Create mode
                if (_mode == "create") { _project = new Project(); }

                // Set the project properties regardless of mode
                _project.Nama = txtNama.Text;
                _project.Deskripsi = txtDeskripsi.Text;
                _project.StartDate = dateStart.Value;
                _project.EndDate = dateEnd.Value;
                _project.Status = cmbStatus.SelectedItem.ToString();
                _project.CreatedBy = _userLoginId;

                // Only set CreatedBy for new projects, keep the original ID for updates
                if (_mode == "create") { _project.CreatedBy = _userLoginId; }

                int result = _mode == "create" ? _controller.Create(_project) : _controller.Update(_project);

                if (result > 0)
                {
                    if (_mode == "create")
                    {
                        OnCreate?.Invoke(_project);
                    }
                    else
                    {
                        OnUpdate?.Invoke(_project);
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // Check for specific database constraint violations
                if (ex.Message.Contains("UNIQUE constraint failed") || ex.Message.Contains("duplicate"))
                {
                    MessageBox.Show("Nama project sudah digunakan. Silakan gunakan nama yang berbeda.", "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FrmEntryProject_Load(object sender, EventArgs e)
        {

            if (_mode == "update" || _mode == "view")
            {
                txtNama.Text = _project.Nama;
                txtDeskripsi.Text = _project.Deskripsi;
                dateStart.Value = _project.StartDate;
                dateEnd.Value = _project.EndDate;
                cmbStatus.SelectedItem = _project.Status;
            }


        }
    }
}