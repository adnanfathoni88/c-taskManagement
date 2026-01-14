using System;
using System.Drawing;
using System.Windows.Forms;
using PerpustakaanAppMVC.Controller;
using PerpustakaanAppMVC.Session;

namespace PerpustakaanAppMVC.View
{
    public partial class FrmLogin : Form
    {
        private UserController _userController;

        public FrmLogin()
        {
            InitializeComponent();
            _userController = new UserController();
            
            // Center the form
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            this.Text = "Login";
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create controls
            Label lblTitle = new Label();
            lblTitle.Text = "Login";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.Size = new Size(100, 30);
            lblTitle.Location = new Point(150, 30);

            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Size = new Size(100, 20);
            lblEmail.Location = new Point(50, 80);

            TextBox txtEmail = new TextBox();
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(250, 25);
            txtEmail.Location = new Point(50, 100);

            Label lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Size = new Size(100, 20);
            lblPassword.Location = new Point(50, 140);

            TextBox txtPassword = new TextBox();
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(250, 25);
            txtPassword.Location = new Point(50, 160);
            txtPassword.UseSystemPasswordChar = true;

            Button btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Size = new Size(100, 30);
            btnLogin.Location = new Point(125, 200);
            btnLogin.Click += BtnLogin_Click;

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            TextBox txtEmail = (TextBox)this.Controls.Find("txtEmail", true)[0];
            TextBox txtPassword = (TextBox)this.Controls.Find("txtPassword", true)[0];

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var user = _userController.Login(email, password);

                if (user != null)
                {
                    // Login successful
                    SessionManager.Login(user);
                    
                    // Close login form and open dashboard
                    this.Hide();
                    
                    var mainForm = new FrmMain();
                    mainForm.FormClosed += (s, args) => this.Close(); // Close login when main form closes
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}