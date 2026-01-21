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

namespace TWEEKLE.View.UserView
{
    public enum _FormMode
    {
        Create,
        Update,
        View
    }

    public delegate void ResetEventHandler(User user);
    public partial class FrmReset : Form
    {

        public event ResetEventHandler OnReset;

        private UserController _controller;

        private _FormMode _mode;

        private User user;

        public FrmReset()
        {
            InitializeComponent();
        }

        public FrmReset(string title, _FormMode mode, User obj) : this()
        {
            this.Text = title;
            _mode = mode;
            user = obj;
        }

        public FrmReset(string title, _FormMode mode, User obj, UserController controller) : this()
        {
            this.Text = title;
            _mode = mode;
            user = obj;
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                var oldPassword = txtPassword1.Text;
                var newPassword = txtPassowrd2.Text;

                if (oldPassword != newPassword)
                {
                    MessageBox.Show("Password tidak sesuai!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // use controller
                int result = _controller.ResetPassword(user.Id, newPassword);
                if (result > 0)
                {
                    OnReset?.Invoke(user);
                    Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
}
}
