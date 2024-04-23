namespace BitwardenSendHelper
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            lblUserId = new Label();
            txtUserId = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblUserId
            // 
            lblUserId.AutoSize = true;
            lblUserId.Location = new Point(20, 27);
            lblUserId.Name = "lblUserId";
            lblUserId.Size = new Size(40, 15);
            lblUserId.TabIndex = 0;
            lblUserId.Text = "UserId";
            // 
            // txtUserId
            // 
            txtUserId.Location = new Point(92, 24);
            txtUserId.Name = "txtUserId";
            txtUserId.Size = new Size(202, 23);
            txtUserId.TabIndex = 1;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(20, 56);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(57, 15);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(92, 53);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '.';
            txtPassword.Size = new Size(202, 23);
            txtPassword.TabIndex = 3;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(92, 95);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(75, 23);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Connect";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.DialogResult = DialogResult.OK;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(219, 95);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Click += btnCancel_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 134);
            Controls.Add(btnCancel);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUserId);
            Controls.Add(lblUserId);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Login";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUserId;
        private TextBox txtUserId;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCancel;
    }
}