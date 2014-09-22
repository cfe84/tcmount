using System;
using System.Windows.Forms;
using System.Configuration;

namespace TCMount
{
    using System.IO;

    public partial class frmEnterPassword : Form
    {
        public string Password { get; set; }

        public frmEnterPassword()
        {
            InitializeComponent();
        }


        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            Password = txtPassword.Text;
        }


        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

    }
}
