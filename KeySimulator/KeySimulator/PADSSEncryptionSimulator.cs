using Avanza.Core.AvanzaKeyStore;
using Avanza.Core.Logging;
using Avanza.Core.Utility;
using System;
using System.Windows.Forms;

namespace EncryptionSimulator
{
    public partial class PADSSEncryptionSimulator : Form
    {
        public PADSSEncryptionSimulator()
        {
            InitializeComponent();
            LogManager.Initialize(EComponentType.VisionServer);

            ServiceManager.Init_PADSS();

            if (ServiceManager.Is64Bit.HasValue)
            {
                this.txtKeySize.Text = ServiceManager.Is64Bit.Value ? "64-bit" : "32-bit";
            }
        }

        private void Init()
        {
            try
            {
                ProductIndex x = ProductIndex.RDVSM;

                switch (cmbxProduct.SelectedIndex)
                {
                    case 0: x = ProductIndex.RDVSM; break;
                    case 1: x = ProductIndex.Vision; break;
                    default: return;
                }

                ServiceManager.LoadDEK(txtbxEncDEK.Text, x);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ServiceManager.Log(ex.Message);
                return;
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            this.Init();

            //9a a7 36 04 35 69 da 18 80 58 7f 11 42 f1 98 4d 44 80 40 86 39 af 24 4f a4 80 f9 02 0a 0b ea c1 3b 36 48 04 42 8a 02 7b 1a 1a f4 c3 0f 29 5c f4 af 4e 5a 6a 09 92 b1 96 0a f9 34 3f cf f9 38 68 

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

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            this.Init();

            try
            {
                this.txtDEK.Text = String.Empty;

                Decrypt();

                this.txtDEK.Text = "Clear DEK : " + Util.ClearDecryptionKeyServer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Decryption Issue");
            }
        }

        private void RdvSMKeySimulator_Load(object sender, EventArgs e)
        {
            this.cmbxProduct.SelectedIndex = 0;
        }

        private void Encrypt()
        {
            ServiceManager.SetGenericDEK();

            if (this.chkbxIsComputeHash.Checked)
            {
                switch (this.cmbxProduct.SelectedIndex)
                {
                    case 0: // RDVSM
                        {
                            if (this.chkbxIsComputeHash.Checked)
                                this.txtEncryptTxt.Text = ServiceManager.ComputeHash(this.txtClearTxt.Text, Cryptographer.HashSalt.RDVSM);
                            break;
                        }


                    case 1: // Vision
                        {
                            if (this.chkbxIsComputeHash.Checked)
                                this.txtEncryptTxt.Text = ServiceManager.ComputeHash(this.txtClearTxt.Text, Cryptographer.HashSalt.Vision);
                            break;
                        }
                }
            }
            else
            {
                this.txtEncryptTxt.Text = ServiceManager.AESEncrypt(this.txtClearTxt.Text);
            }
        }

        private void Decrypt()
        {
            ServiceManager.SetGenericDEK();

            this.txtEncryptTxt.Text = ServiceManager.AESDecrypt(this.txtClearTxt.Text);
        }

        private void PADSSEncryptionSimulator_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogManager.Close();
            this.Dispose();
        }
    }
}
