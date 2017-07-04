namespace 上位机服务器
{
    partial class LoginForm
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
            System.Windows.Forms.Button bt_cancel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.label1 = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.Label();
            this.tx_user = new System.Windows.Forms.TextBox();
            this.tx_password = new System.Windows.Forms.TextBox();
            this.bt_login = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            bt_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bt_cancel
            // 
            bt_cancel.Font = new System.Drawing.Font("微软雅黑", 10F);
            bt_cancel.Location = new System.Drawing.Point(258, 157);
            bt_cancel.Margin = new System.Windows.Forms.Padding(4);
            bt_cancel.Name = "bt_cancel";
            bt_cancel.Size = new System.Drawing.Size(80, 30);
            bt_cancel.TabIndex = 4;
            bt_cancel.Text = "退出";
            bt_cancel.UseVisualStyleBackColor = true;
            bt_cancel.Click += new System.EventHandler(this.bt_cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label1.Location = new System.Drawing.Point(46, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户姓名:";
            // 
            // Password
            // 
            this.Password.AutoSize = true;
            this.Password.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Password.Location = new System.Drawing.Point(45, 111);
            this.Password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(68, 20);
            this.Password.TabIndex = 1;
            this.Password.Text = "用户密码:";
            // 
            // tx_user
            // 
            this.tx_user.Font = new System.Drawing.Font("华文楷体", 13F);
            this.tx_user.Location = new System.Drawing.Point(136, 56);
            this.tx_user.Margin = new System.Windows.Forms.Padding(4);
            this.tx_user.Name = "tx_user";
            this.tx_user.Size = new System.Drawing.Size(176, 30);
            this.tx_user.TabIndex = 2;
            this.tx_user.Text = "admin";
            // 
            // tx_password
            // 
            this.tx_password.Font = new System.Drawing.Font("华文楷体", 13F);
            this.tx_password.Location = new System.Drawing.Point(138, 106);
            this.tx_password.Margin = new System.Windows.Forms.Padding(4);
            this.tx_password.Name = "tx_password";
            this.tx_password.PasswordChar = '*';
            this.tx_password.Size = new System.Drawing.Size(176, 30);
            this.tx_password.TabIndex = 3;
            this.tx_password.Text = "admin";
            // 
            // bt_login
            // 
            this.bt_login.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.bt_login.Location = new System.Drawing.Point(51, 157);
            this.bt_login.Margin = new System.Windows.Forms.Padding(4);
            this.bt_login.Name = "bt_login";
            this.bt_login.Size = new System.Drawing.Size(80, 30);
            this.bt_login.TabIndex = 5;
            this.bt_login.Text = "登录";
            this.bt_login.UseVisualStyleBackColor = true;
            this.bt_login.Click += new System.EventHandler(this.bt_login_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(31, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(335, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "氢钟智能化监控系统登录平台";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 200);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bt_login);
            this.Controls.Add(bt_cancel);
            this.Controls.Add(this.tx_password);
            this.Controls.Add(this.tx_user);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("华文楷体", 11F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Password;
        private System.Windows.Forms.TextBox tx_user;
        private System.Windows.Forms.TextBox tx_password;
        private System.Windows.Forms.Button bt_login;
        private System.Windows.Forms.Label label3;
    }
}