namespace PasswordBox
{
    partial class MainUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUI));
            this.lblToolName = new System.Windows.Forms.Label();
            this.cmbxToolName = new System.Windows.Forms.ComboBox();
            this.grpbxMain = new System.Windows.Forms.GroupBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsMnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.grpbxMain.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblToolName
            // 
            this.lblToolName.AutoSize = true;
            this.lblToolName.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolName.Location = new System.Drawing.Point(10, 49);
            this.lblToolName.Name = "lblToolName";
            this.lblToolName.Size = new System.Drawing.Size(56, 18);
            this.lblToolName.TabIndex = 0;
            this.lblToolName.Text = "N A M E";
            // 
            // cmbxToolName
            // 
            this.cmbxToolName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxToolName.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbxToolName.ForeColor = System.Drawing.Color.Gray;
            this.cmbxToolName.Location = new System.Drawing.Point(13, 74);
            this.cmbxToolName.Name = "cmbxToolName";
            this.cmbxToolName.Size = new System.Drawing.Size(303, 31);
            this.cmbxToolName.TabIndex = 1;
            this.cmbxToolName.SelectionChangeCommitted += new System.EventHandler(this.cmbxToolName_SelectionChangeCommitted);
            // 
            // grpbxMain
            // 
            this.grpbxMain.BackColor = System.Drawing.Color.Transparent;
            this.grpbxMain.Controls.Add(this.lblDesc);
            this.grpbxMain.Controls.Add(this.btnRun);
            this.grpbxMain.Controls.Add(this.txtDesc);
            this.grpbxMain.Controls.Add(this.lblToolName);
            this.grpbxMain.Controls.Add(this.cmbxToolName);
            this.grpbxMain.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpbxMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.grpbxMain.Location = new System.Drawing.Point(12, 45);
            this.grpbxMain.Name = "grpbxMain";
            this.grpbxMain.Size = new System.Drawing.Size(426, 448);
            this.grpbxMain.TabIndex = 2;
            this.grpbxMain.TabStop = false;
            this.grpbxMain.Text = "Password Tools";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc.Location = new System.Drawing.Point(11, 128);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(120, 18);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Text = "D E S C R I P T I O N";
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.SystemColors.Control;
            this.btnRun.Enabled = false;
            this.btnRun.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRun.Location = new System.Drawing.Point(333, 74);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(79, 31);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "R U N";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.Color.White;
            this.txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDesc.ForeColor = System.Drawing.Color.Teal;
            this.txtDesc.Location = new System.Drawing.Point(14, 153);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ReadOnly = true;
            this.txtDesc.Size = new System.Drawing.Size(400, 280);
            this.txtDesc.TabIndex = 2;
            this.txtDesc.Text = "[please select a password tool name]";
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMnuAbout});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip.Size = new System.Drawing.Size(450, 25);
            this.menuStrip.TabIndex = 3;
            // 
            // tsMnuAbout
            // 
            this.tsMnuAbout.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsMnuAbout.ForeColor = System.Drawing.Color.Black;
            this.tsMnuAbout.Name = "tsMnuAbout";
            this.tsMnuAbout.Size = new System.Drawing.Size(63, 21);
            this.tsMnuAbout.Text = "About";
            this.tsMnuAbout.Click += new System.EventHandler(this.tsMnuAbout_Click);
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(450, 505);
            this.Controls.Add(this.grpbxMain);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "MainUI";
            this.Text = "Avanza Password Box";
            this.Load += new System.EventHandler(this.MainUI_Load);
            this.grpbxMain.ResumeLayout(false);
            this.grpbxMain.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblToolName;
        private System.Windows.Forms.ComboBox cmbxToolName;
        private System.Windows.Forms.GroupBox grpbxMain;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsMnuAbout;
    }
}

