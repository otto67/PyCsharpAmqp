
namespace WindowsFormsApp1
{
    partial class RabbitMQCode
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ModeSelector = new System.Windows.Forms.ToolStripComboBox();
            this.TaskOper = new System.Windows.Forms.ToolStripComboBox();
            this.TaskId = new System.Windows.Forms.ToolStripTextBox();
            this.ServerResp = new System.Windows.Forms.TextBox();
            this.ServerRespLabel = new System.Windows.Forms.Label();
            this.ListButton = new System.Windows.Forms.Button();
            this.TaskDescription = new System.Windows.Forms.TextBox();
            this.TaskDescriptionLabel = new System.Windows.Forms.Label();
            this.SendRequest = new System.Windows.Forms.Button();
            this.blockingCheck = new System.Windows.Forms.CheckBox();
            this.Receipt = new System.Windows.Forms.CheckBox();
            this.getButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ModeSelector,
            this.TaskOper,
            this.TaskId});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 27);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // ModeSelector
            // 
            this.ModeSelector.Items.AddRange(new object[] {
            "Send",
            "Listen"});
            this.ModeSelector.Name = "ModeSelector";
            this.ModeSelector.Size = new System.Drawing.Size(121, 23);
            this.ModeSelector.Text = "Mode ";
            this.ModeSelector.SelectedIndexChanged += new System.EventHandler(this.ModeSelector_SelectedIndexChanged);
            // 
            // TaskOper
            // 
            this.TaskOper.Items.AddRange(new object[] {
            "Add",
            "Delete",
            "Complete",
            "List"});
            this.TaskOper.Name = "TaskOper";
            this.TaskOper.Size = new System.Drawing.Size(121, 23);
            this.TaskOper.Tag = "";
            this.TaskOper.Text = "Task operation";
            this.TaskOper.SelectedIndexChanged += new System.EventHandler(this.TaskOper_SelectedIndexChanged);
            // 
            // TaskId
            // 
            this.TaskId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TaskId.Name = "TaskId";
            this.TaskId.Size = new System.Drawing.Size(100, 23);
            this.TaskId.Text = "0";
            // 
            // ServerResp
            // 
            this.ServerResp.Location = new System.Drawing.Point(297, 52);
            this.ServerResp.Multiline = true;
            this.ServerResp.Name = "ServerResp";
            this.ServerResp.Size = new System.Drawing.Size(503, 278);
            this.ServerResp.TabIndex = 3;
            this.ServerResp.Text = "No response yet";
            // 
            // ServerRespLabel
            // 
            this.ServerRespLabel.AutoSize = true;
            this.ServerRespLabel.Location = new System.Drawing.Point(294, 36);
            this.ServerRespLabel.Name = "ServerRespLabel";
            this.ServerRespLabel.Size = new System.Drawing.Size(89, 13);
            this.ServerRespLabel.TabIndex = 4;
            this.ServerRespLabel.Text = "Server Response";
            // 
            // ListButton
            // 
            this.ListButton.Location = new System.Drawing.Point(297, 336);
            this.ListButton.Name = "ListButton";
            this.ListButton.Size = new System.Drawing.Size(75, 23);
            this.ListButton.TabIndex = 5;
            this.ListButton.Text = "List task(s)";
            this.ListButton.UseVisualStyleBackColor = true;
            this.ListButton.Click += new System.EventHandler(this.Receive_Click);
            // 
            // TaskDescription
            // 
            this.TaskDescription.Location = new System.Drawing.Point(25, 71);
            this.TaskDescription.Multiline = true;
            this.TaskDescription.Name = "TaskDescription";
            this.TaskDescription.Size = new System.Drawing.Size(165, 121);
            this.TaskDescription.TabIndex = 6;
            // 
            // TaskDescriptionLabel
            // 
            this.TaskDescriptionLabel.AutoSize = true;
            this.TaskDescriptionLabel.Location = new System.Drawing.Point(22, 55);
            this.TaskDescriptionLabel.Name = "TaskDescriptionLabel";
            this.TaskDescriptionLabel.Size = new System.Drawing.Size(85, 13);
            this.TaskDescriptionLabel.TabIndex = 7;
            this.TaskDescriptionLabel.Text = "Task description";
            // 
            // SendRequest
            // 
            this.SendRequest.Location = new System.Drawing.Point(25, 26);
            this.SendRequest.Name = "SendRequest";
            this.SendRequest.Size = new System.Drawing.Size(75, 23);
            this.SendRequest.TabIndex = 8;
            this.SendRequest.Text = "Execute";
            this.SendRequest.UseVisualStyleBackColor = true;
            this.SendRequest.Click += new System.EventHandler(this.SendRequest_Click);
            // 
            // blockingCheck
            // 
            this.blockingCheck.AutoSize = true;
            this.blockingCheck.Location = new System.Drawing.Point(106, 340);
            this.blockingCheck.Name = "blockingCheck";
            this.blockingCheck.Size = new System.Drawing.Size(67, 17);
            this.blockingCheck.TabIndex = 9;
            this.blockingCheck.Text = "Blocking";
            this.blockingCheck.UseVisualStyleBackColor = true;
            // 
            // Receipt
            // 
            this.Receipt.AutoSize = true;
            this.Receipt.Location = new System.Drawing.Point(106, 30);
            this.Receipt.Name = "Receipt";
            this.Receipt.Size = new System.Drawing.Size(63, 17);
            this.Receipt.TabIndex = 10;
            this.Receipt.Text = "Receipt";
            this.Receipt.UseVisualStyleBackColor = true;
            // 
            // getButton
            // 
            this.getButton.Location = new System.Drawing.Point(34, 306);
            this.getButton.Name = "getButton";
            this.getButton.Size = new System.Drawing.Size(66, 53);
            this.getButton.TabIndex = 11;
            this.getButton.Text = "Get messages";
            this.getButton.UseVisualStyleBackColor = true;
            this.getButton.Click += new System.EventHandler(this.getButton_Click);
            // 
            // RabbitMQCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.getButton);
            this.Controls.Add(this.Receipt);
            this.Controls.Add(this.blockingCheck);
            this.Controls.Add(this.SendRequest);
            this.Controls.Add(this.TaskDescriptionLabel);
            this.Controls.Add(this.TaskDescription);
            this.Controls.Add(this.ListButton);
            this.Controls.Add(this.ServerRespLabel);
            this.Controls.Add(this.ServerResp);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RabbitMQCode";
            this.Text = "RabbitMQ testing";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripComboBox TaskOper;
        private System.Windows.Forms.ToolStripTextBox TaskId;
        private System.Windows.Forms.TextBox ServerResp;
        private System.Windows.Forms.Label ServerRespLabel;
        private System.Windows.Forms.Button ListButton;
        private System.Windows.Forms.TextBox TaskDescription;
        private System.Windows.Forms.Label TaskDescriptionLabel;
        private System.Windows.Forms.Button SendRequest;
        private System.Windows.Forms.CheckBox blockingCheck;
        private System.Windows.Forms.CheckBox Receipt;
        private System.Windows.Forms.ToolStripComboBox ModeSelector;
        private System.Windows.Forms.Button getButton;
    }
}

