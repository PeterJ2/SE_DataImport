namespace SE_DataImport
{
    partial class MainForm
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
            this.connectionString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buildConnectionStringButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.importDirectory = new System.Windows.Forms.TextBox();
            this.findImportDirectoryButton = new System.Windows.Forms.Button();
            this.folderBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.tablePrefix = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.beginImportButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.messages = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // connectionString
            // 
            this.connectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionString.Location = new System.Drawing.Point(111, 10);
            this.connectionString.Name = "connectionString";
            this.connectionString.Size = new System.Drawing.Size(384, 20);
            this.connectionString.TabIndex = 0;
            this.connectionString.Text = "Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection string:";
            // 
            // buildConnectionStringButton
            // 
            this.buildConnectionStringButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buildConnectionStringButton.Location = new System.Drawing.Point(504, 8);
            this.buildConnectionStringButton.Name = "buildConnectionStringButton";
            this.buildConnectionStringButton.Size = new System.Drawing.Size(26, 23);
            this.buildConnectionStringButton.TabIndex = 2;
            this.buildConnectionStringButton.Text = "...";
            this.buildConnectionStringButton.UseVisualStyleBackColor = true;
            this.buildConnectionStringButton.Click += new System.EventHandler(this.buildConnectionStringButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Import directory:";
            // 
            // importDirectory
            // 
            this.importDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importDirectory.Location = new System.Drawing.Point(111, 39);
            this.importDirectory.Name = "importDirectory";
            this.importDirectory.Size = new System.Drawing.Size(384, 20);
            this.importDirectory.TabIndex = 4;
            // 
            // findImportDirectoryButton
            // 
            this.findImportDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.findImportDirectoryButton.Location = new System.Drawing.Point(504, 36);
            this.findImportDirectoryButton.Name = "findImportDirectoryButton";
            this.findImportDirectoryButton.Size = new System.Drawing.Size(26, 23);
            this.findImportDirectoryButton.TabIndex = 5;
            this.findImportDirectoryButton.Text = "...";
            this.findImportDirectoryButton.UseVisualStyleBackColor = true;
            this.findImportDirectoryButton.Click += new System.EventHandler(this.findImportDirectoryButton_Click);
            // 
            // folderBrowserDlg
            // 
            this.folderBrowserDlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDlg.ShowNewFolderButton = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Table prefix:";
            // 
            // tablePrefix
            // 
            this.tablePrefix.Location = new System.Drawing.Point(111, 66);
            this.tablePrefix.Name = "tablePrefix";
            this.tablePrefix.Size = new System.Drawing.Size(100, 20);
            this.tablePrefix.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(218, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(312, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "(may be left blank if database will only contain data from one site)";
            // 
            // beginImportButton
            // 
            this.beginImportButton.Location = new System.Drawing.Point(111, 93);
            this.beginImportButton.Name = "beginImportButton";
            this.beginImportButton.Size = new System.Drawing.Size(75, 23);
            this.beginImportButton.TabIndex = 9;
            this.beginImportButton.Text = "Begin Import";
            this.beginImportButton.UseVisualStyleBackColor = true;
            this.beginImportButton.Click += new System.EventHandler(this.beginImportButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Messages:";
            // 
            // messages
            // 
            this.messages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messages.Location = new System.Drawing.Point(16, 149);
            this.messages.Multiline = true;
            this.messages.Name = "messages";
            this.messages.Size = new System.Drawing.Size(511, 122);
            this.messages.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 278);
            this.Controls.Add(this.messages);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.beginImportButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tablePrefix);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.findImportDirectoryButton);
            this.Controls.Add(this.importDirectory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buildConnectionStringButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectionString);
            this.Name = "MainForm";
            this.Text = "Stack Exchange Data Import";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox connectionString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buildConnectionStringButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox importDirectory;
        private System.Windows.Forms.Button findImportDirectoryButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDlg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tablePrefix;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button beginImportButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox messages;
    }
}

