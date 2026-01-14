using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PerpustakaanAppMVC.Session;

namespace PerpustakaanAppMVC.View.Dashboard
{
    public partial class UcDashboard : BaseUserControl
    {
        private Label lblUserName;
        private Label lblUserEmail;
        private Label lblUserRole;

        public override string PageTitle => "Dashboard";

        public UcDashboard()
        {
            InitializeComponent();
            InitializeAdditionalComponents();
            LoadUserData();
        }

        private void InitializeAdditionalComponents()
        {
            // Create welcome label
            Label lblWelcome = new Label();
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Font = new Font("Arial", 16, FontStyle.Bold);
            lblWelcome.Location = new Point(20, 50);
            lblWelcome.Size = new Size(400, 30);
            lblWelcome.Text = "Welcome to Dashboard";
            this.Controls.Add(lblWelcome);

            // Create user info panel
            Panel pnlUserInfo = new Panel();
            pnlUserInfo.Name = "pnlUserInfo";
            pnlUserInfo.Location = new Point(20, 100);
            pnlUserInfo.Size = new Size(300, 100);
            pnlUserInfo.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(pnlUserInfo);

            // Add user info labels
            Label lblUserNameLabel = new Label();
            lblUserNameLabel.Text = "Name:";
            lblUserNameLabel.Location = new Point(10, 10);
            lblUserNameLabel.Size = new Size(100, 20);
            pnlUserInfo.Controls.Add(lblUserNameLabel);

            lblUserName = new Label();
            lblUserName.Name = "lblUserName";
            lblUserName.Location = new Point(120, 10);
            lblUserName.Size = new Size(150, 20);
            lblUserName.Font = new Font("Arial", 10, FontStyle.Regular);
            pnlUserInfo.Controls.Add(lblUserName);

            Label lblUserEmailLabel = new Label();
            lblUserEmailLabel.Text = "Email:";
            lblUserEmailLabel.Location = new Point(10, 35);
            lblUserEmailLabel.Size = new Size(100, 20);
            pnlUserInfo.Controls.Add(lblUserEmailLabel);

            lblUserEmail = new Label();
            lblUserEmail.Name = "lblUserEmail";
            lblUserEmail.Location = new Point(120, 35);
            lblUserEmail.Size = new Size(150, 20);
            lblUserEmail.Font = new Font("Arial", 10, FontStyle.Regular);
            pnlUserInfo.Controls.Add(lblUserEmail);

            Label lblUserRoleLabel = new Label();
            lblUserRoleLabel.Text = "Role:";
            lblUserRoleLabel.Location = new Point(10, 60);
            lblUserRoleLabel.Size = new Size(100, 20);
            pnlUserInfo.Controls.Add(lblUserRoleLabel);

            lblUserRole = new Label();
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Location = new Point(120, 60);
            lblUserRole.Size = new Size(150, 20);
            lblUserRole.Font = new Font("Arial", 10, FontStyle.Regular);
            pnlUserInfo.Controls.Add(lblUserRole);
        }

        private void LoadUserData()
        {
            if (SessionManager.IsLoggedIn)
            {
                lblUserName.Text = SessionManager.GetCurrentUserName();
                lblUserEmail.Text = SessionManager.CurrentUser?.Email ?? "";
                lblUserRole.Text = SessionManager.GetCurrentUserRole();
            }
            else
            {
                lblUserName.Text = "Guest";
                lblUserEmail.Text = "Not logged in";
                lblUserRole.Text = "N/A";
            }
        }
    }
}
