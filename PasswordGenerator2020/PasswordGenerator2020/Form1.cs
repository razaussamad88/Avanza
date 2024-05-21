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
        public Form1()
        {
            InitializeComponent();
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    // decrypt
        //    txtbxCompPwd.Text = System.Convert.ToBase64String(new Cryptographer().ComputeHash(txtbxLoginUsr.Text.Trim() + txtbxPwd.Text.Trim()));
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            string hashSalt = txtHashSalt.Text.Trim();

            hashSalt = String.IsNullOrEmpty(hashSalt) ? "SYMMETRY" : hashSalt;

            // encrypt
            string pwd = System.Convert.ToBase64String(new Cryptographer().ComputeHash(txtbxLoginUsr.Text.Trim() + txtbxPwd.Text.Trim(), hashSalt));
            txtbxCompPwd.Text = System.Net.WebUtility.HtmlDecode(pwd);
        }
    }
}
