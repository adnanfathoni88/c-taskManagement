using TWEEKLE.Session;
using TWEEKLE.View.Dashboard;
using TWEEKLE.View.ProjectView;
using TWEEKLE.View.RoleView;
using TWEEKLE.View.TaskViewMain;
using TWEEKLE.View.UserView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TWEEKLE.View
{
    public partial class FrmMain : Form
    {
        List<string> menus = new List<string>(); // list menu

        private Dictionary<string, Image> menuIcons; // icon menu

        public FrmMain()
        {
            string userRole = SessionManager.GetCurrentUserRole();

            InitializeComponent();
            InitializeMenuIcons();
            InitializeMenu(userRole);
            InitializeUserInfo();
        }

        private void InitializeUserInfo()
        {
            lbName.Text = SessionManager.GetCurrentUserName();
            lbRole.Text = SessionManager.GetCurrentUserRole();
        }

        // initialize menu berdasarkan role user
        private void InitializeMenu(string userLogin)
        {
            switch (userLogin)
            {
                case "Admin":
                    menus = new List<string> { "Dashboard", "Role", "User", "Project" };
                    break;
                case "Project Manager":
                case "Developer":
                case "Tester":
                case "Viewer":
                    menus = new List<string> { "Dashboard", "Project" };
                    break;
                default:
                    menus = new List<string> { "Dashboard" };
                    break;
            }
        }

        // initialize icon menu
        private void InitializeMenuIcons()
        {
            menuIcons = new Dictionary<string, Image>
            {
                { "Dashboard", Properties.Resources.grid },
                { "Role", Properties.Resources.role},
                { "User", Properties.Resources.user},
                { "Project", Properties.Resources.project}
            };
        }

        private void LoadUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            this.panelContent.Controls.Clear();
            this.panelContent.Controls.Add(uc);
        }

        private void OnLoadTaskViewRequested(int projectId, string projectName)
        {
            LoadUserControl(new UcTask(projectId, projectName));
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            foreach (var item in menus)
            {
                Button btn = new Button();
                btn.Text = $"  {item}";
                btn.Size = new Size(220, 45);
                btn.Padding = new Padding(12, 0, 10, 0);
                btn.Margin = new Padding(0, 5, 0, 0);

                btn.Tag = item;

                if (menuIcons.ContainsKey(item))
                    btn.Image = new Bitmap(menuIcons[item], new Size(16, 16));

                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextAlign = ContentAlignment.MiddleRight;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;

                btn.UseVisualStyleBackColor = false;

                // Ganti warna default
                btn.BackColor = ColorTranslator.FromHtml("#3C467B");
                btn.ForeColor = Color.White;

                // Ganti warna hover
                btn.MouseEnter += (sender2, args) => btn.BackColor = ColorTranslator.FromHtml("#5A6090"); // lebih terang sedikit
                btn.MouseLeave += (sender2, args) => btn.BackColor = ColorTranslator.FromHtml("#3C467B"); // kembali ke default


                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.Click += TaskButton_Click;

                flowSidebar.Controls.Add(btn);
            }

            // Load Dashboard as default view when form loads
            LoadUserControl(new UcDashboard());
        }
        private void TaskButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string menuName = btn.Tag.ToString();

            switch (menuName)
            {
                case "Dashboard":
                    LoadUserControl(new UcDashboard());
                    break;
                case "Role":
                    LoadUserControl(new UcRole1());
                    break;
                case "User":
                    LoadUserControl(new UcUser());
                    break;
                case "Project":
                    var projectControl = new UcProject();
                    projectControl.LoadTaskViewRequested += OnLoadTaskViewRequested;
                    LoadUserControl(projectControl);
                    break;
                default:
                    break;
            }

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                SessionManager.Logout();

                this.Hide();
                new FrmLogin().Show();
            }
        }
    }
}
