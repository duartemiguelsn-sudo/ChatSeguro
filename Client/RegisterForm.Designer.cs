namespace Cliente
{
    partial class RegisterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterForm));
            this.buttonSubmitRegister = new System.Windows.Forms.Button();
            this.textBoxNewPass = new System.Windows.Forms.TextBox();
            this.textBoxNewUser = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSubmitRegister
            // 
            this.buttonSubmitRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.buttonSubmitRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSubmitRegister.ForeColor = System.Drawing.Color.White;
            this.buttonSubmitRegister.Location = new System.Drawing.Point(96, 258);
            this.buttonSubmitRegister.Name = "buttonSubmitRegister";
            this.buttonSubmitRegister.Size = new System.Drawing.Size(86, 30);
            this.buttonSubmitRegister.TabIndex = 9;
            this.buttonSubmitRegister.Text = "Submit";
            this.buttonSubmitRegister.UseVisualStyleBackColor = false;
            this.buttonSubmitRegister.Click += new System.EventHandler(this.buttonSubmitRegister_Click);
            // 
            // textBoxNewPass
            // 
            this.textBoxNewPass.Location = new System.Drawing.Point(96, 208);
            this.textBoxNewPass.Name = "textBoxNewPass";
            this.textBoxNewPass.Size = new System.Drawing.Size(161, 22);
            this.textBoxNewPass.TabIndex = 8;
            this.textBoxNewPass.UseSystemPasswordChar = true;
            // 
            // textBoxNewUser
            // 
            this.textBoxNewUser.Location = new System.Drawing.Point(96, 126);
            this.textBoxNewUser.Name = "textBoxNewUser";
            this.textBoxNewUser.Size = new System.Drawing.Size(161, 22);
            this.textBoxNewUser.TabIndex = 7;
            // 
            // Password
            // 
            this.Password.AutoSize = true;
            this.Password.Location = new System.Drawing.Point(93, 179);
            this.Password.Name = "Password";
            this.Password.Padding = new System.Windows.Forms.Padding(5);
            this.Password.Size = new System.Drawing.Size(77, 26);
            this.Password.TabIndex = 6;
            this.Password.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 97);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(5);
            this.label1.Size = new System.Drawing.Size(80, 26);
            this.label1.TabIndex = 5;
            this.label1.Text = "Username";
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(354, 377);
            this.Controls.Add(this.buttonSubmitRegister);
            this.Controls.Add(this.textBoxNewPass);
            this.Controls.Add(this.textBoxNewUser);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RegisterForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RegisterForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSubmitRegister;
        private System.Windows.Forms.TextBox textBoxNewPass;
        private System.Windows.Forms.TextBox textBoxNewUser;
        private System.Windows.Forms.Label Password;
        private System.Windows.Forms.Label label1;
    }
}