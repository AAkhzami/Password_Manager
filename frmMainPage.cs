using Password_Manager.login;
using Password_Manager.Pass_Key;
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

namespace Password_Manager
{
    public partial class frmMainPage : Form
    {
        private static DataTable _dtAllPassKeysByUserID;
        private DataTable _dtPassKeys;
        frmLogin _frmLogin;
        public frmMainPage(frmLogin login)
        {
            InitializeComponent();
            _frmLogin = login;
        }
        private void frmMainPage_Load(object sender, EventArgs e)
        {
            _dtAllPassKeysByUserID = clsPassKeys.GetAllPassKeysByUserID(clsGlobal.CurrentUser.UserID);
            if (_dtAllPassKeysByUserID.Rows.Count > 0)
                _dtPassKeys = _dtAllPassKeysByUserID.DefaultView.ToTable(true, "KeyID", "Title", "Password", "URL");
            else
                _dtPassKeys = _dtAllPassKeysByUserID;

            dgvPassKeys.DataSource = _dtPassKeys;
            if(dgvPassKeys.Rows.Count > 0)
            {
                dgvPassKeys.Columns[0].HeaderText = "KeyID";
                dgvPassKeys.Columns[0].Width = 50;
                dgvPassKeys.Columns[0].Visible = false;


                dgvPassKeys.Columns[1].HeaderText = "Title";
                dgvPassKeys.Columns[1].Width = 395;

                dgvPassKeys.Columns[2].HeaderText = "Password";
                dgvPassKeys.Columns[2].Width = 390;

                dgvPassKeys.Columns[3].HeaderText = "URL";
                dgvPassKeys.Columns[3].Width = 395;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            frmAddUpdatePassKey frm = new frmAddUpdatePassKey();
            frm.ShowDialog();
            frmMainPage_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int keyID = (int) dgvPassKeys.CurrentRow.Cells[0].Value;

            frmAddUpdatePassKey frm = new frmAddUpdatePassKey(keyID);
            frm.ShowDialog();
            frmMainPage_Load(null, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int keyID = (int)dgvPassKeys.CurrentRow.Cells[0].Value;
            if(clsPassKeys.Delete(keyID))
            {
                MessageBox.Show("Key Deleted Successfully","Delected Successfully",MessageBoxButtons.OK,MessageBoxIcon.Information);
                frmMainPage_Load(null,null);
                return;
            }
            else
            {
                MessageBox.Show("A problem occurred when deleting the key", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void copyPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string password = (string)dgvPassKeys.CurrentRow.Cells[2].Value;
            string decPassword = clsPassKeys.DecryptThePassword(password, clsGlobal.CurrentUser.Password);

            clipboardTimer.Stop(); // stop the last operation 
            Clipboard.SetText(decPassword);
            MessageBox.Show("The password copied! you can used in 20s");
            clipboardTimer.Start();

            
        }

        private void clipboardTimer_Tick(object sender, EventArgs e)
        {
            Clipboard.Clear();
            clipboardTimer.Stop();
        }

        private void pbLogout_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            _frmLogin.Show();
            this.Close();
        }
    }
}
