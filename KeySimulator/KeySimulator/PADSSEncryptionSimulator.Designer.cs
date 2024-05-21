namespace EncryptionSimulator
{
    partial class PADSSEncryptionSimulator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PADSSEncryptionSimulator));
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.txtClearTxt = new System.Windows.Forms.TextBox();
            this.lblClearTxt = new System.Windows.Forms.Label();
            this.txtEncryptTxt = new System.Windows.Forms.TextBox();
            this.lblEncryptTxt = new System.Windows.Forms.Label();
            this.chkbxIsComputeHash = new System.Windows.Forms.CheckBox();
            this.txtDEK = new System.Windows.Forms.TextBox();
            this.txtbxEncDEK = new System.Windows.Forms.TextBox();
            this.lblEncDEK = new System.Windows.Forms.Label();
            this.grpbx = new System.Windows.Forms.GroupBox();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.cmbxProduct = new System.Windows.Forms.ComboBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblKeySize = new System.Windows.Forms.Label();
            this.txtKeySize = new System.Windows.Forms.TextBox();
            this.grpbx.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(351, 229);
            this.btnEncrypt.Margin = new System.Windows.Forms.Padding(4);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(100, 28);
            this.btnEncrypt.TabIndex = 0;
            this.btnEncrypt.Text = "Encrypt!";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // txtClearTxt
            // 
            this.txtClearTxt.Location = new System.Drawing.Point(139, 23);
            this.txtClearTxt.Margin = new System.Windows.Forms.Padding(4);
            this.txtClearTxt.Multiline = true;
            this.txtClearTxt.Name = "txtClearTxt";
            this.txtClearTxt.Size = new System.Drawing.Size(311, 67);
            this.txtClearTxt.TabIndex = 1;
            // 
            // lblClearTxt
            // 
            this.lblClearTxt.AutoSize = true;
            this.lblClearTxt.Location = new System.Drawing.Point(51, 49);
            this.lblClearTxt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClearTxt.Name = "lblClearTxt";
            this.lblClearTxt.Size = new System.Drawing.Size(72, 17);
            this.lblClearTxt.TabIndex = 2;
            this.lblClearTxt.Text = "Clear Text";
            // 
            // txtEncryptTxt
            // 
            this.txtEncryptTxt.Location = new System.Drawing.Point(139, 123);
            this.txtEncryptTxt.Margin = new System.Windows.Forms.Padding(4);
            this.txtEncryptTxt.Multiline = true;
            this.txtEncryptTxt.Name = "txtEncryptTxt";
            this.txtEncryptTxt.ReadOnly = true;
            this.txtEncryptTxt.Size = new System.Drawing.Size(311, 67);
            this.txtEncryptTxt.TabIndex = 1;
            // 
            // lblEncryptTxt
            // 
            this.lblEncryptTxt.AutoSize = true;
            this.lblEncryptTxt.Location = new System.Drawing.Point(19, 149);
            this.lblEncryptTxt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEncryptTxt.Name = "lblEncryptTxt";
            this.lblEncryptTxt.Size = new System.Drawing.Size(103, 17);
            this.lblEncryptTxt.TabIndex = 2;
            this.lblEncryptTxt.Text = "Encrypted Text";
            // 
            // chkbxIsComputeHash
            // 
            this.chkbxIsComputeHash.AutoSize = true;
            this.chkbxIsComputeHash.Location = new System.Drawing.Point(139, 198);
            this.chkbxIsComputeHash.Margin = new System.Windows.Forms.Padding(4);
            this.chkbxIsComputeHash.Name = "chkbxIsComputeHash";
            this.chkbxIsComputeHash.Size = new System.Drawing.Size(123, 21);
            this.chkbxIsComputeHash.TabIndex = 3;
            this.chkbxIsComputeHash.Text = "Compute Hash";
            this.chkbxIsComputeHash.UseVisualStyleBackColor = true;
            // 
            // txtDEK
            // 
            this.txtDEK.BackColor = System.Drawing.Color.White;
            this.txtDEK.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDEK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDEK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDEK.Location = new System.Drawing.Point(0, 415);
            this.txtDEK.Margin = new System.Windows.Forms.Padding(4);
            this.txtDEK.Multiline = true;
            this.txtDEK.Name = "txtDEK";
            this.txtDEK.ReadOnly = true;
            this.txtDEK.Size = new System.Drawing.Size(523, 21);
            this.txtDEK.TabIndex = 5;
            // 
            // txtbxEncDEK
            // 
            this.txtbxEncDEK.Location = new System.Drawing.Point(151, 54);
            this.txtbxEncDEK.Margin = new System.Windows.Forms.Padding(4);
            this.txtbxEncDEK.Name = "txtbxEncDEK";
            this.txtbxEncDEK.Size = new System.Drawing.Size(311, 22);
            this.txtbxEncDEK.TabIndex = 1;
            // 
            // lblEncDEK
            // 
            this.lblEncDEK.AutoSize = true;
            this.lblEncDEK.Location = new System.Drawing.Point(64, 59);
            this.lblEncDEK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEncDEK.Name = "lblEncDEK";
            this.lblEncDEK.Size = new System.Drawing.Size(68, 17);
            this.lblEncDEK.TabIndex = 2;
            this.lblEncDEK.Text = "Enc. DEK";
            // 
            // grpbx
            // 
            this.grpbx.Controls.Add(this.btnDecrypt);
            this.grpbx.Controls.Add(this.txtClearTxt);
            this.grpbx.Controls.Add(this.btnEncrypt);
            this.grpbx.Controls.Add(this.chkbxIsComputeHash);
            this.grpbx.Controls.Add(this.txtEncryptTxt);
            this.grpbx.Controls.Add(this.lblEncryptTxt);
            this.grpbx.Controls.Add(this.lblClearTxt);
            this.grpbx.Location = new System.Drawing.Point(12, 138);
            this.grpbx.Margin = new System.Windows.Forms.Padding(4);
            this.grpbx.Name = "grpbx";
            this.grpbx.Padding = new System.Windows.Forms.Padding(4);
            this.grpbx.Size = new System.Drawing.Size(495, 270);
            this.grpbx.TabIndex = 6;
            this.grpbx.TabStop = false;
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(243, 227);
            this.btnDecrypt.Margin = new System.Windows.Forms.Padding(4);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(100, 28);
            this.btnDecrypt.TabIndex = 4;
            this.btnDecrypt.Text = "Decrypt!";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // cmbxProduct
            // 
            this.cmbxProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxProduct.FormattingEnabled = true;
            this.cmbxProduct.Items.AddRange(new object[] {
            "RDV Settlement Manager",
            "Vision"});
            this.cmbxProduct.Location = new System.Drawing.Point(151, 86);
            this.cmbxProduct.Margin = new System.Windows.Forms.Padding(4);
            this.cmbxProduct.Name = "cmbxProduct";
            this.cmbxProduct.Size = new System.Drawing.Size(311, 24);
            this.cmbxProduct.TabIndex = 8;
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(36, 90);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(98, 17);
            this.lblProduct.TabIndex = 7;
            this.lblProduct.Text = "Product Name";
            // 
            // lblKeySize
            // 
            this.lblKeySize.AutoSize = true;
            this.lblKeySize.Location = new System.Drawing.Point(38, 22);
            this.lblKeySize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKeySize.Name = "lblKeySize";
            this.lblKeySize.Size = new System.Drawing.Size(94, 17);
            this.lblKeySize.TabIndex = 9;
            this.lblKeySize.Text = "AKS Key Size";
            // 
            // txtKeySize
            // 
            this.txtKeySize.BackColor = System.Drawing.SystemColors.Control;
            this.txtKeySize.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtKeySize.Location = new System.Drawing.Point(151, 22);
            this.txtKeySize.Margin = new System.Windows.Forms.Padding(4);
            this.txtKeySize.Name = "txtKeySize";
            this.txtKeySize.ReadOnly = true;
            this.txtKeySize.Size = new System.Drawing.Size(311, 15);
            this.txtKeySize.TabIndex = 10;
            this.txtKeySize.Text = "- - -";
            // 
            // PADSSEncryptionSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 436);
            this.Controls.Add(this.txtKeySize);
            this.Controls.Add(this.lblKeySize);
            this.Controls.Add(this.cmbxProduct);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.grpbx);
            this.Controls.Add(this.txtDEK);
            this.Controls.Add(this.lblEncDEK);
            this.Controls.Add(this.txtbxEncDEK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PADSSEncryptionSimulator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PADSS Encryption Simulator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PADSSEncryptionSimulator_FormClosed);
            this.Load += new System.EventHandler(this.RdvSMKeySimulator_Load);
            this.grpbx.ResumeLayout(false);
            this.grpbx.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.TextBox txtClearTxt;
        private System.Windows.Forms.Label lblClearTxt;
        private System.Windows.Forms.TextBox txtEncryptTxt;
        private System.Windows.Forms.Label lblEncryptTxt;
        private System.Windows.Forms.CheckBox chkbxIsComputeHash;
        private System.Windows.Forms.TextBox txtDEK;
        private System.Windows.Forms.TextBox txtbxEncDEK;
        private System.Windows.Forms.Label lblEncDEK;
        private System.Windows.Forms.GroupBox grpbx;
        private System.Windows.Forms.ComboBox cmbxProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Label lblKeySize;
        private System.Windows.Forms.TextBox txtKeySize;
    }
}

