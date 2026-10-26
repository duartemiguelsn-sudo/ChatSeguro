namespace Client
{
    /// <summary>
    /// Ficheiro gerado pelo Windows Form Designer.
    /// Define o layout e estilo visual do formulário de chat (estilo WhatsApp).
    /// </summary>
    partial class Client
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.textBoxChat = new System.Windows.Forms.RichTextBox();
            this.comboBoxDestinatarios = new System.Windows.Forms.ComboBox();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.panelSidebarHeader = new System.Windows.Forms.Panel();
            this.labelAppName = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelEnviarPara = new System.Windows.Forms.Label();
            this.labelOnline = new System.Windows.Forms.Label();
            this.panelChat = new System.Windows.Forms.Panel();
            this.panelChatHeader = new System.Windows.Forms.Panel();
            this.labelChatTitle = new System.Windows.Forms.Label();
            this.labelCipherInfo = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.labelEncrypted = new System.Windows.Forms.Label();
            this.panelInputArea = new System.Windows.Forms.Panel();
            this.panelSidebar.SuspendLayout();
            this.panelSidebarHeader.SuspendLayout();
            this.panelChat.SuspendLayout();
            this.panelChatHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelInputArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.textBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBoxMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBoxMessage.Location = new System.Drawing.Point(6, 20);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(390, 25);
            this.textBoxMessage.TabIndex = 1;
            // 
            // buttonSend
            // 
            this.buttonSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(138)))), ((int)(((byte)(221)))));
            this.buttonSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSend.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(95)))), ((int)(((byte)(165)))));
            this.buttonSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSend.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.buttonSend.ForeColor = System.Drawing.Color.White;
            this.buttonSend.Location = new System.Drawing.Point(402, 13);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(72, 36);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "➜";
            this.buttonSend.UseVisualStyleBackColor = false;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.buttonQuit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonQuit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.buttonQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQuit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonQuit.ForeColor = System.Drawing.Color.White;
            this.buttonQuit.Location = new System.Drawing.Point(10, 440);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(170, 30);
            this.buttonQuit.TabIndex = 4;
            this.buttonQuit.Text = "Sair";
            this.buttonQuit.UseVisualStyleBackColor = false;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // textBoxChat
            // 
            this.textBoxChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.textBoxChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxChat.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.textBoxChat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBoxChat.Location = new System.Drawing.Point(0, 66);
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.ReadOnly = true;
            this.textBoxChat.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxChat.Size = new System.Drawing.Size(490, 334);
            this.textBoxChat.TabIndex = 4;
            this.textBoxChat.Text = "";
            // 
            // comboBoxDestinatarios
            // 
            this.comboBoxDestinatarios.BackColor = System.Drawing.Color.White;
            this.comboBoxDestinatarios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDestinatarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxDestinatarios.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.comboBoxDestinatarios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.comboBoxDestinatarios.Location = new System.Drawing.Point(10, 104);
            this.comboBoxDestinatarios.Name = "comboBoxDestinatarios";
            this.comboBoxDestinatarios.Size = new System.Drawing.Size(170, 23);
            this.comboBoxDestinatarios.TabIndex = 2;
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.panelSidebar.Controls.Add(this.panelSidebarHeader);
            this.panelSidebar.Controls.Add(this.labelEnviarPara);
            this.panelSidebar.Controls.Add(this.comboBoxDestinatarios);
            this.panelSidebar.Controls.Add(this.labelOnline);
            this.panelSidebar.Controls.Add(this.buttonQuit);
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(190, 480);
            this.panelSidebar.TabIndex = 0;
            // 
            // panelSidebarHeader
            // 
            this.panelSidebarHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(170)))));
            this.panelSidebarHeader.Controls.Add(this.labelAppName);
            this.panelSidebarHeader.Controls.Add(this.labelUsername);
            this.panelSidebarHeader.Location = new System.Drawing.Point(0, 0);
            this.panelSidebarHeader.Name = "panelSidebarHeader";
            this.panelSidebarHeader.Size = new System.Drawing.Size(190, 60);
            this.panelSidebarHeader.TabIndex = 0;
            this.panelSidebarHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelSidebarHeader_Paint);
            // 
            // labelAppName
            // 
            this.labelAppName.AutoSize = true;
            this.labelAppName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelAppName.ForeColor = System.Drawing.Color.White;
            this.labelAppName.Location = new System.Drawing.Point(12, 10);
            this.labelAppName.Name = "labelAppName";
            this.labelAppName.Size = new System.Drawing.Size(94, 20);
            this.labelAppName.TabIndex = 0;
            this.labelAppName.Text = "Chat Seguro";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.labelUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(225)))), ((int)(((byte)(255)))));
            this.labelUsername.Location = new System.Drawing.Point(12, 35);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(0, 13);
            this.labelUsername.TabIndex = 1;
            // 
            // labelEnviarPara
            // 
            this.labelEnviarPara.AutoSize = true;
            this.labelEnviarPara.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.labelEnviarPara.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.labelEnviarPara.Location = new System.Drawing.Point(7, 88);
            this.labelEnviarPara.Name = "labelEnviarPara";
            this.labelEnviarPara.Size = new System.Drawing.Size(67, 13);
            this.labelEnviarPara.TabIndex = 1;
            this.labelEnviarPara.Text = "Enviar para:";
            // 
            // labelOnline
            // 
            this.labelOnline.AutoSize = true;
            this.labelOnline.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.labelOnline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.labelOnline.Location = new System.Drawing.Point(12, 68);
            this.labelOnline.Name = "labelOnline";
            this.labelOnline.Size = new System.Drawing.Size(42, 12);
            this.labelOnline.TabIndex = 3;
            this.labelOnline.Text = "ONLINE";
            // 
            // panelChat
            // 
            this.panelChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelChat.Controls.Add(this.panelChatHeader);
            this.panelChat.Controls.Add(this.textBoxChat);
            this.panelChat.Controls.Add(this.panelFooter);
            this.panelChat.Controls.Add(this.panelInputArea);
            this.panelChat.Location = new System.Drawing.Point(190, 0);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(490, 480);
            this.panelChat.TabIndex = 1;
            // 
            // panelChatHeader
            // 
            this.panelChatHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(170)))));
            this.panelChatHeader.Controls.Add(this.labelChatTitle);
            this.panelChatHeader.Controls.Add(this.labelCipherInfo);
            this.panelChatHeader.Location = new System.Drawing.Point(0, 0);
            this.panelChatHeader.Name = "panelChatHeader";
            this.panelChatHeader.Size = new System.Drawing.Size(490, 60);
            this.panelChatHeader.TabIndex = 0;
            // 
            // labelChatTitle
            // 
            this.labelChatTitle.AutoSize = true;
            this.labelChatTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.labelChatTitle.ForeColor = System.Drawing.Color.White;
            this.labelChatTitle.Location = new System.Drawing.Point(14, 8);
            this.labelChatTitle.Name = "labelChatTitle";
            this.labelChatTitle.Size = new System.Drawing.Size(82, 20);
            this.labelChatTitle.TabIndex = 0;
            this.labelChatTitle.Text = "Chat Geral";
            // 
            // labelCipherInfo
            // 
            this.labelCipherInfo.AutoSize = true;
            this.labelCipherInfo.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.labelCipherInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(225)))), ((int)(((byte)(255)))));
            this.labelCipherInfo.Location = new System.Drawing.Point(14, 30);
            this.labelCipherInfo.Name = "labelCipherInfo";
            this.labelCipherInfo.Size = new System.Drawing.Size(78, 13);
            this.labelCipherInfo.TabIndex = 1;
            this.labelCipherInfo.Text = "🔒 cifrado AES";
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(237)))), ((int)(((byte)(245)))));
            this.panelFooter.Controls.Add(this.labelEncrypted);
            this.panelFooter.Location = new System.Drawing.Point(0, 400);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(490, 22);
            this.panelFooter.TabIndex = 5;
            // 
            // labelEncrypted
            // 
            this.labelEncrypted.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.labelEncrypted.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.labelEncrypted.Location = new System.Drawing.Point(0, 4);
            this.labelEncrypted.Name = "labelEncrypted";
            this.labelEncrypted.Size = new System.Drawing.Size(490, 16);
            this.labelEncrypted.TabIndex = 0;
            this.labelEncrypted.Text = "🔒  As mensagens são cifradas de ponta a ponta";
            this.labelEncrypted.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelInputArea
            // 
            this.panelInputArea.BackColor = System.Drawing.Color.White;
            this.panelInputArea.Controls.Add(this.textBoxMessage);
            this.panelInputArea.Controls.Add(this.buttonSend);
            this.panelInputArea.Location = new System.Drawing.Point(0, 422);
            this.panelInputArea.Name = "panelInputArea";
            this.panelInputArea.Size = new System.Drawing.Size(490, 58);
            this.panelInputArea.TabIndex = 6;
            this.panelInputArea.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelInputArea_Paint);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(680, 480);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.panelChat);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Client";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Seguro";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            this.panelSidebar.ResumeLayout(false);
            this.panelSidebar.PerformLayout();
            this.panelSidebarHeader.ResumeLayout(false);
            this.panelSidebarHeader.PerformLayout();
            this.panelChat.ResumeLayout(false);
            this.panelChatHeader.ResumeLayout(false);
            this.panelChatHeader.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelInputArea.ResumeLayout(false);
            this.panelInputArea.PerformLayout();
            this.ResumeLayout(false);

        }

        // Linha separadora no fundo do cabeçalho da sidebar
        private void PanelSidebarHeader_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var p = new System.Drawing.Pen(System.Drawing.Color.FromArgb(30, 80, 140), 1);
            e.Graphics.DrawLine(p, 0, ((System.Windows.Forms.Panel)sender).Height - 1,
                                   ((System.Windows.Forms.Panel)sender).Width,
                                   ((System.Windows.Forms.Panel)sender).Height - 1);
            p.Dispose();
        }

        // Linha separadora no topo da área de input
        private void PanelInputArea_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var p = new System.Drawing.Pen(System.Drawing.Color.FromArgb(210, 210, 210), 1);
            e.Graphics.DrawLine(p, 0, 0, ((System.Windows.Forms.Panel)sender).Width, 0);
            p.Dispose();
        }

        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.RichTextBox textBoxChat;
        private System.Windows.Forms.ComboBox comboBoxDestinatarios;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelSidebarHeader;
        private System.Windows.Forms.Panel panelChat;
        private System.Windows.Forms.Panel panelChatHeader;
        private System.Windows.Forms.Panel panelInputArea;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label labelAppName;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelOnline;
        private System.Windows.Forms.Label labelEnviarPara;
        private System.Windows.Forms.Label labelChatTitle;
        private System.Windows.Forms.Label labelCipherInfo;
        private System.Windows.Forms.Label labelEncrypted;
    }
}