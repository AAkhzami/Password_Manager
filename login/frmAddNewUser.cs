using DVLD_Project.Global_Classes;
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
    public partial class frmAddNewUser : Form
    {
        public delegate void DataBackEventHandler(int userID, string password);
        public event DataBackEventHandler DataBack;

        int _userID = -1;
        public frmAddNewUser()
        {
            InitializeComponent();
        }

        private void txbUserName_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txbUserName.Text.Trim()))
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

        private void txbEmail_Validating(object sender, CancelEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txbEmail.Text.Trim()))
            {
                if (!clsValidation.ValidateEmail(txbEmail.Text.Trim()))
                    errorProvider1.SetError(txbEmail, "Invalid Email Address Format!");
                else
                    errorProvider1.SetError(txbEmail, "");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure that you want to create this user? You can not edit any information if you created!", "Confirm",MessageBoxButtons.YesNo,
                MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            
            if (!this.ValidateChildren())
                return;

            clsUser user = new clsUser();
            user.UserName =txbUserName.Text.Trim();
            user.Password =txbPassword.Text.Trim();
            user.Email =txbEmail.Text.Trim();
            user.CreatedAt = DateTime.Now;

            if(user.Save())
            {
                btnAdd.Enabled = false;
                MessageBox.Show("User created successfully!","added successfully",MessageBoxButtons.OK,MessageBoxIcon.Information);
                txbUserName.Enabled = false;
                txbPassword.Enabled = false;
                txbEmail.Enabled = false;
                _userID = user.UserID;
                DataBack?.Invoke(_userID,user.Password);

                return;
            }
            else
            {
                MessageBox.Show("Something wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbSeePassword_Click(object sender, EventArgs e)
        {
            if (txbPassword.PasswordChar == '*')
                txbPassword.PasswordChar = '\0';
            else
                txbPassword.PasswordChar = '*';

        }
    }
}
