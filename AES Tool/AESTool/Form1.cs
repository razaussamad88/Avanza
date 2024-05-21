using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision.Web.Common;

namespace PasswordGenerator2020
{
    public partial class Form1 : Form
    {
        static string _connectionString = string.Empty;

        public Form1()
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
