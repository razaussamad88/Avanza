namespace EncryptionSimulator
{
    partial class RdvSMKeySimulator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RdvSMKeySimulator));
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.txtClearTxt = new System.Windows.Forms.TextBox();
            this.lblClearTxt = new System.Windows.Forms.Label();
            this.txtEncryptTxt = new System.Windows.Forms.TextBox();
            this.lblEncryptTxt = new System.Windows.Forms.Label();
            this.chkbxIsComputeHash = new System.Windows.Forms.CheckBox();
            this.cmbxProduct = new System.Windows.Forms.ComboBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.txtDEK = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(193, 278);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncrypt.TabIndex = 0;
            this.btnEncrypt.Text = "Encrypt!";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // txtClearTxt
            // 
            this.txtClearTxt.Location = new System.Drawing.Point(113, 98);
            this.txtClearTxt.Multiline = true;
            this.txtClearTxt.Name = "txtClearTxt";
            this.txtClearTxt.Size = new System.Drawing.Size(234, 55);
            this.txtClearTxt.TabIndex = 1;
            // 
            // lblClearTxt
            // 
            this.lblClearTxt.AutoSize = true;
            this.lblClearTxt.Location = new System.Drawing.Point(47, 119);
            this.lblClearTxt.Name = "lblClearTxt";
            this.lblClearTxt.Size = new System.Drawing.Size(55, 13);
            this.lblClearTxt.TabIndex = 2;
            this.lblClearTxt.Text = "Clear Text";
            // 
            // txtEncryptTxt
            // 
            this.txtEncryptTxt.Location = new System.Drawing.Point(113, 179);
            this.txtEncryptTxt.Multiline = true;
            this.txtEncryptTxt.Name = "txtEncryptTxt";
            this.txtEncryptTxt.ReadOnly = true;
            this.txtEncryptTxt.Size = new System.Drawing.Size(234, 55);
            this.txtEncryptTxt.TabIndex = 1;
            // 
            // lblEncryptTxt
            // 
            this.lblEncryptTxt.AutoSize = true;
            this.lblEncryptTxt.Location = new System.Drawing.Point(23, 200);
            this.lblEncryptTxt.Name = "lblEncryptTxt";
            this.lblEncryptTxt.Size = new System.Drawing.Size(79, 13);
            this.lblEncryptTxt.TabIndex = 2;
            this.lblEncryptTxt.Text = "Encrypted Text";
            // 
            // chkbxIsComputeHash
            // 
            this.chkbxIsComputeHash.AutoSize = true;
            this.chkbxIsComputeHash.Location = new System.Drawing.Point(113, 240);
            this.chkbxIsComputeHash.Name = "chkbxIsComputeHash";
            this.chkbxIsComputeHash.Size = new System.Drawing.Size(96, 17);
            this.chkbxIsComputeHash.TabIndex = 3;
            this.chkbxIsComputeHash.Text = "Compute Hash";
            this.chkbxIsComputeHash.UseVisualStyleBackColor = true;
            // 
            // cmbxProduct
            // 
            this.cmbxProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxProduct.FormattingEnabled = true;
            this.cmbxProduct.Items.AddRange(new object[] {
            "RDV Settlement Manager",
            "Vision"});
            this.cmbxProduct.Location = new System.Drawing.Point(113, 36);
            this.cmbxProduct.Name = "cmbxProduct";
            this.cmbxProduct.Size = new System.Drawing.Size(234, 21);
            this.cmbxProduct.TabIndex = 4;
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(27, 39);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(75, 13);
            this.lblProduct.TabIndex = 2;
            this.lblProduct.Text = "Product Name";
            // 
            // txtDEK
            // 
            this.txtDEK.BackColor = System.Drawing.Color.White;
            this.txtDEK.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDEK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDEK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDEK.Location = new System.Drawing.Point(0, 332);
            this.txtDEK.Multiline = true;
            this.txtDEK.Name = "txtDEK";
            this.txtDEK.ReadOnly = true;
            this.txtDEK.Size = new System.Drawing.Size(392, 22);
            this.txtDEK.TabIndex = 5;
            // 
            // RdvSMKeySimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 354);
            this.Controls.Add(this.txtDEK);
            this.Controls.Add(this.cmbxProduct);
            this.Controls.Add(this.chkbxIsComputeHash);
            this.Controls.Add(this.lblEncryptTxt);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.lblClearTxt);
            this.Controls.Add(this.txtEncryptTxt);
            this.Controls.Add(this.txtClearTxt);
            this.Controls.Add(this.btnEncrypt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RdvSMKeySimulator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PADSS Password Simulator";
            this.Load += new System.EventHandler(this.RdvSMKeySimulator_Load);
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
        private System.Windows.Forms.ComboBox cmbxProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtDEK;
    }
}

