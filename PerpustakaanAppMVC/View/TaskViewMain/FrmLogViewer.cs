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
    public partial class FrmLogViewer : Form
    {
        private LogController _controller = new LogController();
        private List<Log> logs = new List<Log>();
        private int _taskId;

        public FrmLogViewer(int taskId, string taskTitle)
        {
            InitializeComponent();
            _taskId = taskId;
            this.Text = $"Log Viewer - Task: {taskTitle}";
            
            LoadLogs();
        }

        private void LoadLogs()
        {
            dgvLogs.Rows.Clear();
            logs = _controller.GetByTaskId(_taskId);

            foreach (var log in logs)
            {
                // Add a new row with values
                int rowIndex = dgvLogs.Rows.Add();
                var row = dgvLogs.Rows[rowIndex];

                row.Cells["No"].Value = (rowIndex + 1).ToString();
                row.Cells["Action"].Value = log.Action;
                row.Cells["UserName"].Value = log.UserName;
                row.Cells["Description"].Value = log.Description;
                row.Cells["Timestamp"].Value = log.Timestamp;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLogs();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}