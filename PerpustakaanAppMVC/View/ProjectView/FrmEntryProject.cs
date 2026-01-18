using PerpustakaanAppMVC.Controller;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Session;
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

    public enum _FormMode
    {
        Create,
        Update,
        View
    }

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
        private _FormMode _mode;


        public FrmEntryProject()
        {
            _userLoginId = SessionManager.GetCurrentUserId();
            InitializeComponent();
        }

        public FrmEntryProject(string title, _FormMode _mode, Project project = null) : this()
        {
            this.Text = title;
            loadStatus();

            _isNewData = (_mode == _FormMode.Create);
            if (project != null) { loadUserData(project); }
            if (_mode == _FormMode.View) { setViewMode(); }
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
            //cmbStatus.SelectedIndex = project.Status;
        }

        private void loadStatus()
        {
            cmbStatus.Items.Add("Aktif");
            cmbStatus.Items.Add("Nonaktif");
            cmbStatus.SelectedIndex = 0;
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isNewData) _project = new Project();
                _project.Nama = txtNama.Text;
                _project.Deskripsi = txtDeskripsi.Text;
                _project.StartDate = dateStart.Value;
                _project.EndDate = dateEnd.Value;
                _project.Status = cmbStatus.SelectedItem.ToString();
                _project.CreatedBy = _userLoginId;

                int result = _isNewData ? _controller.Create(_project) : _controller.Update(_project);

                if (result > 0)
                {
                    if (_isNewData)
                    {
                        OnCreate?.Invoke(_project);
                    }
                    else
                    {
                        OnUpdate?.Invoke(_project);
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Gagal menyimpan data project.");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }
    }
}