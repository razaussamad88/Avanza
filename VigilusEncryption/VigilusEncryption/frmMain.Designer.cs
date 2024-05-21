namespace VigilusEncryption
{
    partial class frmMain
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
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.txtbxCompPwd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbxLoginUsr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(733, 110);
            this.btnEncrypt.Margin = new System.Windows.Forms.Padding(4);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(100, 28);
            this.btnEncrypt.TabIndex = 7;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // txtbxCompPwd
            // 
            this.txtbxCompPwd.Location = new System.Drawing.Point(203, 182);
            this.txtbxCompPwd.Margin = new System.Windows.Forms.Padding(4);
            this.txtbxCompPwd.MaxLength = 5000;
            this.txtbxCompPwd.Multiline = true;
            this.txtbxCompPwd.Name = "txtbxCompPwd";
            this.txtbxCompPwd.ReadOnly = true;
            this.txtbxCompPwd.Size = new System.Drawing.Size(629, 70);
            this.txtbxCompPwd.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 186);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Result";
            // 
            // txtbxLoginUsr
            // 
            this.txtbxLoginUsr.Location = new System.Drawing.Point(203, 31);
            this.txtbxLoginUsr.Margin = new System.Windows.Forms.Padding(4);
            this.txtbxLoginUsr.MaxLength = 5000;
            this.txtbxLoginUsr.Multiline = true;
            this.txtbxLoginUsr.Name = "txtbxLoginUsr";
            this.txtbxLoginUsr.Size = new System.Drawing.Size(629, 70);
            this.txtbxLoginUsr.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 34);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Content";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(733, 261);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(100, 28);
            this.btnCopy.TabIndex = 7;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(625, 110);
            this.btnDecrypt.Margin = new System.Windows.Forms.Padding(4);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(100, 28);
            this.btnDecrypt.TabIndex = 7;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 321);
            this.Controls.Add(this.txtbxLoginUsr);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtbxCompPwd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Vigilus Encryption";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.TextBox txtbxCompPwd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtbxLoginUsr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnDecrypt;
    }
}

