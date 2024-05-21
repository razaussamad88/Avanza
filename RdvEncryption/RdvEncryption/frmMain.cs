using System;
using System.Windows.Forms;

namespace RdvEncryption
{
    public partial class frmMain : Form
    {
        static string _connectionString = string.Empty;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            txtbxLoginUsr.Text = txtbxLoginUsr.Text.Trim();

            _connectionString = HelperModule.EncryptConnectionString(txtbxLoginUsr.Text);

            txtbxCompPwd.Text = _connectionString;
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            txtbxLoginUsr.Text = txtbxLoginUsr.Text.Trim();

            _connectionString = HelperModule.DecryptConnectionString(txtbxLoginUsr.Text);

            txtbxCompPwd.Text = _connectionString;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_connectionString);
        }
    }
}
