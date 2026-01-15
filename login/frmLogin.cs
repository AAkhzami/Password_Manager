using Password_Manager_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Password_Manager.login
{
    public partial class frmLogin : Form
    {
        clsUser userinfo;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txbUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbUserName.Text.Trim()))
            {
                errorProvider1.SetError(txbUserName, "This field should not be empty!");
            }
            else
                errorProvider1.SetError(txbUserName, "");
        }

        private void txbPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbPassword.Text.Trim()))
            {
                errorProvider1.SetError(txbPassword, "This field should not be empty!");
            }
            else
                errorProvider1.SetError(txbPassword, "");
        }

        private void btnLoign_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;

            userinfo = clsUser.FindByUserNameAndPassword(txbUserName.Text.Trim(), txbPassword.Text.Trim());
            if(userinfo != null)
            {
                clsGlobal.CurrentUser = userinfo;
                this.Hide();
                frmMainPage frm = new frmMainPage(this);
                frm.ShowDialog();
            }
            else
            {
                txbUserName.Focus();
                MessageBox.Show("Wrong Credintials", "Invalid User name/Passwrod!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            

        }

        private void pbSeePassword_Click(object sender, EventArgs e)
        {
            if (txbPassword.PasswordChar == '*')
                txbPassword.PasswordChar = '\0';
            else
                txbPassword.PasswordChar = '*';
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddNewUser form = new frmAddNewUser();
            form.DataBack += LoadData;

            form.ShowDialog();
        }

        private void LoadData(int userId, string password)
        {
            userinfo = clsUser.Find(userId);
            if(userinfo != null)
            {
                txbUserName.Text = userinfo.UserName;
                userinfo.Password = password;
                txbPassword.Text = userinfo.Password;
            }
            else
            {
                MessageBox.Show("Something wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
