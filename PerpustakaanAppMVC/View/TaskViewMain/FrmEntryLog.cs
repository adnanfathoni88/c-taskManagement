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
using System.Windows.Forms;

namespace TWEEKLE.View.TaskViewMain
{
    public partial class FrmEntryLog : Form
    {
        private LogController _controller = new LogController();
        private TaskController _taskController = new TaskController();
        private UserController _userController = new UserController();
        private string _formMode;
        private Log _currentLog;
        private int _taskId;

        // Event for handling create operations
        public event Action<Log> OnCreate;

        public FrmEntryLog(string formMode, int taskId)
        {
            InitializeComponent();
            _formMode = formMode;
            _taskId = taskId;
            this.Text = formMode;
            
            LoadUsers();
            
            // Set default action options
            cmbAction.Items.Add("Created");
            cmbAction.Items.Add("Updated");
            cmbAction.Items.Add("Deleted");
            cmbAction.Items.Add("Started");
            cmbAction.Items.Add("Completed");
            cmbAction.Items.Add("Paused");
            cmbAction.Items.Add("Reopened");
            cmbAction.SelectedIndex = 0; // Default to "Created"
            
            // Pre-select the task
            lblTaskId.Text = taskId.ToString();
        }

        private void LoadUsers()
        {
            var users = _userController.ReadAll();
            cmbUser.DataSource = users;
            cmbUser.DisplayMember = "Name";
            cmbUser.ValueMember = "Id";
            cmbUser.SelectedIndex = -1;
        }

        private Log CollectFormData()
        {
            var log = new Log
            {
                Action = cmbAction.SelectedItem?.ToString() ?? "",
                UserId = (int)cmbUser.SelectedValue,
                TaskId = _taskId,
                Description = txtDescription.Text.Trim()
            };

            return log;
        }

        private void ValidateInput()
        {
            if (cmbAction.SelectedIndex == -1)
            {
                throw new ArgumentException("Action harus dipilih");
            }

            if (cmbUser.SelectedIndex == -1)
            {
                throw new ArgumentException("User harus dipilih");
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateInput();
                var log = CollectFormData();

                int newId = _controller.Create(log);
                if (newId > 0)
                {
                    log.Id = newId;
                    OnCreate?.Invoke(log);
                    MessageBox.Show("Log berhasil ditambahkan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Gagal menambahkan log", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}