using DVLD_Project.Global_Classes;
using Password_Manager.Properties;
using Password_Manager_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Password_Manager.Pass_Key
{
    public partial class frmAddUpdatePassKey : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }
        enMode _Mode;
        int _KeyID = -1;
        clsPassKeys _KeyInfo = new clsPassKeys();
        public frmAddUpdatePassKey()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdatePassKey( int keyID)
        {
            InitializeComponent();
            _KeyID = keyID;
            _Mode = enMode.Update;
        }
        private void txbTitle_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txbTitle.Text.Trim()))
            {
                errorProvider1.SetError(txbTitle, "This field should not be empty!");
            }
            else
            {
                errorProvider1.SetError(txbTitle, "");
            }
        }

        private void txbAccountUser_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbAccountUser.Text.Trim()))
            {
                errorProvider1.SetError(txbAccountUser, "This field should not be empty!");
            }
            else
            {
                errorProvider1.SetError(txbAccountUser, "");
            }
        }

        private void txbPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbPassword.Text.Trim()))
            {
                errorProvider1.SetError(txbPassword, "This field should not be empty!");
            }
            else
            {
                errorProvider1.SetError(txbPassword, "");
            }
        }

        private void txbURL_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbURL.Text.Trim()))
            {
                errorProvider1.SetError(txbURL, "This field should not be empty!");
            }
            else
            {
                errorProvider1.SetError(txbURL, "");
            }
        }

        private void pbSeePassword_Click(object sender, EventArgs e)
        {
            if(txbPassword.PasswordChar == '*')
            {
                txbPassword.PasswordChar = '\0';
            }
            else
                txbPassword.PasswordChar = '*';

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;

            if (!_HandlePersonImage())
                return;
            
            if (MessageBox.Show("Are you sure that you want to add this key?", "Config", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;


            clsPassKeys key = new clsPassKeys();

            key.UserID = clsGlobal.CurrentUser.UserID;
            key.Title = txbTitle.Text.Trim();
            key.AccountUser = txbAccountUser.Text.Trim();
            key.Password = txbPassword.Text.Trim();
            key.URL = txbURL.Text.Trim();

            if (pbImage.ImageLocation != null)
                key.ImagePath = pbImage.ImageLocation;
            else
                key.ImagePath = "";


            if(!key.Save())
            {
                MessageBox.Show("Something wrong, the key was not added!", "wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("New Key added successfully!", "added successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void _ResetDefualValues()
        {
            txbTitle.Text = "";
            txbAccountUser.Text = "";
            txbPassword.Text = "";
            txbURL.Text = "";
            pbImage.Image = Resources.web;
        }
        void _LoadData()
        {
            _KeyInfo = clsPassKeys.Find(_KeyID);
            if(_KeyInfo != null)
            {

                txbTitle.Text = _KeyInfo.Title;
                txbAccountUser.Text = _KeyInfo.AccountUser;
                txbPassword.Text = _KeyInfo.Password;
                txbURL.Text = _KeyInfo.URL;
                pbImage.Image = Resources.web;
                if(!string.IsNullOrWhiteSpace(_KeyInfo.ImagePath))
                {
                    pbImage.ImageLocation = _KeyInfo.ImagePath;
                }

                btnRemove.Visible = (_KeyInfo.ImagePath != "");

            }
        }
        private void frmAddUpdatePassKey_Load(object sender, EventArgs e)
        {
            _ResetDefualValues();
            if (_Mode == enMode.Update)
            {
                _LoadData();
                lblTitle.Text = "Update PassKey";
                this.Text = lblTitle.Text;
                btnAdd.Text = "Update";
            }
            else
            {
                lblTitle.Text = "Add New PassKey";
                this.Text = lblTitle.Text;
                btnAdd.Text = "Add";
                txbTitle.Focus();
            }
            
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Chose Person Image";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pbImage.Load(selectedFilePath);
                btnRemove.Visible = true;
            }
        }

        private bool _HandlePersonImage()
        {

            if (_KeyInfo.ImagePath != pbImage.ImageLocation)
            {
                if (_KeyInfo.ImagePath != "")
                {

                    try
                    {
                        File.Delete(_KeyInfo.ImagePath);

                    }
                    catch (IOException)
                    { }
                }

                if (pbImage.ImageLocation != null)
                {
                    string SourceImageFile = pbImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImageFolder(ref SourceImageFile))
                    {
                        pbImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
