
namespace QuadGamingSecurity
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.PortsListBox = new System.Windows.Forms.ListBox();
            this.WindowsListBox = new System.Windows.Forms.ListBox();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.RefreshPortsListButton = new System.Windows.Forms.Button();
            this.RefreshProcessesListButton = new System.Windows.Forms.Button();
            this.ForegroundSelectedProcessButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ConsoleCheckbox = new System.Windows.Forms.CheckBox();
            this.AutostartCheckbox = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ConsoleTextBox = new System.Windows.Forms.TextBox();
            this.AutoConnectCheckbox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PortsListBox
            // 
            this.PortsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PortsListBox.FormattingEnabled = true;
            this.PortsListBox.Location = new System.Drawing.Point(12, 12);
            this.PortsListBox.Name = "PortsListBox";
            this.PortsListBox.Size = new System.Drawing.Size(560, 95);
            this.PortsListBox.TabIndex = 0;
            this.PortsListBox.SelectedIndexChanged += new System.EventHandler(this.DevicesListBox_SelectedIndexChanged);
            // 
            // WindowsListBox
            // 
            this.WindowsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowsListBox.FormattingEnabled = true;
            this.WindowsListBox.Location = new System.Drawing.Point(12, 143);
            this.WindowsListBox.Name = "WindowsListBox";
            this.WindowsListBox.Size = new System.Drawing.Size(560, 186);
            this.WindowsListBox.TabIndex = 1;
            this.WindowsListBox.SelectedIndexChanged += new System.EventHandler(this.WindowsListBox_SelectedIndexChanged);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "QGS";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.Click += new System.EventHandler(this.NotifyIcon_Click);
            this.NotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // RefreshPortsListButton
            // 
            this.RefreshPortsListButton.Location = new System.Drawing.Point(12, 114);
            this.RefreshPortsListButton.Name = "RefreshPortsListButton";
            this.RefreshPortsListButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshPortsListButton.TabIndex = 2;
            this.RefreshPortsListButton.Text = "Refresh";
            this.RefreshPortsListButton.UseVisualStyleBackColor = true;
            this.RefreshPortsListButton.Click += new System.EventHandler(this.RefreshPortsListButton_Click);
            // 
            // RefreshProcessesListButton
            // 
            this.RefreshProcessesListButton.Location = new System.Drawing.Point(12, 336);
            this.RefreshProcessesListButton.Name = "RefreshProcessesListButton";
            this.RefreshProcessesListButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshProcessesListButton.TabIndex = 3;
            this.RefreshProcessesListButton.Text = "Refresh";
            this.RefreshProcessesListButton.UseVisualStyleBackColor = true;
            this.RefreshProcessesListButton.Click += new System.EventHandler(this.RefreshProcessesListButton_Click);
            // 
            // ForegroundSelectedProcessButton
            // 
            this.ForegroundSelectedProcessButton.Location = new System.Drawing.Point(94, 336);
            this.ForegroundSelectedProcessButton.Name = "ForegroundSelectedProcessButton";
            this.ForegroundSelectedProcessButton.Size = new System.Drawing.Size(100, 23);
            this.ForegroundSelectedProcessButton.TabIndex = 4;
            this.ForegroundSelectedProcessButton.Text = "Foreground now";
            this.ForegroundSelectedProcessButton.UseVisualStyleBackColor = true;
            this.ForegroundSelectedProcessButton.Click += new System.EventHandler(this.ForegroundSelectedProcessButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExitButton.Location = new System.Drawing.Point(485, 0);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 5;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.ConsoleCheckbox);
            this.panel1.Controls.Add(this.AutostartCheckbox);
            this.panel1.Controls.Add(this.ExitButton);
            this.panel1.Location = new System.Drawing.Point(12, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 23);
            this.panel1.TabIndex = 6;
            // 
            // ConsoleCheckbox
            // 
            this.ConsoleCheckbox.AutoSize = true;
            this.ConsoleCheckbox.Location = new System.Drawing.Point(148, 4);
            this.ConsoleCheckbox.Name = "ConsoleCheckbox";
            this.ConsoleCheckbox.Size = new System.Drawing.Size(64, 17);
            this.ConsoleCheckbox.TabIndex = 7;
            this.ConsoleCheckbox.Text = "Console";
            this.ConsoleCheckbox.UseVisualStyleBackColor = true;
            this.ConsoleCheckbox.CheckedChanged += new System.EventHandler(this.ConsoleCheckbox_CheckedChanged);
            // 
            // AutostartCheckbox
            // 
            this.AutostartCheckbox.AutoSize = true;
            this.AutostartCheckbox.Location = new System.Drawing.Point(4, 4);
            this.AutostartCheckbox.Name = "AutostartCheckbox";
            this.AutostartCheckbox.Size = new System.Drawing.Size(138, 17);
            this.AutostartCheckbox.TabIndex = 6;
            this.AutostartCheckbox.Text = "Start on Windows starts";
            this.AutostartCheckbox.UseVisualStyleBackColor = true;
            this.AutostartCheckbox.CheckedChanged += new System.EventHandler(this.AutostartCheckbox_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 521);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(584, 40);
            this.panel2.TabIndex = 7;
            // 
            // ConsoleTextBox
            // 
            this.ConsoleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsoleTextBox.Location = new System.Drawing.Point(13, 366);
            this.ConsoleTextBox.Multiline = true;
            this.ConsoleTextBox.Name = "ConsoleTextBox";
            this.ConsoleTextBox.ReadOnly = true;
            this.ConsoleTextBox.Size = new System.Drawing.Size(559, 154);
            this.ConsoleTextBox.TabIndex = 8;
            // 
            // AutoConnectCheckbox
            // 
            this.AutoConnectCheckbox.AutoSize = true;
            this.AutoConnectCheckbox.Location = new System.Drawing.Point(94, 118);
            this.AutoConnectCheckbox.Name = "AutoConnectCheckbox";
            this.AutoConnectCheckbox.Size = new System.Drawing.Size(104, 17);
            this.AutoConnectCheckbox.TabIndex = 9;
            this.AutoConnectCheckbox.Text = "Connect on start";
            this.AutoConnectCheckbox.UseVisualStyleBackColor = true;
            this.AutoConnectCheckbox.CheckedChanged += new System.EventHandler(this.AutoConnectCheckbox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 561);
            this.Controls.Add(this.AutoConnectCheckbox);
            this.Controls.Add(this.ConsoleTextBox);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.ForegroundSelectedProcessButton);
            this.Controls.Add(this.RefreshProcessesListButton);
            this.Controls.Add(this.RefreshPortsListButton);
            this.Controls.Add(this.WindowsListBox);
            this.Controls.Add(this.PortsListBox);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quad Gaming Security";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox PortsListBox;
        private System.Windows.Forms.ListBox WindowsListBox;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.Button RefreshPortsListButton;
        private System.Windows.Forms.Button RefreshProcessesListButton;
        private System.Windows.Forms.Button ForegroundSelectedProcessButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox ConsoleTextBox;
        private System.Windows.Forms.CheckBox AutostartCheckbox;
        private System.Windows.Forms.CheckBox AutoConnectCheckbox;
        private System.Windows.Forms.CheckBox ConsoleCheckbox;
    }
}

