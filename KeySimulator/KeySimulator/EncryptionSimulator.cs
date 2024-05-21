using Avanza.Core.AvanzaKeyStore;
using Avanza.Core.Utility;
using System;
using System.Windows.Forms;

namespace EncryptionSimulator
{
    public partial class RdvSMKeySimulator : Form
    {
        public RdvSMKeySimulator()
        {
            InitializeComponent();
            this.Init();
        }

        private void Init()
        {
            try
            {
                ServiceManager.Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Your keys (KEK/DEK) is in problem.", "Avanza KeyStore Issue");
                return;
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtDEK.Text = String.Empty;

                Encrypt();

                this.txtDEK.Text = "Clear DEK : " + Util.ClearDecryptionKeyServer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Encryption Issue");
            }
        }

        private void RdvSMKeySimulator_Load(object sender, EventArgs e)
        {
            this.cmbxProduct.SelectedIndex = 0;
        }

        private void Encrypt()
        {
            switch (this.cmbxProduct.SelectedIndex)
            {
                case 0: // RDVSM
                    {
                        ServiceManager.SetDEK(ProductIndex.RDVSM);

                        if (this.chkbxIsComputeHash.Checked)
                            this.txtEncryptTxt.Text = ServiceManager.ComputeHash(this.txtClearTxt.Text, Cryptographer.HashSalt.RDVSM);
                        else
                            this.txtEncryptTxt.Text = ServiceManager.AESEncrypt(this.txtClearTxt.Text);

                        break;
                    }


                case 1: // Vision
                    {
                        ServiceManager.SetDEK(ProductIndex.Vision);

                        if (this.chkbxIsComputeHash.Checked)
                            this.txtEncryptTxt.Text = ServiceManager.ComputeHash(this.txtClearTxt.Text, Cryptographer.HashSalt.Vision);
                        else
                            this.txtEncryptTxt.Text = ServiceManager.AESEncrypt(this.txtClearTxt.Text);

                        break;
                    }
            }
        }
    }
}
