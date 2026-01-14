using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerpustakaanAppMVC.View
{
    public partial class Test : Form
    {

        List<string> tasks = new List<string>()
        {
            "Belajar C#",
            "Kerjain UI",
            "Debug Error",
            "Push ke Git"
        };

        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            foreach (var task in tasks)
            {
                Button btn = new Button();
                btn.Text = task;
                btn.Size = new Size(220, 45);
                btn.Tag = task;

                btn.Image = new Bitmap(Properties.Resources.grid, new Size(24, 24));
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextAlign = ContentAlignment.MiddleRight;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;

                btn.UseVisualStyleBackColor = false;
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;

                btn.Click += TaskButton_Click;

                flowSidebar.Controls.Add(btn);
            }
        }
        private void TaskButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string taskName = btn.Tag.ToString();

            MessageBox.Show($"Task: {taskName}");
        }
    }
}
